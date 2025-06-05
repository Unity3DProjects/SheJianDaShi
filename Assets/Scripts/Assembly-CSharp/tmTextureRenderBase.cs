using System;
using UnityEngine;

[ExecuteInEditMode]
public class tmTextureRenderBase : MonoBehaviour, tmIRebuildable
{
	[Flags]
	public enum ModifiedFlags
	{
		ModifiedMesh = 1,
		ModifiedMaterial = 2,
		ModifiedUV1 = 4,
		ModifiedUV2 = 8
	}

	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private Material material;

	[SerializeField]
	private string mainTexCollectionGUID;

	[SerializeField]
	private string mainTextureID;

	[SerializeField]
	private string lightmapCollectionGUID;

	[SerializeField]
	private string lightmapTextureID;

	private tmTextureCollectionPlatform mainTexCollection;

	private tmTextureCollectionPlatform lightmapCollection;

	protected Mesh sharedMesh;

	protected Material sharedMaterial;

	protected ModifiedFlags modifiedFlag;

	private MeshFilter cachedMeshFilter;

	private SkinnedMeshRenderer cachedSkinnedMeshRenderer;

	private Renderer cachedRenderer;

	public Mesh SharedMesh
	{
		get
		{
			if (sharedMesh == null)
			{
				modifiedFlag |= ModifiedFlags.ModifiedMesh;
				BuildMesh();
			}
			return sharedMesh;
		}
	}

	public Material SharedMaterial
	{
		get
		{
			if (sharedMaterial == null)
			{
				modifiedFlag |= ModifiedFlags.ModifiedMaterial;
				UpdateMaterial();
			}
			return sharedMaterial;
		}
	}

	public Mesh RenderSharedMesh
	{
		get
		{
			if (cachedMeshFilter != null)
			{
				return cachedMeshFilter.sharedMesh;
			}
			if (cachedSkinnedMeshRenderer != null)
			{
				return cachedSkinnedMeshRenderer.sharedMesh;
			}
			return null;
		}
		set
		{
			if (cachedMeshFilter != null)
			{
				cachedMeshFilter.sharedMesh = value;
			}
			else if (cachedSkinnedMeshRenderer != null)
			{
				cachedSkinnedMeshRenderer.sharedMesh = value;
			}
		}
	}

	public Renderer CachedRenderer
	{
		get
		{
			if (cachedRenderer == null)
			{
				cachedRenderer = GetComponent<Renderer>();
				if (cachedRenderer == null)
				{
					if (sharedMesh != null && sharedMesh.boneWeights.Length > 0)
					{
						cachedSkinnedMeshRenderer = base.gameObject.AddComponent<SkinnedMeshRenderer>();
						cachedRenderer = cachedSkinnedMeshRenderer;
					}
					else
					{
						cachedRenderer = base.gameObject.AddComponent<MeshRenderer>();
					}
				}
			}
			return cachedRenderer;
		}
	}

	public ModifiedFlags ModifiedFlag
	{
		get
		{
			return modifiedFlag;
		}
		set
		{
			SetModifiedFlag(value);
		}
	}

	public Mesh Mesh
	{
		get
		{
			return mesh;
		}
		set
		{
			SetMesh(value);
		}
	}

	public Material Material
	{
		get
		{
			return material;
		}
		set
		{
			SetMaterial(value);
		}
	}

	public string MainTexCollectionGUID
	{
		get
		{
			return mainTexCollectionGUID;
		}
		set
		{
			if (mainTexCollectionGUID != value)
			{
				ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
				mainTexCollectionGUID = value;
				mainTexCollection = tmManager.Instance.GetMainTexPlatformCollection(mainTexCollectionGUID);
			}
		}
	}

	public tmTextureCollectionPlatform MainTexCollection
	{
		get
		{
			if (mainTexCollection == null)
			{
				if (!string.IsNullOrEmpty(MainTexCollectionGUID))
				{
					mainTexCollection = tmManager.Instance.GetMainTexPlatformCollection(MainTexCollectionGUID);
				}
				if (mainTexCollection != null)
				{
					ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
				}
			}
			return mainTexCollection;
		}
		set
		{
			if (mainTexCollection != value)
			{
				mainTexCollection = value;
				if (mainTexCollection != null)
				{
					mainTexCollectionGUID = tmUtility.PlatformlessPath(mainTexCollection.collectionGuid);
				}
				ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
			}
		}
	}

	public string MainTextureID
	{
		get
		{
			return mainTextureID;
		}
		set
		{
			SetMainTextureID(value);
		}
	}

	public string MainTextureName
	{
		get
		{
			return MainTexCollection.GetTextureDefenitionByID(MainTextureID).textureName;
		}
		set
		{
			MainTextureID = MainTexCollection.GetTextureDefenitionByName(value).textureGuid;
		}
	}

	public tmTextureDefenition MainTextureDefenition
	{
		get
		{
			if (MainTexCollection != null)
			{
				return MainTexCollection.GetTextureDefenitionByID(MainTextureID);
			}
			return null;
		}
	}

	public string LightmapCollectionGUID
	{
		get
		{
			return lightmapCollectionGUID;
		}
		set
		{
			if (lightmapCollectionGUID != value)
			{
				ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
				lightmapCollectionGUID = value;
				lightmapCollection = tmManager.Instance.GetLightmapPlatformCollection(lightmapCollectionGUID);
			}
		}
	}

