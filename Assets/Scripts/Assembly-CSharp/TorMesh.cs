using System;
using System.Collections.Generic;
using UnityEngine;

public class TorMesh : DrawableMesh
{
	public enum GradientDirection
	{
		Radial = 1,
		Circle = 2
	}

	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private int vertexCount = 20;

	[SerializeField]
	private float segmentAngle = 360f;

	[SerializeField]
	private float smallRadius;

	[SerializeField]
	private float bigRadius = 20f;

	[SerializeField]
	private Vector2 stretchSize;

	[SerializeField]
	private float startAngle;

	[SerializeField]
	private GradientDirection direction;

	[SerializeField]
	private Gradient gradientColors;

	[SerializeField]
	private int smoothWidth;

	[SerializeField]
	private bool isNeedUpdateEditor;

	private List<float> gradientParts = new List<float>();

	public Vector2 StretchSize
	{
		get
		{
			return stretchSize;
		}
		set
		{
			stretchSize = value;
			UpdateMesh();
		}
	}

	public Vector2 BoundingBox
	{
		get
		{
			return new Vector2(2f * (bigRadius + StretchSize.x), 2f * (bigRadius + StretchSize.y));
		}
		set
		{
			Vector2 vector = new Vector2(Mathf.Max(0f, (value.x - 2f * bigRadius) * 0.5f), Mathf.Max(0f, (value.y - 2f * bigRadius) * 0.5f));
			if (!vector.Equals(StretchSize))
			{
				StretchSize = vector;
			}
		}
	}

	public float BigRadius
	{
		get
		{
			return bigRadius;
		}
		set
		{
			bigRadius = value;
			UpdateMesh();
		}
	}

	public float StartAngle
	{
		get
		{
			return startAngle;
		}
		set
		{
			startAngle = value;
		}
	}

	public float SegmentAngle
	{
		get
		{
			return segmentAngle;
		}
		set
		{
			segmentAngle = value;
		}
	}

	private void Awake()
	{
		UpdateMesh();
	}

