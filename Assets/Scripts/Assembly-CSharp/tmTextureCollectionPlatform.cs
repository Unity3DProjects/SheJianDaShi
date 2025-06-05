using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmTextureCollectionPlatform : tmTextureCollectionBase
{
	public tmPlatform platform;

	[SerializeField]
	private string atlasAssetGUID;

	[SerializeField]
	private string assetPath;

	protected Texture2D atlas;

	protected Coroutine unloading;

	private int retainCount;

	private HashSet<tmTextureRenderBase> renderRefs = new HashSet<tmTextureRenderBase>();

	public string AtlasAssetGUID
	{
		get
		{
			return atlasAssetGUID;
		}
		set
		{
			atlasAssetGUID = value;
		}
	}

	public string AssetPath
	{
		get
		{
			return assetPath;
		}
		set
		{
			assetPath = value;
		}
	}

	public Texture2D Atlas
	{
		get
		{
			return atlas;
		}
		set
		{
			atlas = value;
		}
	}

	private int RetainCount
	{
		get
		{
			return retainCount;
		}
		set
		{
			if (retainCount == 0 && value > 0)
			{
				LoadTexture();
			}
			if (retainCount > 0 && value == 0)
			{
				UnloadTexture();
			}
			retainCount = value;
		}
	}

	public static int CollectionWaitingForLoadCount { get; private set; }

	private void Add(tmTextureRenderBase render)
	{
		if (renderRefs.Add(render))
		{
			RetainCount++;
		}
	}

	private void Remove(tmTextureRenderBase render)
	{
		if (renderRefs.Remove(render))
		{
			RetainCount--;
		}
	}

	public static tmTextureCollectionPlatform operator +(tmTextureCollectionPlatform platform, tmTextureRenderBase render)
	{
		if (platform != null)
		{
			platform.Add(render);
		}
		return platform;
	}

	public static tmTextureCollectionPlatform operator -(tmTextureCollectionPlatform platform, tmTextureRenderBase render)
	{
		if (platform != null)
		{
			platform.Remove(render);
		}
		return platform;
	}

	public void LoadTexture()
	{
		unloading.StopCoroutine();
		unloading = null;
		if (!(atlas == null))
		{
			return;
		}
		string path = ((!useStreamingAssets) ? assetPath : (Application.streamingAssetsPath + "/" + tmManager.stramingAssetsDirectory + "/" + assetPath));
		if (ScriptableSingleton<tmSettings>.Instance.isImmediateTextureLoadEnabled)
		{
			atlas = TextureCache.LoadTexture(path, (!useStreamingAssets) ? TextureDataSource.Resources : TextureDataSource.StreamingAssets, importSettings.format, importSettings.generateMipMaps);
			if ((bool)SingletonMonoBehaviour<tmManager>.InstanceIfExist)
			{
				tmManager.Instance.TextureLoadedForCollection(this);
			}
		}
		else
		{
			TextureCache.LoadTextureAsync(path, (!useStreamingAssets) ? TextureDataSource.Resources : TextureDataSource.StreamingAssets, importSettings.format, importSettings.generateMipMaps, TextureDidLoaded);
		}
	}

	private void TextureDidLoaded(Texture2D tex)
	{
		atlas = tex;
		if ((bool)SingletonMonoBehaviour<tmManager>.InstanceIfExist)
		{
			tmManager.Instance.TextureLoadedForCollection(this);
		}
	}

	private void UnloadTexture()
	{
		if (ScriptableSingleton<tmSettings>.Instance.isAtlasesUnloadEnabled && atlas != null)
		{
			unloading = UnloadTextureIterator().StartCoroutine();
		}
	}

	private IEnumerator UnloadTextureIterator()
	{
		yield return new WaitForEndOfFrame();
		if (atlas != null && RetainCount == 0)
		{
			if ((bool)SingletonMonoBehaviour<tmManager>.InstanceIfExist)
			{
				tmManager.Instance.TextureWillUnloadForCollection(this);
			}
			atlas = null;
			TextureCache.UnloadTexture(assetPath);
			unloading = null;
		}
	}
}
