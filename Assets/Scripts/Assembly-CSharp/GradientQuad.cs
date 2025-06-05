using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GradientQuad : MonoBehaviour
{
	public enum GradientDirectionEnum
	{
		Horizontal = 0,
		Vertical = 1
	}

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
	private GradientDirectionEnum gradientDirection;

	[SerializeField]
	private Gradient gradientColors = new Gradient();

	[SerializeField]
	private Vector2 scale = Vector2.one;

	[SerializeField]
	private Vector2 offset = Vector2.zero;

	[SerializeField]
	private bool isUpdate = true;

	private Transform cachedTransform;

	private MeshRenderer cachedRenderer;

	private List<float> portions = new List<float>();

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

	public GradientDirectionEnum GradientDirection
	{
		get
		{
			return gradientDirection;
		}
		set
		{
			if (gradientDirection != value)
			{
				gradientDirection = value;
				UpdateMesh();
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
		UpdateOffset();
		UpdateMeshRenderer();
		UpdateMesh();
	}

	private void OnValidate()
	{
		if (isUpdate)
		{
			UpdateMesh();
		}
	}

	private void InitMaterial()
	{
	}

	private void UpdateMeshRenderer()
	{
	}

	private void UpdateOffset()
	{
		CachedRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
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
		portions.Clear();
		GradientColorKey[] colorKeys = gradientColors.colorKeys;
		for (int i = 0; i < colorKeys.Length; i++)
		{
			GradientColorKey gradientColorKey = colorKeys[i];
			if (!portions.Contains(gradientColorKey.time))
			{
				portions.Add(gradientColorKey.time);
			}
		}
		GradientAlphaKey[] alphaKeys = gradientColors.alphaKeys;
		for (int j = 0; j < alphaKeys.Length; j++)
		{
			GradientAlphaKey gradientAlphaKey = alphaKeys[j];
			if (!portions.Contains(gradientAlphaKey.time))
			{
				portions.Add(gradientAlphaKey.time);
			}
		}
		portions.Sort();
		int count = portions.Count;
		int num = ((count <= 2) ? 1 : (count - 1));
		int num2 = num * 2 + 2;
		Vector3[] array = new Vector3[num2];
		int[] array2 = new int[num * 2 * 3];
		Vector2[] array3 = new Vector2[num2];
		Color[] array4 = new Color[num2];
		Vector2 vector = size * 0.5f;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		switch (anchor)
		{
		case AnchorEnum.LowerLeft:
		case AnchorEnum.LowerCenter:
		case AnchorEnum.LowerRight:
			zero[(int)gradientDirection] = size[(int)gradientDirection];
			zero2 = size;
			zero3.x = (0f - vector.x) * (float)anchor;
			break;
		case AnchorEnum.MiddleLeft:
		case AnchorEnum.MiddleCenter:
		case AnchorEnum.MiddleRight:
			if (gradientDirection == GradientDirectionEnum.Horizontal)
			{
				zero.Set(size.x, 0f - vector.y);
				zero2.Set(size.x, vector.y);
				zero3.x = (0f - vector.x) * (float)(anchor - 3);
			}
			else if (gradientDirection == GradientDirectionEnum.Vertical)
			{
				zero.y = size.y;
				zero2 = size;
				zero3.Set((0f - vector.x) * (float)(anchor - 3), 0f - vector.y);
			}
			break;
		case AnchorEnum.UpperLeft:
		case AnchorEnum.UpperCenter:
		case AnchorEnum.UpperRight:
			if (gradientDirection == GradientDirectionEnum.Horizontal)
			{
				zero.Set(size.x, 0f - size.y);
				zero2.x = size.x;
				zero3.x = (0f - vector.x) * (float)(anchor - 6);
			}
			else if (gradientDirection == GradientDirectionEnum.Vertical)
			{
				zero.y = size.y;
				zero2 = size;
				zero3.Set((0f - vector.x) * (float)(anchor - 6), 0f - size.y);
			}
			break;
		}
		Vector3 localScale = CachedTransform.localScale;
		localScale.x = scale.x * sizeMultiplier.x;
		localScale.y = scale.y * sizeMultiplier.y;
		CachedTransform.localScale = localScale;
		for (int k = 0; k < num2 / 2; k++)
		{
			if (gradientDirection == GradientDirectionEnum.Horizontal)
			{
				array[2 * k] = new Vector3(zero3.x + zero.x * portions[k], zero3.y + zero.y);
				array[2 * k + 1] = new Vector3(zero3.x + zero2.x * portions[k], zero3.y + zero2.y);
				array3[2 * k] = new Vector2(portions[k], 0f);
				array3[2 * k + 1] = new Vector2(portions[k], 1f);
			}
			else if (gradientDirection == GradientDirectionEnum.Vertical)
			{
				array[2 * k] = new Vector3(zero3.x + zero.x, zero3.y + zero.y * portions[k]);
				array[2 * k + 1] = new Vector3(zero3.x + zero2.x, zero3.y + zero2.y * portions[k]);
				array3[2 * k] = new Vector2(0f, portions[k]);
				array3[2 * k + 1] = new Vector2(1f, portions[k]);
			}
			array3[2 * k].Scale(localScale);
			array3[2 * k + 1].Scale(localScale);
			array4[2 * k] = (array4[2 * k + 1] = gradientColors.Evaluate(portions[k]));
		}
		for (int l = 0; l < num; l++)
		{
			array2[6 * l] = 2 * l;
			array2[6 * l + 1] = 2 * l + 1;
			array2[6 * l + 2] = 2 * l + 2;
			array2[6 * l + 3] = 2 * l + 1;
			array2[6 * l + 4] = 2 * l + 3;
			array2[6 * l + 5] = 2 * l + 2;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = array3;
		mesh.colors = array4;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}
