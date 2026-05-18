using System.Collections.Generic;

namespace AD.Tools
{
    internal static class GlobalValuePool
    {
        static GlobalValuePool()
        {
            values = new Dictionary<string, object>();
        }

        private static readonly IDictionary<string, object> values;

        public static T Get<T>(string key)
        {
            return values.TryGetValue(key, out var v) && v is T value ? value : default;
        }

        public static bool TryGet<T>(string key, out T value)
        {
            if (values.TryGetValue(key, out var v) && v is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        public static void Set(string key, object value)
        {
            values[key] = value;
        }
    }
}