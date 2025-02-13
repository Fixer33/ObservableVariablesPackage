using ObservableVariables.Unity;
using UnityEditor;

namespace ObservableVariables.Drawers.Unity
{
    [CustomPropertyDrawer(typeof(ObservableBehaviour))]
    public class ObservableBehaviourDrawer : ObservableVariableDrawer<ObservableBehaviour>
    {
        protected override string GetValueString(ObservableBehaviour val)
        {
            return val.Value == null ? "null" : val.Value.ToString();
        }
    }
}