	public tmTextureCollectionPlatform LightmapCollection
	{
		get
		{
			if (lightmapCollection == null)
			{
				if (!string.IsNullOrEmpty(LightmapCollectionGUID))
				{
					lightmapCollection = tmManager.Instance.GetLightmapPlatformCollection(LightmapCollectionGUID);
				}
				if (lightmapCollection != null)
				{
					ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
				}
			}
			return lightmapCollection;
		}
		set
		{
			if (lightmapCollection != value)
			{
				lightmapCollection = value;
				if (lightmapCollection != null)
				{
					lightmapCollectionGUID = tmUtility.PlatformlessPath(lightmapCollection.collectionGuid);
				}
				ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
			}
		}
	}

	public string LightmapTextureID
	{
		get
		{
			return lightmapTextureID;
		}
		set
		{
			SetLightmapTextureID(value);
		}
	}

	public tmTextureDefenition LightmapTextureDefenition
	{
		get
		{
			if (LightmapCollection != null)
			{
				return LightmapCollection.GetTextureDefenitionByID(LightmapTextureID);
			}
			return null;
		}
	}

	protected virtual void SetModifiedFlag(ModifiedFlags value)
	{
		if (modifiedFlag == (ModifiedFlags)0 && value != 0)
		{
			tmManager.Instance.RegisterModifiedRender(this);
		}
		modifiedFlag = value;
	}

	protected virtual void SetMesh(Mesh value)
	{
		if (mesh != value)
		{
			ModifiedFlag |= ModifiedFlags.ModifiedMesh;
			mesh = value;
		}
	}

	protected virtual void SetMaterial(Material value)
	{
		if (material != value)
		{
			modifiedFlag |= ModifiedFlags.ModifiedMaterial;
			material = value;
			UpdateMaterial();
		}
	}

	protected virtual void SetMainTextureID(string value)
	{
		if (mainTextureID != value)
		{
			ModifiedFlag |= ModifiedFlags.ModifiedMaterial | ModifiedFlags.ModifiedUV1;
			mainTextureID = value;
		}
	}

	protected virtual void SetLightmapTextureID(string value)
	{
		if (lightmapTextureID != value)
		{
			ModifiedFlag |= ModifiedFlags.ModifiedMaterial | ModifiedFlags.ModifiedUV2;
			lightmapTextureID = value;
		}
	}

	public virtual void Rebuild()
	{
		UpdateMaterial();
		BuildMesh();
	}

	private void Reset()
	{
		if (Mesh == null)
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component != null)
			{
				Mesh = component.sharedMesh;
			}
			SkinnedMeshRenderer component2 = GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				Mesh = component2.sharedMesh;
			}
		}
	}

	protected virtual void Awake()
	{
		cachedMeshFilter = GetComponent<MeshFilter>();
		cachedSkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		ModifiedFlag = ModifiedFlags.ModifiedMesh | ModifiedFlags.ModifiedMaterial | ModifiedFlags.ModifiedUV1 | ModifiedFlags.ModifiedUV2;
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual void BuildMesh()
	{
		if (Mesh != null && !string.IsNullOrEmpty(MainTextureID) && MainTexCollection != null)
		{
			tmTextureDefenition mainTextureDefenition = MainTextureDefenition;
			if (mainTextureDefenition != null)
			{
				tmTextureDefenition lightmapTextureDefenition = LightmapTextureDefenition;
				if ((ModifiedFlag & (ModifiedFlags.ModifiedMesh | ModifiedFlags.ModifiedUV1 | ModifiedFlags.ModifiedUV2)) != 0)
				{
					tmManager.Instance.MeshInstance(Mesh, out sharedMesh, mainTextureDefenition, lightmapTextureDefenition, false);
					RenderSharedMesh = sharedMesh;
				}
			}
		}
		ModifiedFlag &= ~(ModifiedFlags.ModifiedMesh | ModifiedFlags.ModifiedUV1 | ModifiedFlags.ModifiedUV2);
	}

	protected virtual void UpdateMaterial()
	{
		if ((ModifiedFlag & ModifiedFlags.ModifiedMaterial) != 0)
		{
			if (Material != null)
			{
				Material.mainTexture = null;
				Material.SetLightmapTexture(null);
				sharedMaterial = MaterialInstance(Material, MainTexCollection, LightmapCollection);
				CachedRenderer.sharedMaterial = sharedMaterial;
			}
			ModifiedFlag &= ~ModifiedFlags.ModifiedMaterial;
		}
	}

	protected virtual Material MaterialInstance(Material original, tmTextureCollectionPlatform mainCollection, tmTextureCollectionPlatform lightmapCollection)
	{
		string text = "mat" + original.GetHashCode();
		string text2 = string.Empty;
		if (mainCollection != null)
		{
			text += mainCollection.collectionGuid;
			text2 += mainCollection.name;
		}
		if (lightmapCollection != null)
		{
			text += lightmapCollection.collectionGuid;
			text2 += lightmapCollection.name;
		}
		Material copy;
		if (tmManager.Instance.GetSharedMaterial(original, mainCollection, lightmapCollection, text, out copy))
		{
			copy.name = original.name + "_" + text2;
			copy.hideFlags = HideFlags.HideAndDontSave;
		}
		return copy;
	}
}
