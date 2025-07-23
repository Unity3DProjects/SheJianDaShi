using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class HWWebGLExport : EditorWindow
{
    private const int Width = 540;
    private const int Height = 540;

    [MenuItem("Tools/导出华为WebGL", false, int.MaxValue)]
    public static void ShowWin()
    {
        GetWindowWithRect<HWWebGLExport>(
            new Rect(Screen.width / 2, Screen.height / 2, Width, Height)
        ).titleContent = new GUIContent("导出华为WebGL");
    }

    private static string SaveFilePath => Path.Combine(Path.GetFullPath("./"), "Assets/Editor/HWWebGLExport_Data.json")
        .Replace("\\", "/");


    private HWWebGLExportData data;
    private bool developmentBuild;
    private bool cleanBuild;

    private void OnEnable()
    {
        developmentBuild = EditorUserBuildSettings.development;
        data = LoadData();
    }

    private void OnDestroy()
    {
        SaveData();
        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
        GUILayout.Space(120);
        HorizontalRegion(() =>
        {
            GUILayout.Space(30);
            developmentBuild = GUILayout.Toggle(developmentBuild, "Development Build");
        });
#if UNITY_2021_1_OR_NEWER
        GUILayout.Space(30);
        HorizontalRegion(() =>
        {
            GUILayout.Space(30);
            cleanBuild = GUILayout.Toggle(cleanBuild, "Clean Build");
            GUILayout.Label("会重构项目");
        });
#endif
        GUILayout.Space(30);
        HorizontalRegion(() =>
        {
            GUILayout.Label("导出路径:", GUILayout.Width(60), GUILayout.Height(24));
            GUILayout.Box(data.path, GUILayout.ExpandWidth(true), GUILayout.Height(24));
        });
        GUILayout.Space(50);
        HorizontalRegion(() =>
        {
            GUILayout.Space(Width / 2 - 100);
            if (GUILayout.Button("选择", GUILayout.Width(85), GUILayout.Height(28)))
            {
                var path = EditorUtility.OpenFolderPanel("选择导出目录",
                    Directory.Exists(data.path) ? data.path ?? "" : "", "");
                if (!string.IsNullOrWhiteSpace(path)) data.path = path.Replace("\\", "/");
            }

            GUILayout.Space(10);
            if (GUILayout.Button("打开", GUILayout.Width(85), GUILayout.Height(28)))
            {
                ShowInExplorer(data.path);
            }

            GUILayout.Space(Width / 2 - 100);
        });
        GUILayout.Space(30);

        HorizontalRegion(() =>
        {
            ColorRegion(new Color(0f, 1f, 0.6f), () =>
            {
                GUILayout.Space(Width / 2 - 100);
                if (GUILayout.Button("导出", GUILayout.Width(185), GUILayout.Height(30))) DoExport();
            });
        });
    }

    private void DoExport()
    {
        Action onBuildDone = null;
        AssetDatabase.SaveAssets();
        SaveData();
        AssetDatabase.Refresh();
        if (data.path == null)
        {
            ShowNotification(new GUIContent("未选择路径"));
            return;
        }

        var buildPath = $"{data.path}/webgl";
        var hwSettingFolder = $"{buildPath}/settings";
        if (Directory.Exists(hwSettingFolder))
        {
            var dictionary = Directory.GetFiles(hwSettingFolder).ToDictionary(e => e, File.ReadAllText);

            onBuildDone += () =>
            {
                Mkdir(hwSettingFolder);
                foreach (var pair in dictionary) File.WriteAllText(pair.Key, pair.Value);
            };
        }

        Mkdir(buildPath);
        Cldir(buildPath);

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
            {
                ShowNotification(new GUIContent($"不支持{BuildTarget.WebGL}平台"));
                return;
            }
        }

        PlayerSettings.WebGL.template = "APPLICATION:Default";
        PlayerSettings.colorSpace = ColorSpace.Gamma;
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, new GraphicsDeviceType[]
        {
            GraphicsDeviceType.OpenGLES2
        });
#if UNITY_2021_1_OR_NEWER
        InvokeMethod("SetLightmapEncodingQualityForPlatformGroup", BuildTargetGroup.WebGL,
            Enum.Parse(AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .First(e => e.GetName().Name == "UnityEditor.CoreModule")
                    .GetType("UnityEditor.LightmapEncodingQuality"),
                "Normal"));
#endif
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.dataCaching = true;
        EditorUserBuildSettings.development = developmentBuild;
#if UNITY_2022_1_OR_NEWER
        PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2021_1_OR_NEWER
        EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif
        EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, buildPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
                .Where(e => e.enabled)
                .Select(e => e.path).ToArray(),
            target = BuildTarget.WebGL,
            targetGroup = BuildTargetGroup.WebGL,
            locationPathName = buildPath,
#if UNITY_2021_1_OR_NEWER
            options = cleanBuild ? BuildOptions.CleanBuildCache : BuildOptions.None,
#endif
        };


        var report = BuildPipeline.BuildPlayer(options);
        onBuildDone?.Invoke();
        if (report.summary.result == BuildResult.Succeeded)
        {
            ShowInExplorer(data.path);
            Debug.Log("构建成功!");
        }
        else ShowNotification(new GUIContent("构建失败"));
    }


    private HWWebGLExportData LoadData()
    {
        if (!File.Exists(SaveFilePath)) return new HWWebGLExportData();
        try
        {
            return JsonUtility.FromJson<HWWebGLExportData>(File.ReadAllText(SaveFilePath)) ?? new HWWebGLExportData();
        }
        catch (Exception e)
        {
            return new HWWebGLExportData();
        }
    }

    private void SaveData()
    {
        Mkdir(Path.GetDirectoryName(SaveFilePath));
        File.WriteAllText(SaveFilePath, JsonUtility.ToJson(data));
    }

    private void ShowInExplorer(string path)
    {
        if (!Directory.Exists(path))
        {
            ShowNotification(new GUIContent($"路径:\n{path}\n不存在"));
            return;
        }

        var process = Process.Start("Explorer.exe", path.Replace("/", "\\"));
        process?.Dispose();
    }

    private static object InvokeMethod(string methodName, params object[] args)
    {
        return typeof(PlayerSettings)
            .GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            ?.Invoke(null, args);
    }

    private static void Mkdir(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    private static void Cldir(string path)
    {
        foreach (var directory in Directory.GetDirectories(path)) Directory.Delete(directory, true);
        foreach (var file in Directory.GetFiles(path)) File.Delete(file);
    }

    protected static void HorizontalRegion(Action action)
    {
        if (action == null) return;
        GUILayout.BeginHorizontal();
        action.Invoke();
        GUILayout.EndHorizontal();
    }

    protected static void ColorRegion(Color color, Action action)
    {
        if (action == null) return;
        var pri = GUI.color;
        GUI.color = color;
        action.Invoke();
        GUI.color = pri;
    }
}

[Serializable]
internal class HWWebGLExportData
{
    public string path;
}