using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ObservableVariables.BasicTypes;
using ObservableVariables.Contract;
using ObservableVariables.Unity;

namespace ObservableVariables
{
    /// <summary>
    /// Holds all global observable variables inside
    /// </summary>
    public static class Variables
    {
        private static readonly Dictionary<Enum, ObservableVariableBase> _dictionary = new();
        
        /// <summary>
        /// Retrieves observable variable of type T by supplied key. If value does not exist - creates new
        /// </summary>
        /// <param name="key">Key for the value. Can be any enum</param>
        /// <typeparam name="T">Type of observable value. For example: ObservableBool</typeparam>
        /// <returns>Observable variable registered with supplied key</returns>
        /// <exception cref="InvalidEnumArgumentException">Thrown if supplied key was already used with another variable type</exception>
        public static T Get<T>(Enum key) where T : ObservableVariableBase, new()
        {
            ObservableVariableBase val = null;
            if (_dictionary.TryGetValue(key, out val) == false)
            {
                val = new T();
                _dictionary.Add(key, val);
            }

            if (val is not T castedVal)
                throw new InvalidEnumArgumentException("This key is already used for another type");

            return castedVal;
        }

        #region Basic types shortcut methods

        public static ObservableFloat GetFloat(Enum key) => Get<ObservableFloat>(key);
        public static ObservableInt GetInt(Enum key) => Get<ObservableInt>(key);
        public static ObservableBool GetBool(Enum key) => Get<ObservableBool>(key);
        public static ObservableString GetString(Enum key) => Get<ObservableString>(key);

        #endregion

        #region Unity observable types shortcut methods

        public static ObservableGameObject GetGameObject(Enum key) => Get<ObservableGameObject>(key);
        public static ObservableBehaviour GetBehaviour(Enum key) => Get<ObservableBehaviour>(key);
        public static ObservableTransform GetTransform(Enum key) => Get<ObservableTransform>(key);
        public static ObservableVector2 GetVector2(Enum key) => Get<ObservableVector2>(key);
        public static ObservableVector3 GetVector3(Enum key) => Get<ObservableVector3>(key);

        #endregion
        
#if UNITY_EDITOR
        /// <summary>
        /// Editor only!
        /// </summary>
        /// <param name="type">Observable variable type</param>
        /// <returns>All keys that hold supplied variable type</returns>
        public static Enum[] Editor_GetAllKeysForType(Type type) =>
            (from dictRecord in _dictionary where dictRecord.Value.GetType() == type select dictRecord.Key).ToArray();

        /// <summary>
        /// Get variable base by key
        /// </summary>
        /// <param name="key">Variable key</param>
        /// <returns>Variable base or NULL, if nothing exists</returns>
        public static ObservableVariableBase Editor_GetExisting(Enum key) =>
            _dictionary.GetValueOrDefault(key);
#endif

#if UNITY_INCLUDE_TESTS
        public static void Tests_Clear()
        {
            _dictionary.Clear();
        }
#endif
    }
}