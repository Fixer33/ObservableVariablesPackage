using ObservableVariables;
using UnityEngine;

namespace Sample
{
    public class ExampleBehaviour1 : MonoBehaviour
    {
        private void Awake()
        {
            Variables.Get<ObservableCustomData>(ExampleKeys.Key1).Value = new CustomData()
            {
                SomeValue = "This is some value for key 1"
            };
        }
    }
}