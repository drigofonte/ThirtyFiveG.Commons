using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Add<K, V>(this IDictionary<K, List<V>> dictionary, K key, V value)
        {
            List<V> values;
            if (dictionary.TryGetValue(key, out values)) { values.Add(value); }
            else
            {
                values = new List<V>();
                values.Add(value);
                dictionary.Add(key, values);
            }
        }

        public static void Add<K, V>(this IDictionary<K, ICollection<V>> dictionary, K key, V value)
        {
            ICollection<V> values;
            if (dictionary.TryGetValue(key, out values))
            {
                values.Add(value);
            }
            else
            {
                values = new List<V>();
                values.Add(value);
                dictionary.Add(key, values);
            }
        }

        public static bool Add<K, V>(this IDictionary<K, ISet<V>> dictionary, K key, V value, IEqualityComparer<V> comparer = null)
        {
            ISet<V> values;
            bool added = false;
            if (dictionary.TryGetValue(key, out values))
            {
                added = values.Add(value);
            }
            else
            {
                if (comparer != null)
                    values = new HashSet<V>(comparer);
                else
                    values = new HashSet<V>();
                added = values.Add(value);
                dictionary.Add(key, values);
            }
            return added;
        }

        public static void ClearAll<K, V>(this IDictionary<K, ICollection<V>> dictionary)
        {
            foreach (ICollection<V> value in dictionary.Values)
                value.Clear();
            dictionary.Clear();
        }

        public static void ClearAll<K, V>(this IDictionary<K, List<V>> dictionary)
        {
            foreach (ICollection<V> value in dictionary.Values)
                value.Clear();
            dictionary.Clear();
        }

        public static void ClearAll<K1, K2, V>(this IDictionary<K1, IDictionary<K2, V>> dictionary)
        {
            foreach (KeyValuePair<K1, IDictionary<K2, V>> value in dictionary)
                value.Value.Clear();
            dictionary.Clear();
        }

        public static void Clear<K, V>(this IDictionary<K, ISet<V>> dictionary)
        {
            foreach (ICollection<V> value in dictionary.Values)
                value.Clear();
            dictionary.Clear();
        }

        public static void UnionWith<K, V>(this IDictionary<K, ISet<V>> dictionary, K key, IEnumerable<V> value)
        {
            ISet<V> values;
            if (dictionary.TryGetValue(key, out values)) { values.UnionWith(value); }
            else
            {
                values = new HashSet<V>();
                values.UnionWith(value);
                dictionary.Add(key, values);
            }
        }

        public static void Add<TOuter, TInner, TValue>(this IDictionary<TOuter, IDictionary<TInner, TValue>> dictionary, TOuter outerKey, TInner innerKey, TValue value, bool forceAdd = false)
        {
            IDictionary<TInner, TValue> innerDictionary;
            if (dictionary.TryGetValue(outerKey, out innerDictionary))
            {
                forceAdd = innerDictionary.ContainsKey(innerKey) && forceAdd;
                bool addValue = forceAdd || !innerDictionary.ContainsKey(innerKey);
                if (forceAdd)
                    innerDictionary.Remove(innerKey);

                if (addValue)
                    innerDictionary.Add(innerKey, value);
            }
            else
            {
                innerDictionary = new Dictionary<TInner, TValue>();
                innerDictionary.Add(innerKey, value);
                dictionary.Add(outerKey, innerDictionary);
            }
        }

        public static void Remove<TOuter, TInner, TValue>(this IDictionary<TOuter, IDictionary<TInner, TValue>> dictionary, TOuter outerKey, TInner innerKey)
        {
            IDictionary<TInner, TValue> innerDictionary;
            if (dictionary.TryGetValue(outerKey, out innerDictionary))
                innerDictionary.Remove(innerKey);
        }

        public static void Sum<TKey>(this IDictionary<TKey, long> dictionary, TKey key, long value)
        {
            long currentValue;
            if (!dictionary.TryGetValue(key, out currentValue))
            {
                currentValue = 0;
            }
            currentValue += value;
            dictionary.Remove(key);
            dictionary.Add(key, currentValue);
        }

        public static void Sum<TKey>(this IDictionary<TKey, short> dictionary, TKey key, short value)
        {
            short currentValue;
            if (!dictionary.TryGetValue(key, out currentValue))
            {
                currentValue = 0;
            }
            currentValue += value;
            dictionary.Remove(key);
            dictionary.Add(key, currentValue);
        }
    }
}
