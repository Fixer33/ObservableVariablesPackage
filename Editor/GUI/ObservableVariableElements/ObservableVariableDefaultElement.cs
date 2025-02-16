using ObservableVariables.Contract;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements
{
    public class ObservableVariableDefaultElement : ObservableVariableElementBase
    {
        public ObservableVariableDefaultElement(ObservableVariableBase variable, StyleSheet styleSheet, string variableName) : base(variable, styleSheet, variableName)
        {
            var label = new Label()
            {
                name = "observable-variable-element__value-edit-element"
            };
            this.Add(label);

            object value = variable.GetValue();
            if (value == null)
                label.text = "null";
            else
                label.text = value.ToString();
        }
    }
}