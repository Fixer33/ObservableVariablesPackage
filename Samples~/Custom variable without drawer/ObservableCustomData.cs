using ObservableVariables.Contract;

namespace Sample
{
    public class ObservableCustomData : ObservableVariableBase<CustomData>
    {
        public ObservableCustomData() : base(){}
        public ObservableCustomData(CustomData data) : base(data){}
        
        public static implicit operator ObservableCustomData(CustomData val) => new(val);
    }
}