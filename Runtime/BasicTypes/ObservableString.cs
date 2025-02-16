using System;
using ObservableVariables.Contract;

namespace ObservableVariables.BasicTypes
{
    [Serializable]
    public class ObservableString : ObservableVariableBase<string>
    {
        public ObservableString() : base() {}
        
        public ObservableString(string value) : base(value)
        {
        }
        
        public static implicit operator ObservableString(string val) => new(val);
    }
}