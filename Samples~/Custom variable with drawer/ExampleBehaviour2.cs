using ObservableVariables;
using UnityEngine;

namespace Sample
{
    public class ExampleBehaviour2 : MonoBehaviour
    {
        public bool LogKey1ValueInUpdate;
        
        private void Start()
        {
            Debug.Log(Variables.Get<ObservableCustomData>(ExampleKeys.Key1).Value);
            Debug.Log(Variables.Get<ObservableCustomData>(ExampleKeys.Key2).Value);
        }

        private void Update()
        {
            if (LogKey1ValueInUpdate)
                Debug.Log(Variables.Get<ObservableCustomData>(ExampleKeys.Key1).Value);
        }
    }
}