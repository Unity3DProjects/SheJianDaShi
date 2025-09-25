using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FileReplaceWindow : BaseEditorWindow
{
    [MenuItem("Tools/工具/文件替换面板", priority = 301)]
    public static void ShowWindow() => ShowBigWindow<FileReplaceWindow>("文件替换", 1000, 800);

    private static bool isFold;
    private static bool showStats;
    private static Vector2 scrollPosition;
    private static Vector2 scrollPosition2;

    private static string searchPath = "Assets/";
    private static string extentionName = ".asset";
    private static string[] replacedPaths;
    private static string[] targetPaths;

    private void OnGUI()
    {
        Space();
        RenderHelpBox("现阶段用于unity Mesh资源替换,保证引用关系", MessageType.Error);
        Space(10);
        replacedPaths = RenderDragableArea(replacedPaths, labelMarginTop: 1f);
        Space();
        searchPath = RenderFolderSelector("替换目标搜索路径", searchPath);
        Space();
        RenderHorizontal(() =>
        {
            RenderLabel("文件拓展名", Width(80), Height(24));
            extentionName = RenderTextField("", extentionName, Height(24));
            Space();
            RenderButton("重命名文件后缀", () =>
            {
                if (replacedPaths == null) return;
                try
                {
                    AssetDatabase.StartAssetEditing();
                    foreach (var path in replacedPaths)
                    {
                        var dir = Path.GetDirectoryName(path);
                        var newPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + extentionName);
                        AssetDatabase.MoveAsset(path, newPath);
                    }
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            });
            RenderButton("搜索目标文件", () =>
            {
                if (replacedPaths == null) return;
                targetPaths = replacedPaths
                    .Select(path =>
                        this.FindAssetAtFullPath(Path.GetFileNameWithoutExtension(path), ".asset", new[] { searchPath })
                            .FirstOrDefault())
                    .Where(path => path != null)
                    .ToArray();
            });
            RenderButton("删除自身并替换", () =>
            {
                ShowConfirmPanel(() =>
                {
                    try
                    {
                        AssetDatabase.StartAssetEditing();
                        if (targetPaths == null || replacedPaths == null) return;
                        if (targetPaths.Length != replacedPaths.Length) return;
                        for (int i = 0; i < replacedPaths.Length; i++)
                        {
                            replacedPaths[i].MoveFileTo(targetPaths[i], true);
                            ShowProgress("", $"{i}/{replacedPaths.Length}", i / (float)replacedPaths.Length);
                        }
                    }
                    finally
                    {
                        AssetDatabase.StopAssetEditing();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        HideProgress();
                    }
                });
            });
        });
        Space();
        RenderLine();
        Space();
        scrollPosition = RenderScrollView(scrollPosition, () =>
        {
            RenderVertical(() =>
            {
                if (replacedPaths != null)
                {
                    foreach (var replacedPath in replacedPaths)
                    {
                        this.RenderAssetAtPath<Mesh>(replacedPath);
                    }
                }
            });
        });
        if (targetPaths != null)
        {
            Space();
            RenderLine();
            Space();
            scrollPosition2 = RenderScrollView(scrollPosition2, () =>
            {
                RenderVertical(() =>
                {
                    foreach (var replacedPath in targetPaths)
                    {
                        this.RenderAssetAtPath<Mesh>(replacedPath);
                    }
                });
            });
        }

        if (targetPaths != null && replacedPaths != null)
        {
            if (targetPaths.Length != replacedPaths.Length)
            {
                RenderHelpBox("文件不匹配,无法替换!", MessageType.Error);
            }
            else
            {
                RenderHelpBox("可以尝试替换!", MessageType.Info);
            }
        }
    }
}