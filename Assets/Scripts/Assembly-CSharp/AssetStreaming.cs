using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class AssetStreaming : Asset
{
	private const string STREAMING_ASSETS_FOLDER = "StreamingAssets/";

	public string name;

	private string fullPath;

	private string streamingAssetPath;

	private string cachedWWWStreamingAssetPath;

	private WWW loadedWWW;

	public string FullPath
	{
		get
		{
			return (!string.IsNullOrEmpty(fullPath)) ? fullPath : string.Empty;
		}
	}

	public string StreamingAssetPath
	{
		get
		{
			if (streamingAssetPath == null)
			{
				int num = FullPath.IndexOf("StreamingAssets/", StringComparison.Ordinal);
				if (num > -1)
				{
					num += "StreamingAssets/".Length;
					streamingAssetPath = FullPath.Substring(num);
					streamingAssetPath = Application.streamingAssetsPath.AppendPathComponent(streamingAssetPath);
				}
				else
				{
					streamingAssetPath = FullPath;
				}
			}
			return streamingAssetPath;
		}
	}

	public override bool IsLoaded
	{
		get
		{
			return loadedWWW != null;
		}
	}

	private string CachedWWWStreamingAssetPath
	{
		get
		{
			if (cachedWWWStreamingAssetPath == null)
			{
				if (!string.IsNullOrEmpty(StreamingAssetPath))
				{
					cachedWWWStreamingAssetPath = StreamingAssetPath.AppendWWWFilePrefix();
					cachedWWWStreamingAssetPath = cachedWWWStreamingAssetPath.AppendWWWJARPrefix();
				}
				else
				{
					cachedWWWStreamingAssetPath = StreamingAssetPath;
				}
			}
			return cachedWWWStreamingAssetPath;
		}
	}

	public AssetStreaming(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			name = Path.GetFileNameWithoutExtension(path);
			fullPath = path;
		}
	}

	public override void Load()
	{
		if (!IsLoaded)
		{
			loadedWWW = new WWW(CachedWWWStreamingAssetPath);
			while (!loadedWWW.isDone)
			{
				Thread.Sleep(1);
			}
			if (!string.IsNullOrEmpty(loadedWWW.error))
			{
				CustomDebug.Log("AssetLink Error WWW :: " + loadedWWW.error);
				loadedWWW.Dispose();
				loadedWWW = null;
			}
		}
	}

	public override void LoadAsync(Action callback)
	{
		if (IsLoaded)
		{
			return;
		}
		loadedWWW = new WWW(CachedWWWStreamingAssetPath);
		loadedWWW.StartCoroutine(delegate
		{
			if (!string.IsNullOrEmpty(loadedWWW.error))
			{
				CustomDebug.Log("AssetLink Error WWW :: " + loadedWWW.error);
				loadedWWW.Dispose();
				loadedWWW = null;
			}
			if (callback != null)
			{
				callback();
			}
		});
	}

	public override void Unload()
	{
		if (IsLoaded)
		{
			loadedWWW.Dispose();
			loadedWWW = null;
		}
	}

	public WWW GetWWW()
	{
		WWW result = null;
		if (IsLoaded)
		{
			result = loadedWWW;
		}
		else
		{
			Load();
			if (IsLoaded)
			{
				result = loadedWWW;
			}
		}
		return result;
	}

	public void GetWWWAsync(Action<WWW> callback)
	{
		WWW result = null;
		if (IsLoaded)
		{
			result = loadedWWW;
			if (callback != null)
			{
				callback(result);
			}
			return;
		}
		LoadAsync(delegate
		{
			if (IsLoaded)
			{
				result = loadedWWW;
			}
			if (callback != null)
			{
				callback(result);
			}
		});
	}
}
