using ObservableVariables.BasicTypes;
using UnityEditor;

namespace ObservableVariables.Drawers.BasicTypes
{
    [CustomPropertyDrawer(typeof(ObservableInt))]
    public class ObservableIntDrawer : ObservableVariableDrawer<ObservableInt>
    {
        protected override string GetValueString(ObservableInt val) => val.Value.ToString();
    }
}