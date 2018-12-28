using System;
using System.Collections.ObjectModel;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void DisposeAll<T>(this ObservableCollection<T> collection)
            where T : class, IDisposable
        {
            if (collection != null)
            {
                foreach (T t in collection)
                    t.Dispose();
                collection.Clear();
            }
        }
    }
}
