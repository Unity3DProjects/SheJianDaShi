using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.AssetsProssesor
{
    public class TextureProcessorWindow : BaseEditorWindow
    {
        [MenuItem("Tools/资源处理/图片处理器", priority = 100)]
        private static void CreateWindow() => ShowBigWindow<TextureProcessorWindow>("图片处理器");

        private static string searchPath = "Assets";

        private static List<TextureInfo> texturInfos = new List<TextureInfo>();

        private static List<TextureInfo> texturInfosCache = new List<TextureInfo>();

        private static List<string> texturPaths = new List<string>();

        private Vector2 scrollPos;

        private static int filterSize;

        void OnGUI()
        {
            Space();
            searchPath = RenderFolderSelector("搜索路径", searchPath);
            Space();
            RenderHorizontal(() =>
            {
                RenderButton("初始化缓存", InitCache);
                RenderButton("筛选", FilterTexture, GUILayout.Width(80), GUILayout.Height(24));
            });
            Space();
            RenderHorizontal(() =>
            {
                RenderButton("扫描所有图片", SearchAllTexture);
                RenderButton("扫描默认贴图", () => { SearchTextureByType(TextureImporterType.Default); });
                RenderButton("扫描精灵图片", () => { SearchTextureByType(TextureImporterType.Sprite); });
                RenderButton("扫描法线贴图", () => { SearchTextureByType(TextureImporterType.NormalMap); });
                RenderButton("扫描光照贴图", () => { SearchTextureByType(TextureImporterType.Lightmap); });
            });

            //TODO 筛选设置和图片设置
            Space();
            RenderHorizontal(() =>
            {
                RenderLabel("按大小筛选(kb)", GUILayout.Width(100));
                filterSize = RenderIntSlider(filterSize, 0, 1024);
            });


            Space();
            RenderLine(Color.cyan, 0.6f);
            Space();
            var totalSize = texturInfos.Sum(info => info.size);
            RenderLabel($"贴图数量：{texturPaths.Count} , 总大小：{EditorUtility.FormatBytes(totalSize)}");
            Space(5);
            scrollPos = RenderScrollView(scrollPos, () =>
            {
                if (texturPaths != null)
                {
                    foreach (var info in texturInfos)
                    {
                        RenderHorizontal(() =>
                        {
                            this.RenderAssetAtPath<Texture2D>(info.path);
                            RenderLabel(EditorUtility.FormatBytes(info.size),
                                GUILayout.Width(100f));
                        });
                    }
                }
            });

            Space();
            RenderHorizontal(() =>
            {
                RenderButton("删除所有图片", Color.red, () => { ShowConfirmPanel(DeleteAllTexture); }, Height(24f),Width(100));
                RenderButton("图片四倍化", () => { ShowConfirmPanel(() => { QuadruplingTexture(texturPaths); }); });
                RenderButton("图片压缩设置", () => { ShowConfirmPanel(() => { ModifyTextureSeeting(texturPaths); }); });
                RenderButton("图片最大尺寸减半", () => { ShowConfirmPanel(() => { ModTextureMaxSize(texturPaths); }); });
            });
        }


        #region MenuItem拓展

        [MenuItem("Assets/资源处理/四倍化", false)]
        public static void QuadruplingTextureByRightClick()
        {
            QuadruplingTexture(Selection.GetFiltered<Texture2D>(SelectionMode.Assets).ToList());
        }

        [MenuItem("Assets/资源处理/四倍化", true)]
        public static bool QuadruplingTextureByRightClickValidate()
        {
            return Selection.GetFiltered<Texture2D>(SelectionMode.Assets)?.Length > 0;
        }

        #endregion

        #region 表现方法

        private void InitCache()
        {
            texturInfosCache = FindTextures()
                .Select(path =>
                    new TextureInfo(path, GetTextureSize(AssetDatabase.LoadAssetAtPath<Texture2D>(path))))
                .OrderByDescending(info => info.size)
                .ToList();
            texturInfos = new List<TextureInfo>(texturInfosCache);
            texturPaths = texturInfosCache.Select(info => info.path).ToList();
        }

        private void FilterTexture()
        {
            texturInfos = texturInfosCache.Where(info => info.size > filterSize * 1024).ToList();
            texturPaths = texturInfos.Select(info => info.path).ToList();
        }

        private void SearchAllTexture()
        {
            texturInfos = FindTextures()
                .Select(path =>
                    new TextureInfo(path, GetTextureSize(AssetDatabase.LoadAssetAtPath<Texture2D>(path))))
                .OrderByDescending(info => info.size)
                .ToList();

            texturPaths = texturInfos.Select(info => info.path).ToList();
        }

        private void SearchTextureByType(TextureImporterType textureImporterType)
        {
            texturInfos = FindTextures(textureImporterType).Select(path =>
                    new TextureInfo(path, GetTextureSize(AssetDatabase.LoadAssetAtPath<Texture2D>(path))))
                .OrderByDescending(info => info.size)
                .ToList();

            texturPaths = texturInfos.Select(info => info.path).ToList();
        }

        #endregion


        private static List<string> FindTextures()
        {
            List<string> assetPathList = new List<string>();
            var guids = AssetDatabase.FindAssets("t:Texture", new[] { searchPath });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null && asset is Texture2D)
                {
                    var texureImportor = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (texureImportor == null)
                        continue;
                    assetPathList.Add(path);
                }
            }

            return assetPathList;
        }

        private List<string> FindTextures(TextureImporterType textureImporterType)
        {
            List<string> assetPathList = new List<string>();
            var guids = AssetDatabase.FindAssets("t:Texture", new[] { searchPath });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null && asset is Texture2D)
                {
                    var texureImportor = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (texureImportor == null)
                        continue;
                    if (texureImportor.textureType == textureImporterType)
                    {
                        assetPathList.Add(path);
                    }
                }
            }

            return assetPathList;
        }

        private void DeleteAllTexture()
        {
            if (texturInfos == null) return;

            foreach (var info in texturInfos)
            {
                AssetDatabase.DeleteAsset(info.path);
            }

            Debug.Log("删除所有贴图!");
            AssetDatabase.Refresh();
        }

        private int GetTextureSize(Texture2D texture)
        {
            System.Type type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
            System.Reflection.MethodInfo methodInfo = type.GetMethod("GetStorageMemorySize",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public);
            // Debug.Log("内存占用：" +
            //           EditorUtility.FormatBytes(UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(texture)));
            // Debug.Log("硬盘占用：" + EditorUtility.FormatBytes((int)methodInfo.Invoke(null, new object[] { texture })));
            return (int)methodInfo.Invoke(null, new object[] { texture });
        }

        /// <summary>
        /// 图片四倍化
        /// </summary>
        private static void QuadruplingTexture(List<string> paths)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var processedCount = 0;

            if (paths == null && paths.Count == 0)
            {
                Debug.Log("没有需要处理的图片");
                return;
            }

            var count = paths.Count;
            var index = 0;
            try
            {
                foreach (var path in paths)
                {
                    if (AssetDatabase.LoadMainAssetAtPath(path) is Texture2D tex) //写法:可以声明变量
                    {
                        ShowProgress("图片四倍化处理", $"{index}/{count} {path}", index / (float)count);
                        index++;

                        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                        if (tex.width % 4 == 0 && tex.height % 4 == 0)
                        {
                            continue;
                        }

                        //更改图片属性，可读，否则无法获取Pixel
                        importer.isReadable = true;
                        importer.SaveAndReimport();

                        Vector2Int v2 = GetFourSize(tex.width, tex.height);
                        Texture2D texCopy = new Texture2D(v2.x, v2.y);
                        //从原来图像上根据现在的大小计算像素点
                        for (int h = 0; h < v2.y; h++)
                        {
                            for (int w = 0; w < v2.x; w++)
                            {
                                var pixel = tex.GetPixelBilinear(w / (v2.x * 1.0f), h / (v2.y * 1.0f));
                                texCopy.SetPixel(w, h, pixel);
                            }
                        }

                        texCopy.Apply();
                        File.WriteAllBytes(path, texCopy.EncodeToPNG());
                        //!恢复不可读
                        importer.isReadable = false;
                        importer.SaveAndReimport();
                        processedCount++;
                        stringBuilder.AppendLine(path);
                    }

                    AssetDatabase.Refresh();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                HideProgress();
                Debug.Log($"处理{processedCount}个资源成功 \n{stringBuilder}");
            }
        }

        /// <summary>
        /// 图片四倍化
        /// </summary>
        private static void QuadruplingTexture(List<Texture2D> texture2Ds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var processedCount = 0;

            if (texture2Ds == null && texture2Ds.Count == 0)
            {
                Debug.Log("没有需要处理的图片");
                return;
            }

            var count = texture2Ds.Count;
            var index = 0;
            try
            {
                foreach (var texture2D in texture2Ds)
                {
                    if (texture2D is Texture2D) //写法:可以声明变量
                    {
                        AssetDatabase.TryGetGUIDAndLocalFileIdentifier(texture2D, out var guid, out long fileID);
                        ShowProgress("图片四倍化处理", $"{index}/{count} {texture2D}", index / (float)count);
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        index++;
                        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                        if (texture2D.width % 4 == 0 && texture2D.height % 4 == 0)
                        {
                            continue;
                        }

                        //更改图片属性，可读，否则无法获取Pixel
                        importer.isReadable = true;
                        importer.SaveAndReimport();

                        Vector2Int v2 = GetFourSize(texture2D.width, texture2D.height);
                        Texture2D texCopy = new Texture2D(v2.x, v2.y);
                        //从原来图像上根据现在的大小计算像素点
                        for (int h = 0; h < v2.y; h++)
                        {
                            for (int w = 0; w < v2.x; w++)
                            {
                                var pixel = texture2D.GetPixelBilinear(w / (v2.x * 1.0f), h / (v2.y * 1.0f));
                                texCopy.SetPixel(w, h, pixel);
                            }
                        }

                        texCopy.Apply();
                        File.WriteAllBytes(path, texCopy.EncodeToPNG());
                        //!恢复不可读
                        importer.isReadable = false;
                        importer.SaveAndReimport();
                        processedCount++;
                        stringBuilder.AppendLine(path);
                    }

                    AssetDatabase.Refresh();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                HideProgress();
                Debug.Log($"处理{processedCount}个资源成功 \n{stringBuilder}");
            }
        }

        #region 尺寸修改

        /// <summary>
        /// 修改图片最大尺寸
        /// </summary>
        private static void ModTextureMaxSize(List<string> texturPaths)
        {
            if (texturPaths == null || texturPaths.Count <= 0)
            {
                Debug.Log("没有需要处理的图片");
                return;
            }

            var processedCount = 0;
            try
            {
                AssetDatabase.StartAssetEditing();
                for (int i = 0; i < texturPaths.Count; i++)
                {
                    EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{texturPaths.Count}",
                        (float)i / texturPaths.Count);
                    string path = texturPaths[i];
                    if (AssetDatabase.LoadMainAssetAtPath(path) is Texture2D texture2D)
                    {
                        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                        if (importer.textureType == TextureImporterType.Sprite)
                            continue;
                        var maxLength = texture2D.width > texture2D.height ? texture2D.width : texture2D.height;
                        TextureImporterPlatformSettings targetPlatformSettings =
                            importer.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());
                        int maxSize = GetTextureMaxSize(maxLength);
                        var curMaxSize = targetPlatformSettings.maxTextureSize;
                        if (curMaxSize < maxSize)
                        {
                            maxSize = curMaxSize / 2;
                            maxSize = GetTextureMaxSize(maxSize);
                        }
                        else
                        {
                            maxSize = GetTextureMaxSize(maxSize / 2);
                        }

                        targetPlatformSettings.overridden = true;
                        targetPlatformSettings.maxTextureSize = maxSize;
                        importer.SetPlatformTextureSettings(targetPlatformSettings);
                        importer.SaveAndReimport();
                    }

                    processedCount++;
                }

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                Debug.Log($"处理完{processedCount}资源.");
                EditorUtility.ClearProgressBar();
                AssetDatabase.StopAssetEditing();
            }
        }

        private static void CoerectMaxSizeSingle(TextureImporter texImporter, int maxLength, bool needHalf = false)
        {
        }

        private static int GetTextureMaxSize(int curSize)
        {
            if (curSize >= 4096)
            {
                curSize = 4096;
            }

            if (curSize > 2048 && curSize <= 4096)
            {
                curSize = 4096;
            }
            else if (curSize > 1024 && curSize <= 2048)
            {
                curSize = 2048;
            }
            else if (curSize > 512 && curSize <= 1024)
            {
                curSize = 1024;
            }
            else if (curSize > 256 && curSize <= 512)
            {
                curSize = 512;
            }
            else if (curSize > 128 && curSize <= 256)
            {
                curSize = 256;
            }
            else if (curSize > 64 && curSize <= 128)
            {
                curSize = 128;
            }
            else if (curSize > 32 && curSize <= 64)
            {
                curSize = 64;
            }
            else if (curSize > 16 && curSize <= 32)
            {
                curSize = 32;
            }

            if (curSize < 16)
            {
                curSize = 16;
            }

            return curSize;
        }

        #endregion


        /// <summary>
        /// 修改贴图设置
        /// </summary>
        /// <param name="Paths"></param>
        private static void ModifyTextureSeeting(List<string> Paths)
        {
            if (texturPaths == null || texturPaths.Count <= 0)
            {
                Debug.Log("没有需要处理的图片");
                return;
            }

            try
            {
                AssetDatabase.StartAssetEditing();
                StringBuilder builder = new StringBuilder();
                var processedCount = 0;
                builder.AppendLine("处理过路径：");

                for (int i = 0; i < Paths.Count; i++)
                {
                    EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{Paths.Count}",
                        (float)i / Paths.Count);
                    if (AssetDatabase.LoadMainAssetAtPath(Paths[i]) is Texture2D)
                    {
                        TextureImporter importer = AssetImporter.GetAtPath(Paths[i]) as TextureImporter;
                        WebGL_Handler(importer);
                    }

                    builder.AppendLine($"{Paths[i]}");
                    processedCount++;
                }

                Debug.Log($"处理完成{processedCount}资源\n{builder}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
            }
        }


        private static void WebGL_Handler(TextureImporter texImporter)
        {
            TextureImporterPlatformSettings webglSettings2 =
                texImporter.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());
            TextureImporterPlatformSettings targetPlatformSettings =
                texImporter.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());

            targetPlatformSettings.overridden = true;


            //? 2021 直接设置bool值就行
            //targetPlatformSettings.format = texFormat;  //压缩格式算法

            //targetPlatformSettings.crunchedCompression = true;


            //! 2019 需要指定压缩格式
            var _format = texImporter.GetAutomaticFormat("WebGL");

            //Debug.Log("格式1 ：" + targetPlatformSettings.format.ToString().FH1_SpringGreen());
            // Debug.Log("格式2 ：" + _format.ToString().FH1_SpringGreen());


            if (_format == TextureImporterFormat.RGBA32 ||
                _format == TextureImporterFormat.RGBA16 ||
                _format == TextureImporterFormat.DXT5)
            {
                targetPlatformSettings.format = TextureImporterFormat.DXT5Crunched;
            }

            else if (_format == TextureImporterFormat.RGB16 || _format == TextureImporterFormat.RGB24 ||
                     _format == TextureImporterFormat.DXT1)
            {
                targetPlatformSettings.format = TextureImporterFormat.DXT1Crunched;
            }

            targetPlatformSettings.compressionQuality = 20; //压缩质量
            targetPlatformSettings.maxTextureSize = texImporter.maxTextureSize;

            texImporter.SetPlatformTextureSettings(targetPlatformSettings);
            texImporter.mipmapEnabled = false;
            texImporter.isReadable = false;
            texImporter.SaveAndReimport();
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// 目标尺寸，宽高整数4处理
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Vector2Int GetFourSize(int width, int height)
        {
            while (width % 4 != 0)
            {
                width++;
            }

            while (height % 4 != 0)
            {
                height++;
            }

            return new Vector2Int(Mathf.Max(4, width), Mathf.Max(4, height));
        }
    }
}

public class TextureInfo
{
    public TextureInfo(string path, int size)
    {
        this.path = path;
        this.size = size;
    }

    public string path;

    public int size;
}