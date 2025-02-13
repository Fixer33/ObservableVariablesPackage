using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableFloat : ObservableVariableBase<float>
    {
        public ObservableFloat() : base(){}
        
        public ObservableFloat(float value) : base(value)
        {
        }
        
        public static implicit operator ObservableFloat(float val) => new(val);
    }
}