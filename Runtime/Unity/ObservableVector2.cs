using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableVector2 : ObservableVariableBase<Vector2>
    {
        public ObservableVector2(Vector2 value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
        
        public static implicit operator ObservableVector2(Vector2 val) => new(val);
    }
}