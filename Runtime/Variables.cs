using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ObservableVariables.Contract;

namespace ObservableVariables
{
    public static class Variables
    {
        private static readonly Dictionary<Enum, ObservableVariableBase> _dictionary = new();
        
        public static T Get<T>(Enum key) where T : ObservableVariableBase, new()
        {
            ObservableVariableBase val = null;
            if (_dictionary.TryGetValue(key, out val) == false)
            {
                val = new T();
                _dictionary.Add(key, val);
            }

            if (val is not T castedVal)
                throw new InvalidEnumArgumentException("Wrong key or value type");

            return castedVal;
        }
        
#if UNITY_EDITOR
        public static Enum[] Editor_GetAllKeysForType(Type type) =>
            (from dictRecord in _dictionary where dictRecord.Value.GetType() == type select dictRecord.Key).ToArray();

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