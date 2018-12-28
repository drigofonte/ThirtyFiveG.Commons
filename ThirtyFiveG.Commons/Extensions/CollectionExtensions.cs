using System;
using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class CollectionExtensions
    {
        public static void DisposeAll<T>(this ICollection<T> collection)
            where T : class, IDisposable
        {
            if (collection != null)
            {
                foreach (T t in collection)
                    t.Dispose();
                collection.Clear();
            }
        }

        public static void AddRange<T>(this ICollection<T> o, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                o.Add(item);
            }
        }
    }
}
