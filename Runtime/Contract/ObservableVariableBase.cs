#nullable enable
using System;

namespace ObservableVariables.Contract
{
    [Serializable]
    public abstract class ObservableVariableBase<T> : ObservableVariableBase
    {
        public delegate void ValueChangedDelegate(T newValue);
        public event ValueChangedDelegate ValueChanged = delegate { };
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(_value);
            }
        }

        protected T _value;

        protected ObservableVariableBase(T value, bool canBeModifiedInEditor = false)
        {
            _value = value;
        }
    }
    
    public abstract class ObservableVariableBase{}
}