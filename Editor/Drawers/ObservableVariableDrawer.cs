using System.Reflection;
using ObservableVariables.Contract;
using UnityEditor;
using UnityEngine;

namespace ObservableVariables.Editor.Drawers
{
    /// <summary>
    /// A base class for drawing serialized field of type ObservableVariable
    /// </summary>
    /// <typeparam name="T">Observable variable type</typeparam>
    [CustomPropertyDrawer(typeof(ObservableVariableBase<>))]
    public abstract class ObservableVariableDrawer<T> : PropertyDrawer where T : ObservableVariableBase
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            object targetObject = GetTargetObjectOfProperty(property);
            FieldInfo valueField = targetObject?.GetType().GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
            object value = valueField?.GetValue(targetObject);
            
            if (Application.isPlaying == false)
            {
                EditorGUI.LabelField(position, label.text);
            }
            else
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
                Rect fieldRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);
                
                EditorGUI.LabelField(labelRect, label.text);

                if (targetObject is not T castedVal)
                {
                    EditorGUI.LabelField(fieldRect, "Failed to get value");
                }
                else
                {
                    EditorGUI.LabelField(fieldRect, GetValueString(castedVal));
                }
            }
            
            EditorGUI.EndProperty();
        }
        
        private object GetTargetObjectOfProperty(SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string[] path = property.propertyPath.Split('.');

            foreach (var part in path)
            {
                FieldInfo field = obj.GetType().GetField(part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field == null) return null;
                obj = field.GetValue(obj);
            }

            return obj;
        }

        protected abstract string GetValueString(T val);
    }
}