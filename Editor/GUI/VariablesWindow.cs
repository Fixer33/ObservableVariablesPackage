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

        private Type[] _possibleTypes;
        private int _currentType;
        private int _refreshCounter;
        
        private Button _nextTypeBtn, _prevTypeBtn;
        private Label _currentTypeHeader;
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
            _currentTypeHeader = rootVisualElement.Q<Label>("runtime-panel-header__text");
            _nextTypeBtn = rootVisualElement.Q<Button>("runtime-panel-header__next-btn");
            _nextTypeBtn.clicked += NextTypeBtnClicked;
            _prevTypeBtn = rootVisualElement.Q<Button>("runtime-panel-header__prev-btn");
            _prevTypeBtn.clicked += PrevTypeBtnClicked;
            _recordsContainer = rootVisualElement.Q<ScrollView>("runtime-panel__record-container");

            SetCurrentType(_currentType);
        }

        private void SetCurrentType(int index)
        {
            if (index < 0)
                index = _possibleTypes.Length - 1;
            else if (index >= _possibleTypes.Length)
                index = 0;
            _currentType = index % _possibleTypes.Length;

            _currentTypeHeader.text = _possibleTypes[_currentType].Name;
            _recordsContainer.contentContainer.Clear();

            var variableKeys = Variables.Editor_GetAllKeysForType(_possibleTypes[_currentType]);
            foreach (var variableKey in variableKeys)
            {
                _recordsContainer.contentContainer.Add(
                    ObservableVariableElementFactory.Create(Variables.Editor_GetExisting(variableKey), variableKey));
            }
        }

        private void PrevTypeBtnClicked()
        {
            SetCurrentType(_currentType - 1);
        }

        private void NextTypeBtnClicked()
        {
            SetCurrentType(_currentType + 1);
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