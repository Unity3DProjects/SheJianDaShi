using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.Build;
using System.IO.Compression;
using System.Collections.Generic;

namespace QGMiniGameCore
{
    public class QGEditorWindowNew : EditorWindow, IUnityCompatible
    {
        QGWindowHepler qGWindowHepler;

        [MenuItem("联盟快游戏 / 转换快游戏", false, 1)]
        public static void OpenWindow()
        {
            QGLog.Log("OpenWindow");
            var win = GetWindow(typeof(QGEditorWindowNew), false, "联盟快游戏转换工具面板");
            win.minSize = new Vector2(350, 400);
            win.Show();
            QGGameTools.CheckUpdate();
        }

        private void OnEnable()
        {
            OnInitEnv();
            EditorUtility.ClearProgressBar();
            qGWindowHepler = new QGWindowHepler(this);
            qGWindowHepler.SetUnityCompatible(this);
            qGWindowHepler.SetIsBuildRpk(true);
            QGLog.Log("OnEnable");
            qGWindowHepler.OnConfigSetting();
            if (!QGCoreEnv.QG_UNITY_2019_1_OR_NEWER)
            {
                QGLog.LogError("目前仅支持 Unity2019以上的版本！");
            }
        }

        // 打包构建可编辑面板UI
        private void OnGUI()
        {
            qGWindowHepler.OnGUISetting();
        }

        void IUnityCompatible.OnUseCodeSize(bool useCodeSize)
        {
            QGLog.Log("OnUseCodeSize useCodeSize " + useCodeSize + ", is UNITY_2021 " + QGCoreEnv.QG_UNITY_2021);
            if (!useCodeSize)
            {
                return;
            }
            // #if UNITY_2021_1_OR_NEWER
#if UNITY_2021
            PlayerSettings.colorSpace = ColorSpace.Gamma;
            EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif
        }
        void IUnityCompatible.OnZipFile(string zipOutPath, Dictionary<string, string> fileMap)
        {
            using (FileStream fs = new FileStream(zipOutPath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var pair in fileMap)
                {
                    string sourceFilePath = pair.Key;
                    string entryName = pair.Value;

                    if (!File.Exists(sourceFilePath))
                        throw new FileNotFoundException($"源文件不存在: {sourceFilePath}");

                    ZipArchiveEntry entry = archive.CreateEntry(entryName);
                    using (FileStream fileStream = File.OpenRead(sourceFilePath))
                    using (Stream entryStream = entry.Open())
                    {
                        fileStream.CopyTo(entryStream);
                    }
                }
            }
        }
        private void OnDisable()
        {
            QGWindowHepler.Instance.OnConfigUpdate();
        }

        public static void OnInitEnv()
        {
#if UNITY_2019
            QGCoreEnv.QG_UNITY_2019 = true;
#endif
#if UNITY_2019_1_OR_NEWER
            QGCoreEnv.QG_UNITY_2019_1_OR_NEWER = true;
#endif
#if UNITY_2020_1_OR_NEWER
            QGCoreEnv.QG_UNITY_2020_1_OR_NEWER = true;
#endif
#if UNITY_2020_3_OR_NEWER
            QGCoreEnv.QG_UNITY_2020_3_OR_NEWER = true;
#endif
#if UNITY_2021_1_OR_NEWER
            QGCoreEnv.QG_UNITY_2021_1_OR_NEWER = true;
#endif
#if TUANJIE_2022_3_OR_NEWER
            QGCoreEnv.QG_TUANJIE_2022_3_OR_NEWER = true;
#endif
#if UNITY_2021
            QGCoreEnv.QG_UNITY_2021 = true;
#endif
#if UNITY_2022
            QGCoreEnv.QG_UNITY_2022 = true;
#endif
        }

    }
}
