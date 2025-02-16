using ObservableVariables;
using UnityEngine;

namespace Sample
{
    public class ExampleBehaviour2 : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log(Variables.Get<ObservableCustomData>(ExampleKeys.Key1).Value);
            Debug.Log(Variables.Get<ObservableCustomData>(ExampleKeys.Key2).Value);
        }
    }
}