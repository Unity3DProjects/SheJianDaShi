using UnityEditor;

namespace AD.Editor.Tools
{
    public class TextureProsesser : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if (assetPath.Contains("_Arts") || assetPath.Contains("AgreementHealth") || assetPath.Contains("_Sprites"))
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.mipmapEnabled = false;
                TextureImporterPlatformSettings settings = new TextureImporterPlatformSettings();
                settings.name = "WebGL";
                settings.overridden = true;
                settings.compressionQuality = 40;
                settings.format = TextureImporterFormat.DXT5Crunched;


                if (textureImporter.DoesSourceTextureHaveAlpha())
                {
                    textureImporter.SetPlatformTextureSettings(settings);
                }
                else
                {
                    settings.format = TextureImporterFormat.DXT1Crunched;
                    textureImporter.SetPlatformTextureSettings(settings);
                }
            }
        }
    }
}