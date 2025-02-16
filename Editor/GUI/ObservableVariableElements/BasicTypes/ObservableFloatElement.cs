using ObservableVariables.BasicTypes;
using UnityEngine.UIElements;

namespace ObservableVariables.GUI.ObservableVariableElements.BasicTypes
{
    public class ObservableFloatElement : ObservableVariableElementBase<ObservableFloat, float, FloatField>
    {
        public ObservableFloatElement(ObservableFloat variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(FloatField editField, float activeValue, EventCallback<ChangeEvent<float>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(onInputValueChanged);
        }

        protected override float GetInputFieldValue(FloatField inputField) => inputField.value;
        protected override void SetInputFieldValue(FloatField inputField, float value) => inputField.value = value;
    }
}