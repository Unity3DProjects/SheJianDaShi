using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AD.Tools
{
    public class LRU<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private class Node
        {
            public Node pre, next;
            public TKey key;
            public TValue value;

            public Node(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }

            public Node(KeyValuePair<TKey, TValue> item)
            {
                this.key = item.Key;
                this.value = item.Value;
            }
        }

        private int limitCount;
        private Dictionary<TKey, Node> dictionary;
        private Node start, end;

        public LRU()
        {
            start = new Node(default, default);
            end = new Node(default, default);
            start.next = end;
            end.pre = start;
            dictionary = new Dictionary<TKey, Node>();
        }

        public LRU(int limitCount) : this()
        {
            this.limitCount = limitCount;
        }

        public int LimitCount
        {
            get => Math.Max(limitCount, 0);
            set
            {
                limitCount = Math.Max(value, 0);
                Update();
            }
        }


        private void Update()
        {
            while (Count > LimitCount)
                Remove(start.next.key);
        }

        private void RemoveNode(Node node)
        {
            node.pre.next = node.next;
            node.next.pre = node.pre;
        }

        private void AddAtLastNode(Node node)
        {
            node.pre = end.pre;
            end.pre.next = node;
            node.next = end;
            end.pre = node;
        }

        private void UpdateNode(Node node)
        {
            if (node.next == end) return;
            RemoveNode(node);
            AddAtLastNode(node);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var t = start.next;
            while (t != end)
            {
                yield return new KeyValuePair<TKey, TValue>(t.key, t.value);
                t = t.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        public void Clear()
        {
            dictionary.Clear();
            start.next = end;
            end.pre = start;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new IndexOutOfRangeException($"{nameof(arrayIndex)} : {arrayIndex}");
            if (array.Length - arrayIndex < this.Count)
                throw new ArgumentException($"{nameof(array)} too small");
            var t = start.next;
            while (t != end)
            {
                array[arrayIndex++] = new KeyValuePair<TKey, TValue>(t.key, t.value);
                t = t.next;
            }
        }


        public void Add(TKey key, TValue value)
        {
            if (!dictionary.TryGetValue(key, out var node))
            {
                node = new Node(key, value);
                dictionary.Add(key, node);
                AddAtLastNode(node);
            }
            else
            {
                node.value = value;
                UpdateNode(node);
            }

            Update();
        }

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (!dictionary.TryGetValue(key, out var node)) return false;
            dictionary.Remove(key);
            RemoveNode(node);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (dictionary.TryGetValue(key, out var node))
            {
                value = node.value;
                UpdateNode(node);
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[TKey key]
        {
            get => TryGetValue(key, out var value) ? value : default;
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values.Select(e => e.value).ToList();

        public override string ToString()
        {
            var builder = new StringBuilder();
            bool first = true; 
            foreach (var pair in this)
            {
                if (!first) builder.Append("\n");
                builder.Append(pair);
                first = false;
            }
            return builder.ToString();
        }
    }
}