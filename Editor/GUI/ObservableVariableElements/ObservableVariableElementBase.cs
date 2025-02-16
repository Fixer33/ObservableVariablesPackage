using ObservableVariables.Contract;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements
{
    public abstract class ObservableVariableElementBase : VisualElement
    {
        protected ObservableVariableElementBase(ObservableVariableBase variable, StyleSheet styleSheet, string variableName)
        {
            this.styleSheets.Add(styleSheet);
            
            this.Add(new Label()
            {
                name = "observable-variable-element__name",
                text = variableName
            });
        }
    }
    
    /// <summary>
    /// Element vase for drawing and editing element value
    /// </summary>
    /// <typeparam name="T1">Observable variable type</typeparam>
    /// <typeparam name="T2">Observable variable value type</typeparam>
    /// <typeparam name="T3">Input field element type</typeparam>
    public abstract class ObservableVariableElementBase<T1, T2, T3> : ObservableVariableElementBase 
        where T1 : ObservableVariableBase<T2>
        where T3 : VisualElement, new()
    {
        protected T1 Variable { get; private set; }
        protected T3 InputField { get; private set; }
        
        protected ObservableVariableElementBase(T1 variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
            Variable = variable;

            InputField = new T3
            {
                name = "observable-variable-element__value-edit-element"
            };

            Variable.ValueChanged += OnVariableValueChanged;
            
            Initialize(InputField, Variable.Value, OnInputValueChanged);
            this.Add(InputField);
        }

        ~ObservableVariableElementBase()
        {
            Variable.ValueChanged -= OnVariableValueChanged;
        }

        private void OnVariableValueChanged(T2 newValue)
        {
            var value = GetInputFieldValue(InputField);
            if (value == null && Variable.Value == null)
                return;
            
            if (value is not null && value.Equals(Variable.Value))
                return;
            
            SetInputFieldValue(InputField, newValue);
        }

        private void OnInputValueChanged(ChangeEvent<T2> value)
        {
            // T2 value = GetInputFieldValue(InputField);
            if (value.newValue == null && Variable.Value == null)
                return;
            
            if (value.newValue is not null && value.newValue.Equals(Variable.Value))
                return;

            Variable.Value = value.newValue;
        }
        
        protected abstract void Initialize(T3 editField, T2 activeValue, EventCallback<ChangeEvent<T2>> onInputValueChanged);
        protected abstract T2 GetInputFieldValue(T3 inputField);
        protected abstract void SetInputFieldValue(T3 inputField, T2 value);
    }
}