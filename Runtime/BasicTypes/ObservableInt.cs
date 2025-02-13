using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableInt : ObservableVariableBase<int>
    {
        public ObservableInt() : base(){}
        
        public ObservableInt(int value) : base(value)
        {
        }
        
        public static implicit operator ObservableInt(int val) => new(val);
    }
}