using System;
using UnityEngine;

public abstract class TextureProvider
{
	protected string url;

	protected Texture2D texture;

	protected TextureState state;

	protected CallbackCache<Texture2D> callbacks = new CallbackCache<Texture2D>();

	public Texture2D Texture
	{
		get
		{
			return texture;
		}
	}

	public bool IsLoaded
	{
		get
		{
			return state == TextureState.Loaded;
		}
	}

	public bool InProgress
	{
		get
		{
			return state == TextureState.Loading;
		}
	}

	protected TextureProvider(string path)
	{
		url = path;
	}

	public abstract Texture2D Load(string path, TextureFormat format, bool mipmaps);

	public abstract void LoadAsync(string path, TextureFormat format, bool mipmaps, Action<Texture2D> callback);

	public abstract void UnloadTexture();
}
