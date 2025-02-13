using System;
using ObservableVariables.Contract;
using UnityEngine;

namespace ObservableVariables.Unity
{
    [Serializable]
    public class ObservableTransform : ObservableVariableBase<Transform>
    {
        public ObservableTransform(Transform value, bool canBeModifiedInEditor = false) : base(value, canBeModifiedInEditor)
        {
        }
    }
}