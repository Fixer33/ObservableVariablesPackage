using ObservableVariables.Unity;
using UnityEditor;

namespace ObservableVariables.Drawers.Unity
{
    [CustomPropertyDrawer(typeof(ObservableGameObject))]
    public class ObservableGameObjectDrawer : ObservableVariableDrawer<ObservableGameObject>
    {
        protected override string GetValueString(ObservableGameObject val)
        {
            if (val.Value == null)
                return "null";

            if (val.Value == false)
                return "[Destroyed] " + val.Value.name;

            return val.Value.name;
        }
    }
}