using System.Net;
using System.Linq;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class WebHeaderCollectionExtensions
    {
        public static bool TryGetValue(this WebHeaderCollection headerCollection, string key, out string header)
        {
            header = null;
            bool result = headerCollection.AllKeys.Contains(key);
            if (result == true)
            {
                header = headerCollection[key];
            }
            return result;
        }

        public static void Add(this WebHeaderCollection headerCollection, string key, string value)
        {
            headerCollection[key] = value;
        }
    }
}
