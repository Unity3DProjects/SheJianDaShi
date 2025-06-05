using System;
using UnityEngine;

public class TextureProviderStream : TextureProvider
{
	private string loadingKey;

	public TextureProviderStream(string path)
		: base(path)
	{
	}

	public override Texture2D Load(string path, TextureFormat format, bool mipmaps)
	{
		if (state == TextureState.Loading)
		{
			StopLoading();
		}
		if (state != TextureState.Loaded || !texture)
		{
			texture = TextureHelper.Instance.LoadImageToTexture(path, mipmaps, format);
			state = TextureState.Loaded;
		}
		return texture;
	}

	public override void LoadAsync(string path, TextureFormat format, bool mipmaps, Action<Texture2D> callback)
	{
		if (state == TextureState.Loaded)
		{
			callback(texture);
		}
		else
		{
			if (state != TextureState.NotLoaded)
			{
				return;
			}
			state = TextureState.Loading;
			callbacks.AddCallback(path, callback);
			loadingKey = TextureHelper.Instance.LoadImageToTextureAsync(path, delegate(Texture2D tex)
			{
				if (state == TextureState.Loading)
				{
					texture = tex;
					state = TextureState.Loaded;
					loadingKey = null;
					if (!callbacks.Call(path, texture))
					{
						UnloadTexture();
					}
				}
			}, mipmaps, format);
		}
	}

	public override void UnloadTexture()
	{
		if (state == TextureState.Loaded && texture != null)
		{
			TextureHelper.Instance.UnloadTexture(texture);
			texture = null;
		}
		if (state == TextureState.Loading)
		{
			StopLoading();
		}
		state = TextureState.NotLoaded;
	}

	private void StopLoading()
	{
		if (state == TextureState.Loading && !string.IsNullOrEmpty(loadingKey))
		{
			TextureHelper.Instance.CancelLoadAsync(loadingKey);
			loadingKey = null;
			callbacks.RemoveAll(url);
		}
	}
}
