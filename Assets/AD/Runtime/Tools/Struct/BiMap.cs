using System;
using System.Collections;
using System.Collections.Generic;

namespace AD.Tools
{
    /// <summary>
    /// 双向映射（BiMap）数据结构
    /// 提供键到值和值到键的双向查找
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class BiMap<TKey, TValue> : 
        IDictionary<TKey, TValue>, 
        IReadOnlyDictionary<TKey, TValue>
    {
        #region 内部存储

        private readonly Dictionary<TKey, TValue> _forwardDictionary;
        private readonly Dictionary<TValue, TKey> _reverseDictionary;
        private readonly IEqualityComparer<TKey> _keyComparer;
        private readonly IEqualityComparer<TValue> _valueComparer;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个空的BiMap
        /// </summary>
        public BiMap() : this(0)
        {
        }

        /// <summary>
        /// 使用指定的键和值比较器初始化BiMap
        /// </summary>
        public BiMap(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(0, keyComparer, valueComparer)
        {
        }

        /// <summary>
        /// 使用指定容量和比较器初始化BiMap
        /// </summary>
        public BiMap(int capacity, IEqualityComparer<TKey> keyComparer = null,
            IEqualityComparer<TValue> valueComparer = null)
        {
            _keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            _forwardDictionary = new Dictionary<TKey, TValue>(capacity, _keyComparer);
            _reverseDictionary = new Dictionary<TValue, TKey>(capacity, _valueComparer);
        }

        /// <summary>
        /// 从现有字典初始化BiMap
        /// </summary>
        public BiMap(IDictionary<TKey, TValue> dictionary) : this(dictionary, null, null)
        {
        }

        /// <summary>
        /// 从现有字典初始化BiMap，使用指定的比较器
        /// </summary>
        public BiMap(IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(dictionary.Count, keyComparer, valueComparer)
        {
            foreach (var kvp in dictionary)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 通过键获取值
        /// </summary>
        public TValue GetValue(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return _forwardDictionary[key];
        }

        /// <summary>
        /// 通过值获取键
        /// </summary>
        public TKey GetKey(TValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _reverseDictionary[value];
        }

        /// <summary>
        /// 尝试通过键获取值
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return _forwardDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 尝试通过值获取键
        /// </summary>
        public bool TryGetKey(TValue value, out TKey key)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _reverseDictionary.TryGetValue(value, out key);
        }

        /// <summary>
        /// 添加键值对（确保键和值都唯一）
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (_forwardDictionary.ContainsKey(key))
                throw new ArgumentException($"An element with key '{key}' already exists.", nameof(key));

            if (_reverseDictionary.ContainsKey(value))
                throw new ArgumentException($"An element with value '{value}' already exists.", nameof(value));

            _forwardDictionary.Add(key, value);
            _reverseDictionary.Add(value, key);
        }

        /// <summary>
        /// 尝试添加键值对（如果键或值已存在则返回false）
        /// </summary>
        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null || value == null) return false;

            if (_forwardDictionary.ContainsKey(key) || _reverseDictionary.ContainsKey(value))
                return false;

            _forwardDictionary.Add(key, value);
            _reverseDictionary.Add(value, key);

            return true;
        }

        /// <summary>
        /// 更新或添加键值对
        /// 如果键存在则更新对应的值，如果值已与其他键关联则抛出异常
        /// </summary>
        public void Set(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            // 如果键已存在
            if (_forwardDictionary.TryGetValue(key, out var oldValue))
            {
                // 如果新值等于旧值，直接返回
                if (_valueComparer.Equals(oldValue, value))
                    return;

                // 检查新值是否已与其他键关联
                if (_reverseDictionary.ContainsKey(value))
                    throw new ArgumentException($"Value '{value}' is already associated with another key.",
                        nameof(value));

                // 更新双向映射
                _forwardDictionary[key] = value;
                _reverseDictionary.Remove(oldValue);
                _reverseDictionary.Add(value, key);
            }
            else
            {
                // 键不存在，正常添加（会检查值是否唯一）
                Add(key, value);
            }
        }

        /// <summary>
        /// 通过键移除元素
        /// </summary>
        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (!_forwardDictionary.TryGetValue(key, out var value))
                return false;

            bool removed = _forwardDictionary.Remove(key) && _reverseDictionary.Remove(value);

            return removed;
        }

        /// <summary>
        /// 通过值移除元素
        /// </summary>
        public bool RemoveByValue(TValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (!_reverseDictionary.TryGetValue(value, out var key))
                return false;

            bool removed = _reverseDictionary.Remove(value) && _forwardDictionary.Remove(key);

            return removed;
        }

        /// <summary>
        /// 清空所有元素
        /// </summary>
        public void Clear()
        {
            int count = _forwardDictionary.Count;
            if (count == 0) return;

            _forwardDictionary.Clear();
            _reverseDictionary.Clear();
        }

        /// <summary>
        /// 检查是否包含指定的键
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return _forwardDictionary.ContainsKey(key);
        }

        /// <summary>
        /// 检查是否包含指定的值
        /// </summary>
        public bool ContainsValue(TValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _reverseDictionary.ContainsKey(value);
        }

        /// <summary>
        /// 检查是否包含指定的键值对
        /// </summary>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _forwardDictionary.TryGetValue(item.Key, out var value) &&
                   _valueComparer.Equals(value, item.Value);
        }

        /// <summary>
        /// 通过键查找对应的值（如果键不存在则抛出异常）
        /// </summary>
        public TValue this[TKey key]
        {
            get => _forwardDictionary[key];
            set => Set(key, value);
        }

        #endregion

        #region 集合接口实现

        public int Count => _forwardDictionary.Count;
        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _forwardDictionary.Keys;
        public ICollection<TValue> Values => _forwardDictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _forwardDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
                return Remove(item.Key);
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Destination array is not large enough.");

            int i = arrayIndex;
            foreach (var kvp in _forwardDictionary)
            {
                array[i++] = kvp;
            }
        }

        #endregion

        #region 特殊功能

        /// <summary>
        /// 获取反向视图（值到键的映射）
        /// 注意：这是只读视图，对原BiMap的修改会反映到视图中
        /// </summary>
        public IReadOnlyDictionary<TValue, TKey> Reverse => _reverseDictionary;

        /// <summary>
        /// 交换键和值（创建新的BiMap）
        /// </summary>
        public BiMap<TValue, TKey> Inverse()
        {
            var inverseMap = new BiMap<TValue, TKey>(Count, _valueComparer, _keyComparer);

            foreach (var kvp in _forwardDictionary)
            {
                inverseMap._forwardDictionary.Add(kvp.Value, kvp.Key);
                inverseMap._reverseDictionary.Add(kvp.Key, kvp.Value);
            }

            return inverseMap;
        }

        /// <summary>
        /// 合并另一个BiMap（跳过重复的键和值）
        /// </summary>
        public void MergeWith(BiMap<TKey, TValue> other, bool overwrite = false)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            foreach (var kvp in other._forwardDictionary)
            {
                if (overwrite)
                {
                    // 如果允许覆盖，使用Set方法
                    Set(kvp.Key, kvp.Value);
                }
                else
                {
                    // 否则使用TryAdd
                    TryAdd(kvp.Key, kvp.Value);
                }
            }
        }

        #endregion

        #region 实用方法

        /// <summary>
        /// 转换为标准的Dictionary
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(_forwardDictionary, _keyComparer);
        }

        /// <summary>
        /// 获取所有键值对的字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"BiMap<{typeof(TKey).Name}, {typeof(TValue).Name}>[{Count}]";
        }

        #endregion
    }
}