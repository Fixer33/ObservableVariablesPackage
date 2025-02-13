using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableVector2 : ObservableVariableBase<Vector2>
    {
        public ObservableVector2() : base(){}
        
        public ObservableVector2(Vector2 value) : base(value)
        {
        }
        
        public static implicit operator ObservableVector2(Vector2 val) => new(val);
    }
}