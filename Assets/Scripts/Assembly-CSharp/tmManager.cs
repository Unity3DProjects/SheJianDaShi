using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class tmManager : SingletonMonoBehaviour<tmManager>
{
	public static string stramingAssetsDirectory = "tmAtlases";

	[SerializeField]
	private List<Mesh> keepImMemoryMeshes = new List<Mesh>();

	private List<tmIRebuildable> modifiedRenders = new List<tmIRebuildable>();

	private Dictionary<string, Material> materialMap = new Dictionary<string, Material>();

	private Dictionary<tmTextureCollectionPlatform, List<Material>> mainTextureMap = new Dictionary<tmTextureCollectionPlatform, List<Material>>();

	private Dictionary<tmTextureCollectionPlatform, List<Material>> lightmapMap = new Dictionary<tmTextureCollectionPlatform, List<Material>>();

	private Dictionary<string, WeakReference> meshMap = new Dictionary<string, WeakReference>();

	private Dictionary<string, tmTextureCollectionPlatform> collectionMap = new Dictionary<string, tmTextureCollectionPlatform>();

	private Dictionary<int, string> destroyedMeshes = new Dictionary<int, string>();

	private bool hasNew;

	[Button]
	public string Btn = "Clear";

	public new static tmManager Instance
	{
		get
		{
			return SingletonMonoBehaviour<tmManager>.InstanceIfExist;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (ScriptableSingleton<tmSettings>.Instance.isAtlasesPreloadEnabled)
		{
			List<tmResourceCollectionLink> allResourceLinks = tmUtility.GetAllResourceLinks(ScriptableSingleton<tmSettings>.Instance.CurrentPlatform);
			for (int i = 0; i < allResourceLinks.Count; i++)
			{
				allResourceLinks[i].collection.LoadTexture();
				CustomDebug.LogFormat("Texture preload for {0}", allResourceLinks[i].collection.name);
			}
		}
	}

	public void Clear()
	{
		meshMap.Clear();
		materialMap.Clear();
		collectionMap.Clear();
		mainTextureMap.Clear();
		lightmapMap.Clear();
	}

	public void ClearMaterials()
	{
		materialMap.Clear();
	}

	public void Flush()
	{
		if (!hasNew)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		tmIRebuildable[] array = modifiedRenders.ToArray();
		foreach (tmIRebuildable tmIRebuildable2 in array)
		{
			if (!tmIRebuildable2.IsNull())
			{
				tmIRebuildable2.Rebuild();
			}
			modifiedRenders.Remove(tmIRebuildable2);
		}
		realtimeSinceStartup = (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f;
		hasNew = modifiedRenders.Count > 0;
	}

	public void Flush(tmIRebuildable[] renders)
	{
		foreach (tmIRebuildable tmIRebuildable2 in renders)
		{
			if (!tmIRebuildable2.IsNull())
			{
				tmIRebuildable2.Rebuild();
				modifiedRenders.Remove(tmIRebuildable2);
			}
		}
		hasNew = modifiedRenders.Count > 0;
	}

	public void PreloadAtlases(List<tmResourceCollectionLink> _links)
	{
		List<tmResourceCollectionLink> allResourceLinksFor = tmUtility.GetAllResourceLinksFor(ScriptableSingleton<tmSettings>.Instance.CurrentPlatform, _links);
		for (int i = 0; i < allResourceLinksFor.Count; i++)
		{
			allResourceLinksFor[i].collection.LoadTexture();
			CustomDebug.LogFormat("Texture preload for {0}", allResourceLinksFor[i].collection.name);
		}
	}

	public void RegisterModifiedRender(tmIRebuildable render)
	{
		modifiedRenders.Add(render);
		hasNew = true;
	}

	public bool GetSharedMaterial(Material original, tmTextureCollectionPlatform mainCollection, tmTextureCollectionPlatform lightmapCollection, string hashKey, out Material copy)
	{
		bool result = false;
		if (!materialMap.TryGetValue(hashKey, out copy))
		{
			copy = UnityEngine.Object.Instantiate(original);
			materialMap.Add(hashKey, copy);
			if (mainCollection != null)
			{
				copy.mainTexture = mainCollection.Atlas;
				List<Material> value;
				if (!mainTextureMap.TryGetValue(mainCollection, out value))
				{
					value = new List<Material>();
					mainTextureMap.Add(mainCollection, value);
				}
				value.Add(copy);
			}
			if (lightmapCollection != null)
			{
				copy.SetLightmapTexture(lightmapCollection.Atlas);
				List<Material> value2;
				if (!lightmapMap.TryGetValue(lightmapCollection, out value2))
				{
					value2 = new List<Material>();
					lightmapMap.Add(lightmapCollection, value2);
				}
				value2.Add(copy);
			}
			result = true;
		}
		return result;
	}

	public void TextureLoadedForCollection(tmTextureCollectionPlatform collection)
	{
		List<Material> value;
		if (mainTextureMap.TryGetValue(collection, out value))
		{
			value.ForEach(delegate(Material m)
			{
				m.mainTexture = collection.Atlas;
			});
		}
		else if (lightmapMap.TryGetValue(collection, out value))
		{
			value.ForEach(delegate(Material m)
			{
				m.SetLightmapTexture(collection.Atlas);
			});
		}
	}

	public void TextureWillUnloadForCollection(tmTextureCollectionPlatform collection)
	{
		foreach (Material value in materialMap.Values)
		{
			if (value.mainTexture == collection.Atlas)
			{
				value.mainTexture = null;
			}
			if (value.GetLightmapTexture() == collection.Atlas)
			{
				value.SetLightmapTexture(null);
			}
		}
	}

	public void MeshInstance(Mesh original, out Mesh sharedMesh, tmTextureDefenition mainTextureDef, tmTextureDefenition lightmapTextureDef, bool makeNoLongerReadable)
	{
		string key = "mesh" + original.GetHashCode() + ((mainTextureDef == null) ? string.Empty : mainTextureDef.textureGuid) + ((lightmapTextureDef == null) ? string.Empty : lightmapTextureDef.textureGuid);
		string text = string.Empty + ((mainTextureDef == null) ? string.Empty : mainTextureDef.textureName) + ((lightmapTextureDef == null) ? string.Empty : lightmapTextureDef.textureName);
		sharedMesh = null;
		WeakReference value = null;
		int instanceID = original.GetInstanceID();
		if (meshMap.TryGetValue(key, out value))
		{
			if (value.IsAlive && !value.Target.IsNull())
			{
				sharedMesh = value.Target as Mesh;
			}
			else
			{
				meshMap.Remove(key);
				destroyedMeshes.Remove(instanceID);
			}
		}
		if (sharedMesh == null)
		{
			if (Application.isPlaying && destroyedMeshes.ContainsKey(instanceID))
			{
				CustomDebug.LogError("duplicate : " + original.name + "  " + text + " vs " + destroyedMeshes[instanceID]);
			}
			if (!original.isReadable)
			{
				CustomDebug.LogError("NOT READABLE = " + original.name);
			}
			sharedMesh = ((!keepImMemoryMeshes.Contains(original)) ? original : UnityEngine.Object.Instantiate(original));
			sharedMesh.name = original.name + "_" + mainTextureDef.textureName;
			value = new WeakReference(sharedMesh);
			meshMap.Add(key, value);
			if (!keepImMemoryMeshes.Contains(original) && !destroyedMeshes.ContainsKey(instanceID))
			{
				destroyedMeshes.Add(instanceID, text);
			}
			RemapUV(ref sharedMesh, mainTextureDef, lightmapTextureDef);
			if (makeNoLongerReadable)
			{
				sharedMesh.UploadMeshData(true);
			}
		}
	}

	public void RemapUV(ref Mesh mesh, tmTextureDefenition mainTextureDefenition, tmTextureDefenition lightmapTextureDefenition)
	{
		if (!ScriptableSingleton<tmSettings>.Instance.rebuildMesh)
		{
			return;
		}
		Vector2[] uv = mesh.uv;
		if (uv != null && uv.Length > 0)
		{
			Rect uv2 = mainTextureDefenition.uv;
			Rect offset = mainTextureDefenition.offset;
			uv2.center += offset.center;
			uv2.size += offset.size;
			int num = uv.Length;
			for (int i = 0; i < num; i++)
			{
				Vector2 vector = uv[i];
				Vector2 vector2 = new Vector2(uv2.x + uv2.width * vector.x, uv2.y + uv2.height * vector.y);
				uv[i] = vector2;
			}
			mesh.uv = uv;
		}
		if (lightmapTextureDefenition == null)
		{
			return;
		}
		Vector2[] uv3 = mesh.uv2;
		if (uv3 != null && uv3.Length > 0)
		{
			Rect uv4 = lightmapTextureDefenition.uv;
			Rect offset2 = lightmapTextureDefenition.offset;
			uv4.center += offset2.center;
			uv4.size += offset2.size;
			int num2 = uv3.Length;
			for (int j = 0; j < num2; j++)
			{
				Vector2 vector3 = uv3[j];
				Vector2 vector4 = new Vector2(uv4.x + uv4.width * vector3.x, uv4.y + uv4.height * vector3.y);
				uv3[j] = vector4;
			}
			mesh.uv2 = uv3;
		}
	}

	public tmTextureCollectionPlatform GetMainTexPlatformCollection(string collectionGuid)
	{
		return GetPlatformCollectionWithFallback(collectionGuid, ScriptableSingleton<tmSettings>.Instance.CurrentPlatform);
	}

	public tmTextureCollectionPlatform GetLightmapPlatformCollection(string collectionGuid)
	{
		return GetPlatformCollectionWithFallback(collectionGuid, ScriptableSingleton<tmSettings>.Instance.LightmapPlatform);
	}

	public tmTextureCollectionPlatform GetPlatformCollectionWithFallback(string collectionGuid, tmPlatform platform)
	{
		tmTextureCollectionPlatform platformCollection = GetPlatformCollection(collectionGuid, platform);
		if (platformCollection == null)
		{
			tmPlatform[] allPlatfrorms = tmSettings.allPlatfrorms;
			foreach (tmPlatform platform2 in allPlatfrorms)
			{
				platformCollection = GetPlatformCollection(collectionGuid, platform2);
				if (platformCollection != null)
				{
					return platformCollection;
				}
			}
		}
		return platformCollection;
	}

	public tmTextureCollectionPlatform GetPlatformCollection(string collectionGuid, tmPlatform platform)
	{
		string text = tmUtility.PathForPlatform(collectionGuid, platform);
		if (collectionMap.ContainsKey(text))
		{
			return collectionMap[text];
		}
		tmResourceCollectionLink tmResourceCollectionLink2 = tmUtility.ResourceLinkByGUID(text);
		if (tmResourceCollectionLink2 != null)
		{
			collectionMap.Add(text, tmResourceCollectionLink2.collection);
			return tmResourceCollectionLink2.collection;
		}
		return null;
	}
}
