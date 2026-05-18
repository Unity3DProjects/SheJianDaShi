using System;
using AD.Attributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Scripting;

namespace AD.Decorator
{
    [Serializable, Preserve, ADParamDecoratorEnumType("计时器")]
    public class TimerDecorator : IADParamDecorator
    {
        public float intervalSeconds = 60;
        public DateTime? prePassTime;

        public bool Pass
        {
            get
            {
                if (intervalSeconds <= 0) return true;
                if (prePassTime.HasValue
                    && DateTime.Now - prePassTime.Value < TimeSpan.FromSeconds(intervalSeconds))
                    return false;
                prePassTime = DateTime.Now;
                return true;
            }
        }

        public override string ToString()
        {
            return $"Timer : {intervalSeconds}";
        }
#if UNITY_EDITOR
        private bool foldout;
        public void Layout()
        {
            GUILayout.BeginHorizontal();
            intervalSeconds = EditorGUILayout.FloatField(new GUIContent("间隔(s)"), intervalSeconds);
            GUILayout.EndHorizontal();
        }
#endif
    }
}