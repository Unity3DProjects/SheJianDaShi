using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FFMpegCore;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.AssetsProssesor
{
    public class AudioClipProcessorWindow : BaseEditorWindow
    {
        private static List<AudioClipInfo> audioClipInfos = new List<AudioClipInfo>();
        private static List<AudioClipInfo> cachedAudioClipInfos = new List<AudioClipInfo>();
        private static string searchPath = "Assets/";
        private static Vector2 scrollPosition;
        private static float compressRatio = 0.5f;
        private static int filterSize = 0;

        [MenuItem("Tools/资源处理/音频处理器", priority = 106)]
        public static void ShowWindow() => ShowBigWindow<AudioClipProcessorWindow>();

        private void Awake()
        {
            Init();
        }

        private void OnGUI()
        {
            Space();
            searchPath = RenderFolderSelector("搜索路径", searchPath);
            if (cachedAudioClipInfos.Count > 0)
            {
                Space();
                RenderHorizontal(() =>
                {
                    compressRatio = RenderFloatSlider("压缩比例", compressRatio, 0, 1);
                    filterSize = RenderIntSlider("按大小过滤(kb)", filterSize, 0, cachedAudioClipInfos.First().size / 1024);
                });
            }

            Space();
            RenderHorizontal(() =>
            {
                RenderButton("搜索音频", () =>
                {
                    audioClipInfos = AssetDatabase
                        .FindAssets("t:AudioClip", new string[] { searchPath })
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(info =>
                        {
                            var audio = new AudioClipInfo();
                            audio.path = info;
                            audio.size = GetCompressedAudioSize(AssetDatabase.LoadAssetAtPath<AudioClip>(info));
                            return audio;
                        })
                        .OrderByDescending(info => info.size)
                        .ToList();

                    cachedAudioClipInfos = new List<AudioClipInfo>(audioClipInfos);
                });
                RenderButton("过滤音频",
                    () =>
                    {
                        audioClipInfos = cachedAudioClipInfos.Where(info => info.size > filterSize * 1024).ToList();
                    });
            });
            Space();
            RenderLine(Color.cyan);
            Space();
            RenderLabel(
                $"总音频数:{audioClipInfos.Count}  总大小:{EditorUtility.FormatBytes(audioClipInfos.Sum(info => info.size))}");
            Space();
            scrollPosition = RenderScrollView(scrollPosition, () =>
            {
                foreach (var info in audioClipInfos)
                {
                    RenderHorizontal(() =>
                    {
                        this.RenderAssetAtPath<AudioClip>(info.path);
                        RenderLabel($"{EditorUtility.FormatBytes(info.size)}", GUILayout.Width(100));
                        RenderButton("压缩",
                            () => { CompressAudioClips(new List<string>() { info.path }, compressRatio); },
                            GUILayout.Width(50));
                    });
                }
            });
            if (cachedAudioClipInfos.Count > 0)
            {
                Space();
                RenderButton("压缩所有音频", () =>
                {
                    ShowConfirmPanel(() =>
                    {
                        var audios = audioClipInfos.Select(info => info.path).ToArray();
                        CompressAudioClips(audios, compressRatio);
                    });
                });
            }
        }

        public static void CompressAudioClips(IEnumerable<string> arrey, float compressRatio)
        {
            AssetDatabase.StartAssetEditing();
            int c = 0, total = arrey.Count();
            CompressWithPercentage(arrey, compressRatio, (path, result) =>
            {
                EditorUtility.DisplayProgressBar(
                    "开始处理",
                    $"{c + 1}/{total}",
                    (float)c / total);
                c++;
                if (c == total)
                {
                    AssetDatabase.StopAssetEditing();
                    EditorUtility.ClearProgressBar();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                if (!result) return;
                if (Path.GetExtension(path) != ".ogg") File.Delete(path);
            });
        }


        private static int GetCompressedAudioSize(AudioClip clip)
        {
            string path = AssetDatabase.GetAssetPath(clip);
            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer != null)
            {
                var auidoImporter = typeof(AudioImporter);
                var compSizeInfo =
                    auidoImporter.GetProperty("compSize", BindingFlags.NonPublic | BindingFlags.Instance);
                if (compSizeInfo != null)
                {
                    var compSize = compSizeInfo.GetValue(importer);
                    return (int)compSize;
                }
            }

            return 0;
        }

        private static void Init()
        {
            var info = new DirectoryInfo(Application.dataPath);
            var folder = info.GetDirectories("FFMpegCore", SearchOption.AllDirectories).First();
            GlobalFFOptions.Configure(options => options.BinaryFolder = folder.FullName);
        }

        private static async void CompressWithPercentage(IEnumerable<string> filePaths, float percentage,
            Action<string, bool> onProcess)
        {
            onProcess += RenameMeta;
            onProcess += ReplaceAudio;
            foreach (var filePath in filePaths)
            {
                var _path = filePath;

                bool result = false;
                try
                {
                    var analysis = await FFProbe.AnalyseAsync(filePath);

                    var time = (long)(analysis.Duration.Ticks * percentage);
                    result = await Compress(filePath, new TimeSpan(time));
                }
                finally
                {
                    onProcess?.Invoke(filePath, result);
                }
            }

            Debug.Log("音频处理完成！");
        }

        private static async Task<bool> Compress(string path, TimeSpan duration)
        {
            if (duration.TotalSeconds < 0.15f)
            {
                Debug.LogError($"<{Path.GetFileName(path)}> 压缩失败,文件已经无法继续压缩!");
                return false;
            }

            var outputPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(path) + ".ogg");
            return await FFMpegArguments
                .FromFileInput(Path.GetFullPath(path))
                .OutputToFile(Path.GetFullPath(outputPath), File.Exists(outputPath),
                    options =>
                        options
                            .Seek(TimeSpan.Zero)
                            .WithDuration(duration)
                            .WithAudioCodec("libvorbis")
                            .WithAudioSamplingRate(16000)
                )
                .ProcessAsynchronously();
        }

        private static void RenameMeta(string path, bool result)
        {
            if (!result) return;
            var metaPath = $"{path}.meta";
            if (!File.Exists(metaPath)) return;
            var outputPath = Path.Combine(Path.GetDirectoryName(path),
                Path.GetFileNameWithoutExtension(path) + ".ogg.meta");
            if (File.Exists(outputPath)) return;
            File.Move(metaPath, outputPath);
        }

        private static void ReplaceAudio(string path, bool result)
        {
            if (!result) return;
            var tmpFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(path) + ".ogg");
            var targetFile = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty,
                Path.GetFileNameWithoutExtension(path) + ".ogg");
            if (!File.Exists(tmpFile)) return;
            if (File.Exists(targetFile)) File.Delete(targetFile);
            File.Move(tmpFile, targetFile);
        }
    }

    public class AudioClipInfo
    {
        public string path;

        public int size;
    }
}