using System.Globalization;
using ObservableVariables.BasicTypes;
using ObservableVariables.Editor.Drawers;
using UnityEditor;

namespace ObservableVariables.Drawers.BasicTypes
{
    [CustomPropertyDrawer(typeof(ObservableFloat))]
    public class ObservableFloatDrawer : ObservableVariableDrawer<ObservableFloat>
    {
        protected override string GetValueString(ObservableFloat val) => val.Value.ToString(CultureInfo.InvariantCulture);
    }
}