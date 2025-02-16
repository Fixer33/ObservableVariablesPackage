#if UNITY_EDITOR
using ObservableVariables.Contract;
using ObservableVariables.Editor.GUI.ObservableVariableElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sample
{
    public static class ObservableVariableOverrideInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            Debug.Log("123");
            ObservableVariableElementFactory.AddOverride(new ObservableCustomDataElementOverrideData());
        }
    }

    public record ObservableCustomDataElementOverrideData : IObservableVariableElementFactoryOverride
    {
        public bool IsApplicableTo(ObservableVariableBase variable)
        {
            return variable is ObservableCustomData;
        }

        public ObservableVariableElementBase CreateInputField(ObservableVariableBase variable, StyleSheet mainStyleSheet,
            string variableName)
        {
            return new ObservableCustomDataElement(variable as ObservableCustomData, mainStyleSheet, variableName);
        }
    }
    
    public class ObservableCustomDataElement : ObservableVariableElementBase<ObservableCustomData, CustomData, TextField>
    {
        private CustomData _data;
        
        public ObservableCustomDataElement(ObservableCustomData variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
        }

        protected override void Initialize(TextField editField, CustomData activeValue, EventCallback<ChangeEvent<CustomData>> onInputValueChanged)
        {
            _data = activeValue;
            editField.value = _data.SomeValue;
            editField.RegisterValueChangedCallback(args =>
            {
                _data.SomeValue = args.newValue;
                onInputValueChanged?.Invoke(ChangeEvent<CustomData>.GetPooled(_data, _data));
            });
        }

        protected override CustomData GetInputFieldValue(TextField inputField)
        {
            return new CustomData()
            {
                SomeValue = inputField.value
            };
        }

        protected override void SetInputFieldValue(TextField inputField, CustomData value) =>
            inputField.value = value.SomeValue;
    }
}
#endif