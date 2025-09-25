using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fix.Editor
{
    public abstract class FixEditorWindow : EditorWindow
    {
        private static readonly int AssetsNameLenght = "Assets".Length;
        private void Awake() => currentDrag = null;

        private void OnDestroy() => currentDrag = null;

        private static string[] currentDrag;
        protected static void DragRegion(Action<string[]> action, string text, params GUILayoutOption[] options)
        {
            HorizontalRegion(drawRect =>
            {
                GUILayout.Box(text, options);
                UnityEngine.Event currentEvent = UnityEngine.Event.current;
                if (drawRect.Contains(currentEvent.mousePosition))
                {
                    switch (currentEvent.type)
                    {
                        case EventType.DragUpdated:
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                            currentDrag = DragAndDrop.paths;
                            break;
                        case EventType.DragPerform:
                            if(currentDrag!=null)
                            {
                                action?.Invoke(currentDrag);
                                currentDrag = null;
                            }
                            break;
                        case EventType.DragExited:
                            if(currentDrag!=null)
                            {
                                action?.Invoke(currentDrag);
                                currentDrag = null;
                            }
                            break;
                    }
                }
            });
        }
        protected static void DragRegion(Action<string[]> action, GUIContent content, params GUILayoutOption[] options)
        {
            HorizontalRegion(drawRect =>
            {
                GUILayout.Box(content, options);
                UnityEngine.Event currentEvent = UnityEngine.Event.current;
                if (drawRect.Contains(currentEvent.mousePosition))
                {
                    switch (currentEvent.type)
                    {
                        case EventType.DragUpdated:
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                            currentDrag = DragAndDrop.paths;
                            break;
                        case EventType.DragPerform:
                            if(currentDrag!=null)action?.Invoke(currentDrag);
                            break;
                        case EventType.DragExited:
                            if(currentDrag!=null)action?.Invoke(currentDrag);
                            break;
                    }
                }
            });
        }
        protected static void HorizontalRegion(Action<Rect> action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            var rect = EditorGUILayout.BeginHorizontal(options);
            action.Invoke(rect);
            EditorGUILayout.EndHorizontal();
        }

        protected static void HorizontalRegion(Action action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            GUILayout.BeginHorizontal(options);
            action.Invoke();
            GUILayout.EndHorizontal();
        }
        protected static void VerticalRegion(Action<Rect> action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            var rect = EditorGUILayout.BeginVertical(options);
            action.Invoke(rect);
            GUILayout.EndVertical();
        }
        protected static void VerticalRegion(Action action,params GUILayoutOption[] options)
        {
            if (action == null) return;
            GUILayout.BeginVertical(options);
            action.Invoke();
            GUILayout.EndVertical();
        }

        protected static void ColorRegion(Color color, Action action)
        {
            if (action == null) return;
            var pri = GUI.color;
            GUI.color = color;
            action.Invoke();
            GUI.color = pri;
        }
        protected void StateRegion(bool state, Action action)
        {
            if (action == null) return;
            var b = GUI.enabled;
            GUI.enabled = state;
            action.Invoke();
            GUI.enabled = b;
        }
        protected static void Space(int space = 10) => GUILayout.Space(space);

        protected static IEnumerable<string> GetTotalFiles(IEnumerable<string> paths)
        {
            var files = new List<string>();
            foreach (var path in paths)
            {
                if (File.Exists(path)) files.Add(path);
                else if (Directory.Exists(path)) files.AddRange(Directory.EnumerateFiles(path));
            }

            return files.Where(e => !".meta".Equals(Path.GetExtension(e),StringComparison.InvariantCultureIgnoreCase)).Select(e=>e.Replace("\\","/"));
        }
        protected void ShowInExplorer(string path)
        {
            if (!Directory.Exists(path))
            {
                ShowNotification(new GUIContent($"路径:\n{path}\n不存在"));
                return;
            }

            var process = Process.Start("Explorer.exe", path.Replace("/", "\\"));
            process?.Dispose();
        }

        protected static string GetWorkspaceFolder(string workspaceName)
        {
            var dataPath = Application.dataPath;
            return $"{dataPath.Substring(0, dataPath.Length - AssetsNameLenght)}Library/Fix/{workspaceName}";
        }
    }
}
