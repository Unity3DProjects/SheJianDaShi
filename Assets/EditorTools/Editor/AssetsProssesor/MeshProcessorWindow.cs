using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;

public class MeshProcessorWindow : BaseEditorWindow
{
    private static float Quality = 0.5f;

    [MenuItem("Tools/资源处理/Mesh处理器", priority = 105)]
    private static void CreateWindow() => ShowBigWindow<MeshProcessorWindow>("Mesh处理器");

    private static string searchPath = "Assets";

    private static List<MeshInfo> meshInfos = new List<MeshInfo>();

    private static List<string> meshPaths = new List<string>();

    private Vector2 scrollPos;

    [MenuItem("Assets/资源处理/压缩模型", false)]
    public static void QuadruplingTextureByRightClick()
    {
        DoCompressMesh(Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToArray(), 0.5f);
    }

    [MenuItem("Assets/资源处理/压缩模型", true)]
    public static bool QuadruplingTextureByRightClickValidate()
    {
        return Selection.GetFiltered<Mesh>(SelectionMode.Assets)?.Length > 0;
    }

    private void OnGUI()
    {
        Space();
        searchPath = RenderFolderSelector("搜索路径", searchPath);
        Space();

        Quality = RenderFloatSlider("压缩质量:", Quality, 0, 1);

        RenderHorizontal(() =>
        {
            RenderButton("扫描所有Mesh", GetAllMesh);
            RenderButton("筛选带有BlendShape", () =>
            {
                if (meshInfos == null) return;
                meshInfos = meshInfos
                    .Where(info => AssetDatabase.LoadAssetAtPath<Mesh>(info.path).blendShapeCount > 0)
                    .ToList();
                meshPaths = meshInfos.Select(info => info.path).ToList();
            });
        });

        Space();
        RenderLine(Color.cyan, 0.6f);
        Space();
        scrollPos = RenderScrollView(scrollPos, () =>
        {
            if (meshInfos != null)
            {
                RenderLabel(
                    $"Mesh数量:{meshInfos.Count} , Mesh总大小:{EditorUtility.FormatBytes(meshInfos.Sum(info => info.Size))}   (仅显示Top300)");
                Space(5);
                foreach (var info in meshInfos.Take(300))
                {
                    RenderHorizontal(() =>
                    {
                        this.RenderAssetAtPath<Mesh>(info.path);
                        // RenderLabel($"顶点数:{info.vertexCount}", GUILayout.Width(100f));
                        RenderLabel($"大小:{EditorUtility.FormatBytes(info.Size)}",
                            GUILayout.Width(100f));
                        RenderButton("压缩", () => { DoCompressMesh(new[] { info.path }, Quality); },
                            GUILayout.Width(80));
                    });
                }
            }
        });
        if (meshInfos != null)
        {
            if (meshInfos.Count > 0)
            {
                RenderHorizontal(() =>
                {
                    RenderButton("压缩全部Mesh",
                        () => { ShowConfirmPanel(() => { DoCompressMesh(meshPaths.ToArray(), Quality); }); });
                    RenderButton("删除BlendShape",
                        () =>
                        {
                            ShowConfirmPanel(() =>
                            {
                                ClearBlendShap(meshInfos
                                    .Select(info => AssetDatabase.LoadAssetAtPath<Mesh>(info.path)).ToArray());
                            });
                        });
                });
            }
        }
    }

    private static void GetAllMesh()
    {
        /*meshInfos = AssetDatabase.FindAssets("t:Mesh", new[] { searchPath })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(path => new MeshInfo(
                path,
                AssetDatabase.LoadAssetAtPath<Mesh>(path).vertexCount,
                AssetDatabase.LoadAssetAtPath<Mesh>(path).GetVertexBufferStride(0)
            ))
            .OrderByDescending(info => info.Size)
            .ToList();*/

        meshPaths = meshInfos.Select(info => info.path).ToList();
    }

    private static void DoCompressMesh(string[] paths, float quality)
    {
        Debug.Log("Start compress");
        AssetDatabase.StartAssetEditing();
        try
        {
            int c = 0, total = paths.Length;

            foreach (var path in paths)
            {
                EditorUtility.DisplayProgressBar(
                    "开始处理",
                    $"{c + 1}/{total}",
                    (float)c / total);
                c++;
                var simplifier = new MeshSimplifier
                {
                    // VertexLinkDistance = 0.0001f,
                    // EnableSmartLink = true,
                    // PreserveUVSeamEdges = true,
                    PreserveBorderEdges = true,
                };
                var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if (mesh == null)
                    continue;
                if (AssetDatabase.IsSubAsset(mesh)) return;
                simplifier.Initialize(mesh);
                simplifier.SimplifyMesh(quality);
                var newMesh = simplifier.ToMesh();
                MeshUtility.Optimize(newMesh);
                AssetDatabase.CreateAsset(newMesh, path);
                // AssetImporter.GetAtPath(path).SaveAndReimport();
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Done");
        }
    }

    /// <summary>
    /// 移除BlendShape
    /// </summary>
    /// <param name="mesh"></param>
    private void ClearBlendShap(Mesh[] meshs)
    {
        try
        {
            var index = 0;
            foreach (var mesh in meshs)
            {
                mesh.ClearBlendShapes();
                EditorUtility.SetDirty(mesh);
                index++;
                ShowProgress("删除BlendShape", $"{index}/{meshs.Length}", index / (float)meshs.Length);
            }
        }
        finally
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            HideProgress();
            Debug.Log($"{nameof(ClearBlendShap)}!");
        }
    }
}

public class MeshInfo
{
    public string path;

    public int vertexCount;

    public int stride;

    public int Size => vertexCount * stride;

    public MeshInfo(string path, int vertexCount, int stride)
    {
        this.path = path;
        this.vertexCount = vertexCount;
        this.stride = stride;
    }
}