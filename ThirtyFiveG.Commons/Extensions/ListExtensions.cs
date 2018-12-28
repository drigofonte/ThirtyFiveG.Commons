using System;
using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ListExtensions
    {
        public static void DisposeAll<T>(this List<T> list)
            where T : class, IDisposable
        {
            if (list != null)
            {
                foreach (T t in list)
                    t.Dispose();
                list.Clear();
            }
        }
    }
}
