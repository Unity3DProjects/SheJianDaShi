using System;
using System.IO;
using System.Threading;
using UnityEngine;

[Serializable]
public class AssetLink : Asset
{
	private const string RESOURCE_FOLDER = "Resources/";

	private const string STREAMING_ASSETS_FOLDER = "StreamingAssets/";

	public string name;

	public string assetGUID;

	private string fullPath;

	private string resourcePath;

	private string streamingAssetPath;

	private string cachedWWWStreamingAssetPath;

	private UnityEngine.Object loadedAsset;

	private WWW loadedWWW;

	public string FullPath
	{
		get
		{
			if (fullPath == null)
			{
				fullPath = GUIDMapper.GUIDToAssetPath(assetGUID);
				// Debug.LogError("lllllllll"+fullPath);
			}
			return fullPath;
		}
	}

	public string ResourcePath
	{
		get
		{
			if (resourcePath == null)
			{
				// Debug.LogError(assetGUID);
				int num = FullPath.IndexOf("Resources/", StringComparison.Ordinal);
				if (num > -1)
				{
					num += "Resources/".Length;
					resourcePath = FullPath.Substring(num);
					resourcePath = resourcePath.RemovePathExtention();
				}
				else
				{
					resourcePath = string.Empty;
				}
			}
			return resourcePath;
		}
	}

	public string StreamingAssetPath
	{
		get
		{
			if (streamingAssetPath == null)
			{
				if (FullPath == null)
				{
					return string.Empty;
				}
				int num = FullPath.IndexOf("StreamingAssets/", StringComparison.Ordinal);
				if (num > -1)
				{
					num += "StreamingAssets/".Length;
					streamingAssetPath = FullPath.Substring(num);
					streamingAssetPath = Application.streamingAssetsPath.AppendPathComponent(streamingAssetPath);
				}
				else
				{
					streamingAssetPath = string.Empty;
				}
			}
			return streamingAssetPath;
		}
	}

	public bool IsResource
	{
		get
		{
			return ResourcePath.Length > 0;
		}
	}

	public bool IsStreamingAsset
	{
		get
		{
			return StreamingAssetPath.Length > 0;
		}
	}

	public override bool IsLoaded
	{
		get
		{
			return loadedAsset != null || loadedWWW != null;
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
					cachedWWWStreamingAssetPath = string.Empty;
				}
			}
			return cachedWWWStreamingAssetPath;
		}
	}

	public AssetLink(UnityEngine.Object asset)
	{
		SetAsset(asset);
	}

	[Obsolete("For RESOURCES use AssetResource, for STREAMING ASSETS use AssetStreaming")]
	public AssetLink(string path)
	{
		name = Path.GetFileNameWithoutExtension(path);
		fullPath = path;
	}

	public override void Load()
	{
		if (IsLoaded)
		{
			return;
		}
		if (IsResource)
		{
			loadedWWW = null;
			loadedAsset = Resources.Load(ResourcePath, typeof(UnityEngine.Object));
		}
		else if (IsStreamingAsset)
		{
			loadedAsset = null;
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
		if (IsResource)
		{
			loadedWWW = null;
			ResourceRequest request = Resources.LoadAsync(ResourcePath, typeof(UnityEngine.Object));
			request.StartCoroutine(delegate
			{
				loadedAsset = request.asset;
				if (callback != null)
				{
					callback();
				}
			});
		}
		else
		{
			if (!IsStreamingAsset)
			{
				return;
			}
			loadedAsset = null;
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
	}

	public override void Unload()
	{
		if (IsLoaded)
		{
			if (IsResource)
			{
				Resources.UnloadAsset(loadedAsset);
				loadedAsset = null;
				loadedWWW = null;
			}
			else if (IsStreamingAsset)
			{
				loadedWWW.Dispose();
				loadedWWW = null;
				loadedAsset = null;
			}
		}
	}

	public UnityEngine.Object GetAsset()
	{
		UnityEngine.Object result = null;
		if (IsLoaded)
		{
			result = loadedAsset;
		}
		else
		{
			Load();
			if (IsLoaded)
			{
				result = loadedAsset;
			}
		}
		return result;
	}

	public void GetAssetAsync(Action<UnityEngine.Object> callback)
	{
		UnityEngine.Object result = null;
		if (IsLoaded)
		{
			result = loadedAsset;
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
				result = loadedAsset;
			}
			if (callback != null)
			{
				callback(result);
			}
		});
	}

	public UnityEngine.Object GetInstance()
	{
		UnityEngine.Object result = null;
		if (IsLoaded)
		{
			result = UnityEngine.Object.Instantiate(loadedAsset);
		}
		else
		{
			Load();
			if (IsLoaded)
			{
				result = UnityEngine.Object.Instantiate(loadedAsset);
			}
		}
		return result;
	}

	public void GetInstanceAsync(Action<UnityEngine.Object> callback)
	{
		UnityEngine.Object result = null;
		if (IsLoaded)
		{
			result = UnityEngine.Object.Instantiate(loadedAsset);
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
				result = UnityEngine.Object.Instantiate(loadedAsset);
			}
			if (callback != null)
			{
				callback(result);
			}
		});
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

	protected virtual void SetAsset(UnityEngine.Object asset)
	{
	}
}
