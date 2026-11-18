using System;
using System.Linq;
using ObservableVariables.Contract;
using ObservableVariables.Editor.GUI.ObservableVariableElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI
{
    internal class VariablesWindow : EditorWindow
    {
        [MenuItem("Tools/Fixer33/Variables window")]
        private static void ShowWindow()
        {
            var window = GetWindow<VariablesWindow>();
            window.titleContent = new GUIContent("Variables window");
            window.Show();
        }

        private const string LastTypePrefKey = "ObservableVariables.VariablesWindow.LastType";

        private Type[] _possibleTypes;
        private int _currentType;
        private int _refreshCounter;

        private DropdownField _typeDropdown;
        private ScrollView _recordsContainer;
        private VisualElement _nonRuntimePanel, _runtimePanel;

        private void CreateGUI()
        {
            string loadPath = "Packages/com.fixer33.observable-variables/Editor/GUI/Markup and stylesheets/VariablesWindow";
#if PACKAGES_DEV
            loadPath = "Assets/" + loadPath;
#endif

            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath + ".uxml");
            if (tree != null)
            {
                VisualElement root = rootVisualElement;
                tree.CloneTree(root);
            }

            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(loadPath + ".uss");
            if (style != null)
            {
                rootVisualElement.styleSheets.Add(style);
            }

            _possibleTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(i => i.IsSubclassOf(typeof(ObservableVariableBase)) && i.IsAbstract == false).ToArray();

            _nonRuntimePanel = rootVisualElement.Q<VisualElement>("variables-window__non-runtime-panel");
            _runtimePanel = rootVisualElement.Q<VisualElement>("variables-window__runtime-panel");
            _recordsContainer = rootVisualElement.Q<ScrollView>("runtime-panel__record-container");

            _typeDropdown = rootVisualElement.Q<DropdownField>("runtime-panel__type-dropdown");
            _typeDropdown.choices = _possibleTypes.Select(t => t.Name).ToList();

            // Restore last selected type; fall back to the first one if it no longer exists
            string saved = EditorPrefs.GetString(LastTypePrefKey, string.Empty);
            _currentType = Mathf.Max(0, Array.FindIndex(_possibleTypes, t => t.Name == saved));
            if (_possibleTypes.Length > 0)
                _typeDropdown.SetValueWithoutNotify(_possibleTypes[_currentType].Name);
            _typeDropdown.RegisterValueChangedCallback(OnTypeChanged);

            SetCurrentType(_currentType);
        }

        private void OnTypeChanged(ChangeEvent<string> evt)
        {
            int index = Math.Max(0, Array.FindIndex(_possibleTypes, t => t.Name == evt.newValue));
            EditorPrefs.SetString(LastTypePrefKey, _possibleTypes[index].Name);
            SetCurrentType(index);
        }

        private void SetCurrentType(int index)
        {
            if (_possibleTypes.Length == 0)
                return;
            if (index < 0 || index >= _possibleTypes.Length)
                index = 0;
            _currentType = index;

            _recordsContainer.contentContainer.Clear();

            var variableKeys = Variables.Editor_GetAllKeysForType(_possibleTypes[_currentType]);
            foreach (var variableKey in variableKeys)
            {
                _recordsContainer.contentContainer.Add(
                    ObservableVariableElementFactory.Create(Variables.Editor_GetExisting(variableKey), variableKey));
            }
        }

        private void Update()
        {
            if (_nonRuntimePanel == null || _runtimePanel == null)
                return;

            _nonRuntimePanel.visible = Application.isPlaying == false;
            _runtimePanel.visible = Application.isPlaying;

            if (Application.isPlaying && ++_refreshCounter > 380)
            {
                _refreshCounter = 0;
                SetCurrentType(_currentType);
            }
        }
    }
}
