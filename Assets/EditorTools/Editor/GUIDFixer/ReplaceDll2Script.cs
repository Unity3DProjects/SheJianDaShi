using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fix.Editor
{
    public class ReplaceDll2Script : FixEditorWindow
    {
        private const string Title = "Dll->Script";
        private static readonly string[] FindRoot = new[] { "Assets" };

        [MenuItem("Tools/GUID Fixer/"  + Title, priority = 500)]
        private static void ShowWindow()
        {
            GetWindowWithRect<ReplaceDll2Script>(new Rect(Screen.width / 2, Screen.height / 2, 500, 500)).titleContent =
                new GUIContent(Title);
        }


        private const string Extension = ".dll";
        private string WorkspaceFolder => GetWorkspaceFolder(nameof(ReplaceDll2Script));

        private readonly List<string> dragDlls = new List<string>(),
            existDlls = new List<string>(),
            selectedDlls = new List<string>();


        private bool existFoldout;
        private Vector2 dragPos, existPos, selectedPos;

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            dragDlls.Clear();
            existDlls.Clear();
            selectedDlls.Clear();
            existDlls.AddRange(Directory.GetFiles(WorkspaceFolder));
        }

        private void OnGUI()
        {
            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                if (GUILayout.Button($"切换为 {SerializationMode.ForceText.ToString()} 模式"))
                {
                    EditorSettings.serializationMode = SerializationMode.ForceText;
                }

                return;
            }

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 12,
                    stretchWidth = true
                },
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fixedWidth = 24,
                    fixedHeight = 24,
                    fontSize = 12
                };

            DragRegion(
                paths =>
                {
                    dragDlls.AddRange(
                        paths.Where(e =>
                                Extension.Equals(Path.GetExtension(e), StringComparison.CurrentCultureIgnoreCase))
                            .Where(e => !dragDlls.Contains(e)));
                }, "拖入Dll文件", GUILayout.MinHeight(64), GUILayout.MinWidth(512));
            GUILayout.Label("拖入的Dll:");
            if (dragDlls.Count != 0)
            {
                HorizontalRegion(() =>
                {
                    dragPos = EditorGUILayout.BeginScrollView(dragPos, GUILayout.MaxHeight(80));
                    foreach (var dragDll in dragDlls)
                        GUILayout.Label(Path.GetFileNameWithoutExtension(dragDll), labelStyle);
                    EditorGUILayout.EndScrollView();
                });
            }

            HorizontalRegion(rect =>
            {
                Space(4);
                EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, rect.width, 4), new Color(0.0f, 1f, 1f, 0.4f));
            }, GUILayout.Height(4), GUILayout.ExpandWidth(true));
            HorizontalRegion(() =>
            {
                GUILayout.Label("已保存的Dll:");
                if (GUILayout.Button("添加全部", GUILayout.Width(80))) existDlls.ForEach(AddSelectedDll);
            });
            if (existFoldout = EditorGUILayout.Foldout(existFoldout, ""))
            {
                HorizontalRegion(() =>
                {
                    existPos = EditorGUILayout.BeginScrollView(existPos, false, true, GUILayout.MaxHeight(80));
                    foreach (var existedDll in existDlls)
                        HorizontalRegion(() =>
                        {
                            GUILayout.Label(Path.GetFileNameWithoutExtension(existedDll), labelStyle);
                            if (GUILayout.Button("+", buttonStyle)) AddSelectedDll(existedDll);
                        });
                    EditorGUILayout.EndScrollView();
                });
            }

            HorizontalRegion(rect =>
            {
                Space(4);
                EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, rect.width, 4), new Color(0.0f, 0.4f, 1f, 0.4f));
            }, GUILayout.Height(4), GUILayout.ExpandWidth(true));
            HorizontalRegion(() => GUILayout.Label("用于替换的Dll:"));


            HorizontalRegion(() =>
            {
                selectedPos = EditorGUILayout.BeginScrollView(selectedPos, GUILayout.MaxHeight(80));
                var array = selectedDlls.ToArray();
                for (var i = 0; i < array.Length; i++)
                {
                    int index = i;
                    HorizontalRegion(() =>
                    {
                        var dllPath = array[index];
                        GUILayout.Label(Path.GetFileNameWithoutExtension(dllPath), labelStyle);
                        if (GUILayout.Button("x", buttonStyle)) selectedDlls.RemoveAt(index);
                    });
                }

                EditorGUILayout.EndScrollView();
            });
            Space(40);
            HorizontalRegion(() =>
            {
                Space(85);
                if (GUILayout.Button("保存", GUILayout.Width(320), GUILayout.Height(40))) Save();
            });
            Space(30);
            HorizontalRegion(() =>
            {
                Space(85);
                if (GUILayout.Button("替换", GUILayout.Width(320), GUILayout.Height(40))) Replace();
            });
        }


        private void Save()
        {
            try
            {
                DoSave();
                ShowNotification(new GUIContent("保存成功"));
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
                Debug.LogException(e);
            }
        }

        private void Replace()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                DoReplace();
                ShowNotification(new GUIContent("替换完成"));
            }
            catch (Exception e)
            {
                ShowNotification(new GUIContent(e.Message));
                Debug.LogException(e);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void DoSave()
        {
            if (dragDlls.Count == 0) return;

            WorkspaceFolder.Mkdir();

            foreach (var dllPath in dragDlls)
            {
                var savePath = GetSavePath(dllPath);
                var entries = new Dictionary<string, Entry>();
                foreach (var script in AssetDatabase.LoadAllAssetRepresentationsAtPath(dllPath).OfType<MonoScript>())
                {
                    string tfn;
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out string guid, out long localId)
                        && (tfn = script.GetClass()?.FullName) != null)
                        entries.Add(tfn, new Entry() { guid = guid, localId = localId });
                }

                //File.WriteAllText(savePath, JsonConvert.SerializeObject(entries, Formatting.Indented));
            }

            Refresh();
        }


        private void DoReplace()
        {
            var o2n = new List<KeyValuePair<string, string>>();

            foreach (var dllPath in selectedDlls)
            {
                if (!File.Exists(dllPath)) throw new Exception($"未找到{dllPath}的信息文件");
                var oldEntries = JsonConvert.DeserializeObject<Dictionary<string, Entry>>(File.ReadAllText(dllPath));
                if (oldEntries == null) throw new Exception($"{dllPath}的信息文件损坏");
                var newEntries = new Dictionary<string, Entry>();
                foreach (var script in AssetDatabase
                             .FindAssets("t:MonoScript")
                             .Select(AssetDatabase.GUIDToAssetPath)
                             .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)
                             .Where(e => e != null))
                {
                    string key = script.GetClass()?.FullName;
                    if (key == null) continue;
                    if (oldEntries.TryGetValue(key, out var entry) &&
                        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out string guid, out long localId) &&
                        entry.guid != guid)
                        newEntries.Add(key, new Entry() { guid = guid, localId = localId });
                }

                var notMatch = newEntries.Where(e => !oldEntries.ContainsKey(e.Key)).Select(e => e.Key).ToArray();
                if (notMatch.Length > 0)
                {
                    if (!EditorUtility.DisplayDialog("存在未匹配的内容", string.Join("\n", notMatch), "继续替换", "取消替换")) return;
                }

                foreach (var pair in newEntries)
                    if (oldEntries.TryGetValue(pair.Key, out var entry))
                        o2n.Add(new KeyValuePair<string, string>(GetString(entry), GetString(pair.Value)));
            }

            if (o2n.Count <= 0) return;
            var paths = AssetDatabase.GetAllAssetPaths().Where(File.Exists).Where(path => path.StartsWith("Assets"))
                .ToArray();
            try
            {
                int i = 0, c = paths.Length;
                foreach (var path in paths)
                {
                    EditorUtility.DisplayProgressBar("正在替换", $"{i}/{c} {path}", (float)i / c);
                    var content = File.ReadAllText(path);
                    bool repFlag = false;
                    foreach (var rep in o2n)
                    {
                        if (!content.Contains(rep.Key)) continue;
                        repFlag = true;
                        ReplaceRef(ref content, rep.Key, rep.Value);
                    }

                    if (repFlag) File.WriteAllText(path, content);
                    i++;
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void AddSelectedDll(string path)
        {
            if (!selectedDlls.Contains(path)) selectedDlls.Add(path);
        }

        private static string GetString(Entry entry) => $"fileID: {entry.localId}, guid: {entry.guid}";

        private static void ReplaceRef(ref string content, string oldString, string newString)
        {
            content = content.Replace(oldString, newString);
        }

        private string GetSavePath(string dllPath) =>
            $"{WorkspaceFolder}/{Path.GetFileNameWithoutExtension(dllPath)}.json";

        public class Entry
        {
            public string guid;
            public long localId;
        }
    }
}