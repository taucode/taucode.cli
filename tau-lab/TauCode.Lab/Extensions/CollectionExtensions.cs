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
    }
}
