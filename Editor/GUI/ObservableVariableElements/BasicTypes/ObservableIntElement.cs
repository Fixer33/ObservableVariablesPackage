using ObservableVariables.BasicTypes;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements.BasicTypes
{
    public class ObservableIntElement : ObservableVariableElementBase<ObservableInt, int, IntegerField>
    {
        public ObservableIntElement(ObservableInt variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(IntegerField editField, int activeValue, EventCallback<ChangeEvent<int>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override int GetInputFieldValue(IntegerField inputField) => inputField.value;
        protected override void SetInputFieldValue(IntegerField inputField, int value) => inputField.value = value;
    }
}