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
                IObservableVariableEventHolder? eventHolder;
                if (_valueIsEventHolder)
                {
                    eventHolder = _value as IObservableVariableEventHolder;
                    eventHolder?.OnDataRemovedObservableVariable(this);
                }
                
                _value = value;

                eventHolder = value as IObservableVariableEventHolder;
                eventHolder?.OnDataStoredInObservableVariable(this);
                _valueIsEventHolder = eventHolder != null;
                
                ValueChanged?.Invoke(_value);
            }
        }

        private bool _valueIsEventHolder;
        private T _value;

        protected ObservableVariableBase() : base()
        {
            _value = default!;
            _valueIsEventHolder = _value is IObservableVariableEventHolder;
        }
        
        protected ObservableVariableBase(T value)
        {
            _value = value;
            _valueIsEventHolder = _value is IObservableVariableEventHolder;
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