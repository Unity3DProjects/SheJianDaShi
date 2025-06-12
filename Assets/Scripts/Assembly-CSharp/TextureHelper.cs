using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public class TextureHelper : SingletonMonoBehaviour<TextureHelper>
{
	private enum Format
	{
		Format_RGBA32 = 0,
		Format_RGB16 = 8,
		Format_A8 = 0x10
	}

	private struct LoadResult
	{
		public IntPtr texturePtr;

		public int width;

		public int height;
	}

	private struct AsyncResult
	{
		public long texturePtr;

		public int width;

		public int height;

		public string loadKey;
	}

	private struct AsyncRequest
	{
		public Action<Texture2D> callback;

		public bool doMipMaps;

		public TextureFormat textureFormat;

		public string path;
	}

	private Dictionary<string, AsyncRequest> requests = new Dictionary<string, AsyncRequest>();

	private static TextureHelper inst;

	public new static TextureHelper Instance
	{
		get
		{
			if (!inst)
			{
				inst = SingletonMonoBehaviour<TextureHelper>.InstanceIfExist;
				if (inst == null)
				{
					GameObject gameObject = new GameObject("TextureHelper");
					inst = gameObject.AddComponent<TextureHelper>();
					gameObject.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			return inst;
		}
	}

	private static Format TextureFormatToInternal(TextureFormat fmt)
	{
		switch (fmt)
		{
		case TextureFormat.RGB565:
			return Format.Format_RGB16;
		case TextureFormat.Alpha8:
			return Format.Format_A8;
		default:
			return Format.Format_RGBA32;
		}
	}

	public Texture2D LoadImageToTexture(string imagePath, bool doMipMaps = false, TextureFormat curFormat = TextureFormat.RGBA32)
	{
		Texture2D texture2D = null;
		Texture2D texture2D2 = new Texture2D(16, 16, curFormat, doMipMaps);
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.inventain.android.FileManager");
		string text = androidJavaClass.CallStatic<string>("GetTexturePointer", new object[1] { imagePath });
		string[] array = text.Split(' ');
		if (array.Length > 2)
		{
			texture2D2 = Texture2D.CreateExternalTexture(int.Parse(array[0]), int.Parse(array[1]), TextureFormat.ARGB32, doMipMaps, false, (IntPtr)int.Parse(array[2]));
		}
		texture2D = texture2D2;
		if (texture2D != null)
		{
			// texture2D.hideFlags = HideFlags.DontSave;
			texture2D.wrapMode = TextureWrapMode.Clamp;
		}
		return texture2D;
	}

	public string LoadImageToTextureAsync(string imagePath, Action<Texture2D> callback, bool doMipMaps = false, TextureFormat curFormat = TextureFormat.RGBA32)
	{
		callback(LoadImageToTexture(imagePath, doMipMaps, curFormat));
		return string.Empty;
	}

	public void CancelLoadAsync(string loadKey)
	{
		requests.Remove(loadKey);
	}

	public void UnloadTexture(Texture2D texture)
	{
		if (!texture)
		{
		}
	}

	private void Native_AsyncLoadCallback(string JSON_loadResult)
	{
		AsyncResult asyncResult = Json.Deserialize<AsyncResult>(JSON_loadResult);
		IntPtr intPtr = new IntPtr(asyncResult.texturePtr);
		AsyncRequest value;
		if (intPtr != IntPtr.Zero && requests.TryGetValue(asyncResult.loadKey, out value))
		{
			Texture2D texture2D = null;
			if (texture2D != null)
			{
				texture2D.name = Path.GetFileName(value.path);
				texture2D.hideFlags = HideFlags.DontSave;
				texture2D.wrapMode = TextureWrapMode.Clamp;
			}
			value.callback(texture2D);
		}
	}
}
