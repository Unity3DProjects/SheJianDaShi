using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AD.Tools
{
    [Serializable]
    public abstract class SerializableRefDictionary<TKey, TValue, TKeySave, TValueSave> :
        IDictionary<TKey, TValue>,
        ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKeySave> keys = new List<TKeySave>();

        [SerializeField] private List<TValueSave> values = new List<TValueSave>();

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();


        /// <summary>
        /// OnBeforeSerialize implementation.
        /// </summary>
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in dictionary)
            {
                keys.Add(SerializeKey(kvp.Key));
                values.Add(SerializeValue(kvp.Value));
            }
        }


        protected abstract TKeySave SerializeKey(TKey key);
        protected abstract bool TryDeserializeKey(TKeySave keySave, out TKey key);
        protected abstract TValueSave SerializeValue(TValue value);
        protected abstract bool TryDeserializeValue(TValueSave valueSave, out TValue value);

        /// <summary>
        /// OnAfterDeserialize implementation.
        /// </summary>
        public void OnAfterDeserialize()
        {
            dictionary.Clear();

            for (int i = 0; i < keys.Count; i++)
            {
                if (TryDeserializeKey(keys[i], out var key) && TryDeserializeValue(values[i], out var value))
                {
                    if (!dictionary.ContainsKey(key))
                        dictionary.Add(key, value);
                }
            }
        }


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.TryGetValue(item.Key, out TValue value) &&
                   EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < dictionary.Count)
                throw new ArgumentException("The array does not have enough space.");

            int i = arrayIndex;
            foreach (var kvp in dictionary)
            {
                array[i] = kvp;
                i++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                return dictionary.Remove(item.Key);
            }

            return false;
        }

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public ICollection<TKey> Keys => dictionary.Keys;

        public ICollection<TValue> Values => dictionary.Values;
    }
}