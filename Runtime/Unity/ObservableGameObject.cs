using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableGameObject : ObservableVariableBase<GameObject>
    {
        public ObservableGameObject(GameObject value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }

        public static implicit operator ObservableGameObject(GameObject go) => new(go);
    }
}