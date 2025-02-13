using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableFloat : ObservableVariableBase<float>
    {
        public ObservableFloat(float value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
        
        public static implicit operator ObservableFloat(float val) => new(val);
    }
}