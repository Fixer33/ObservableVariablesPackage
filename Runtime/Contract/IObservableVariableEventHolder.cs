namespace ObservableVariables.Contract
{
    public interface IObservableVariableEventHolder<in TVariableType> : IObservableVariableEventHolder 
        where TVariableType : ObservableVariableBase
    {
        public void OnDataStoredInObservableVariable(TVariableType variable);
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

    public interface IObservableVariableEventHolder
    {
        public void OnDataStoredInObservableVariable(ObservableVariableBase variable);
        public void OnDataRemovedObservableVariable(ObservableVariableBase variable);
    }
}