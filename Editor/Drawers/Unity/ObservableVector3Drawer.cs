using ObservableVariables.Unity;
using UnityEditor;

namespace ObservableVariables.Editor.Drawers.Unity
{
    [CustomPropertyDrawer(typeof(ObservableVector3))]
    public class ObservableVector3Drawer : ObservableVariableDrawer<ObservableVector3>
    {
        protected override string GetValueString(ObservableVector3 val)
        {
            return val.Value.ToString();
        }
    }
}