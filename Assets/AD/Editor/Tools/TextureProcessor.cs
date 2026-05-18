using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    [InitializeOnLoad]
    internal class TextureProcessor
    {
        static TextureProcessor()
        {
            EditorApplication.projectChanged += OnProjectUpdate;
        }

        private const string Root = "Assets/_Sprites";

        private const string Platform = "WebGL";
        private const float Interval = 0.35f;

        private static readonly ISet<string> ImageExtension = new HashSet<string>()
        {
            ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".tiff", ".psd"
        };

        private static double updateTime;

        private static void OnProjectUpdate()
        {
            updateTime = EditorApplication.timeSinceStartup + Interval;
            EditorApplication.delayCall -= UpdateRoot;
            EditorApplication.delayCall += UpdateRoot;
        }


        private static void UpdateRoot()
        {
            if (EditorApplication.timeSinceStartup < updateTime)
            {
                EditorApplication.delayCall -= UpdateRoot;
                EditorApplication.delayCall += UpdateRoot;
                return;
            }

            if (!Directory.Exists(Root)) return;
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (var assetPath in Directory
                    .GetFiles("Assets/_Sprites", "*", SearchOption.AllDirectories)
                    .Where(e => ImageExtension.Contains(Path.GetExtension(e).ToLower()))
                    .Select(e => e.Replace("\\", "/"))
                    .Distinct()
                ) OnPreprocessTexture(assetPath);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        private static void OnPreprocessTexture(string assetPath)
        {
            if (string.IsNullOrWhiteSpace(assetPath)) return;
            if (!(AssetImporter.GetAtPath(assetPath) is TextureImporter textureImporter)) return;

            // 先检查是否有alpha通道（这个检查比较快）
            bool hasAlpha = textureImporter.DoesSourceTextureHaveAlpha();

            // 获取或创建平台设置
            var settings = textureImporter.GetPlatformTextureSettings(Platform);

            // 检查当前设置是否已经符合要求，避免不必要的处理
            if (ShouldSkipProcessing(textureImporter, settings, hasAlpha))
                return;

            // 应用设置
            ApplySettings(textureImporter, settings, hasAlpha);
            textureImporter.SaveAndReimport();
        }

        private static bool ShouldSkipProcessing(TextureImporter importer,
            TextureImporterPlatformSettings settings,
            bool hasAlpha)
        {
            return importer.textureType == TextureImporterType.Sprite &&
                   importer.spriteImportMode == SpriteImportMode.Single &&
                   !importer.mipmapEnabled &&
                   settings.overridden &&
                   settings.compressionQuality <= 40 &&
                   settings.format ==
                   (hasAlpha ? TextureImporterFormat.DXT5Crunched : TextureImporterFormat.DXT1Crunched);
        }

        private static void ApplySettings(TextureImporter importer,
            TextureImporterPlatformSettings settings,
            bool hasAlpha)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;

            settings.name = Platform;
            settings.overridden = true;
            settings.compressionQuality = Mathf.Clamp(settings.compressionQuality, 0, 40);
            settings.format = hasAlpha
                ? TextureImporterFormat.DXT5Crunched
                : TextureImporterFormat.DXT1Crunched;

            importer.SetPlatformTextureSettings(settings);
        }
    }
}