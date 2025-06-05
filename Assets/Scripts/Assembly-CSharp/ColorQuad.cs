using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ColorQuad : MonoBehaviour
{
	public enum AnchorEnum
	{
		LowerLeft = 0,
		LowerCenter = 1,
		LowerRight = 2,
		MiddleLeft = 3,
		MiddleCenter = 4,
		MiddleRight = 5,
		UpperLeft = 6,
		UpperCenter = 7,
		UpperRight = 8
	}

	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private AnchorEnum anchor = AnchorEnum.MiddleCenter;

	[SerializeField]
	private Vector2 size = Vector2.zero;

	[SerializeField]
	private Vector2 sizeMultiplier = Vector2.one;

	[SerializeField]
	private Color tintColor = Color.white;

	[SerializeField]
	private Vector2 scale = Vector2.one;

	[SerializeField]
	private Vector2 offset = Vector2.zero;

	[SerializeField]
	private bool isUpdate = true;

	private Transform cachedTransform;

	private MeshRenderer cachedRenderer;

	public AnchorEnum Anchor
	{
		get
		{
			return anchor;
		}
		set
		{
			if (anchor != value)
			{
				anchor = value;
				UpdateMesh();
			}
		}
	}

	public Vector2 Size
	{
		get
		{
			return size;
		}
		set
		{
			if (size != value)
			{
				size = value;
				UpdateMesh();
			}
		}
	}

	public Vector2 SizeMultiplier
	{
		get
		{
			return sizeMultiplier;
		}
	}

	public Color TintColor
	{
		get
		{
			return tintColor;
		}
		set
		{
			if (tintColor != value)
			{
				tintColor = value;
				UpdateMeshRenderer();
				UpdateColor();
			}
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
			if (scale != value)
			{
				scale = value;
				UpdateMeshRenderer();
				UpdateMesh();
			}
		}
	}

	public Vector2 Offset
	{
		get
		{
			return offset;
		}
		set
		{
			if (offset != value)
			{
				offset = value;
				UpdateOffset();
			}
		}
	}

	private Transform CachedTransform
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

	private MeshRenderer CachedRenderer
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
		InitMaterial();
		UpdateColor();
		UpdateOffset();
		UpdateMeshRenderer();
		UpdateMesh();
	}

	private void OnValidate()
	{
		if (isUpdate)
		{
			UpdateColor();
			UpdateOffset();
			UpdateMeshRenderer();
			UpdateMesh();
		}
	}

	private void InitMaterial()
	{
		CachedRenderer.sharedMaterial = new Material(CachedRenderer.sharedMaterial);
	}

	private void UpdateMeshRenderer()
	{
		CachedRenderer.enabled = tintColor.a > 0.01f && Mathf.Min(scale.x * sizeMultiplier.x, scale.y * sizeMultiplier.y) > 0.01f && Mathf.Min(size.x, size.y) > 0f;
	}

	private void UpdateOffset()
	{
		CachedRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
	}

	private void UpdateColor()
	{
		CachedRenderer.sharedMaterial.SetColor("_TintColor", tintColor);
	}

	private void UpdateMesh()
	{
		if (mesh == null)
		{
			mesh = GetComponent<MeshFilter>().sharedMesh;
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.hideFlags = HideFlags.DontSave;
				GetComponent<MeshFilter>().mesh = mesh;
			}
		}
		GetComponent<MeshFilter>().sharedMesh = mesh;
		mesh.Clear();
		int num = 1;
		int num2 = 4;
		Vector3[] array = new Vector3[num2];
		int[] array2 = new int[num * 2 * 3];
		Vector2[] array3 = new Vector2[num2];
		Vector2 vector = size * 0.5f;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		switch (anchor)
		{
		case AnchorEnum.LowerLeft:
		case AnchorEnum.LowerCenter:
		case AnchorEnum.LowerRight:
			zero.x = size.x;
			zero2 = size;
			zero3.x = (0f - vector.x) * (float)anchor;
			break;
		case AnchorEnum.MiddleLeft:
		case AnchorEnum.MiddleCenter:
		case AnchorEnum.MiddleRight:
			zero.Set(size.x, 0f - vector.y);
			zero2.Set(size.x, vector.y);
			zero3.x = (0f - vector.x) * (float)(anchor - 3);
			break;
		case AnchorEnum.UpperLeft:
		case AnchorEnum.UpperCenter:
		case AnchorEnum.UpperRight:
			zero.Set(size.x, 0f - size.y);
			zero2.x = size.x;
			zero3.x = (0f - vector.x) * (float)(anchor - 6);
			break;
		}
		Vector3 localScale = CachedTransform.localScale;
		localScale.x = scale.x * sizeMultiplier.x;
		localScale.y = scale.y * sizeMultiplier.y;
		CachedTransform.localScale = localScale;
		for (int i = 0; i < num2 / 2; i++)
		{
			array[2 * i] = new Vector3(zero3.x + zero.x * (float)i, zero3.y + zero.y);
			array[2 * i + 1] = new Vector3(zero3.x + zero2.x * (float)i, zero3.y + zero2.y);
			array3[2 * i] = new Vector2(i, 0f);
			array3[2 * i + 1] = new Vector2(i, 1f);
			array3[2 * i].Scale(localScale);
			array3[2 * i + 1].Scale(localScale);
		}
		for (int j = 0; j < num; j++)
		{
			array2[6 * j] = 2 * j;
			array2[6 * j + 1] = 2 * j + 1;
			array2[6 * j + 2] = 2 * j + 2;
			array2[6 * j + 3] = 2 * j + 1;
			array2[6 * j + 4] = 2 * j + 3;
			array2[6 * j + 5] = 2 * j + 2;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}
