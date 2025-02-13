using ObservableVariables.BasicTypes;
using UnityEditor;

namespace ObservableVariables.Drawers.BasicTypes
{
    [CustomPropertyDrawer(typeof(ObservableInt))]
    public class ObservableBoolDrawer : ObservableVariableDrawer<ObservableBool>
    {
        protected override string GetValueString(ObservableBool val) => val.Value.ToString();
    }
}

