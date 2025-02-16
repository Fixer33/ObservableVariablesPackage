using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ObservableVariables.Contract;

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