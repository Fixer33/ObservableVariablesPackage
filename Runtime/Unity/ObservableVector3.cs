using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableVector3 : ObservableVariableBase<Vector3>
    {
        public ObservableVector3() : base(){}
        
        public ObservableVector3(Vector3 value) : base(value)
        {
        }
        
        public static implicit operator ObservableVector3(Vector3 val) => new(val);
    }
}