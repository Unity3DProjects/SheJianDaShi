using UnityEngine;

public class tmBatchObject : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private tmBatchingType batchingType;

	[SerializeField]
	private Transform root;

	private tmBatchedInstance batchInstance;

	private bool batched;

	private bool forcedBatching;

	private Transform cachedTransform;

	private MeshFilter cachedMeshFilter;

	private Renderer cachedRenderer;

	private SkinnedMeshRenderer cachedSkinnedRenderer;

	private bool skinnedMeshRender;

	private tmTextureRenderBase cachedTextureRender;

	private bool hasTextureRender;

	public bool Batched
	{
		get
		{
			return batched;
		}
		set
		{
			if (batched ^ value)
			{
				batched = value;
				CachedRenderer.enabled = !batched;
			}
		}
	}

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public Renderer CachedRenderer
	{
		get
		{
			if (cachedRenderer == null)
			{
				cachedRenderer = GetComponent<Renderer>();
			}
			return cachedRenderer;
		}
	}

	public MeshFilter MeshFilter
	{
		get
		{
			if (cachedMeshFilter == null)
			{
				cachedMeshFilter = GetComponent<MeshFilter>();
			}
			return cachedMeshFilter;
		}
	}

	public SkinnedMeshRenderer SkinnedMeshRender
	{
		get
		{
			if (!skinnedMeshRender)
			{
				skinnedMeshRender = true;
				cachedSkinnedRenderer = GetComponent<SkinnedMeshRenderer>();
			}
			return cachedSkinnedRenderer;
		}
	}

	public Mesh Mesh
	{
		get
		{
			if (MeshFilter != null)
			{
				return MeshFilter.sharedMesh;
			}
			if (SkinnedMeshRender != null)
			{
				return SkinnedMeshRender.sharedMesh;
			}
			return null;
		}
	}

	public Material Material
	{
		get
		{
			return CachedRenderer.sharedMaterial;
		}
	}

	public tmBatchedInstance BatchInstance
	{
		get
		{
			return batchInstance;
		}
		set
		{
			batchInstance = value;
		}
	}

	public tmBatchingType BatchingType
	{
		get
		{
			return batchingType;
		}
		set
		{
			if (batchingType != value)
			{
				Unbatch();
				batchingType = value;
				Batch();
			}
		}
	}

	public Transform Root
	{
		set
		{
			root = value;
		}
	}

	public tmTextureRenderBase CachedTextureRender
	{
		get
		{
			if (!hasTextureRender && cachedTextureRender == null)
			{
				cachedTextureRender = GetComponent<tmTextureRenderBase>();
				hasTextureRender = true;
			}
			return cachedTextureRender;
		}
	}

	public bool ForcedBatching
	{
		get
		{
			return forcedBatching;
		}
		set
		{
			forcedBatching = value;
			if (ForcedBatching != value && BatchInstance != null)
			{
				BatchInstance.ForcedBatchChanged(this);
			}
		}
	}

	public void SetUp()
	{
		if (GetComponent<SkinnedMeshRenderer>() != null)
		{
			BatchingType = tmBatchingType.None;
		}
		else if (Mesh != null)
		{
			if (Mesh.vertexCount < 1024)
			{
				BatchingType = tmBatchingType.Dynamic;
				Root = null;
			}
			else
			{
				BatchingType = tmBatchingType.Static;
			}
		}
	}

	protected virtual void OnEnable()
	{
		if ((BatchingType == tmBatchingType.Dynamic || BatchingType == tmBatchingType.Skinning) && CachedTextureRender == null)
		{
			if (batchInstance != null)
			{
				Unbatch();
			}
			Batch();
		}
	}

	protected virtual void OnDisable()
	{
		Unbatch();
	}

	public void Rebuild()
	{
		if (BatchingType == tmBatchingType.Dynamic || BatchingType == tmBatchingType.Skinning)
		{
			Unbatch();
			Batch();
		}
	}

	public void Batch()
	{
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy && BatchingType != tmBatchingType.None && ScriptableSingleton<tmSettings>.Instance.batching)
		{
			tmBatchingManager.Instance.BatchObject(this, root, false);
		}
	}

	public void Unbatch()
	{
		if ((BatchingType == tmBatchingType.Dynamic || BatchingType == tmBatchingType.Skinning) && BatchInstance != null)
		{
			BatchInstance.Remove(this);
		}
	}

	public void MarkMeshModified()
	{
		if (BatchInstance != null)
		{
			BatchInstance.Modified = true;
		}
	}
}
