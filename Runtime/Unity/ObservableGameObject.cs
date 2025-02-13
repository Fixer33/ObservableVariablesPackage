using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableGameObject : ObservableVariableBase<GameObject>
    {
        public ObservableGameObject() : base(){}
        
        public ObservableGameObject(GameObject value) : base(value)
        {
        }

        public static implicit operator ObservableGameObject(GameObject go) => new(go);
    }
}