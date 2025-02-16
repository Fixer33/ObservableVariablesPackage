using ObservableVariables.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements.Unity
{
    public class ObservableVector2Element : ObservableVariableElementBase<ObservableVector2, Vector2, Vector2Field>
    {
        public ObservableVector2Element(ObservableVector2 variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(Vector2Field editField, Vector2 activeValue, EventCallback<ChangeEvent<Vector2>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override Vector2 GetInputFieldValue(Vector2Field inputField) => inputField.value;
        protected override void SetInputFieldValue(Vector2Field inputField, Vector2 value) => inputField.value = value;
    }
}