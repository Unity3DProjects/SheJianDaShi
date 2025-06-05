using System;
using System.Collections.Generic;
using UnityEngine;
using tk2dRuntime;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class MeshBatcher : MonoBehaviour, ISpriteCollectionForceBuild
{
	[Serializable]
	public struct TrianglesStruct
	{
		public int[] triangles;
	}

	[SerializeField]
	private Vector3 scale = Vector3.one;

	public BatchedMesh[] batchedSprites;

	[SerializeField]
	private Vector3[] vertices;

	[SerializeField]
	private List<TrianglesStruct> indices;

	[SerializeField]
	private Vector2[] uvs;

	[SerializeField]
	private Color[] colors;

	private Mesh mesh;

	private MeshFilter cachedFilter;

	private MeshRenderer cachedRenderer;

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

	private void Awake()
	{
		Mesh mesh = new Mesh();
		if (vertices != null && vertices.Length > 0)
		{
			mesh.vertices = vertices;
		}
		if (uvs != null && uvs.Length > 0)
		{
			mesh.uv = uvs;
		}
		if (indices != null && indices.Count > 0)
		{
			mesh.subMeshCount = indices.Count;
			for (int i = 0; i < indices.Count; i++)
			{
				mesh.SetTriangles(indices[i].triangles, i);
			}
		}
		if (colors != null && colors.Length > 0)
		{
			mesh.colors = colors;
		}
		CachedFilter.mesh = mesh;
	}

	protected void OnDestroy()
	{
		if ((bool)mesh)
		{
			UnityEngine.Object.Destroy(mesh);
			mesh = null;
			CachedFilter.mesh = mesh;
			CachedRenderer.sharedMaterials = new Material[0];
		}
	}

	public void Build()
	{
		if (batchedSprites != null && batchedSprites.Length > 0)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.hideFlags = HideFlags.DontSave;
				CachedFilter.mesh = mesh;
			}
			else
			{
				mesh.Clear();
			}
			SortBatchedSprites();
			BuildRenderMesh();
			uvs = mesh.uv;
			indices = new List<TrianglesStruct>();
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				TrianglesStruct item = new TrianglesStruct
				{
					triangles = mesh.GetTriangles(i)
				};
				indices.Add(item);
			}
			vertices = mesh.vertices;
			colors = mesh.colors;
		}
		else if (mesh != null)
		{
			mesh.Clear();
			mesh = null;
			CachedFilter.mesh = mesh;
			CachedRenderer.sharedMaterials = new Material[0];
		}
	}

	private void SortBatchedSprites()
	{
		List<BatchedMesh> list = new List<BatchedMesh>();
		List<BatchedMesh> list2 = new List<BatchedMesh>();
		List<BatchedMesh> list3 = new List<BatchedMesh>();
		BatchedMesh[] array = batchedSprites;
		foreach (BatchedMesh batchedMesh in array)
		{
			if (!batchedMesh.IsDrawn)
			{
				list3.Add(batchedMesh);
				continue;
			}
			Material material = GetMaterial(batchedMesh);
			if (material != null)
			{
				if (material.renderQueue == 2000)
				{
					list.Add(batchedMesh);
				}
				else
				{
					list2.Add(batchedMesh);
				}
			}
			else
			{
				list.Add(batchedMesh);
			}
		}
		List<BatchedMesh> list4 = new List<BatchedMesh>(list.Count + list2.Count + list3.Count);
		list4.AddRange(list);
		list4.AddRange(list2);
		list4.AddRange(list3);
		Dictionary<BatchedMesh, int> dictionary = new Dictionary<BatchedMesh, int>();
		int num = 0;
		foreach (BatchedMesh item in list4)
		{
			dictionary[item] = num++;
		}
		foreach (BatchedMesh item2 in list4)
		{
			if (item2.parentId != -1)
			{
				item2.parentId = dictionary[batchedSprites[item2.parentId]];
			}
		}
		batchedSprites = list4.ToArray();
	}

	private Material GetMaterial(BatchedMesh bs)
	{
		switch (bs.type)
		{
		case BatchedMesh.Type.EmptyGameObject:
			return null;
		case BatchedMesh.Type.FBXMesh:
			return bs.fbxMaterial;
		default:
			return bs.GetSpriteDefinition().materialInst;
		}
	}

	private void BuildRenderMesh()
	{
		List<Material> list = new List<Material>();
		List<List<int>> list2 = new List<List<int>>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		int num = 0;
		BatchedMesh[] array = batchedSprites;
		foreach (BatchedMesh batchedMesh in array)
		{
			if (!batchedMesh.IsDrawn)
			{
				break;
			}
			int num2 = 0;
			int item = 0;
			switch (batchedMesh.type)
			{
			case BatchedMesh.Type.FBXMesh:
				num2 = batchedMesh.fbxMesh.vertexCount;
				item = batchedMesh.fbxMesh.triangles.Length;
				break;
			}
			num += num2;
			list3.Add(num2);
			list4.Add(item);
		}
		Vector3[] array2 = new Vector3[num];
		Color32[] array3 = new Color32[num];
		Vector2[] array4 = new Vector2[num];
		int num3 = 0;
		Material material = null;
		List<int> list5 = null;
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = scale.x;
		identity.m11 = scale.y;
		identity.m22 = scale.z;
		int num4 = 0;
		BatchedMesh[] array5 = batchedSprites;
		foreach (BatchedMesh batchedMesh2 in array5)
		{
			if (!batchedMesh2.IsDrawn)
			{
				break;
			}
			if (batchedMesh2.type == BatchedMesh.Type.EmptyGameObject)
			{
				num4++;
				continue;
			}
			int num5 = list3[num4];
			int num6 = list4[num4];
			Material material2 = GetMaterial(batchedMesh2);
			if (material2 != material)
			{
				if (material != null)
				{
					list.Add(material);
					list2.Add(list5);
				}
				material = material2;
				list5 = new List<int>();
			}
			Vector3[] array6 = new Vector3[num5];
			Vector2[] array7 = new Vector2[num5];
			Color32[] array8 = new Color32[num5];
			int[] array9 = new int[num6];
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			switch (batchedMesh2.type)
			{
			case BatchedMesh.Type.FBXMesh:
			{
				for (int k = 0; k < batchedMesh2.fbxMesh.vertices.Length; k++)
				{
					array6[k] = batchedMesh2.fbxMesh.vertices[k];
				}
				for (int l = 0; l < batchedMesh2.fbxMesh.uv.Length; l++)
				{
					array7[l] = batchedMesh2.fbxMesh.uv[l];
				}
				for (int m = 0; m < batchedMesh2.fbxMesh.triangles.Length; m++)
				{
					array9[m] = num3 + batchedMesh2.fbxMesh.triangles[m];
				}
				for (int n = 0; n < num5; n++)
				{
					if (batchedMesh2.fbxMesh.colors32.Length > 0)
					{
						array8[n] = batchedMesh2.fbxMesh.colors32[n];
					}
					else
					{
						array8[n] = Color.white;
					}
				}
				break;
			}
			}
			Matrix4x4 matrix4x = identity * batchedMesh2.relativeMatrix;
			for (int num7 = 0; num7 < num5; num7++)
			{
				Vector3 v = Vector3.Scale(array6[num7], batchedMesh2.baseScale);
				v = matrix4x.MultiplyPoint(v);
				array2[num3 + num7] = v;
				array4[num3 + num7] = array7[num7];
				array3[num3 + num7] = array8[num7];
			}
			list5.AddRange(array9);
			num3 += num5;
			num4++;
		}
		if (list5 != null)
		{
			list.Add(material);
			list2.Add(list5);
		}
		if ((bool)mesh)
		{
			mesh.vertices = array2;
			mesh.uv = array4;
			mesh.colors32 = array3;
			mesh.subMeshCount = list2.Count;
			for (int num8 = 0; num8 < list2.Count; num8++)
			{
				mesh.SetTriangles(list2[num8].ToArray(), num8);
			}
			mesh.RecalculateBounds();
		}
		CachedRenderer.sharedMaterials = list.ToArray();
	}

	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return false;
	}

	public void ForceBuild()
	{
		Build();
	}
}
