using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>使用流程：</para>
/// <para>1.使用 dnspy 将这个文件中的代码编译到原游戏可调用的程序集</para>
/// <para>2.主动调用 SpriteRTOut.Output(); 越早调用越稳定</para>
/// <para>3.运行游戏，等待导出结束</para>
/// </summary>
public sealed class SpriteRTOut : MonoBehaviour
{
    private const string OutputPath = @"D:\UGit\bowmasters\Library\Fix\SpriteFix";
    private const string OutputFileName = "RuntimeSpriteData.json";
    private const string InfoFileName = "RuntimeSpriteDataInfo.txt";

    [Serializable]
    private sealed class SpriteRuntimeFile
    {
        public string unityVersion;
        public string productName;
        public string createdAt;
        public List<SpriteRuntimeData> sprites = new List<SpriteRuntimeData>();
    }

    [Serializable]
    private sealed class SpriteRuntimeData
    {
        public string name;
        public string textureName;
        public Rect rect;
        public Vector4 border;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public bool packed;
        public SpritePackingMode packingMode;
        public SpritePackingRotation packingRotation;
        public Vector2[] vertices;
        public Vector2[] uv;
        public int[] triangles;
    }

    private static SpriteRTOut instance;
    private static bool started;

    private static readonly Dictionary<string, SpriteRuntimeData> keyToData =
        new Dictionary<string, SpriteRuntimeData>();

    public static void Output()
    {
        if (started) return;
        started = true;
        Instance.StartCoroutine(Instance.OutputCoroutine());
    }

    private static SpriteRTOut Instance
    {
        get
        {
            if (instance != null) return instance;
            var go = new GameObject($"[{nameof(SpriteRTOut)}]");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<SpriteRTOut>();
            return instance;
        }
    }

    private IEnumerator OutputCoroutine()
    {
        var outputDir = OutputPath;
        var outputPath = Path.Combine(outputDir, OutputFileName);
        var infoPath = Path.Combine(outputDir, InfoFileName);
        if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
        keyToData.Clear();

        var lines = new List<string>
        {
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Application.productName,
            Application.unityVersion,
        };

        Exception caughtException = null;
        try
        {
            LoadStreamingAssetBundles(lines);
            CollectSprites();
        }
        catch (Exception e)
        {
            caughtException = e;
        }

        if (caughtException == null)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var asyncOp = SceneManager.LoadSceneAsync(i, LoadSceneMode.Single);
                while (!asyncOp.isDone)
                    yield return null;

                try
                {
                    lines.Add("Scene: " + SceneManager.GetActiveScene().path);
                    CollectSprites();
                }
                catch (Exception e)
                {
                    caughtException = e;
                    break;
                }
            }
        }

        try
        {
            if (caughtException == null)
            {
                CollectSprites();

                var file = new SpriteRuntimeFile
                {
                    unityVersion = Application.unityVersion,
                    productName = Application.productName,
                    createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    sprites = keyToData.Values.OrderBy(e => e.textureName).ThenBy(e => e.name).ToList()
                };

                File.WriteAllText(outputPath, JsonUtility.ToJson(file, true));
                lines.Add("Sprite Count: " + file.sprites.Count);
                lines.Add("Output: " + outputPath);
            }
            else
            {
                lines.Add(caughtException.ToString());
                Debug.LogException(caughtException);
            }
        }
        finally
        {
            File.WriteAllLines(infoPath, lines.ToArray());
        }
    }

    private static void LoadStreamingAssetBundles(ICollection<string> lines)
    {
        string root = Application.streamingAssetsPath;
        if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
        {
            lines.Add("Without StreamingAssets");
            return;
        }

        foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
        {
            try
            {
                var bundle = AssetBundle.LoadFromFile(file);
                if (bundle != null)
                    lines.Add("Bundle: " + file);
            }
            catch
            {
            }
        }
    }

    private static void CollectSprites()
    {
        foreach (var sprite in Resources.FindObjectsOfTypeAll<Sprite>())
        {
            if (sprite == null || sprite.texture == null) continue;
            var data = CreateData(sprite);
            keyToData[GetKey(data)] = data;
        }
    }

    private static SpriteRuntimeData CreateData(Sprite sprite)
    {
        return new SpriteRuntimeData
        {
            name = sprite.name,
            textureName = sprite.texture != null ? sprite.texture.name : string.Empty,
            rect = sprite.rect,
            border = sprite.border,
            pivot = sprite.pivot,
            pixelsPerUnit = sprite.pixelsPerUnit,
            packed = sprite.packed,
            packingMode = sprite.packingMode,
            packingRotation = sprite.packingRotation,
            vertices = sprite.vertices != null ? sprite.vertices.ToArray() : Array.Empty<Vector2>(),
            uv = sprite.uv != null ? sprite.uv.ToArray() : Array.Empty<Vector2>(),
            triangles = sprite.triangles != null ? sprite.triangles.Select(e => (int) e).ToArray() : Array.Empty<int>(),
        };
    }

    private static string GetKey(SpriteRuntimeData data)
    {
        return string.Concat(
            data.textureName, "|",
            data.name, "|",
            data.rect.x.ToString("F4"), "|",
            data.rect.y.ToString("F4"), "|",
            data.rect.width.ToString("F4"), "|",
            data.rect.height.ToString("F4"), "|",
            data.packingMode, "|",
            data.packingRotation);
    }
}