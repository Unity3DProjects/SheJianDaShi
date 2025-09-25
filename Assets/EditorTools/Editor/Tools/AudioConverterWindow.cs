using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using FFMpegCore;

public class AudioConverterWindow : BaseEditorWindow
{
    [MenuItem("Tools/工具 /音频转换器", priority = 303)]
    public static void ShowWindow() => ShowBigWindow<AudioConverterWindow>();

    private static string[] inputPaths;
    private static bool overwriteExisting = true;
    private static Vector2 scrollPosition;

    private void Awake()
    {
        Init();
    }

    private static void Init()
    {
        var info = new DirectoryInfo(Application.dataPath);
        var folder = info.GetDirectories("FFMpegCore", SearchOption.AllDirectories).First();
        GlobalFFOptions.Configure(options => options.BinaryFolder = folder.FullName);
    }

    private void OnGUI()
    {
        Space();
        GUILayout.Label("WAV to OGG Audio Converter", EditorStyles.boldLabel);
        Space(10);
        inputPaths = RenderDragableArea(inputPaths, labelMarginTop: 2f);
        Space(10);
        RenderHorizontal(() =>
        {
            Space(100);
            overwriteExisting = EditorGUILayout.Toggle("是否覆盖已存在的", overwriteExisting);
            RenderButton("转换", () => { ConverAllWaveToOgg(inputPaths); });
            Space(100);
        });
        Space();
        RenderLine();
        scrollPosition = RenderScrollView(scrollPosition, () =>
        {
            RenderVertical(() =>
            {
                if (inputPaths != null)
                {
                    foreach (var path in inputPaths)
                    {
                        RenderHorizontal(() => { RenderLabel(path); });
                    }
                }
            });
        });
    }

    private static void ConverAllWaveToOgg(string[] paths)
    {
        if (paths == null) return;
        try
        {
            AssetDatabase.StartAssetEditing();
            var index = 0;
            foreach (var path in paths)
            {
                ConvertWavToOgg(path);
                index++;
                ShowProgress("音频格式转换", $"{index}/{paths.Length}", index / (float)paths.Length);
            }
        }
        finally
        {
            HideProgress();
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("转换完成！");
        }
    }

    private static void ConvertWavToOgg(string input)
    {
        var output = Path.Combine(Path.GetDirectoryName(input), Path.GetFileNameWithoutExtension(input) + ".ogg");
        try
        {
            string outputDir = Path.GetDirectoryName(output);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            if (File.Exists(output) && !overwriteExisting)
            {
                EditorUtility.DisplayDialog("File Exists", "Output file already exists and overwrite is disabled.",
                    "OK");
                return;
            }

            FFMpegArguments
                .FromFileInput(input)
                .OutputToFile(output, true, options => options
                    .WithAudioCodec("libvorbis") // OGG codec
                    .WithVariableBitrate(4)) // VBR quality (1-10, 10 is best)
                .ProcessSynchronously();

            Debug.Log($"Successfully converted {input} to {output}");

            if (output.StartsWith(Application.dataPath))
            {
                string relativePath = "Assets" + output.Substring(Application.dataPath.Length);
                AssetDatabase.ImportAsset(relativePath);
                AssetDatabase.Refresh();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Conversion failed: {e}");
        }
    }
}