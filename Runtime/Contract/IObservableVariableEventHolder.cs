namespace ObservableVariables.Contract
{
    /// <summary>
    /// Interface for objects that can receive events when they are stored in or removed from an <see cref="ObservableVariableBase"/>.
    /// </summary>
    /// <typeparam name="TVariableType">The specific type of observable variable.</typeparam>
    public interface IObservableVariableEventHolder<in TVariableType> : IObservableVariableEventHolder 
        where TVariableType : ObservableVariableBase
    {
        /// <summary>
        /// Called when the data is stored in an observable variable.
        /// </summary>
        /// <param name="variable">The variable instance.</param>
        public void OnDataStoredInObservableVariable(TVariableType variable);

        /// <summary>
        /// Called when the data is removed from an observable variable.
        /// </summary>
        /// <param name="variable">The variable instance.</param>
        public void OnDataRemovedObservableVariable(TVariableType variable);

        void IObservableVariableEventHolder.OnDataRemovedObservableVariable(ObservableVariableBase variable)
        {
            if (variable is TVariableType castedVariable)
                OnDataRemovedObservableVariable(castedVariable);
        }

        void IObservableVariableEventHolder.OnDataStoredInObservableVariable(ObservableVariableBase variable)
        {
            if (variable is TVariableType castedVariable)
                OnDataStoredInObservableVariable(castedVariable);
        }
    }

    /// <summary>
    /// Base interface for observable variable event holders.
    /// </summary>
    public interface IObservableVariableEventHolder
    {
        /// <summary>
        /// Called when the data is stored in an observable variable.
        /// </summary>
        /// <param name="variable">The variable instance.</param>
        public void OnDataStoredInObservableVariable(ObservableVariableBase variable);

        /// <summary>
        /// Called when the data is removed from an observable variable.
        /// </summary>
        /// <param name="variable">The variable instance.</param>
        public void OnDataRemovedObservableVariable(ObservableVariableBase variable);
    }
}