using ObservableVariables.BasicTypes;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements.BasicTypes
{
    public class ObservableStringElement : ObservableVariableElementBase<ObservableString, string, TextField>
    {
        public ObservableStringElement(ObservableString variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(TextField editField, string activeValue, EventCallback<ChangeEvent<string>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override string GetInputFieldValue(TextField inputField) => inputField.value;
        protected override void SetInputFieldValue(TextField inputField, string value) => inputField.value = value;
    }
}