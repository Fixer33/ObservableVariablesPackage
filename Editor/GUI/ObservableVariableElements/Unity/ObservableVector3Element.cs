using ObservableVariables.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.GUI.ObservableVariableElements.Unity
{
    public class ObservableVector3Element : ObservableVariableElementBase<ObservableVector3, Vector3, Vector3Field>
    {
        public ObservableVector3Element(ObservableVector3 variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(Vector3Field editField, Vector3 activeValue, EventCallback<ChangeEvent<Vector3>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override Vector3 GetInputFieldValue(Vector3Field inputField) => inputField.value;
        protected override void SetInputFieldValue(Vector3Field inputField, Vector3 value) => inputField.value = value;
    }
}