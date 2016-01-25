using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoSystem.UI.Tools.Model
{
    /// <summary>
    /// Container with extenction methods of Enumerable.
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        /// For-each loop for collections.
        /// For mutable collections use conversion to immutable collection.
        /// </summary>
        /// <typeparam name="T">Specialization item type. </typeparam>
        /// <param name="items">Collection. </param>
        /// <param name="action">Loop body delegate. </param>
        public static void ForEachEx<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// For-each loop for collections with index.
        /// For mutable collections use conversion to immutable collection.
        /// </summary>
        /// <typeparam name="T">Specialization item type. </typeparam>
        /// <param name="items">Collection. </param>
        /// <param name="action">Loop body delegate. </param>
        public static void ForEachEx<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            foreach (var record in items.Select((val, i) => new {Value = val, Index = i}))
            {
                action(record.Value, record.Index);
            }
        }

    }
}
