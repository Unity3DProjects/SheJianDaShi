using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AD.Tools
{
    public static class ReflectorManager
    {
        public const int CacheCount = 25;

        private struct Entry
        {
            public Type type;
            public string token;
            public BindingFlags flags;

            public Entry(Type type, string token, BindingFlags flags)
            {
                this.type = type;
                this.token = token;
                this.flags = flags;
            }

            public bool Equals(Entry other)
            {
                return Equals(type, other.type) && token == other.token && flags == other.flags;
            }

            public override bool Equals(object obj)
            {
                return obj is Entry other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (type != null ? type.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (token != null ? token.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int) flags;
                    return hashCode;
                }
            }

            public override string ToString()
            {
                return $"{nameof(type)}: {type}, {nameof(token)}: {token}, {nameof(flags)}: {flags}";
            }
        }

        private static LRU<Entry, MemberInfo> cache = new LRU<Entry, MemberInfo>(CacheCount);

        public static FieldInfo ReflectInstanceField(Type type, string name) => ReflectField(type, name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        public static FieldInfo ReflectField(Type type, string name, BindingFlags bindingAttr)
        {
            var entry = new Entry(type, name, bindingAttr);
            if (cache.TryGetValue(entry, out var result)) return (FieldInfo) result;
            while (type != null && result == null)
            {
                result = type.GetField(name, bindingAttr);
                type = type.BaseType;
            }

            cache[entry] = result;
            return (FieldInfo) result;
        }

        public static PropertyInfo ReflectProp(Type type, string name, BindingFlags bindingAttr)
        {
            var entry = new Entry(type, name, bindingAttr);
            if (cache.TryGetValue(entry, out var result)) return (PropertyInfo) result;
            while (type != null && result == null)
            {
                result = type.GetProperty(name, bindingAttr);
                type = type.BaseType;
            }

            cache[entry] = result;
            return (PropertyInfo) result;
        }
    }
}