	public override void UpdateMesh()
	{
		if (smoothWidth < 0)
		{
			smoothWidth = 0;
		}
		mesh = new Mesh();
		CheckSpriteCollectionData();
		base.CachedMeshFilter.sharedMesh = mesh;
		mesh.Clear();
		gradientParts.Clear();
		SmoothGradient();
		GradientColorKey[] colorKeys = gradientColors.colorKeys;
		for (int i = 0; i < colorKeys.Length; i++)
		{
			GradientColorKey gradientColorKey = colorKeys[i];
			if (!gradientParts.Contains(gradientColorKey.time))
			{
				gradientParts.Add(gradientColorKey.time);
			}
		}
		GradientAlphaKey[] alphaKeys = gradientColors.alphaKeys;
		for (int j = 0; j < alphaKeys.Length; j++)
		{
			GradientAlphaKey gradientAlphaKey = alphaKeys[j];
			if (!gradientParts.Contains(gradientAlphaKey.time))
			{
				gradientParts.Add(gradientAlphaKey.time);
			}
		}
		gradientParts.Sort();
		float num = segmentAngle / 360f;
		int count = gradientParts.Count;
		int num2 = count - 1;
		int num3 = (int)((float)vertexCount * num) + 2;
		int num4 = num3 * count;
		Vector3[] vertices = new Vector3[num4];
		int[] array = new int[(num3 - 1) * num2 * 2 * 3];
		Vector2[] array2 = new Vector2[num4];
		Color[] array3 = new Color[num4];
		Vector3[] array4 = new Vector3[num4];
		float num5 = bigRadius + (float)smoothWidth - smallRadius;
		float num6 = segmentAngle * ((float)Math.PI / 180f) / (float)(num3 - 1);
		for (int k = 0; k < num3; k++)
		{
			float num7 = (float)k * num6;
			float x = Mathf.Sin(num7 + startAngle * ((float)Math.PI / 180f));
			float y = Mathf.Cos(num7 + startAngle * ((float)Math.PI / 180f));
			for (int l = 0; l < count; l++)
			{
				float num8 = (float)k / (float)num3;
				float num9 = smallRadius + num5 * gradientParts[l];
				vertices[k * count + l] = new Vector3(x, y, 0f) * num9 * SizeHelper.HeightFactor;
				StretchVertex(ref vertices[k * count + l]);
				if (isUVFromSpriteAvailible)
				{
					array2[k * count + l] = uvFromSprite;
				}
				else
				{
					array2[k * count + l] = new Vector2(num8, gradientParts[l]);
				}
				if (direction == GradientDirection.Radial)
				{
					array3[k * count + l] = gradientColors.Evaluate(gradientParts[l]);
				}
				else
				{
					array3[k * count + l] = gradientColors.Evaluate(num8);
				}
				array4[k * count + l] = Vector3.up;
			}
		}
		int num10 = 0;
		for (int m = 0; m < num3 - 1; m++)
		{
			for (int n = 0; n < num2; n++)
			{
				array[num10] = (count * m + n) % num4;
				array[num10 + 1] = (count * m + 1 + n) % num4;
				array[num10 + 2] = (count * (m + 1) + n) % num4;
				array[num10 + 3] = (count * (m + 1) + n) % num4;
				array[num10 + 4] = (count * m + 1 + n) % num4;
				array[num10 + 5] = (count * (m + 1) + 1 + n) % num4;
				num10 += 6;
			}
		}
		if (vertAnchor != VerticalAnchor.Center || horizAnchor != HorizontalAnchor.Center)
		{
			RecalculateAnchor(ref vertices);
		}
		mesh.vertices = vertices;
		mesh.triangles = array;
		mesh.uv = array2;
		mesh.colors = array3;
		mesh.normals = array4;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		originalMeshColors = array3;
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
		GradientColorKey[] array = new GradientColorKey[gradientColors.colorKeys.Length];
		for (int i = 0; i < gradientColors.colorKeys.Length; i++)
		{
			GradientColorKey gradientColorKey = gradientColors.colorKeys[i];
			array[i] = new GradientColorKey(c, gradientColorKey.time);
		}
		gradientColors.SetKeys(array, gradientColors.alphaKeys);
		UpdateMesh();
	}

	private void SmoothGradient()
	{
		if (smoothWidth > 0 && gradientColors.colorKeys.Length >= 2 && gradientColors.alphaKeys.Length >= 2)
		{
			GradientAlphaKey[] array;
			if (smallRadius < 0.1f)
			{
				array = new GradientAlphaKey[3]
				{
					new GradientAlphaKey(255f, 0f),
					new GradientAlphaKey(255f, 1f - (float)smoothWidth / (bigRadius + (float)smoothWidth)),
					new GradientAlphaKey(0f, 1f)
				};
			}
			else
			{
				array = new GradientAlphaKey[4];
				array[0] = new GradientAlphaKey(0f, 0f);
				array[0] = new GradientAlphaKey(255f, (float)smoothWidth / (bigRadius + (float)smoothWidth));
				array[1] = new GradientAlphaKey(255f, 1f - (float)smoothWidth / (bigRadius + (float)smoothWidth));
				array[2] = new GradientAlphaKey(0f, 1f);
			}
			gradientColors.SetKeys(gradientColors.colorKeys, array);
		}
	}

	private void StretchVertex(ref Vector3 v)
	{
		if (v.x > 0f)
		{
			v.x += StretchSize.x * SizeHelper.HeightFactor;
		}
		else if (v.x < 0f)
		{
			v.x -= StretchSize.x * SizeHelper.HeightFactor;
		}
		if (v.y > 0f)
		{
			v.y += StretchSize.y * SizeHelper.HeightFactor;
		}
		else if (v.y < 0f)
		{
			v.y -= StretchSize.y * SizeHelper.HeightFactor;
		}
	}
}
