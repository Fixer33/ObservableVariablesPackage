#nullable enable
using System;

namespace ObservableVariables.Contract
{
    /// <summary>
    /// Base class for observable variables with value change notification.
    /// </summary>
    /// <typeparam name="T">The type of the value being observed.</typeparam>
    [Serializable]
    public abstract class ObservableVariableBase<T> : ObservableVariableBase
    {
        public delegate void ValueChangedDelegate(T newValue);
        
        /// <summary>
        /// Event triggered when the value of the variable changes.
        /// </summary>
        public event ValueChangedDelegate ValueChanged = delegate { };
        
        /// <summary>
        /// The current value of the variable. Triggers <see cref="ValueChanged"/> when modified.
        /// Also handles <see cref="IObservableVariableEventHolder"/> notifications.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(_value, value))
                    return;

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