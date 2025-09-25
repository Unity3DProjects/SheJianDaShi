using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 用于更改纹理的尺寸
/// 操作方式：拖拽到指定位置，然后进行操作
/// </summary>
public class MenuItem_TextureEx : EditorWindow
{
    private string m_tipName = "需要4化的文件夹路径/文件";

    private List<string> _listPaths = new List<string>();

    private bool IsToBig = true; //向着更大缩放
    private bool IsFloder = false;
    private readonly Color m_targetFilesColor = new Color(0, 1, 072f, 0.5f);

    private bool b_isSprite;

    private BuildTarget m_buildTarget;



    //背景资源宽高4倍处理
    [MenuItem("Tools/WebGL图片处理", priority = 101)]
    private static void TextureEx()
    {
        Debug.Log("打开窗口");
        GetWindowWithRect<MenuItem_TextureEx>(new Rect(Screen.width / 2, Screen.height / 2, 500, 500)).titleContent = new GUIContent("图片处理");
    }


    void OnGUI()
    {
        //! 实现拖拽
        Rect drawRect = EditorGUILayout.BeginHorizontal();
        GUILayout.Box(m_tipName, GUILayout.MinHeight(64), GUILayout.MinWidth(512));
        EditorGUILayout.EndHorizontal();

        //? 文件夹和文件
        GUILayout.Space(10);

        GUI.color = m_targetFilesColor;
        for (int i = 0; i < _listPaths.Count; i++)
        {
            GUILayout.Label(_listPaths[i]);
        }

        UnityEngine.Event currentEvent = UnityEngine.Event.current;
        //拖拽范围内
        if (drawRect.Contains(currentEvent.mousePosition))
        {
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                    break;
                case EventType.DragPerform:
                    string[] paths = DragAndDrop.paths;
                    for (int i = 0; i < paths.Length; i++)
                    {
                        if (!_listPaths.Contains(paths[i]))
                        {
                            _listPaths.Add(paths[i]);
                        }
                    }

                    break;
            }
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        GUIStyle style = GUI.skin.toggle;
        style.fontSize = 14;
        GUI.color = Color.white;
        IsToBig = GUILayout.Toggle(IsToBig, "向着更大的4化处理", style);

        GUI.color = Color.yellow;
        IsFloder = GUILayout.Toggle(IsFloder, "是文件夹吗", style);

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUI.color = Color.white;
        if (GUILayout.Button("清除", GUILayout.Width(256), GUILayout.Height(40)))
        {
            _listPaths.Clear();
        }

        if (GUILayout.Button("开始处理大小4化", GUILayout.Width(256), GUILayout.Height(40)))
        {

            if (_listPaths == null || _listPaths.Count <= 0)
            {
                // Debug.LogError("要处理的列表为空".FH4_OrangeRed()); return;

            }


            if (IsFloder)
                ExFloder();
            else
                ExFile();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        b_isSprite = GUILayout.Toggle(b_isSprite, "类型改成Sprite");
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("设置贴图【WebGL】", GUILayout.Width(256), GUILayout.Height(40)))
        {
            if (_listPaths == null || _listPaths.Count <= 0)
            {
                // Debug.LogError("要处理的列表为空".FH4_OrangeRed()); return;

            }

            if (IsFloder)
                HandelTexFormatSetting_Folder();
            else
            {
                HandelTexFormatSetting_File();
            }
        }

        if (GUILayout.Button("设置贴图【Android】", GUILayout.Width(256), GUILayout.Height(40)))
        {
            if (_listPaths == null || _listPaths.Count <= 0)
            {
                // Debug.LogError("要处理的列表为空".FH4_OrangeRed()); return;

            }

            if (IsFloder)
                HandelTexFormatSetting_Folder();
            else
            {
                HandelTexFormatSetting_File();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("修改【最大尺寸】大于2048的", GUILayout.Width(256), GUILayout.Height(40)))
        {
            if (_listPaths == null || _listPaths.Count <= 0)
            {
                // Debug.LogError("要处理的列表为空".FH4_OrangeRed()); return;
            }

            if (IsFloder)
            {
                HandelMaxsize_Folder();
            }
            else
            {
                HandelMaxsize_File();
            }

        }

        GUILayout.EndHorizontal();





    }



    #region ---------------------------- 图片大小






    private void ExFile()
    {
        try
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("处理过路径：");

            for (int i = 0; i < _listPaths.Count; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{_listPaths.Count}", (float)i / _listPaths.Count);
                string path = _listPaths[i];
                ExTex(path);  //!处理贴图

                builder.AppendLine($"{path}");
            }

            Debug.Log(builder.ToString());
        }
        catch (Exception e)
        {
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private void ExFloder()
    {
        try
        {
            string[] guids = AssetDatabase.FindAssets("t:texture ", _listPaths.ToArray());

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("处理过路径：");

            for (int i = 0; i < guids.Length; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{guids.Length}", (float)i / guids.Length);
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                ExTex(path);

                builder.AppendLine($"{path}");
            }

            Debug.Log(builder.ToString());
        }
        catch (Exception e)
        {
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
    #endregion




    #region ---------------------------- 修改最大尺寸


    private void HandelMaxsize_File()
    {
        try
        {
            //StringBuilder builder = new StringBuilder();
            //builder.AppendLine("处理过路径：");
            string strInfo = "";

            for (int i = 0; i < _listPaths.Count; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{_listPaths.Count}", (float)i / _listPaths.Count);
                string path = _listPaths[i];
                ModifyMaxsize(path);  //!处理贴图

                //builder.AppendLine($"{path}");
                //builder.AppendLine("  ");

                // strInfo += Path.GetFileName(path).FH1_SpringGreen() + " 、";
            }

            // Debug.Log(strInfo);
        }
        catch (Exception e)
        {
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }



    private void HandelMaxsize_Folder()
    {
        try
        {
            string[] guids = AssetDatabase.FindAssets("t:texture ", _listPaths.ToArray()/*列表*/);

            //StringBuilder builder = new StringBuilder();
            //builder.AppendLine("处理过的路径：");

            string strInfo = "";

            for (int i = 0; i < guids.Length; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{guids.Length}", (float)i / guids.Length);
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);


                ModifyMaxsize(path);

                //builder.AppendLine($"{path}");
                //builder.AppendLine("  ");
                //builder.AppendLine(Path.GetFileName(path).FH5_Yellow());

                // strInfo += Path.GetFileName(path).FH1_SpringGreen() + " 、";
            }

            // Debug.Log(strInfo);
        }
        catch (Exception e)
        {
        }
        finally
        {
            EditorUtility.ClearProgressBar();  //! 清除进度
        }
    }

    private void ModifyMaxsize(string path)
    {
        if (AssetDatabase.LoadMainAssetAtPath(path) is Texture2D)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            Handle_Maxsize(importer);
        }
    }


    private void Handle_Maxsize(TextureImporter texImporter)
    {
        TextureImporterPlatformSettings targetPlatformSettings = texImporter.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());



        int maxSize = targetPlatformSettings.maxTextureSize;
        if (maxSize < 2048) return;


        targetPlatformSettings.overridden = true;

        //A.png 的尺寸是 1024 x 1024 , 修改前A的最大尺寸为2048 , 那这样即使改为 1024 ,大小也不会变

        //! 大于2048的必改
        if (maxSize >= 2048)
        {

           // 客服邮箱：673530815@qq.com Debug.LogFormat("修改前 : {0} 平台的 最大尺寸 = {1}", targetPlatformSettings.name.FH1_SpringGreen(), maxSize.FH4_OrangeRed());
            targetPlatformSettings.maxTextureSize = 1024;
        }


        texImporter.SetPlatformTextureSettings(targetPlatformSettings);
        texImporter.mipmapEnabled = false;
        texImporter.isReadable = false;
        texImporter.SaveAndReimport();
        AssetDatabase.Refresh();
        _listPaths.Clear();
    }

    #endregion

    #region ---------------------------- 处理纹理的 WebGL 平台设置格式

    private void HandelTexFormatSetting(string path)
    {
        if (AssetDatabase.LoadMainAssetAtPath(path) is Texture2D)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.mipmapEnabled = false;  //! 禁用 mipmap
            WebGL_Handler(importer);
        }
    }


    private void HandelTexFormatSetting_File()
    {

        try
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("处理过路径：");

            for (int i = 0; i < _listPaths.Count; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{_listPaths.Count}", (float)i / _listPaths.Count);
                string path = _listPaths[i];
                HandelTexFormatSetting(path);  //!处理贴图

                builder.AppendLine($"{path}");
            }

            //TODO:【编辑器】文件名字变色
            Debug.Log(builder.ToString());
        }
        catch (Exception e)
        {

        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

    }


    private void HandelTexFormatSetting_Folder()
    {
        try
        {
            string[] guids = AssetDatabase.FindAssets("t:texture ", _listPaths.ToArray()/*列表*/);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("处理过路径：");

            for (int i = 0; i < guids.Length; i++)
            {
                EditorUtility.DisplayProgressBar("开始处理", $"{i + 1}/{guids.Length}", (float)i / guids.Length);
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                HandelTexFormatSetting(path);

                builder.AppendLine($"{path}");
            }

            Debug.Log(builder.ToString());
        }
        catch (Exception e)
        {
        }
        finally
        {
            EditorUtility.ClearProgressBar();  //! 清除进度
        }
    }

    #endregion


    public void ExTex(string path)
    {
        //!++ 判断是不是贴图
        //写法 : 判断是不是贴图
        if (AssetDatabase.LoadMainAssetAtPath(path) is Texture2D tex)  //写法:可以声明变量
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;


            if (tex.width % 4 == 0 && tex.height % 4 == 0)
            {
                return;
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
            AssetDatabase.Refresh();
            _listPaths.Clear();
        }
    }



    private void WebGL_Handler(TextureImporter texImporter)
    {
        TextureImporterPlatformSettings webglSettings2 = texImporter.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());
        TextureImporterPlatformSettings targetPlatformSettings = texImporter.GetPlatformTextureSettings(BuildTarget.WebGL.ToString());

        if (b_isSprite)
            texImporter.textureType = TextureImporterType.Sprite;  //
        targetPlatformSettings.overridden = true;

        #region ------------------------------- 压缩格式不用自己选,设置crunchedCompression就行了

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


        #endregion

        targetPlatformSettings.compressionQuality = 32;  //压缩质量
        targetPlatformSettings.maxTextureSize = texImporter.maxTextureSize;

        texImporter.SetPlatformTextureSettings(targetPlatformSettings);
        texImporter.mipmapEnabled = false;
        texImporter.isReadable = false;
        texImporter.SaveAndReimport();
        AssetDatabase.Refresh();
        _listPaths.Clear();
    }





    /// <summary>
    /// 目标尺寸，宽高整数4处理
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Vector2Int GetFourSize(int width, int height)
    {
        if (IsToBig)
        {
            while (width % 4 != 0)
            {
                width++;
            }

            while (height % 4 != 0)
            {
                height++;
            }
        }
        else
        {
            while (width % 4 != 0)
            {
                width--;
            }

            while (height % 4 != 0)
            {
                height--;
            }
        }

        return new Vector2Int(Mathf.Max(4, width), Mathf.Max(4, height));
    }
}
