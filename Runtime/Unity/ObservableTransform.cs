using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableTransform : ObservableVariableBase<Transform>
    {
        public ObservableTransform() : base(){}
        
        public ObservableTransform(Transform value) : base(value)
        {
        }
        
        public static implicit operator ObservableTransform(Transform val) => new(val);
    }
}