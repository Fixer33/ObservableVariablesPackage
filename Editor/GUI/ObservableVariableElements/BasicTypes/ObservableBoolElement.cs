using ObservableVariables.BasicTypes;
using UnityEngine.UIElements;

namespace ObservableVariables.GUI.ObservableVariableElements.BasicTypes
{
    public class ObservableBoolElement : ObservableVariableElementBase<ObservableBool, bool, Toggle>
    {
        public ObservableBoolElement(ObservableBool variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(Toggle editField, bool activeValue, EventCallback<ChangeEvent<bool>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override bool GetInputFieldValue(Toggle inputField) => inputField.value;
        protected override void SetInputFieldValue(Toggle inputField, bool value) => inputField.value = value;
    }
}