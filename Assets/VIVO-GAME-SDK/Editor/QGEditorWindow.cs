using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace QGMiniGame
{
    public class QGEditorWindowNew : EditorWindow
    {

        [MenuItem("VIVO小游戏 / 转换小游戏", false, 1)]
        public static void OpenWindow()
        {
#if !(UNITY_2019_1_OR_NEWER )
            QGLog.LogError("目前仅支持 Unity2019以上的版本！");
#endif
            var win = GetWindow(typeof(QGEditorWindowNew), false, "vivo小游戏转换工具面板");
            win.minSize = new Vector2(350, 400);
            win.Show();
            QGGameTools.CheckUpdate();
        }

        public QGGameConfig qgConfig;

        private Vector2 scrollRoot;

        private void OnEnable()
        {
            QGLog.Log("OnEnable");
            OnConfigSetting();
        }

        private void OnFocus()
        {
            //QGLog.Log("OnFocus");

        }

        // 打包构建可编辑面板UI
        private void OnGUI()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot);
            QGSettingsHelperInterface.helper.OnSettingsGUI(this);
            OnBuildButtonGUI();
            QGSettingsHelperInterface.helper.OnOtherGUI(this);
            EditorGUILayout.EndScrollView();
        }

        private void OnConfigSetting()
        {
            if (qgConfig == null)
            {
                qgConfig = QGGameTools.GetEditorConfig();
            }

            // 支持CP手动修改webgl_vivo/src/manifest.json小游戏配置时更新插件面板
            QGGameTools.CheckConfigByManifestChanged(qgConfig);

            // 构建产物目录
            QGSettingsHelperInterface.helper.SetBuildSrc(qgConfig.buildSrc);
            // 基本信息
            QGSettingsHelperInterface.helper.SetBgImageSrc(qgConfig.envConfig.bgImageSrc);
            QGSettingsHelperInterface.helper.SetIcon(qgConfig.envConfig.icon);
            QGSettingsHelperInterface.helper.SetPackage(qgConfig.envConfig.package);
            QGSettingsHelperInterface.helper.SetName(qgConfig.envConfig.name);
            QGSettingsHelperInterface.helper.SetVersionCode(qgConfig.envConfig.versionCode);
            QGSettingsHelperInterface.helper.SetVersionName(qgConfig.envConfig.versionName);
            QGSettingsHelperInterface.helper.SetMinPlatformVersion(qgConfig.envConfig.minPlatformVersion);
            QGSettingsHelperInterface.helper.SetDeviceOrientation(qgConfig.envConfig.deviceOrientation);

            // 注：CDN资源服务器加载和vivo分包加载只能二选一
            if (qgConfig.useSelfLoading)  // CDN
            {
                QGSettingsHelperInterface.helper.SetUseSelfLoading();
                QGSettingsHelperInterface.helper.SetWasmUrl(qgConfig.envConfig.wasmUrl);
            }
            else if (qgConfig.useSubPkgLoading) // vivo分包
            {
                QGSettingsHelperInterface.helper.SetUseSubPkgLoading();
            }

            //
            if (qgConfig.useAddressable)
            {
                QGSettingsHelperInterface.helper.SetStreamingAssetsUrl(qgConfig.envConfig.streamingAssetsUrl);
                QGSettingsHelperInterface.helper.SetUseAddressable();
            }

            // 
            if (qgConfig.usePreAsset)
            {
                QGSettingsHelperInterface.helper.SetPreLoadUrls(qgConfig.envConfig.preloadUrl);
            }

            QGSettingsHelperInterface.helper.SetIsUseWebgl2(qgConfig.useWebgl2);
            QGSettingsHelperInterface.helper.SetIsUseCodeSize(qgConfig.useCodeSize);

            //背景图片设置方式
            QGSettingsHelperInterface.helper.SetUsTargetBgType(qgConfig.usTargetBgType);
        }

        private void OnConfigUpdate()
        {
            QGLog.Log("OnConfigUpdate");
            if (qgConfig == null)
            {
                qgConfig = QGGameTools.GetEditorConfig();
            }
            qgConfig.buildSrc = QGSettingsHelperInterface.helper.GetBuildSrc();

            // 基本信息
            qgConfig.envConfig.bgImageSrc = QGSettingsHelperInterface.helper.GetBgImageSrc();
            qgConfig.envConfig.icon = QGSettingsHelperInterface.helper.GetIcon();
            qgConfig.envConfig.package = QGSettingsHelperInterface.helper.GetPackage();
            qgConfig.envConfig.name = QGSettingsHelperInterface.helper.GetName();
            qgConfig.envConfig.versionCode = QGSettingsHelperInterface.helper.GetVersionCode();
            qgConfig.envConfig.versionName = QGSettingsHelperInterface.helper.GetVersionName();
            qgConfig.envConfig.minPlatformVersion = QGSettingsHelperInterface.helper.GetMinPlatformVersion();
            qgConfig.envConfig.deviceOrientation = QGSettingsHelperInterface.helper.GetDeviceOrientation();

            // 首包资源加载方式（原文档：自定义Loading及Unity分包配置），注：CDN资源服务器加载和vivo分包加载只能二选一
            // 文档地址：https://minigame.vivo.com.cn/documents/#/lesson/engine/unity/engine-unity-loading
            // 即wasm资源，对应以下三种方式：
            // 1 IsUseSelfLoading:CDN，即，CP使用自己的CDN资源地址
            // 2 IsUseSubPkgLoading：vivo分包，即，使用vivo分包资源，见文档解释
            // 3 默认方式：小游戏包内，即，资源直接打包在游戏包体内，该方式的rpk包体会很大
            qgConfig.useSelfLoading = QGSettingsHelperInterface.helper.IsUseSelfLoading();
            qgConfig.useSubPkgLoading = QGSettingsHelperInterface.helper.IsUseSubPkgLoading();
            qgConfig.envConfig.wasmUrl = QGSettingsHelperInterface.helper.GetWasmUrl();

            // Addressable/AssetBundle教程
            // 文档地址：https://minigame.vivo.com.cn/documents/#/lesson/engine/unity/engine-unity-addressable
            qgConfig.useAddressable = QGSettingsHelperInterface.helper.IsUseAddressable();
            qgConfig.envConfig.streamingAssetsUrl = QGSettingsHelperInterface.helper.GetStreamingAssetsUrl();

            // 预下载文件列表
            String preLoadUrls = QGSettingsHelperInterface.helper.GetPreLoadUrls();
            qgConfig.envConfig.preloadUrl = preLoadUrls;
            qgConfig.usePreAsset = preLoadUrls != String.Empty;

            qgConfig.useWebgl2 = QGSettingsHelperInterface.helper.IsUseWebgl2();
            qgConfig.useCodeSize = QGSettingsHelperInterface.helper.IsUseCodeSize();
            //背景图片设置方式
            qgConfig.usTargetBgType = QGSettingsHelperInterface.helper.GetUsTargetBgType();
        }

        private void OnBuildButtonGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.MinWidth(10));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("生成并转换"), GUILayout.Width(100), GUILayout.Height(25)))
            {
                OnConfigUpdate();
                DoBuild();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();
        }

        // 构建webgl
        public void DoBuild()
        {
            QGLog.Log("DoBuild");

            QGGameTools.SaveEditorConfigLocal(qgConfig);

            if (qgConfig.buildSrc == String.Empty)
            {
                ShowNotification(new GUIContent("请先选择游戏导出路径"));
            }
            else
            {
                // 判断是否是webgl平台
                if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
                {
                    if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
                    {
                        ShowNotification(new GUIContent("构建失败，请配置unity webgl构建环境"));
                        return;
                    }
                }

                if (qgConfig.useSelfLoading)
                {

                    if (qgConfig.envConfig.wasmUrl == String.Empty)
                    {
                        ShowNotification(new GUIContent("构建失败，首包资源加载方式选择CDN时，请配置CDN资源地址"));
                        return;
                    }
                }

                if (qgConfig.useSelfLoading && qgConfig.useSubPkgLoading)
                {
                    ShowNotification(new GUIContent("构建失败，只能选择一种自定义Loading加载方式"));
                    return;
                }

                QGGameTools.SetPlayer(qgConfig.useWebgl2, qgConfig.useCodeSize);
                QGGameTools.DelectDir(qgConfig.GetWebGlPath());

                QGGameTools.BuildWebGL(qgConfig.GetWebGlPath());

                if (!Directory.Exists(qgConfig.GetWebGlPath()))
                {
                    ShowNotification(new GUIContent("构建失败，WebGl项目未成功生成"));
                    return;
                }
                QGGameTools.CreateEnvConfig(qgConfig);
                QGGameTools.ConvetWebGl(qgConfig);
            }
        }

    }
}
