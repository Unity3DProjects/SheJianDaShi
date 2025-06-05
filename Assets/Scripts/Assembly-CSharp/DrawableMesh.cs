using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class DrawableMesh : MonoBehaviour
{
	public enum VerticalAnchor
	{
		Top = 0,
		Center = 1,
		Bottom = 2
	}

	public enum HorizontalAnchor
	{
		Left = 0,
		Center = 1,
		Right = 2
	}

	[SerializeField]
	private tk2dSpriteCollectionData data;

	[SerializeField]
	private string spriteAtlasName;

	private tk2dSpriteCollectionData dataInstance;

	protected Vector2 uvFromSprite;

	protected bool isUVFromSpriteAvailible;

	[SerializeField]
	public VerticalAnchor vertAnchor;

	[SerializeField]
	public HorizontalAnchor horizAnchor;

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	[SerializeField]
	private Color color = Color.white;

	protected Color[] originalMeshColors;

	protected Vector3[] originalVertices;

	[SerializeField]
	private Vector2 scale = Vector3.one;

	protected MeshRenderer CachedMeshRenderer
	{
		get
		{
			if (meshRenderer == null)
			{
				meshRenderer = GetComponent<MeshRenderer>();
			}
			return meshRenderer;
		}
	}

	protected MeshFilter CachedMeshFilter
	{
		get
		{
			if (meshFilter == null)
			{
				meshFilter = GetComponent<MeshFilter>();
			}
			return meshFilter;
		}
	}

	public Color Color
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
			UpdateAnimatedColors();
		}
	}

	public Vector2 Scale
	{
		get
		{
			return scale;
		}
		set
		{
			scale = value;
			UpdateAnimatedScale();
		}
	}

	public virtual void UpdateMesh()
	{
	}

	public virtual void SetBaseMeshColor(Color c)
	{
	}

	protected virtual void UpdateAnimatedColors()
	{
		if (originalMeshColors == null)
		{
			return;
		}
		Mesh sharedMesh = CachedMeshFilter.sharedMesh;
		Color[] array = (Color[])originalMeshColors.Clone();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Color color = array[i];
				array[i] = new Color(color.r * this.color.r, color.g * this.color.g, color.b * this.color.b, color.a * this.color.a);
			}
			sharedMesh.colors = array;
		}
	}

	protected virtual void UpdateAnimatedScale()
	{
		if (originalVertices == null)
		{
			return;
		}
		Mesh sharedMesh = CachedMeshFilter.sharedMesh;
		Vector3[] array = (Vector3[])originalVertices.Clone();
		if (originalVertices != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = array[i];
				array[i] = new Vector3(vector.x * scale.x, vector.y * scale.y, 0f);
			}
			sharedMesh.vertices = array;
		}
	}

	protected void CheckSpriteCollectionData()
	{
		if (!(data != null))
		{
			return;
		}
		dataInstance = data.inst;
		if (!(dataInstance != null))
		{
			return;
		}
		tk2dSpriteDefinition[] spriteDefinitions = dataInstance.spriteDefinitions;
		foreach (tk2dSpriteDefinition tk2dSpriteDefinition2 in spriteDefinitions)
		{
			if (tk2dSpriteDefinition2.name.Equals(spriteAtlasName))
			{
				isUVFromSpriteAvailible = true;
				uvFromSprite = tk2dSpriteDefinition2.uvs[0];
				CachedMeshRenderer.sharedMaterial = tk2dSpriteDefinition2.materialInst;
				break;
			}
		}
	}

	protected void RecalculateAnchor(ref Vector3[] vertices)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		Vector3[] array = vertices;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 vector = array[i];
			num = Mathf.Max(num, vector.x);
			num3 = Mathf.Min(num3, vector.x);
			num2 = Mathf.Max(num2, vector.y);
			num4 = Mathf.Min(num4, vector.y);
		}
		Vector3 zero = Vector3.zero;
		if (vertAnchor == VerticalAnchor.Bottom)
		{
			zero.y = num4;
		}
		else if (vertAnchor == VerticalAnchor.Top)
		{
			zero.y = num2;
		}
		if (horizAnchor == HorizontalAnchor.Left)
		{
			zero.x = num3;
		}
		else if (horizAnchor == HorizontalAnchor.Right)
		{
			zero.x = num;
		}
		for (int j = 0; j < vertices.Length; j++)
		{
			vertices[j] -= zero;
		}
	}
}
