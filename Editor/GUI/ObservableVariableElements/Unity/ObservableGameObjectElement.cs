using ObservableVariables.Unity;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements.Unity
{
    public class ObservableGameObjectElement : ObservableVariableElementBase<ObservableGameObject, GameObject, ObjectField>
    {
        public ObservableGameObjectElement(ObservableGameObject variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(ObjectField editField, GameObject activeValue, EventCallback<ChangeEvent<GameObject>> onInputValueChanged)
        {
            editField.value = activeValue;

            editField.RegisterValueChangedCallback(args =>
                onInputValueChanged?.Invoke(
                    ChangeEvent<GameObject>.GetPooled(args.previousValue as GameObject, args.newValue as GameObject)));
        }

        protected override GameObject GetInputFieldValue(ObjectField inputField) => inputField.value as GameObject;
        protected override void SetInputFieldValue(ObjectField inputField, GameObject value) => inputField.value = value;
    }
}