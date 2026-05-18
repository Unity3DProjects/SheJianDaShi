using System;
using System.Collections;
using System.Collections.Generic;
using AD.Tools;

namespace AD.NodeTree
{
    public sealed class Tree : IADInit<TreeDataContainer>
    {
        private const char GroupSeparator = '/';

        private static string NormalizeGroupPath(string groupPath)
        {
            if (string.IsNullOrWhiteSpace(groupPath)) return Key.NoGroup;

            var split = groupPath.Split(new[] {GroupSeparator}, StringSplitOptions.RemoveEmptyEntries);
            return split.Length == 0 ? Key.NoGroup : string.Join(GroupSeparator.ToString(), split);
        }

        private static string CombineGroupPath(string parentGroupPath, string groupPath)
        {
            parentGroupPath = NormalizeGroupPath(parentGroupPath);
            groupPath = NormalizeGroupPath(groupPath);

            if (parentGroupPath == Key.NoGroup) return groupPath;
            if (groupPath == Key.NoGroup) return parentGroupPath;
            return $"{parentGroupPath}{GroupSeparator}{groupPath}";
        }

        public sealed class Key
        {
            public const string NoGroup = "";
            private string group = NoGroup;
            public readonly string key;
            public readonly string description;

            public string Group
            {
                get => string.IsNullOrEmpty(@group) ? NoGroup : @group;
                set => @group = NormalizeGroupPath(value);
            }

            public Key(string key)
            {
                this.key = key;
                this.description = key;
            }

            public Key(string key, string description)
            {
                this.key = key;
                this.description = description;
            }

            private bool Equals(Key other)
            {
                return Group == other.Group && key == other.key;
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is Key other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Group != null ? Group.GetHashCode() : 0) * 397) ^ (key != null ? key.GetHashCode() : 0);
                }
            }
        }

        [Serializable]
        public abstract class Value
        {
            public virtual bool Resettable => false;

            public virtual void Reset()
            {
            }

            public abstract void Load(Value other);
        }

        public class TreeData : IDictionary<Key, Value>
        {
            private readonly IDictionary<string, List<Key>> group2Keys;
            private readonly IDictionary<string, List<Key>> stringKey2Keys;
            private readonly IDictionary<Key, Value> dict;
            private Group group;

            public TreeData()
            {
                group2Keys = new Dictionary<string, List<Key>>();
                stringKey2Keys = new Dictionary<string, List<Key>>();
                dict = new Dictionary<Key, Value>();
            }  

            // IEnumerable<KeyValuePair<Key, Value>>
            public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
            {
                return dict.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            // ICollection<KeyValuePair<Key, Value>>
            public void Add(KeyValuePair<Key, Value> item) => Add(item.Key, item.Value);

            public void Clear()
            {
                group2Keys.Clear();
                stringKey2Keys.Clear();
                dict.Clear();
            }

            public bool Contains(KeyValuePair<Key, Value> item)
            {
                return ContainsKey(item.Key);
            }

            public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
            {
                dict.CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<Key, Value> item)
            {
                return Remove(item.Key);
            }

            public int Count
            {
                get { return dict.Count; }
            }

            public bool IsReadOnly
            {
                get { return dict.IsReadOnly; }
            }

            // IDictionary<Key, Value>
            public void Add(Key key, Value value)
            {
                key.Group = @group?.fullGroupName;
                if (dict.ContainsKey(key)) return;
                group2Keys.AddItem(key.Group, key);
                stringKey2Keys.AddItem(key.key, key);
                dict.Add(key, value);
            }

            public bool ContainsKey(Key key)
            {
                return dict.ContainsKey(key);
            }

            public bool ContainsKey(string groupName, string key)
            {
                return TryGetValue(groupName, key, out _);
            }

            public bool Remove(Key key)
            {
                if (!dict.ContainsKey(key)) return false;

                // 从主字典中移除
                dict.Remove(key);

                // 从分组集合中移除
                if (group2Keys.TryGetValue(key.Group, out var keys))
                {
                    keys.Remove(key);
                    if (keys.Count == 0)
                        group2Keys.Remove(key.Group);
                }

                // 从字符串映射中移除
                if (stringKey2Keys.TryGetValue(key.key, out var strKeys))
                {
                    strKeys.Remove(key);
                    if (strKeys.Count == 0)
                        stringKey2Keys.Remove(key.key);
                }

                return true;
            }

            public bool Remove(string groupName, string key)
            {
                groupName = NormalizeGroupPath(groupName);
                if (stringKey2Keys.TryGetValue(key, out var keys))
                {
                    foreach (var k in keys)
                    {
                        if (k.Group == groupName)
                        {
                            return Remove(k);
                        }
                    }
                }

                return false;
            }


            public bool TryGetValue(Key key, out Value value)
            {
                return dict.TryGetValue(key, out value);
            }

            public bool TryGetValue(string groupName, string key, out Value value)
            {
                groupName = NormalizeGroupPath(groupName);
                if (stringKey2Keys.TryGetValue(key, out var keys))
                {
                    foreach (var k in keys)
                    {
                        if (k.Group == groupName)
                        {
                            return dict.TryGetValue(k, out value);
                        }
                    }
                }

                value = default(Value);
                return false;
            }

            public bool TryGetGroup(string groupName, out List<Key> keys)
            {
                groupName = NormalizeGroupPath(groupName);
                return group2Keys.TryGetValue(groupName, out keys);
            }

            public IDictionary<string, List<Key>> Group2Keys => group2Keys;

            public IEnumerable<Key> NoGroupKeys => group2Keys.TryGetValue(Key.NoGroup, out var value) ? value : new List<Key>();

            public Value this[Key key]
            {
                get => dict[key];
                set => dict[key] = value;
            }

            public ICollection<Key> Keys => dict.Keys;

            public ICollection<Value> Values => dict.Values;

            public Group CreateGroup(string groupName)
            {
                return new Group(groupName, this, @group);
            }

            public class Group : IDisposable
            {
                internal string groupName;
                internal string fullGroupName;
                private Group parent;
                private TreeData data;

                internal Group(string groupName, TreeData data, Group parent)
                {
                    this.groupName = NormalizeGroupPath(groupName);
                    this.fullGroupName = CombineGroupPath(parent?.fullGroupName, this.groupName);
                    this.parent = parent;
                    this.data = data;
                    data.@group = this;
                }

                public void Dispose()
                {
                    if (data.@group != this)
                        throw new Exception("Invalid Group");
                    data.@group = parent;
                }
            }
        }

        public TreeData Data { get; }
        private INode Root { get; }

        public Tree(INode root)
        {
            Data = new TreeData();
            Root = root;
            Root.InitTree(this);
        }

        public void Init(TreeDataContainer dataContainer)
        {
            if (dataContainer != null)
            {
                foreach (var pair in dataContainer.Data)
                {
                    if (Data.TryGetValue(pair.Key.@group, pair.Key.key, out var value))
                    {
                        value.Load(pair.Value);
                    }
                }
            }

            Root.Init();
        }

        public void Call(AdSourceType type) => Root.Call(type);
    }
}
