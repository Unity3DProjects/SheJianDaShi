using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace AD.Tools
{
    public static class LogEx
    {
        public static void Log(this string msg, ELogColor color = ELogColor.Blue)
        {
            LogMessage(msg, color);
        }

        public static void Log(this object obj, string msg, ELogColor color = ELogColor.Blue)
        {
            LogMessage(msg, color);
        }
        public static T Log<T>(this T obj, ELogColor color = ELogColor.Blue)
        {
            LogMessage(obj?.ToString(), color);
            return obj;
        }
        [Conditional("UNITY_EDITOR")]
        public static void LogInEditor(this string msg, ELogColor color = ELogColor.Blue)
        {
            LogMessage(msg, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogInEditor(this object obj, string msg, ELogColor color = ELogColor.Blue)
        {
            LogMessage(msg, color);
        }

        private static void LogMessage(string msg, ELogColor color = ELogColor.White)
        {
            switch (color)
            {
                case ELogColor.White:
                    Debug.Log(msg);
                    break;
                case ELogColor.Blue:
                    Debug.Log($"<color=#40E0D0>{msg}</color>");
                    break;
                case ELogColor.Green:
                    Debug.Log($"<color=#7FFFAA>{msg}</color>");
                    break;
                case ELogColor.Black:
                    Debug.Log($"<color=#2F4F4F>{msg}</color>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }
    }

    public enum ELogColor
    {
        White,
        Blue,
        Green,
        Black,
    }
}