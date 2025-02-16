using ObservableVariables.Contract;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements
{
    public interface IObservableVariableElementFactoryOverride
    {
        public bool IsApplicableTo(ObservableVariableBase variable);

        public ObservableVariableElementBase CreateInputField(ObservableVariableBase variable,
            StyleSheet mainStyleSheet, string variableName);
    }
}