using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableInt : ObservableVariableBase<int>
    {
        public ObservableInt(int value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
        
        public static implicit operator ObservableInt(int val) => new(val);
    }
}