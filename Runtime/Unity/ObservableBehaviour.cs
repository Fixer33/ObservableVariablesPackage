using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableBehaviour : ObservableVariableBase<Behaviour>
    {
        public ObservableBehaviour() : base(){}
        
        public ObservableBehaviour(Behaviour value) : base(value)
        {
        }
        
        public static implicit operator ObservableBehaviour(Behaviour val) => new(val);
    }
}