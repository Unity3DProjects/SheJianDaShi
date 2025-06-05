using System;
using System.IO;
using UnityEngine;

public class AssetResource : Asset
{
	private const string RESOURCE_FOLDER = "Resources/";

	public string name;

	private string fullPath;

	private string resourcePath;

	private UnityEngine.Object loadedAsset;

	public string FullPath
	{
		get
		{
			return (!string.IsNullOrEmpty(fullPath)) ? fullPath : string.Empty;
		}
	}

	public string ResourcePath
	{
		get
		{
			if (resourcePath == null)
			{
				int num = FullPath.IndexOf("Resources/", StringComparison.Ordinal);
				if (num > -1)
				{
					num += "Resources/".Length;
					resourcePath = FullPath.Substring(num);
					resourcePath = resourcePath.RemovePathExtention();
				}
				else
				{
					resourcePath = FullPath;
				}
			}
			return resourcePath;
		}
	}

	public override bool IsLoaded
	{
		get
		{
			return loadedAsset != null;
		}
	}

	public AssetResource(string path)
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
			loadedAsset = Resources.Load(ResourcePath, typeof(UnityEngine.Object));
		}
	}

	public override void LoadAsync(Action callback)
	{
		if (IsLoaded)
		{
			return;
		}
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

	public override void Unload()
	{
		if (IsLoaded)
		{
			Resources.UnloadAsset(loadedAsset);
			loadedAsset = null;
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
}
