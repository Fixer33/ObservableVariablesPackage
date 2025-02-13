using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableBool : ObservableVariableBase<bool>
    {
        public ObservableBool(bool value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
        
        public static implicit operator ObservableBool(bool val) => new(val);
    }
}