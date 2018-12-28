using Newtonsoft.Json;
using System;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ClassExtensions
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.None, MissingMemberHandling = MissingMemberHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.All, TypeNameHandling = TypeNameHandling.None };

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }
            string json = JsonConvert.SerializeObject(source, _settings);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
