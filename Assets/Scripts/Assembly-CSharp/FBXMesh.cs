using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class FBXMesh : MonoBehaviour
{
	[SerializeField]
	private Mesh referenceFBX;

	[SerializeField]
	private Material referenceMaterial;

	private MeshFilter cachedFilter;

	private MeshRenderer cachedRenderer;

	public Mesh ReferenceFBX
	{
		get
		{
			return referenceFBX;
		}
		set
		{
			referenceFBX = value;
		}
	}

	public Material ReferenceMaterial
	{
		get
		{
			return referenceMaterial;
		}
		set
		{
			referenceMaterial = value;
		}
	}

	public MeshFilter CachedFilter
	{
		get
		{
			if (cachedFilter == null)
			{
				cachedFilter = GetComponent<MeshFilter>();
			}
			return cachedFilter;
		}
	}

	public MeshRenderer CachedRenderer
	{
		get
		{
			if (cachedRenderer == null)
			{
				cachedRenderer = GetComponent<MeshRenderer>();
			}
			return cachedRenderer;
		}
	}

	public int SortingOrder
	{
		get
		{
			return CachedRenderer.sortingOrder;
		}
		set
		{
			if (CachedRenderer.sortingOrder != value)
			{
				CachedRenderer.sortingOrder = value;
			}
		}
	}

	private void Awake()
	{
		Build();
	}

	public void Build()
	{
		if (referenceMaterial != null && referenceFBX != null)
		{
			CachedRenderer.material = referenceMaterial;
			CachedFilter.mesh = referenceFBX;
		}
	}
}
