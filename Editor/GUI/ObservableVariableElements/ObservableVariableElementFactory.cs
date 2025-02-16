using System;
using System.Collections.Generic;
using ObservableVariables.BasicTypes;
using ObservableVariables.Contract;
using ObservableVariables.Editor.GUI.ObservableVariableElements.BasicTypes;
using ObservableVariables.Editor.GUI.ObservableVariableElements.Unity;
using ObservableVariables.Unity;
using UnityEditor;
using UnityEngine.UIElements;

namespace ObservableVariables.Editor.GUI.ObservableVariableElements
{
    public static class ObservableVariableElementFactory
    {
        private static IList<IObservableVariableElementFactoryOverride> _overrides = new List<IObservableVariableElementFactoryOverride>();
        
        public static ObservableVariableElementBase Create(ObservableVariableBase variable, Enum key)
        {
            string loadPath = "Packages/com.fixer33.observable-variables/Editor/GUI/Markup and stylesheets/ObservableVariableElement.uss";
#if PACKAGES_DEV
            loadPath = "Assets/" + loadPath;
#endif
            string variableName = $"{key.GetType().Name}.{key.ToString()}";
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(loadPath);
            
            foreach (var @override in _overrides)
            {
                if (@override.IsApplicableTo(variable))
                    return @override.CreateInputField(variable, styleSheet, variableName);
            }
            
            switch (variable)
            {
                case ObservableFloat observableFloat:
                    return new ObservableFloatElement(observableFloat, styleSheet, variableName);
                case ObservableInt observableInt:
                    return new ObservableIntElement(observableInt, styleSheet, variableName);
                case ObservableBool observableBool:
                    return new ObservableBoolElement(observableBool, styleSheet, variableName);
                case ObservableString observableString:
                    return new ObservableStringElement(observableString, styleSheet, variableName);
                case ObservableBehaviour observableBehaviour:
                    return new ObservableBehaviourElement(observableBehaviour, styleSheet, variableName);
                case ObservableGameObject observableGameObject:
                    return new ObservableGameObjectElement(observableGameObject, styleSheet, variableName);
                case ObservableTransform observableTransform:
                    return new ObservableTransformElement(observableTransform, styleSheet, variableName);
                case ObservableVector2 observableVector2:
                    return new ObservableVector2Element(observableVector2, styleSheet, variableName);
                case ObservableVector3 observableVector3:
                    return new ObservableVector3Element(observableVector3, styleSheet, variableName);
            }

            return new ObservableVariableDefaultElement(variable, styleSheet, variableName);
        }

        public static void AddOverride(IObservableVariableElementFactoryOverride @override)
        {
            if (HasOverride(@override) == false)
                _overrides.Add(@override);
        }

        public static void RemoveOverride(IObservableVariableElementFactoryOverride @override)
        {
            if (HasOverride(@override))
                _overrides.Remove(@override);
        }

        public static bool HasOverride(IObservableVariableElementFactoryOverride @override) =>
            _overrides.Contains(@override);
    }
}