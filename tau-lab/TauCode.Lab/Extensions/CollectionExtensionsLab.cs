using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Lab.Extensions
{
    public static class CollectionExtensionsLab
    {
        public static List<T> ToSortedList<T, TProperty>(this IEnumerable<T> collection, Func<T, TProperty> selector, IComparer<TProperty> comparer = null)
        {
            return collection
                .OrderBy(selector, comparer ?? Comparer<TProperty>.Default)
                .ToList();
        }

        public static bool IsEquivalentToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary1,
            IEnumerable<KeyValuePair<TKey, TValue>> dictionary2)
        {
            // todo: check not null, both are indeed Dictionary<TKey, TValue>

            var realDictionary1 = (Dictionary<TKey, TValue>)dictionary1;
            var realDictionary2 = (Dictionary<TKey, TValue>)dictionary2;

            if (realDictionary1.Count != realDictionary2.Count)
            {
                return false;
            }

            foreach (var pair1 in dictionary1)
            {
                var key1 = pair1.Key;
                var value1 = pair1.Value;

                var exists2 = realDictionary2.TryGetValue(key1, out var value2);
                if (!exists2)
                {
                    return false;
                }

                if (!Equals(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
