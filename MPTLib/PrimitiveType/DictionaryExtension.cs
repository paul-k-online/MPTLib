using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.PrimitiveType
{
    public static class DictionaryExtension
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (key == null)
                return defaultValue;
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static void AddRangeWithUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> collection)
        {
            if (source == null)
                throw new ArgumentNullException("Source is null");

            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source[item.Key] = item.Value;
            }
        }
    }
}
