using ObservableVariables.Unity;
using UnityEditor;

namespace ObservableVariables.Drawers.Unity
{
    [CustomPropertyDrawer(typeof(ObservableVector2))]
    public class ObservableVector2Drawer : ObservableVariableDrawer<ObservableVector2>
    {
        protected override string GetValueString(ObservableVector2 val)
        {
            return val.Value.ToString();
        }
    }
}