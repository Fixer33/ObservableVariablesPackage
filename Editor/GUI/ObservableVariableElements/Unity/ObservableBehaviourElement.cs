using ObservableVariables.Unity;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements.Unity
{
    public class ObservableBehaviourElement : ObservableVariableElementBase<ObservableBehaviour, Behaviour, ObjectField>
    {
        public ObservableBehaviourElement(ObservableBehaviour variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(ObjectField editField, Behaviour activeValue, EventCallback<ChangeEvent<Behaviour>> onInputValueChanged)
        {
            editField.value = activeValue;
            editField.RegisterValueChangedCallback(args =>
            {
                if (args.newValue != null && args.newValue is not Behaviour)
                {
                    Debug.LogWarning($"The value you are trying to set ({args.newValue}) is not a behaviour!");
                    return;
                }
                
                onInputValueChanged?.Invoke(
                    ChangeEvent<Behaviour>.GetPooled(args.previousValue as Behaviour, args.newValue as Behaviour));
            });
        }

        protected override Behaviour GetInputFieldValue(ObjectField inputField) => inputField.value as Behaviour;
        protected override void SetInputFieldValue(ObjectField inputField, Behaviour value) =>
            inputField.value = value;
    }
}