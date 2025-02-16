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

        protected ObservableVariableBase()
        {
            _value = default!;
        }
        
        protected ObservableVariableBase(T value)
        {
            _value = value;
        }

        public sealed override object GetValue() => Value;

        public static implicit operator T(ObservableVariableBase<T> val) => val.Value;
    }

    public abstract class ObservableVariableBase
    {
        protected ObservableVariableBase(){}

        public abstract object GetValue();
    }
}