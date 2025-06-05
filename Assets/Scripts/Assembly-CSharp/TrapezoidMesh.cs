using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TrapezoidMesh : DrawableMesh
{
	[SerializeField]
	private float topWidth;

	[SerializeField]
	private float bottomWidth;

	[SerializeField]
	private float height;

	[SerializeField]
	private float smoothFactorX = 0.01f;

	[SerializeField]
	private float smoothFactorY = 0.01f;

	public float TopWidth
	{
		get
		{
			return topWidth;
		}
		set
		{
			topWidth = value;
			UpdateMesh();
		}
	}

	public float BottomWidth
	{
		get
		{
			return bottomWidth;
		}
		set
		{
			bottomWidth = value;
			UpdateMesh();
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
			UpdateMesh();
		}
	}

	private void Awake()
	{
		UpdateMesh();
	}

	public override void UpdateMesh()
	{
		Mesh mesh = new Mesh();
		base.CachedMeshFilter.sharedMesh = mesh;
		CheckSpriteCollectionData();
		Vector3[] vertices = new Vector3[8];
		Vector2[] array = new Vector2[8];
		Color[] array2 = new Color[8];
		int[] triangles = new int[30]
		{
			0, 2, 1, 0, 3, 2, 4, 3, 0, 4,
			7, 3, 7, 2, 3, 7, 6, 2, 6, 1,
			2, 6, 5, 1, 5, 0, 1, 5, 4, 0
		};
		vertices[0] = new Vector3((0f - bottomWidth) * 0.5f, (0f - height) * 0.5f, 0f);
		vertices[1] = new Vector3((0f - topWidth) * 0.5f, height * 0.5f, 0f);
		vertices[2] = new Vector3(topWidth * 0.5f, height * 0.5f, 0f);
		vertices[3] = new Vector3(bottomWidth * 0.5f, (0f - height) * 0.5f, 0f);
		for (int i = 4; i < 8; i++)
		{
			vertices[i] = new Vector3(vertices[i - 4].x * (1f + smoothFactorX), vertices[i - 4].y * (1f + smoothFactorY), 1f);
		}
		for (int j = 0; j < 8; j++)
		{
			vertices[j] *= SizeHelper.HeightFactor;
			array2[j] = Color.white;
			if (j > 3)
			{
				array2[j].a = 0f;
			}
			if (isUVFromSpriteAvailible)
			{
				array[j] = uvFromSprite;
			}
			else
			{
				array[j] = Vector2.zero;
			}
		}
		RecalculateAnchor(ref vertices);
		mesh.vertices = vertices;
		mesh.colors = array2;
		mesh.triangles = triangles;
		mesh.uv = array;
		mesh.RecalculateNormals();
		originalMeshColors = array2;
		originalVertices = vertices;
		if (base.Color != Color.white)
		{
			UpdateAnimatedColors();
		}
		if (base.Scale != Vector2.one)
		{
			UpdateAnimatedScale();
		}
	}

	public override void SetBaseMeshColor(Color c)
	{
		base.Color = c;
		UpdateAnimatedColors();
	}
}
