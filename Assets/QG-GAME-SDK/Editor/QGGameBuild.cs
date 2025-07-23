using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace QGMiniGameCore
{
    public class QGGameBuild : IUnityCompatible
    {

        private static QGGameBuild instance;
        public static QGGameBuild Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QGGameBuild();
                }
                return instance;
            }
        }

        void IUnityCompatible.OnUseCodeSize(bool useCodeSize)
        {
            QGLog.Log("OnUseCodeSize useCodeSize " + useCodeSize + ", is UNITY_2021 " + QGCoreEnv.QG_UNITY_2021);
            if (!useCodeSize)
            {
                return;
            }
            // #if UNITY_2021_1_OR_NEWER
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

        public void DoBuild()
        {
            QGEditorWindowNew.OnInitEnv();
            QGWindowHepler.Instance.SetUnityCompatible(Instance);
            QGWindowHepler.Instance.SetIsBuildRpk(true);
            QGWindowHepler.Instance.OnConfigSetting();
            QGWindowHepler.Instance.DoBuildCMD();
        }
    }
}
