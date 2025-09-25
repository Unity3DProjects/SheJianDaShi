using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class ReplaceRefWindow : BaseEditorWindow
{
    [MenuItem("Tools/GUID Fixer/" + "文件夹引用替换", priority = 503)]
    public static void Open() => ShowBigWindow<ReplaceRefWindow>(width: 600, height: 720);

    private List<string> SavedKeys = new List<string>();

    private List<string> replaceCaheKeys = new List<string>();

    private Dictionary<string, Dictionary<string, string>> oldGUIDs =
        new Dictionary<string, Dictionary<string, string>>();

    private Dictionary<string, ScriptInfo> matchingDic = new Dictionary<string, ScriptInfo>();

    private string folderPath;


    private Vector2 pos1;
    private Vector2 pos2;
    private Vector2 pos3;

    private void Awake()
    {
        Load();
    }

    private void OnGUI()
    {
        Space();
        folderPath = RenderDragableArea(folderPath, "拖拽文件夹");
        Space();
        RenderLabel(folderPath);
        Space();
        RenderLine(Color.cyan);

        pos1 = RenderScrollView(pos1, () =>
        {
            var array = SavedKeys.ToArray();
            foreach (var key in array)
            {
                RenderHorizontal(() =>
                {
                    RenderLabel(key);
                    RenderButton("添加", () =>
                    {
                        SavedKeys.Remove(key);
                        replaceCaheKeys.Add(key);
                    }, GUILayout.Width(60));
                });
            }
        }, GUILayout.Height(100f));
        RenderLine(Color.green);
        pos2 = RenderScrollView(pos2, () =>
        {
            var array = replaceCaheKeys.ToArray();
            foreach (var key in array)
            {
                RenderHorizontal(() =>
                {
                    RenderLabel(key);
                    RenderButton("X", () =>
                    {
                        replaceCaheKeys.Remove(key);
                        SavedKeys.Add(key);
                    }, GUILayout.Width(60));
                });
            }
        }, GUILayout.Height(100f));

        RenderLine(Color.yellow);

        pos3 = RenderScrollView(pos3, () =>
        {
            if (oldGUIDs != null)
            {
                foreach (var kv in oldGUIDs)
                {
                    if (replaceCaheKeys.Contains(kv.Key))
                    {
                        RenderLabel($"{kv.Key}({kv.Value.Count})");
                        Space();
                        foreach (var infos in kv.Value)
                        {
                            if (matchingDic.ContainsKey(infos.Key))
                            {
                                RenderLabel($"{infos.Key}:  {infos.Value}", Color.green);
                            }
                            else
                            {
                                RenderLabel($"{infos.Key}:  {infos.Value}");
                            }
                        }

                        RenderLine(lineHeight: 0.8f);
                    }
                }
            }
        }, GUILayout.Height(300f));

        RenderLine(Color.magenta);
        Space(10);
        RenderHorizontal(() =>
        {
            RenderButton("保存", () =>
            {
                Dictionary<string, string> scrtptInfo = new Dictionary<string, string>();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("脚本GUID:");
                var monoScripts = this.FindAndLoadAssetAtPath<MonoScript>("t:Script", new[] { folderPath });
                foreach (var monoScript in monoScripts)
                {
                    if (monoScript == null)
                        continue;
                    if (monoScript.GetClass() == null)
                        continue;

                    var type = monoScript.GetClass();
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(monoScript, out string guid, out long fileID))
                    {
                        stringBuilder.AppendLine($"{type.FullName}: {guid}");
                        scrtptInfo[type.FullName] = guid;
                    }
                }

                Debug.Log(stringBuilder);

                if (monoScripts.Length > 0)
                {
                    oldGUIDs[folderPath] = scrtptInfo;
                    if (!SavedKeys.Contains(folderPath) && !replaceCaheKeys.Contains(folderPath))
                        SavedKeys.Add(folderPath);
                    Save();
                }
            });
            RenderButton("检索匹配", () =>
            {
                var scripts = this.FindAndLoadAssetAtPath<MonoScript>("t:MonoScript", null);

                foreach (var script in scripts)
                {
                    if (script == null)
                        continue;
                    if (script.GetClass() == null)
                        continue;

                    string key = script.GetClass()?.FullName;
                    if (key == null) continue;

                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(script, out string guid, out long fileID))
                    {
                        foreach (var replaceCaheKey in replaceCaheKeys)
                        {
                            if (oldGUIDs != null)
                            {
                                if (oldGUIDs.TryGetValue(replaceCaheKey, out var dic))
                                {
                                    if (dic.ContainsKey(key))
                                    {
                                        matchingDic[key] = new ScriptInfo(dic[key], guid);
                                    }
                                }
                            }
                        }
                    }
                }
            });
            RenderButton("清除匹配项", () => { matchingDic?.Clear(); });
            RenderButton("开始替换", () =>
            {
                var paths = AssetDatabase.GetAllAssetPaths().Where(File.Exists).Where(path => path.StartsWith("Assets"))
                    .ToArray();
                try
                {
                    int i = 0, c = paths.Length;
                    foreach (var path in paths)
                    {
                        EditorUtility.DisplayProgressBar("正在替换", $"{i}/{c} {path}", (float)i / c);
                        var content = File.ReadAllText(path);

                        bool needWrite = false;
                        foreach (var kv in matchingDic)
                        {
                            if (!content.Contains(kv.Value.OldGUID)) continue;
                            ReplaceReference(ref content, kv.Value.OldGUID, kv.Value.NewGUID);
                            needWrite = true;
                        }

                        if (needWrite)
                            File.WriteAllText(path, content);
                        i++;
                    }
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                    AssetDatabase.Refresh();
                }
            });
        });
        RenderButton("清除所有数据", () =>
        {
            SavedKeys.Clear();
            replaceCaheKeys.Clear();
            oldGUIDs.Clear();
            Save();
        });
    }

    /// <summary>
    /// 替换引用 
    /// </summary>
    private static void ReplaceReference(ref string content, string oldGUID, string newGUID)
    {
        var oldID = $"guid: {oldGUID}";
        var newID = $"guid: {newGUID}";
        content = content.Replace(oldID, newID);
    }

    private void Save()
    {
        UnityEngine.PlayerPrefs.SetString("root", JsonConvert.SerializeObject(oldGUIDs));
        UnityEngine.PlayerPrefs.SetString(nameof(SavedKeys), JsonConvert.SerializeObject(SavedKeys));
        UnityEngine.PlayerPrefs.SetString(nameof(replaceCaheKeys),
            JsonConvert.SerializeObject(replaceCaheKeys));
    }

    private void Load()
    {
        SavedKeys = JsonConvert.DeserializeObject<List<string>>(UnityEngine.PlayerPrefs.GetString(nameof(SavedKeys)));
        replaceCaheKeys =
            JsonConvert.DeserializeObject<List<string>>(UnityEngine.PlayerPrefs.GetString(nameof(replaceCaheKeys)));
        oldGUIDs =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(
                UnityEngine.PlayerPrefs.GetString("root"));

        if (SavedKeys == null) SavedKeys = new List<string>();
        if (replaceCaheKeys == null) replaceCaheKeys = new List<string>();
        if (oldGUIDs == null) oldGUIDs = new Dictionary<string, Dictionary<string, string>>();
    }
}

[Serializable]
public class ScriptInfo
{
    public string OldGUID;
    public string NewGUID;

    public ScriptInfo(string oldGuid, string newGuid)
    {
        OldGUID = oldGuid;
        NewGUID = newGuid;
    }
}