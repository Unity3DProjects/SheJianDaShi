#if EXIST_AD_TOOLBAR
using System;
using System.IO;
using System.Linq;
using AD.Editor.Macro;
using ToolbarExtension;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AD.Editor.ToolBar
{
    public class ADToolBar
    {
        public const string GitURL = @"http://192.168.1.66:233/aabb/MiniGameSDK.git";
        public const string BackUpGitURL = @"http://101.42.134.168:9091/Unity3D/MiniGameSDK.git#upm";

        [Toolbar(OnGUISide.Left, 300)]
        private static void RenderLeft()
        {
#if UNITY_2021_1_OR_NEWER
            GUILayout.Space(480);
#else
            GUILayout.Space(100);
#endif
            GUILayout.BeginHorizontal();
            var origin = GUI.color;
            GUI.color = new Color(1f, 0.5f, 0.7f);
            GUILayout.Label($"RPK{ADEditorConst.VersionString}", GUILayout.Height(20));


            GUI.color = origin;

            var origin2 = GUI.color;
            GUI.color = new Color(0.3f, 0.7f, 1f);

            RenderCheckUpdate();

#if UNITY_2021_1_OR_NEWER
            if (GUILayout.Button("导入AD"))
#else
            if (GUILayout.Button("导入AD", GUILayout.Width(80)))
#endif
            {
                if (!TryImportAdPackage(out var path))
                {
                    CheckUpdate(GitURL);
                }
            }

            RenderImportSDK();
            GUI.color = origin;

#if !UNITY_2021_1_OR_NEWER
            GUILayout.Space(300);
#endif
            GUILayout.EndHorizontal();
        }

        [Toolbar(OnGUISide.Right, 111)]
        private static void RenderRight()
        {
            OnExport();
        }

        private static AddRequest request;

        #region Impl

        private static void RenderCheckUpdate()
        {
#if UNITY_2021_1_OR_NEWER
            if (GUILayout.Button("检查更新"))
#else
            if (GUILayout.Button("检查更新", GUILayout.Width(80)))
#endif
            {
                CheckUpdate(GitURL);
            }
        }

        private static void RenderImportSDK()
        {
            var platform = MacroToggleWindow.GetCurrentPlatform();
            if (platform == null) return;
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return;
                default: break;
            }
#if UNITY_2021_1_OR_NEWER
            if (GUILayout.Button("导入SDK"))
#else
            if (GUILayout.Button("导入SDK", GUILayout.Width(85)))
#endif
            {
                var resPath = $"AD_SDK_{platform.Value.ToString()}";
                var path = AssetDatabase
                    .FindAssets(resPath, new[] {"Packages"})
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(t => t.EndsWith(".unitypackage", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault(t => Path.GetFileNameWithoutExtension(t).Contains(resPath));

                if (!string.IsNullOrWhiteSpace(path))
                {
                    AssetDatabase.ImportPackage(path, true);
                }
                else
                {
                    Debug.LogError("未找到SDK! 请先检查更新");
                }
            }
        }

        private static void OnExport()
        {
            var origin = GUI.color;
            GUI.color = Color.green;
#if UNITY_2021_1_OR_NEWER
            if (GUILayout.Button("导出包体"))
#else
            if (GUILayout.Button("导出包体", GUILayout.Width(80)))
#endif

            {
#if VIVO
                EditorApplication.ExecuteMenuItem("VIVO小游戏/转换小游戏");
#elif OPPO
                EditorApplication.ExecuteMenuItem("OPPO小游戏/打包工具");
#elif RongYao
                EditorApplication.ExecuteMenuItem("联盟快游戏/转换快游戏");
#elif HuaWei
                EditorApplication.ExecuteMenuItem("Tools/导出华为WebGL");
#elif XiaoMi
                EditorApplication.ExecuteMenuItem("小米快游戏/转换快游戏");
#else
                Debug.Log("当前平台不支持快捷打开导出面板");
#endif
            }

            GUI.color = origin;
        }

        #endregion

        private static void CheckUpdate(string requestURL)
        {
            if (request != null && !request.IsCompleted)
            {
                Debug.LogError("请勿重复请求,检查更新中,请稍等...");
                return;
            }

            Debug.Log($"检查更新中,请稍等...");
            request = Client.Add(requestURL);
            EditorApplication.update += OnCheckUpdate;
        }

        private static void OnCheckUpdate()
        {
            if (request == null) return;
            if (request.IsCompleted)
            {
                EditorApplication.update -= OnCheckUpdate;
                if (request.Status == StatusCode.Success)
                {
                    Debug.Log($"最新版本号：{request.Result.name} {request.Result.version}");
                }
                else
                {
                    CheckUpdate(BackUpGitURL);
                    Debug.LogError("更新请求失败!");
                    Debug.Log($"请尝试手动安装:{GitURL}");
                }
            }
        }

        private static bool TryImportAdPackage(out string path)
        {
            path = AssetDatabase
                .FindAssets("AD_RPK", new[] {"Packages"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(t => t.EndsWith(".unitypackage", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault(t => Path.GetFileNameWithoutExtension(t).Contains("AD_RPK"));

            var canImport = !string.IsNullOrEmpty(path);
            if (canImport)
                AssetDatabase.ImportPackage(path, true);
            return canImport;
        }
    }
}
#endif