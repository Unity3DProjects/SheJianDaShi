using System;
using System.Collections.Generic;
using UnityEngine;

public static class TextureCache
{
	private static Dictionary<string, TextureProvider> providers = new Dictionary<string, TextureProvider>();

	public static bool HasIncompleteDownloads
	{
		get
		{
			foreach (TextureProvider value in providers.Values)
			{
				if (value.InProgress)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static string PlatformPath(string path)
	{
		path = path.Replace("2x", tk2dSystem.CurrentPlatform);
		path = path.Replace("4x", tk2dSystem.CurrentPlatform);
		return path;
	}

	public static Texture2D LoadTexture(string path, TextureDataSource source, TextureFormat format, bool mipmaps)
	{
		TextureProvider providerForPath = GetProviderForPath(path, source);
		return providerForPath.Load(path, format, mipmaps);
	}

	public static void LoadTextureAsync(string path, TextureDataSource source, TextureFormat format, bool mipmaps, Action<Texture2D> action)
	{
		TextureProvider providerForPath = GetProviderForPath(path, source);
		providerForPath.LoadAsync(path, format, mipmaps, action);
	}

	public static void UnloadTexture(string path)
	{
		TextureProvider value;
		if (providers.TryGetValue(path, out value))
		{
			value.UnloadTexture();
		}
	}

	public static void UnloadTexture(Texture2D texture)
	{
		if (texture == null)
		{
			return;
		}
		foreach (TextureProvider value in providers.Values)
		{
			if (value.Texture == texture)
			{
				value.UnloadTexture();
			}
		}
	}

	private static TextureProvider GetProviderForPath(string path, TextureDataSource source)
	{
		TextureProvider value;
		if (!providers.TryGetValue(path, out value))
		{
			switch (source)
			{
			case TextureDataSource.WWW:
				value = new TextureProviderWWW(path);
				break;
			case TextureDataSource.Resources:
				value = new TextureProviderResources(path);
				break;
			case TextureDataSource.StreamingAssets:
				value = new TextureProviderStream(path);
				break;
			}
			providers.Add(path, value);
		}
		return value;
	}
}
