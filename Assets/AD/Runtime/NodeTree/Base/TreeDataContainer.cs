using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.NodeTree
{
    [Serializable]
    public class TreeDataContainer : ISerializationCallbackReceiver
    {
        internal Container Data { get; } = new Container();


        [SerializeField] private List<SaveKey> keys = new List<SaveKey>();
        [SerializeField] private List<SaveValue> values = new List<SaveValue>();

        public bool ReadFromTree(Tree tree)
        {
            bool update = false;
            var removed = new HashSet<SaveKey>();
            var data = Data;
            foreach (var pair in data)
            {
                if (!tree.Data.ContainsKey(pair.Key.@group, pair.Key.key))
                    removed.Add(pair.Key);
            }

            foreach (var key in removed)
            {
                data.Remove(key);
                update = true;
            }

            foreach (var pair in tree.Data)
            {
                var saveKey = new SaveKey()
                {
                    @group = pair.Key.Group,
                    key = pair.Key.key
                };
                if (data.ContainsKey(saveKey)) continue;
                update = true;
                data[saveKey] = pair.Value;
            }

            return update;
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in Data)
            {
                keys.Add(pair.Key);
                values.Add(new SaveValue() {value = pair.Value});
            }
        }

        public void OnAfterDeserialize()
        {
            for (var i = values.Count - 1; i >= 0; i--)
                if (values[i].value == null)
                {
                    keys.RemoveAt(i);
                    values.RemoveAt(i);
                }

            Data.Clear();
            for (int i = 0; i < keys.Count; i++)
                Data[keys[i]] = values[i].value;
        }

        internal class Container : IDictionary<SaveKey, Tree.Value>
        {
            private readonly Dictionary<SaveKey, Tree.Value> _dict = new Dictionary<SaveKey, Tree.Value>();

            // IEnumerable<KeyValuePair<SaveKey, Tree.Value>>
            public IEnumerator<KeyValuePair<SaveKey, Tree.Value>> GetEnumerator() => _dict.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            // ICollection<KeyValuePair<SaveKey, Tree.Value>>
            public void Add(KeyValuePair<SaveKey, Tree.Value> item) => _dict.Add(item.Key, item.Value);
            public void Clear() => _dict.Clear();
            public bool Contains(KeyValuePair<SaveKey, Tree.Value> item) => _dict.Contains(item);

            public void CopyTo(KeyValuePair<SaveKey, Tree.Value>[] array, int arrayIndex) =>
                ((ICollection<KeyValuePair<SaveKey, Tree.Value>>) _dict).CopyTo(array, arrayIndex);

            public bool Remove(KeyValuePair<SaveKey, Tree.Value> item) =>
                ((ICollection<KeyValuePair<SaveKey, Tree.Value>>) _dict).Remove(item);

            public int Count => _dict.Count;
            public bool IsReadOnly => ((ICollection<KeyValuePair<SaveKey, Tree.Value>>) _dict).IsReadOnly;

            // IDictionary<SaveKey, Tree.Value>
            public void Add(SaveKey key, Tree.Value value) => _dict.Add(key, value);
            public bool ContainsKey(SaveKey key) => _dict.ContainsKey(key);
            public bool Remove(SaveKey key) => _dict.Remove(key);
            public bool TryGetValue(SaveKey key, out Tree.Value value) => _dict.TryGetValue(key, out value);

            public Tree.Value this[SaveKey key]
            {
                get => _dict[key];
                set => _dict[key] = value;
            }

            public ICollection<SaveKey> Keys => _dict.Keys;
            public ICollection<Tree.Value> Values => _dict.Values;

            // 扩展方法：支持通过 group 和 key 直接查询
            public bool ContainsKey(string group, string key)
            {
                var saveKey = new SaveKey {@group = group, key = key};
                return _dict.ContainsKey(saveKey);
            }

            public bool TryGetValue(string group, string key, out Tree.Value value)
            {
                var saveKey = new SaveKey {@group = group, key = key};
                return _dict.TryGetValue(saveKey, out value);
            }

            public Tree.Value GetValue(string group, string key)
            {
                var saveKey = new SaveKey {@group = group, key = key};
                return _dict[saveKey];
            }
        }

        #region save

        [Serializable]
        internal class SaveKey
        {
            public string group;
            public string key;

            private bool Equals(SaveKey other)
            {
                return @group == other.@group && key == other.key;
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is SaveKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((@group != null ? @group.GetHashCode() : 0) * 397) ^ (key != null ? key.GetHashCode() : 0);
                }
            }

            public override string ToString()
            {
                return $"{nameof(@group)}: {@group}, {nameof(key)}: {key}";
            }
        }

        [Serializable]
        internal class SaveValue : ISerializationCallbackReceiver
        {
            public string aqType;
            public string data;
            public Tree.Value value;

            public void OnBeforeSerialize()
            {
                aqType = value.GetType().AssemblyQualifiedName;
                data = JsonUtility.ToJson(value);
            }

            public void OnAfterDeserialize()
            {
                try
                {
                    value = (Tree.Value) JsonUtility.FromJson(data, Type.GetType(aqType));
                }
                catch
                {
                }
            }
        }

        #endregion
    }
}