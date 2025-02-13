using System;
using System.Collections.Generic;
using System.ComponentModel;
using ObservableVariables.Contract;

namespace ObservableVariables
{
    public static class Variables
    {
        private static Dictionary<Enum, ObservableVariableBase> _dictionary = new();
        
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

#if UNITY_INCLUDE_TESTS
        public static void Tests_Clear()
        {
            _dictionary.Clear();
        }
#endif
    }
}