using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class DictionaryExtensions
    {
        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue) 
            where K : notnull
        {
            if (dictionary.TryGetValue(key, out var val))
            {
                return val;
            }

            return defaultValue;
        }

        public static V ConstructDefaultValue<K, V>(this Dictionary<K, V> dictionary, K key)
            where K : notnull 
            where V : new()
        {
            if (dictionary.TryGetValue(key, out var val))
            {
                return val;
            }

            var d = new V();
            dictionary[key] = d;
            return d;
        }
    }
}
