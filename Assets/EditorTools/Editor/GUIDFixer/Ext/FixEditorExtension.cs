using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

namespace Fix.Editor
{
    public static class FixEditorExtension
    {
        public static T Log<T>(this T t)
        {
            Debug.Log(t);
            return t;
        }
        public static T LogError<T>(this T t)
        {
            Debug.LogError(t);
            return t;
        }
        public static T LogException<T>(this T t)
        {
            Debug.LogException(new Exception(t?.ToString()));
            return t;
        }
        public static string ParentPath(this string propertyPath)
        {
            int indexOf;
            return (indexOf = propertyPath.LastIndexOf('.')) != -1 ? propertyPath.Substring(0, indexOf) : string.Empty;
        }
 
        public static object Find(this object obj, string actions) =>
            actions.Split('.').Aggregate(obj, (o, action) => o.InternalFind(action));
 
        private static readonly Regex Indexer = new Regex(@"(?<=data\[).*(?=\])");
 
        private static object InternalFind(this object obj, string action)
        {
            if (string.IsNullOrWhiteSpace(action)) return obj;
            switch (Indexer.IsMatch(action))
            {
                case true:
                    var index = int.Parse(Indexer.Match(action).Value);
                    var array = (Array)obj;
                    return array.GetValue(index);
                case false:
                    switch (action)
                    {
                        case "Array":
                            return ((IEnumerable) obj).OfType<object>().ToArray();
                        default:
                            var type = obj.GetType();
                        {
                            var value = type.GetField(action,
                                BindingFlags.Instance
                                | BindingFlags.Public
                                | BindingFlags.NonPublic
                                | BindingFlags.Default
                                | BindingFlags.GetField)?.GetValue(obj);
                            if (value != null) return value;
                        }
                        {
                            var value = type.GetProperty(action,
                                BindingFlags.Instance
                                | BindingFlags.Public
                                | BindingFlags.NonPublic
                                | BindingFlags.Default
                                | BindingFlags.GetProperty)?.GetValue(obj);
                            if (value != null) return value;
                        }
                            return null;
                    }
                    default: return null;
            }
        }

        public static bool TryGet<T>(this IEnumerable<T> source, Predicate<T> predicate, out T result)
        {
            foreach (var t in source)
            {
                if (!predicate(t)) continue;
                result = t;
                return true;
            }

            result = default;
            return false;
        }

        public static void Mkdir(this string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static void Rmdir(this string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path,true);
        }
        public static void CopyTo(this string source, string target)
        {
            
            InnerCopyTo(source,target);
        }
        static void InnerCopyTo(string source, string target)
        {
            target.Mkdir();
            var info = new DirectoryInfo(source);
            foreach (var file in info.GetFiles()) file.CopyTo($"{target}/{file.Name}", true);
            foreach (var dir in info.GetDirectories())
                InnerCopyTo(dir.FullName, $"{target}/{dir.Name}");
        }
        public static int BinaryLE<T>(this IList<T> list, T value,IComparer<T> cmp)
        {
            int r = list.Count - 1;
            if (r < 0 || cmp.Compare(list[r], value) <= 0) return r;
            int l = 0;
            while (l < r)
            {
                int mid = ((r - l) >> 1) + l;
                if (cmp.Compare(list[mid], value) <= 0) l = mid + 1;
                else r = mid;
            }

            return l - 1;
        }
        public static string FormatPath(this string path) => path.Replace("\\", "/");
        public static string ReFormatPath(this string path) => path.Replace("/", "\\");
        public static string GetCurrentUnityPath() => Process.GetCurrentProcess().Modules[0].FileName;
    }
}