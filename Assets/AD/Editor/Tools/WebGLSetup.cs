using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

public class WebGLSetup
{
    [MenuItem("Tools/AD/设置WebGL", false, int.MaxValue)]
    public static void Set()
    {
        SetPlayerSetting();
    }

    private static void SetPlayerSetting()
    {
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
#if UNITY_2022_1_OR_NEWER
        PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2021_1_OR_NEWER
        EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static object InvokeMethod(string methodName, params object[] args)
    {
        return typeof(PlayerSettings)
            .GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            ?.Invoke(null, args);
    }
}