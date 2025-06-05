using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CircleColoredMesh : tk2dBaseSprite
{
	[SerializeField]
	private int vertexCount = 10;

	public int VertexCount
	{
		get
		{
			return vertexCount;
		}
		set
		{
			vertexCount = value;
		}
	}

	private new void Awake()
	{
		base.Awake();
		Build();
		UpdateMaterial();
	}

	public override void Build()
	{
		MeshClear();
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3[] array = new Vector3[vertexCount * 2 - 1];
		Vector3[] array2 = new Vector3[vertexCount * 2 - 1];
		Color[] array3 = new Color[vertexCount * 2 - 1];
		Vector2[] array4 = new Vector2[vertexCount * 2 - 1];
		for (int i = 0; i < vertexCount; i++)
		{
			float f = ((float)i - 1f) / ((float)vertexCount - 1f) * (float)Math.PI * 2f;
			array[i] = new Vector3(base.scale.x * Mathf.Sin(f), base.scale.y * Mathf.Cos(f), 0f);
			array3[i] = base.color;
			array2[i] = Vector3.one;
			array4[i] = currentSprite.uvs[0];
			if (i > 0)
			{
				array[vertexCount - 1 + i] = new Vector3(1.03f * base.scale.x * Mathf.Sin(f), 1.03f * base.scale.y * Mathf.Cos(f), 0f);
				array3[vertexCount - 1 + i] = base.color;
				array3[vertexCount - 1 + i].a = 0f;
				array2[vertexCount - 1 + i] = Vector3.one;
				array4[vertexCount - 1 + i] = currentSprite.uvs[0];
			}
		}
		array[0] = Vector3.zero;
		int[] array5 = new int[(vertexCount - 1) * 9];
		for (int j = 1; j < vertexCount - 1; j++)
		{
			array5[(j - 1) * 9] = 0;
			array5[(j - 1) * 9 + 1] = j + 1;
			array5[(j - 1) * 9 + 2] = j;
			array5[(j - 1) * 9 + 3] = j;
			array5[(j - 1) * 9 + 4] = vertexCount + j;
			array5[(j - 1) * 9 + 5] = vertexCount - 1 + j;
			array5[(j - 1) * 9 + 6] = j;
			array5[(j - 1) * 9 + 7] = j + 1;
			array5[(j - 1) * 9 + 8] = vertexCount + j;
		}
		array5[(vertexCount - 2) * 9] = 0;
		array5[(vertexCount - 2) * 9 + 1] = 1;
		array5[(vertexCount - 2) * 9 + 2] = vertexCount - 1;
		array5[(vertexCount - 2) * 9 + 3] = vertexCount - 1;
		array5[(vertexCount - 2) * 9 + 4] = vertexCount - 1 + 1;
		array5[(vertexCount - 2) * 9 + 5] = (vertexCount - 1) * 2;
		array5[(vertexCount - 2) * 9 + 6] = vertexCount - 1;
		array5[(vertexCount - 2) * 9 + 7] = 1;
		array5[(vertexCount - 2) * 9 + 8] = vertexCount;
		base.MeshVertices = array;
		base.MeshColors = array3;
		base.MeshUV = array4;
		base.MeshTriangles = array5;
	}

	protected override void UpdateMaterial()
	{
		base.CurrentMaterial = base.CurrentSprite.materialInst;
	}

	protected override void UpdateColors()
	{
	}

	protected override void UpdateVertices()
	{
	}

	protected override void UpdateGeometry()
	{
	}

	protected override int GetCurrentVertexCount()
	{
		return vertexCount;
	}
}
