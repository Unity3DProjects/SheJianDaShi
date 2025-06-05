using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class SkeletonUtilityMeshCollider : MonoBehaviour
{
	[SerializeField]
	private MeshCollider cachedCollider;

	[HideInInspector]
	[SerializeField]
	private Vector3[] meshVertices;

	[HideInInspector]
	[SerializeField]
	private int[] meshTriangles;

	[HideInInspector]
	[SerializeField]
	private Vector2[] meshUVs;

	private Mesh cachedMesh;

	public MeshCollider CachedCollider
	{
		get
		{
			if (cachedCollider == null)
			{
				cachedCollider = GetComponent<MeshCollider>();
				cachedCollider.convex = true;
			}
			return cachedCollider;
		}
	}

	public Mesh CachedMesh
	{
		get
		{
			if (cachedMesh == null && (bool)CachedCollider)
			{
				cachedMesh = CachedCollider.sharedMesh;
			}
			return cachedMesh;
		}
		set
		{
			if (value != null)
			{
				cachedMesh = value;
				CachedCollider.sharedMesh = value;
				meshVertices = value.vertices;
				meshTriangles = value.triangles;
				meshUVs = value.uv;
			}
		}
	}

	public bool IsTrigger
	{
		get
		{
			return CachedCollider.isTrigger;
		}
		set
		{
			CachedCollider.isTrigger = value;
		}
	}

	private void Awake()
	{
		if ((bool)CachedCollider)
		{
			Mesh mesh = new Mesh();
			mesh.vertices = meshVertices;
			mesh.triangles = meshTriangles;
			mesh.uv = meshUVs;
			CachedCollider.sharedMesh = mesh;
		}
	}
}
