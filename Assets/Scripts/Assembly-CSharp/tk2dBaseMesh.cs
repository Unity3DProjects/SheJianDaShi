using UnityEngine;

public abstract class tk2dBaseMesh : MonoBehaviour
{
	private Mesh cachedMesh;

	private MeshFilter cachedMeshFilter;

	private Transform cachedTransform;

	private Renderer cachedRenderer;

	private bool batchObjectInitialized;

	private tmBatchObject batchObject;

	public static bool globalDisableRoundShift = true;

	public bool isRoundShiftDisabled = true;

	private Vector3 totalShift = Vector3.zero;

	private Mesh CachedMesh
	{
		get
		{
			if (cachedMesh == null)
			{
				cachedMesh = new Mesh();
				cachedMesh.hideFlags = HideFlags.DontSave;
				CachedMeshFilter.mesh = cachedMesh;
			}
			return cachedMesh;
		}
	}

	private MeshFilter CachedMeshFilter
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

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return base.transform;
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

	private tmBatchObject BatchObject
	{
		get
		{
			if (!batchObjectInitialized)
			{
				batchObject = GetComponent<tmBatchObject>();
				if (batchObject != null)
				{
					batchObject.ForcedBatching = true;
				}
				batchObjectInitialized = true;
			}
			return batchObject;
		}
	}

	private bool BatchObjectIsActive
	{
		get
		{
			return BatchObject != null && BatchObject.BatchingType != tmBatchingType.None;
		}
		set
		{
			if (BatchObjectIsActive != value && BatchObject != null)
			{
				if (value)
				{
					BatchObject.BatchingType = tmBatchingType.Skinning;
				}
				else
				{
					BatchObject.BatchingType = tmBatchingType.None;
				}
			}
		}
	}

	protected Vector3 TotalShift
	{
		get
		{
			return totalShift;
		}
	}

	protected Vector3[] MeshVertices
	{
		get
		{
			return CachedMesh.vertices;
		}
		set
		{
			CachedMesh.vertices = value;
			MarkMeshModified();
		}
	}

	protected Vector3[] MeshNormals
	{
		get
		{
			return CachedMesh.normals;
		}
		set
		{
			CachedMesh.normals = value;
			MarkMeshModified();
		}
	}

	protected Vector4[] MeshTangents
	{
		get
		{
			return CachedMesh.tangents;
		}
		set
		{
			CachedMesh.tangents = value;
			MarkMeshModified();
		}
	}

	protected Color[] MeshColors
	{
		get
		{
			return CachedMesh.colors;
		}
		set
		{
			CachedMesh.colors = value;
			MarkMeshModified();
		}
	}

	protected Color32[] MeshColors32
	{
		get
		{
			return CachedMesh.colors32;
		}
		set
		{
			CachedMesh.colors32 = value;
			MarkMeshModified();
		}
	}

	protected Vector2[] MeshUV
	{
		get
		{
			return CachedMesh.uv;
		}
		set
		{
			CachedMesh.uv = value;
			MarkMeshModified();
		}
	}

	protected Vector2[] MeshUV2
	{
		get
		{
			return CachedMesh.uv2;
		}
		set
		{
			CachedMesh.uv2 = value;
			MarkMeshModified();
		}
	}

	protected int[] MeshTriangles
	{
		get
		{
			return CachedMesh.triangles;
		}
		set
		{
			CachedMesh.triangles = value;
			MarkMeshModified();
		}
	}

	protected Bounds MeshBounds
	{
		get
		{
			return CachedMesh.bounds;
		}
		set
		{
			CachedMesh.bounds = value;
			MarkMeshModified();
		}
	}

	protected Material CurrentMaterial
	{
		get
		{
			return CachedRenderer.sharedMaterial;
		}
		set
		{
			if (CurrentMaterial != value)
			{
				CachedRenderer.material = value;
				MarkMaterialModified();
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (cachedMesh != null)
		{
			if (cachedMeshFilter != null && cachedMeshFilter.sharedMesh == cachedMesh)
			{
				cachedMeshFilter.mesh = null;
			}
			Object.Destroy(cachedMesh);
		}
	}

	private bool UpdateBatchObjectIsActive()
	{
		bool batchObjectIsActive = BatchObjectIsActive;
		BatchObjectIsActive = cachedMesh != null && CurrentMaterial != null;
		return batchObjectIsActive != BatchObjectIsActive;
	}

	private void MarkMeshModified()
	{
		if (!UpdateBatchObjectIsActive() && BatchObjectIsActive)
		{
			BatchObject.MarkMeshModified();
		}
	}

	private void MarkMaterialModified()
	{
		if (!UpdateBatchObjectIsActive() && BatchObjectIsActive)
		{
			BatchObject.Rebuild();
		}
	}

	protected virtual void Update()
	{
		if (!globalDisableRoundShift && !isRoundShiftDisabled && CachedTransform.hasChanged)
		{
			CachedTransform.hasChanged = false;
			Vector3 vector = CachedTransform.RoundShift();
			if (vector != totalShift)
			{
				totalShift = vector;
				RoundShiftChanged();
			}
		}
	}

	protected abstract void RoundShiftChanged();

	protected void MeshClear()
	{
		CachedMesh.Clear();
		MarkMeshModified();
	}

	protected void MeshRecalculateBounds()
	{
		CachedMesh.RecalculateBounds();
	}
}
