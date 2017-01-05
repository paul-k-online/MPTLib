using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPT.PrimitiveType
{
    public static class DictionaryExtension
    {
        public static void AddRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (source == null)
                throw new ArgumentNullException("Source is null");

            if (collection == null)
                throw new ArgumentNullException("Collection is null");

            foreach (var item in collection)
            {
                source[item.Key] = item.Value;
                /*
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    ///
                }
                */
            }
        }
    }
}
