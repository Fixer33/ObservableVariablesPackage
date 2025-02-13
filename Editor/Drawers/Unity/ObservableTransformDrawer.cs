using ObservableVariables.Unity;
using UnityEditor;

namespace ObservableVariables.Drawers.Unity
{
    [CustomPropertyDrawer(typeof(ObservableTransform))]
    public class ObservableTransformDrawer : ObservableVariableDrawer<ObservableTransform>
    {
        protected override string GetValueString(ObservableTransform val)
        {
            if (val.Value == null)
                return "null";
            return val.Value.gameObject.name;
        }
    }
}