using System;
using AD.Tools;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal class TimeSelector : BaseEditorWindow
    {
        public static void ShowTimeSelectorWindow(Vector2 position,string title, DateTime time, Action<DateTime> callback)
        {
            var window = GetWindowWithRect<TimeSelector>(
                new Rect(position, new Vector2(415, 330))
            );
            window.time = time;
            window.callback = callback;
            window.maximized = false;
            window.titleContent = new GUIContent(title);
            window.Show(true);
        }

        private class Date : IFormattable
        {
            public int year;
            public int month;
            public int day;
            public int hour;
            public int min;
            public int sec;

            private Date()
            {
            }

            public static implicit operator Date(DateTime dt) => new Date()
            {
                year = dt.Year,
                month = dt.Month,
                day = dt.Day,
                hour = dt.Hour,
                min = dt.Minute,
                sec = dt.Second,
            };

            public static implicit operator DateTime(Date d) => new DateTime(
                d.year,
                d.month,
                d.day,
                d.hour,
                d.min,
                d.sec
            );

            public override string ToString()
            {
                return ToString(TimeCheck.Format, null);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return ((DateTime) this).ToString(format, formatProvider);
            }
        }

        private Date time;
        private Action<DateTime> callback;


        private void OnGUI()
        {
            if (time == null)
            {
                Close();
                return;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, Height(30));
            Draw_yMd();
            EditorGUILayout.Separator();
            Draw_Hms();
            EditorGUILayout.EndHorizontal();
            Space(5);
            ColorSpace(new Color(0.1f, 0.1f, 0.11f), 5);
            Space(5);

            EditorGUILayout.BeginHorizontal(Height(120));
            Scroll_yMd();
            EditorGUILayout.Separator();
            Scroll_Hms();
            EditorGUILayout.EndHorizontal();
            if (GUI.changed) Save();

            Space(5);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, Height(30));
            Label("使用<color=red>点击/鼠标滚轮</color>修改\n滚轮修改时,按下 Ctrl 变更x5 Shift 变更x10", new GUIStyle(GUI.skin.label)
            {
                richText = true
            });
            Module(() =>
            {
                if (GUILayout.Button("保存", Width(52), Height(30)))
                {
                    Save();
                    Close();
                }
            }, ColorR(new Color(0.2f, 0.8f, 0)));

            EditorGUILayout.EndHorizontal();
        }

        private void Save()
        {
            Clamp(ref time.year, 2024, 9999);
            Clamp(ref time.month, 1, 12);
            Clamp(ref time.day, 1, DateTime.DaysInMonth(time.year, time.month));
            Clamp(ref time.hour, 0, 23);
            Clamp(ref time.min, 0, 59);
            Clamp(ref time.sec, 0, 59);
            try
            {
                callback?.Invoke(time);
            }
            catch
            {
                Close();
            }
        }


        private void Draw_yMd()
        {
            EditorGUILayout.BeginHorizontal(Width(180));
            IntField("年", ref time.year, 2024, 9999);
            IntField("月", ref time.month, 1, 12);
            IntField("日", ref time.day, 1, DateTime.DaysInMonth(time.year, time.month));
            EditorGUILayout.EndHorizontal();
        }

        private void Draw_Hms()
        {
            EditorGUILayout.BeginHorizontal(Width(180));
            IntField("时", ref time.hour, 0, 23);
            IntField("分", ref time.min, 0, 59);
            IntField("秒", ref time.sec, 0, 59);
            EditorGUILayout.EndHorizontal();
        }

        private void Scroll_yMd()
        {
            EditorGUILayout.BeginHorizontal(Width(180));
            Scroll("年", ref time.year, 2024, 9999);
            Scroll("月", ref time.month, 1, 12);
            Scroll("日", ref time.day, 1, DateTime.DaysInMonth(time.year, time.month));
            EditorGUILayout.EndHorizontal();
        }

        private void Scroll_Hms()
        {
            EditorGUILayout.BeginHorizontal(Width(180));
            Scroll("时", ref time.hour, 0, 23);
            Scroll("分", ref time.min, 0, 59);
            Scroll("秒", ref time.sec, 0, 59);
            EditorGUILayout.EndHorizontal();
        }

        private static void IntField(string label, ref int value, int min, int max)
        {
            EditorGUILayout.BeginHorizontal(Width(60));
            value = EditorGUILayout.IntField(value, Width(40));
            EditorGUILayout.LabelField(label, Width(20));
            value = Mathf.Clamp(value, min, max);
            EditorGUILayout.EndHorizontal();
        }

        private static void Scroll(string label, ref int value, int min, int max)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, Height(120), Width(50));

            // 标签
            EditorGUILayout.LabelField($"{value} {label}", new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter
            }, Width(50));


            // 中间的滚动区域
            DrawScrollArea(ref value, min, max);


            EditorGUILayout.EndVertical();
        }

        private static void DrawScrollArea(ref int value, int min, int max)
        {
            Rect areaRect = GUILayoutUtility.GetRect(30, 200);

            // 绘制背景
            EditorGUI.DrawRect(areaRect, new Color(0.15f, 0.15f, 0.15f, 0.3f));

            // 绘制边框
            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Handles.DrawSolidRectangleWithOutline(areaRect, Color.clear, new Color(0.3f, 0.3f, 0.3f));
            int left = -3, right = 3;
            var itemSize = new Vector2(52, 24);
            var startDelta = (areaRect.height - itemSize.y) / (right - left + 1);
            var offset = new Vector2((areaRect.width - itemSize.x) / 2, 0);
            var itemStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };
            for (int i = left; i <= right; i++)
            {
                int v = value + i;
                if (v < min || v > max) continue;
                var color = i == 0
                    ? new Color(0.4f, 0.9f, 0.8f)
                    : ADUtils.Lerp(
                        new Color(0.8f, 0.8f, 0.8f),
                        new Color(0.55f, 0.55f, 0.55f),
                        (Math.Abs(i) - 1) / 3f);
                using (new ColorScope(color))
                {
                    if (GUI.Button(
                        new Rect(new Vector2(areaRect.x, areaRect.y + (i - left) * startDelta) + offset, itemSize),
                        v.ToString(),
                        itemStyle))
                    {
                        value = v;
                    }
                }
            }


            // 处理鼠标事件
            HandleEvents(areaRect, ref value, min, max);
        }

        private static void HandleEvents(Rect areaRect, ref int value, int min, int max)
        {
            Event e = Event.current;

            // 鼠标滚轮
            if (e.type == EventType.ScrollWheel && areaRect.Contains(e.mousePosition))
            {
                value = Mathf.Clamp(value + HandleDeltaValue(e), min, max);
                GUI.changed = true;
                e.Use();
            }
        }

        private static int HandleDeltaValue(Event e)
        {
            int multi = 1;
            if (e.control) multi *= 5;
            if (e.shift) multi *= 10;
            return (int) Mathf.Sign(e.delta.y) * multi;
        }

        private static void Clamp(ref int value, int min, int max)
        {
            value = Mathf.Clamp(value, min, max);
        }

        private class ColorScope : GUI.Scope
        {
            private Color pri;

            public ColorScope(Color color)
            {
                pri = GUI.color;
                GUI.color = color;
            }

            protected override void CloseScope()
            {
                GUI.color = pri;
            }
        }
    }
}