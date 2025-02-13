using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableBool : ObservableVariableBase<bool>
    {
        public ObservableBool() : base() {}
        
        public ObservableBool(bool value) : base(value)
        {
        }
        
        public static implicit operator ObservableBool(bool val) => new(val);
    }
}