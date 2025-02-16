using ObservableVariables.Unity;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.GUI.ObservableVariableElements.Unity
{
    public class ObservableTransformElement : ObservableVariableElementBase<ObservableTransform, Transform, ObjectField>
    {
        public ObservableTransformElement(ObservableTransform variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(ObjectField editField, Transform activeValue, EventCallback<ChangeEvent<Transform>> onInputValueChanged)
        {
            editField.value = activeValue;

            editField.RegisterValueChangedCallback(args =>
                onInputValueChanged?.Invoke(
                    ChangeEvent<Transform>.GetPooled(args.previousValue as Transform, args.newValue as Transform)));
        }

        protected override Transform GetInputFieldValue(ObjectField inputField) => inputField.value as Transform;
        protected override void SetInputFieldValue(ObjectField inputField, Transform value) =>
            inputField.value = value;
    }
}