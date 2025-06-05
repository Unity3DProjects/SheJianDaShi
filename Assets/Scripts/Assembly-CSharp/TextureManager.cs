using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TextureManager
{
	public struct TextureInfo
	{
		public bool exist;

		public int width;

		public int height;
	}

	private static bool initialized = false;

	private static Dictionary<IntPtr, Texture2D> loadingTextures = new Dictionary<IntPtr, Texture2D>();

	private static bool isExistExternalLoader = false;

	private static ITextureLoader externalLoader = null;

	public static void RegisterLoader(ITextureLoader newLoader)
	{
		if (newLoader != null)
		{
			if (isExistExternalLoader)
			{
				externalLoader.Deinit();
			}
			externalLoader = newLoader;
			initialized = false;
		}
	}

	public static Texture2D LoadTexture(string imagePath, bool doMipMaps)
	{
		Initialize();
		Texture2D text = null;
		if (isExistExternalLoader)
		{
			TextureInfo info = externalLoader.GetInfo(imagePath);
			if (info.exist)
			{
				IntPtr intPtr = externalLoader.LoadTexture(imagePath, doMipMaps);
				if (intPtr != IntPtr.Zero)
				{
					text = Texture2D.CreateExternalTexture(info.width, info.height, TextureFormat.RGBA32, doMipMaps, false, intPtr);
				}
			}
		}
		else
		{
			text = LoadTextureInternal(imagePath, doMipMaps);
		}
		return SetupTexture(text, doMipMaps);
	}

	public static Texture2D LoadTextureAsync(string imagePath, bool doMipMaps, int preloadWidth = 4, int preloadHeight = 4)
	{
		Initialize();
		Texture2D texture2D = null;
		if (isExistExternalLoader)
		{
			TextureInfo info = externalLoader.GetInfo(imagePath);
			if (info.exist)
			{
				lock (loadingTextures)
				{
					IntPtr intPtr = externalLoader.LoadTextureAsync(imagePath, doMipMaps);
					if (intPtr != IntPtr.Zero)
					{
						texture2D = Texture2D.CreateExternalTexture(info.width, info.height, TextureFormat.RGBA32, doMipMaps, false, intPtr);
						loadingTextures.Add(intPtr, texture2D);
					}
				}
			}
		}
		else
		{
			texture2D = LoadTextureInternalAsync(imagePath, doMipMaps);
		}
		return SetupTexture(texture2D, doMipMaps);
	}

	public static void UnloadTexture(Texture2D texture)
	{
		if (!texture)
		{
			return;
		}
		IntPtr nativeTexturePtr = texture.GetNativeTexturePtr();
		if (!(nativeTexturePtr != IntPtr.Zero))
		{
			return;
		}
		lock (loadingTextures)
		{
			Texture2D value = null;
			if (loadingTextures.TryGetValue(nativeTexturePtr, out value))
			{
				loadingTextures.Remove(nativeTexturePtr);
			}
			if (isExistExternalLoader)
			{
				externalLoader.ReleaseTexture(nativeTexturePtr);
			}
		}
	}

	private static void Initialize()
	{
		if (!initialized)
		{
			if (externalLoader != null)
			{
				isExistExternalLoader = true;
			}
			if (isExistExternalLoader)
			{
				externalLoader.Init(OnTextureLoad, OnTextureFail);
			}
			initialized = true;
		}
	}

	private static Texture2D LoadTextureInternal(string imagePath, bool doMipMaps)
	{
		Texture2D texture2D = null;
		IntPtr nativeTexturePtr = Texture2D.blackTexture.GetNativeTexturePtr();
		texture2D = Texture2D.CreateExternalTexture(4, 4, TextureFormat.RGBA32, doMipMaps, false, nativeTexturePtr);
		using (AssetStreaming assetStreaming = new AssetStreaming(imagePath))
		{
			WWW wWW = assetStreaming.GetWWW();
			if (wWW != null)
			{
				wWW.LoadImageIntoTexture(texture2D);
			}
		}
		return texture2D;
	}

	private static Texture2D LoadTextureInternalAsync(string imagePath, bool doMipMaps, int preloadWidth = 4, int preloadHeight = 4)
	{
		Texture2D texture2D = null;
		IntPtr texture = UnityEngine.Object.Instantiate(Texture2D.blackTexture).GetNativeTexturePtr();
		if (texture != IntPtr.Zero)
		{
			lock (loadingTextures)
			{
				texture2D = Texture2D.CreateExternalTexture(preloadWidth, preloadHeight, TextureFormat.RGBA32, doMipMaps, false, texture);
				loadingTextures.Add(texture, texture2D);
				AssetStreaming streamTexture = new AssetStreaming(imagePath);
				streamTexture.GetWWWAsync(delegate(WWW wwwTexture)
				{
					if (wwwTexture != null)
					{
						Texture2D value = null;
						if (loadingTextures.TryGetValue(texture, out value))
						{
							loadingTextures.Remove(texture);
							if ((bool)value)
							{
								wwwTexture.LoadImageIntoTexture(value);
							}
						}
						wwwTexture.Dispose();
					}
					else
					{
						loadingTextures.Remove(texture);
					}
					streamTexture.Unload();
				});
			}
		}
		return texture2D;
	}

	private static Texture2D LoadTextureEditor(string imagePath, bool doMipMaps)
	{
		Texture2D texture2D = new Texture2D(4, 4, TextureFormat.RGBA32, doMipMaps);
		byte[] array = null;
		if (File.Exists(imagePath))
		{
			array = File.ReadAllBytes(imagePath);
		}
		else
		{
			texture2D = null;
			CustomDebug.LogError("File <" + imagePath + "> does not exist");
		}
		if (array == null || !texture2D.LoadImage(array))
		{
			if (array == null)
			{
				CustomDebug.LogError("Image bytes is null");
			}
			else
			{
				CustomDebug.Log("Can't load image");
			}
			texture2D = null;
		}
		return texture2D;
	}

	private static Texture2D SetupTexture(Texture2D text, bool doMipMaps)
	{
		Texture2D texture2D = text;
		if (texture2D == null)
		{
			texture2D = new Texture2D(4, 4, TextureFormat.RGBA32, doMipMaps, false);
		}
		if (texture2D != null)
		{
			texture2D.hideFlags = HideFlags.DontSave;
			texture2D.wrapMode = TextureWrapMode.Clamp;
		}
		return texture2D;
	}

	private static void OnTextureLoad(IntPtr textureID, IntPtr newTexturePointer)
	{
		lock (loadingTextures)
		{
			if (!(newTexturePointer != IntPtr.Zero))
			{
				return;
			}
			Texture2D value = null;
			if (loadingTextures.TryGetValue(textureID, out value))
			{
				loadingTextures.Remove(textureID);
				if ((bool)value)
				{
					value.UpdateExternalTexture(newTexturePointer);
				}
			}
		}
	}

	private static void OnTextureFail(IntPtr textureID, IntPtr newTexturePointer)
	{
		lock (loadingTextures)
		{
			Texture2D value = null;
			if (loadingTextures.TryGetValue(textureID, out value))
			{
				loadingTextures.Remove(textureID);
			}
		}
	}
}
