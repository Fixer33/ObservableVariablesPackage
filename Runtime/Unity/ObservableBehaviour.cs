using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableBehaviour : ObservableVariableBase<Behaviour>
    {
        public ObservableBehaviour(Behaviour value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
        
        public static implicit operator ObservableBehaviour(Behaviour val) => new(val);
    }
}