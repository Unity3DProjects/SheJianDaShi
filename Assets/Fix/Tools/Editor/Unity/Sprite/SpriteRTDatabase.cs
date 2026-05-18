using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Fix.Editor
{
    internal static class SpriteRTDatabase
    {
        private const string OutputFileName = "RuntimeSpriteData.json";
        private static string Folder => nameof(SpriteFix).GetWorkspaceFolder();
        private static string OutputFilePath => $"{Folder}/{OutputFileName}";

        [Serializable]
        internal class SpriteRuntimeFile
        {
            public string unityVersion;
            public string productName;
            public string createdAt;
            public List<SpriteRuntimeData> sprites = new List<SpriteRuntimeData>();
        }

        [Serializable]
        internal class SpriteRuntimeData
        {
            public string name;
            public string textureName;
            public Rect rect;
            public Vector4 border;
            public Vector2 pivot;
            public float pixelsPerUnit;
            public bool packed;
            public SpritePackingMode packingMode;
            public SpritePackingRotation packingRotation;
            public Vector2[] vertices;
            public Vector2[] uv;
            public int[] triangles;
        }

        private static string lastLoadedPath;
        private static DateTime lastWriteTimeUtc;
        private static SpriteRuntimeFile cachedFile;
        private static Dictionary<string, List<SpriteRuntimeData>> spriteNameToData;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            TrySyncOutputPath();
        }

        public static bool HasDataFile() => File.Exists(OutputFilePath);

        public static bool TryGetData(Sprite sprite, out SpriteRuntimeData data)
        {
            data = null;
            if (sprite == null) return false;
            EnsureLoaded();
            if (spriteNameToData == null) return false;
            if (!spriteNameToData.TryGetValue(sprite.name, out var candidates) || candidates.Count == 0) return false;

            if (candidates.Count == 1)
            {
                data = candidates[0];
                return true;
            }

            data = candidates
                .OrderByDescending(candidate => GetMatchScore(sprite, candidate))
                .FirstOrDefault();
            return data != null;
        }

        private static void EnsureLoaded()
        {
            string path = OutputFilePath;
            if (!File.Exists(path))
            {
                cachedFile = null;
                spriteNameToData = null;
                lastLoadedPath = path;
                lastWriteTimeUtc = default;
                return;
            }

            var writeTimeUtc = File.GetLastWriteTimeUtc(path);
            if (cachedFile != null && lastLoadedPath == path && writeTimeUtc == lastWriteTimeUtc) return;

            var content = File.ReadAllText(path);
            cachedFile = JsonUtility.FromJson<SpriteRuntimeFile>(content);
            spriteNameToData = cachedFile?.sprites?
                .Where(e => e != null && !string.IsNullOrEmpty(e.name))
                .GroupBy(e => e.name)
                .ToDictionary(e => e.Key, e => e.ToList());
            lastLoadedPath = path;
            lastWriteTimeUtc = writeTimeUtc;
        }

        private static float GetMatchScore(Sprite sprite, SpriteRuntimeData candidate)
        {
            float score = 0f;
            if (candidate == null) return float.MinValue;
            if (candidate.textureName == sprite.texture?.name) score += 1024f;
            if (candidate.packed == sprite.packed) score += 256f;
            if (candidate.packingMode == sprite.packingMode) score += 64f;
            if (candidate.packingRotation == sprite.packingRotation) score += 32f;
            score -= Mathf.Abs(candidate.rect.width - sprite.rect.width) * 4f;
            score -= Mathf.Abs(candidate.rect.height - sprite.rect.height) * 4f;
            score -= (candidate.pivot - sprite.pivot).sqrMagnitude * 0.05f;
            score -= (candidate.border - sprite.border).sqrMagnitude * 0.01f;
            score -= Mathf.Abs((candidate.vertices?.Length ?? 0) - sprite.vertices.Length) * 0.5f;
            score -= Mathf.Abs((candidate.triangles?.Length ?? 0) - sprite.triangles.Length) * 0.25f;
            return score;
        }

        internal static void PingOutputScript()
        {
            var scriptPath = AssetDatabase.FindAssets($"t:MonoScript {nameof(SpriteRTOut)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(scriptPath)) return;
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath));
        }

        private static void TrySyncOutputPath()
        {
            var scriptPath = AssetDatabase.FindAssets($"t:MonoScript {nameof(SpriteRTOut)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(scriptPath)) return;

            var text = File.ReadAllText(scriptPath);
            var folder = Folder;
            var replaced = Regex.Replace(text, @"(?<=private const string OutputPath \= \@"")(.*?)(?=""\;)",
                Path.GetFullPath(folder));
            if (replaced == text) return;
            folder.Mkdir();
            File.WriteAllText(scriptPath, replaced);
        }
    }
}