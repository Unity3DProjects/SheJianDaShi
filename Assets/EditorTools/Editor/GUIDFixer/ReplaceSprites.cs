using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ReplaceSprites : BaseEditorWindow
{
    private static string searchPath = "Assets";

    private static List<string> totalSpritePaths = new List<string>();

    private static List<string> oldSprites = new List<string>();
    private static List<string> newSprites = new List<string>();

    /// <summary>
    /// 匹配的精灵图
    /// </summary>
    private static Dictionary<string, SpriteGUIDS> matchSprites = new Dictionary<string, SpriteGUIDS>();

    private Vector2 s1;
    private Vector2 s2;

    [MenuItem("Tools/GUID Fixer/" + "修复精灵面板", priority = 502)]
    public static void Open() => ShowBigWindow<ReplaceSprites>();

    private void OnGUI()
    {
        Space();
        searchPath = RenderFolderSelector("选择筛选路径", searchPath);
        Space();

        RenderHorizontal(() =>
        {
            RenderButton("搜索精灵图",
                () =>
                {
                    totalSpritePaths = AssetDatabase.FindAssets("t:Sprite", new[] { searchPath })
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .ToList();

                    oldSprites = totalSpritePaths
                        .Where(path => Path.GetExtension(path).Equals(".asset"))
                        .GroupBy(path => Path.GetFileNameWithoutExtension(path))
                        .Select(group => group.First())
                        .ToList();

                    newSprites = totalSpritePaths
                        .Where(path => Path.GetExtension(path).Equals(".png"))
                        .GroupBy(path => Path.GetFileNameWithoutExtension(path))
                        .Select(group => group.First())
                        .ToList();
                    try
                    {
                        matchSprites.Clear();
                        var dic = oldSprites.ToDictionary(path => { return Path.GetFileNameWithoutExtension(path); });

                        var dic2 = newSprites.ToDictionary(path => { return Path.GetFileNameWithoutExtension(path); })
                            .Where(kvp => dic.ContainsKey(kvp.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);


                        foreach (var kv in dic2)
                        {
                            matchSprites[kv.Key] = new SpriteGUIDS(AssetDatabase.AssetPathToGUID(dic[kv.Key]),
                                AssetDatabase.AssetPathToGUID(kv.Value));
                        }

                        if (matchSprites.Count <= 0)
                        {
                            Debug.Log("未检索到配对精灵图");
                            return;
                        }
                        else
                        {
                            Debug.Log($"检索到配对精灵图数量：{matchSprites.Count}");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                });

            RenderButton("开始替换精灵", () =>
            {
                try
                {
                    AssetDatabase.StartAssetEditing();
                    if (ReplaceRef(matchSprites))
                    {
                        Debug.Log("精灵图修复成功");
                    }
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            });
        });

        Space();
        if (matchSprites.Count > 0)
        {
            RenderLabel($"检索到配对精灵图数量：{matchSprites.Count}", Color.green
            );
        }

        Space();
        RenderLine(Color.yellow);
        s1 = RenderScrollView(s1, () =>
        {
            RenderLabel($"旧引用的精灵图:数量({oldSprites.Count})", Color.yellow);
            foreach (var path in oldSprites)
            {
                if (matchSprites.ContainsKey(Path.GetFileNameWithoutExtension(path)))
                {
                    RenderLabel(path, Color.green);
                }
                else
                {
                    RenderLabel(path);
                }
            }
        });
        Space();
        RenderLine(Color.yellow);
        s2 = RenderScrollView(s2, () =>
        {
            RenderLabel($"新生成的精灵图:数量({newSprites.Count})", Color.yellow);
            foreach (var path in newSprites)
            {
                if (matchSprites.ContainsKey(Path.GetFileNameWithoutExtension(path)))
                {
                    RenderLabel(path, Color.green);
                }
                else
                {
                    RenderLabel(path);
                }
            }
        });
    }

    private static bool ReplaceRef(Dictionary<string, SpriteGUIDS> spritesToReplace)
    {
        if (spritesToReplace != null && spritesToReplace.Count > 0)
        {
            var root = new string[] { "Assets" };
            var paths = AssetDatabase
                .FindAssets("t:Scene", root)
                .Concat(AssetDatabase.FindAssets("t:Prefab", root))
                .Concat(AssetDatabase.FindAssets("t:Prefab", root))
                .Concat(AssetDatabase.FindAssets("t:ScriptableObject", root))
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
            try
            {
                int i = 0, c = paths.Length;
                foreach (var path in paths)
                {
                    EditorUtility.DisplayProgressBar("正在替换", $"{i}/{c} {path}", (float)i / c);
                    var content = File.ReadAllText(path);
                    foreach (var kv in spritesToReplace)
                    {
                        ReplaceRef(ref content, kv.Value.oldGUID, kv.Value.newGUID);
                    }

                    File.WriteAllText(path, content);
                    i++;
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        return false;
    }

    /// <summary>
    /// 替换引用 
    /// </summary>
    private static void ReplaceRef(ref string content, string old, string @new)
    {
        var oldID = $"fileID: 21300000, guid: {old}, type: 2";
        var newID = $"fileID: 21300000, guid: {@new}, type: 3";
        content = content.Replace(oldID, newID);
    }
}

public class SpriteGUIDS
{
    public string oldGUID;

    public string newGUID;

    public SpriteGUIDS(string oldGuid, string newGuid)
    {
        oldGUID = oldGuid;
        newGUID = newGuid;
    }
}