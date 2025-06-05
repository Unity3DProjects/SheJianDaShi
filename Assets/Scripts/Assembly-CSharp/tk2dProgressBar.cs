using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class tk2dProgressBar : tk2dSprite
{
	public enum FillDirection
	{
		Horizontal = 0,
		Vertical = 1,
		Radial90 = 2,
		Radial180 = 3,
		Radial360 = 4
	}

	private static float[] constRadial360Matrix = new float[16]
	{
		0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0.5f, 1f, 0f, 0.5f,
		0.5f, 1f, 0f, 0.5f, 0f, 0.5f
	};

	private Vector3[] tempVertices;

	private Vector2[] tempUvs;

	private Color[] tempColors;

	private int[] tempIndexes;

	private List<Vector3> tempVertexList = new List<Vector3>();

	private List<Vector2> tempUVList = new List<Vector2>();

	private Vector2[] tempOXY;

	private Vector2[] tempOUV;

	[SerializeField]
	private FillDirection mFillDirection = FillDirection.Radial360;

	[SerializeField]
	private float mFillAmount = 1f;

	[SerializeField]
	private bool mInvert;

	[SerializeField]
	private bool mRotated;

	private bool changed;

	public FillDirection fillDirection
	{
		get
		{
			return mFillDirection;
		}
		set
		{
			if (mFillDirection != value)
			{
				mFillDirection = value;
				changed = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mFillAmount != num)
			{
				mFillAmount = num;
				changed = true;
			}
		}
	}

	public bool invert
	{
		get
		{
			return mInvert;
		}
		set
		{
			if (mInvert != value)
			{
				mInvert = value;
				changed = true;
			}
		}
	}

	public bool rotated
	{
		get
		{
			return mRotated;
		}
		set
		{
			if (mRotated != value)
			{
				mRotated = value;
				changed = true;
			}
		}
	}

	public override void Build()
	{
		BuildMesh();
		UpdateMaterial();
		CreateCollider();
	}

	protected override void UpdateGeometry()
	{
		BuildMesh();
	}

	protected override void UpdateVertices()
	{
		BuildMesh();
	}

	protected override void Update()
	{
		base.Update();
		if (changed)
		{
			BuildMesh();
		}
	}

	private void BuildMesh()
	{
		MeshClear();
		if (meshColliderMesh != null)
		{
			meshColliderMesh.Clear();
		}
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Bounds bounds = currentSprite.GetBounds();
		Vector3 vector = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
		float num = mFillAmount;
		bool flag = false;
		if (!isRoundShiftDisabled && (mFillDirection == FillDirection.Horizontal || mFillDirection == FillDirection.Vertical))
		{
			float num2 = ((mFillDirection != FillDirection.Horizontal) ? vector.y : vector.x);
			num *= num2;
			num = ((!mInvert) ? Mathf.Floor(num) : Mathf.Ceil(num));
			num /= num2;
			flag = true;
		}
		OnFill(ref tempVertices, ref tempUvs, ref tempColors, num);
		Vector3 vector2 = new Vector3((0f - bounds.size.x) * 0.5f * base.scale.x, bounds.size.y * 0.5f * base.scale.y) + bounds.center;
		Vector3 b = vector;
		b.Scale(base.scale);
		for (int i = 0; i < tempVertices.Length; i++)
		{
			tempVertices[i] = Vector3.Scale(tempVertices[i], b) + vector2;
			if (!isRoundShiftDisabled)
			{
				if (flag)
				{
					tempVertices[i] = RoundVertex(tempVertices[i]);
				}
				else
				{
					tempVertices[i] = FloorVertex(tempVertices[i]);
				}
			}
		}
		if (rotated ^ (currentSprite.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW))
		{
			float num3 = 1f;
			float num4 = 1f;
			float num5 = 0f;
			float num6 = 0f;
			Vector2[] uvs = currentSprite.uvs;
			for (int j = 0; j < uvs.Length; j++)
			{
				Vector2 vector3 = uvs[j];
				num3 = ((!(num3 > vector3.x)) ? num3 : vector3.x);
				num4 = ((!(num4 > vector3.y)) ? num4 : vector3.y);
				num5 = ((!(num5 < vector3.x)) ? num5 : vector3.x);
				num6 = ((!(num6 < vector3.y)) ? num6 : vector3.y);
			}
			float num7 = num5 - num3;
			float num8 = num6 - num4;
			Vector2 vector4 = new Vector2(num3, num4);
			Vector2 vector5 = new Vector2(num5, num4);
			for (int k = 0; k < tempUvs.Length; k++)
			{
				Vector2 vector6 = tempUvs[k] - vector4;
				tempUvs[k] = vector5 + new Vector2((0f - num7) * vector6.y / num8, num8 * vector6.x / num7);
			}
		}
		else if (rotated ^ (currentSprite.flipped == tk2dSpriteDefinition.FlipMode.Tk2d))
		{
			float num9 = 1f;
			float num10 = 1f;
			float num11 = 0f;
			float num12 = 0f;
			Vector2[] uvs2 = currentSprite.uvs;
			for (int l = 0; l < uvs2.Length; l++)
			{
				Vector2 vector7 = uvs2[l];
				num9 = ((!(num9 > vector7.x)) ? num9 : vector7.x);
				num10 = ((!(num10 > vector7.y)) ? num10 : vector7.y);
				num11 = ((!(num11 < vector7.x)) ? num11 : vector7.x);
				num12 = ((!(num12 < vector7.y)) ? num12 : vector7.y);
			}
			float num13 = num11 - num9;
			float num14 = num12 - num10;
			Vector2 vector8 = new Vector2(num9, num10);
			Vector2 vector9 = new Vector2(num9, num10);
			for (int m = 0; m < tempUvs.Length; m++)
			{
				Vector2 vector10 = tempUvs[m] - vector8;
				tempUvs[m] = vector9 + new Vector2(num13 * vector10.y / num14, num14 * vector10.x / num13);
			}
		}
		int num15 = tempVertices.Length / 4;
		tempIndexes = ArrayExtention.EnsureLength(tempIndexes, num15 * 6);
		for (int n = 0; n < num15; n++)
		{
			tempIndexes[6 * n] = 4 * n;
			tempIndexes[6 * n + 1] = 4 * n + 1;
			tempIndexes[6 * n + 2] = 4 * n + 2;
			tempIndexes[6 * n + 3] = 4 * n + 2;
			tempIndexes[6 * n + 4] = 4 * n + 3;
			tempIndexes[6 * n + 5] = 4 * n;
		}
		base.MeshVertices = tempVertices;
		base.MeshUV = tempUvs;
		base.MeshColors = tempColors;
		base.MeshTriangles = tempIndexes;
		if (meshColliderMesh != null)
		{
			meshColliderMesh.vertices = tempVertices;
			meshColliderMesh.uv = tempUvs;
			meshColliderMesh.colors = tempColors;
			meshColliderMesh.triangles = tempIndexes;
		}
		changed = false;
	}

	private bool AdjustRadial(Vector3[] xy, Vector2[] uv, float fill, bool invert)
	{
		Vector2[] array = new Vector2[xy.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = xy[i];
		}
		bool flag = AdjustRadial(array, uv, fill, invert);
		for (int j = 0; j < xy.Length; j++)
		{
			if (flag)
			{
				xy[j] = array[j];
			}
			else
			{
				xy[j] = Vector2.zero;
			}
		}
		return flag;
	}

	private bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (!invert)
		{
			num = 1f - num;
		}
		num *= (float)Math.PI / 2f;
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Cos(num);
		if (num2 > num3)
		{
			num3 *= 1f / num2;
			num2 = 1f;
			if (!invert)
			{
				xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
				xy[3].y = xy[0].y;
				uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
				uv[3].y = uv[0].y;
			}
		}
		else if (num3 > num2)
		{
			num2 *= 1f / num3;
			num3 = 1f;
			if (invert)
			{
				xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
				xy[1].x = xy[0].x;
				uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
				uv[1].x = uv[0].x;
			}
		}
		else
		{
			num2 = 1f;
			num3 = 1f;
		}
		if (invert)
		{
			xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
			uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
		}
		else
		{
			xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
			uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
		}
		return true;
	}

	private void Rotate(Vector2[] v, int offset)
	{
		for (int i = 0; i < offset; i++)
		{
			Vector2 vector = new Vector2(v[3].x, v[3].y);
			v[3].x = v[2].y;
			v[3].y = v[2].x;
			v[2].x = v[1].y;
			v[2].y = v[1].x;
			v[1].x = v[0].y;
			v[1].y = v[0].x;
			v[0].x = vector.y;
			v[0].y = vector.x;
		}
	}

	public void OnFill(ref Vector3[] verts_array, ref Vector2[] uvs_array, ref Color[] cols_array, float curFillAmount)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float x = 0f;
		float y = 0f;
		float num = 1f;
		float num2 = -1f;
		float num3 = 1f;
		float num4 = 1f;
		float num5 = 0f;
		float num6 = 0f;
		Vector2[] uvs = currentSprite.uvs;
		for (int i = 0; i < uvs.Length; i++)
		{
			Vector2 vector = uvs[i];
			num3 = ((!(num3 > vector.x)) ? num3 : vector.x);
			num4 = ((!(num4 > vector.y)) ? num4 : vector.y);
			num5 = ((!(num5 < vector.x)) ? num5 : vector.x);
			num6 = ((!(num6 < vector.y)) ? num6 : vector.y);
		}
		if (mFillDirection == FillDirection.Horizontal || mFillDirection == FillDirection.Vertical)
		{
			float num7 = (num5 - num3) * curFillAmount;
			float num8 = (num6 - num4) * curFillAmount;
			if (fillDirection == FillDirection.Horizontal)
			{
				if (mInvert)
				{
					x = 1f - curFillAmount;
					num3 = num5 - num7;
				}
				else
				{
					num *= curFillAmount;
					num5 = num3 + num7;
				}
			}
			else if (fillDirection == FillDirection.Vertical)
			{
				if (mInvert)
				{
					num2 *= curFillAmount;
					num4 = num6 - num8;
				}
				else
				{
					y = 0f - (1f - curFillAmount);
					num6 = num4 + num8;
				}
			}
		}
		bool flag = fillDirection == FillDirection.Radial90 || fillDirection == FillDirection.Horizontal || fillDirection == FillDirection.Vertical;
		verts_array = ArrayExtention.EnsureLength(verts_array, 4, !flag);
		uvs_array = ArrayExtention.EnsureLength(uvs_array, 4, !flag);
		verts_array[0] = new Vector2(num, y);
		verts_array[1] = new Vector2(num, num2);
		verts_array[2] = new Vector2(x, num2);
		verts_array[3] = new Vector2(x, y);
		uvs_array[0] = new Vector2(num5, num6);
		uvs_array[1] = new Vector2(num5, num4);
		uvs_array[2] = new Vector2(num3, num4);
		uvs_array[3] = new Vector2(num3, num6);
		if (flag)
		{
			if (fillDirection == FillDirection.Radial90)
			{
				AdjustRadial(verts_array, uvs_array, curFillAmount, mInvert);
			}
		}
		else
		{
			tempVertexList.Clear();
			tempUVList.Clear();
			tempOXY = ArrayExtention.EnsureLength(tempOXY, 4);
			tempOUV = ArrayExtention.EnsureLength(tempOUV, 4);
			if (fillDirection == FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					tempOXY[0] = new Vector2(0f, 0f);
					tempOXY[1] = new Vector2(0f, 1f);
					tempOXY[2] = new Vector2(1f, 1f);
					tempOXY[3] = new Vector2(1f, 0f);
					tempOUV[0] = new Vector2(0f, 0f);
					tempOUV[1] = new Vector2(0f, 1f);
					tempOUV[2] = new Vector2(1f, 1f);
					tempOUV[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (j > 0)
						{
							Rotate(tempOXY, j);
							Rotate(tempOUV, j);
						}
					}
					else if (j < 1)
					{
						Rotate(tempOXY, 1 - j);
						Rotate(tempOUV, 1 - j);
					}
					float a;
					float b;
					if (j == 1)
					{
						a = ((!mInvert) ? 1f : 0.5f);
						b = ((!mInvert) ? 0.5f : 1f);
					}
					else
					{
						a = ((!mInvert) ? 0.5f : 1f);
						b = ((!mInvert) ? 1f : 0.5f);
					}
					tempOXY[1].y = Mathf.Lerp(a, b, tempOXY[1].y);
					tempOXY[2].y = Mathf.Lerp(a, b, tempOXY[2].y);
					tempOUV[1].y = Mathf.Lerp(a, b, tempOUV[1].y);
					tempOUV[2].y = Mathf.Lerp(a, b, tempOUV[2].y);
					float fill = curFillAmount * 2f - (float)j;
					bool flag2 = j % 2 == 1;
					if (!AdjustRadial(tempOXY, tempOUV, fill, !flag2))
					{
						continue;
					}
					if (mInvert)
					{
						flag2 = !flag2;
					}
					if (flag2)
					{
						for (int k = 0; k < 4; k++)
						{
							a = Mathf.Lerp(verts_array[0].x, verts_array[2].x, tempOXY[k].x);
							b = Mathf.Lerp(verts_array[0].y, verts_array[2].y, tempOXY[k].y);
							float x2 = Mathf.Lerp(uvs_array[0].x, uvs_array[2].x, tempOUV[k].x);
							float y2 = Mathf.Lerp(uvs_array[0].y, uvs_array[2].y, tempOUV[k].y);
							tempVertexList.Add(new Vector3(a, b, 0f));
							tempUVList.Add(new Vector2(x2, y2));
						}
						continue;
					}
					for (int num9 = 3; num9 > -1; num9--)
					{
						a = Mathf.Lerp(verts_array[0].x, verts_array[2].x, tempOXY[num9].x);
						b = Mathf.Lerp(verts_array[0].y, verts_array[2].y, tempOXY[num9].y);
						float x3 = Mathf.Lerp(uvs_array[0].x, uvs_array[2].x, tempOUV[num9].x);
						float y3 = Mathf.Lerp(uvs_array[0].y, uvs_array[2].y, tempOUV[num9].y);
						tempVertexList.Add(new Vector3(a, b, 0f));
						tempUVList.Add(new Vector2(x3, y3));
					}
				}
			}
			else if (fillDirection == FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					tempOXY[0] = new Vector2(0f, 0f);
					tempOXY[1] = new Vector2(0f, 1f);
					tempOXY[2] = new Vector2(1f, 1f);
					tempOXY[3] = new Vector2(1f, 0f);
					tempOUV[0] = new Vector2(0f, 0f);
					tempOUV[1] = new Vector2(0f, 1f);
					tempOUV[2] = new Vector2(1f, 1f);
					tempOUV[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (l > 0)
						{
							Rotate(tempOXY, l);
							Rotate(tempOUV, l);
						}
					}
					else if (l < 3)
					{
						Rotate(tempOXY, 3 - l);
						Rotate(tempOUV, 3 - l);
					}
					for (int m = 0; m < 4; m++)
					{
						int num10 = ((!mInvert) ? (l * 4) : ((3 - l) * 4));
						float a2 = constRadial360Matrix[num10];
						float b2 = constRadial360Matrix[num10 + 1];
						float a3 = constRadial360Matrix[num10 + 2];
						float b3 = constRadial360Matrix[num10 + 3];
						tempOXY[m].x = Mathf.Lerp(a2, b2, tempOXY[m].x);
						tempOXY[m].y = Mathf.Lerp(a3, b3, tempOXY[m].y);
						tempOUV[m].x = Mathf.Lerp(a2, b2, tempOUV[m].x);
						tempOUV[m].y = Mathf.Lerp(a3, b3, tempOUV[m].y);
					}
					float fill2 = curFillAmount * 4f - (float)l;
					bool flag3 = l % 2 == 1;
					if (!AdjustRadial(tempOXY, tempOUV, fill2, !flag3))
					{
						continue;
					}
					if (mInvert)
					{
						flag3 = !flag3;
					}
					if (flag3)
					{
						for (int n = 0; n < 4; n++)
						{
							float x4 = Mathf.Lerp(verts_array[0].x, verts_array[2].x, tempOXY[n].x);
							float y4 = Mathf.Lerp(verts_array[0].y, verts_array[2].y, tempOXY[n].y);
							float x5 = Mathf.Lerp(uvs_array[0].x, uvs_array[2].x, tempOUV[n].x);
							float y5 = Mathf.Lerp(uvs_array[0].y, uvs_array[2].y, tempOUV[n].y);
							tempVertexList.Add(new Vector3(x4, y4, 0f));
							tempUVList.Add(new Vector2(x5, y5));
						}
						continue;
					}
					for (int num11 = 3; num11 > -1; num11--)
					{
						float x6 = Mathf.Lerp(verts_array[0].x, verts_array[2].x, tempOXY[num11].x);
						float y6 = Mathf.Lerp(verts_array[0].y, verts_array[2].y, tempOXY[num11].y);
						float x7 = Mathf.Lerp(uvs_array[0].x, uvs_array[2].x, tempOUV[num11].x);
						float y7 = Mathf.Lerp(uvs_array[0].y, uvs_array[2].y, tempOUV[num11].y);
						tempVertexList.Add(new Vector3(x6, y6, 0f));
						tempUVList.Add(new Vector2(x7, y7));
					}
				}
			}
			verts_array = ArrayExtention.EnsureLength(verts_array, tempVertexList.Count);
			uvs_array = ArrayExtention.EnsureLength(uvs_array, tempUVList.Count);
			for (int num12 = 0; num12 < tempVertexList.Count; num12++)
			{
				verts_array[num12] = tempVertexList[num12];
			}
			for (int num13 = 0; num13 < tempUVList.Count; num13++)
			{
				uvs_array[num13] = tempUVList[num13];
			}
		}
		cols_array = ArrayExtention.EnsureLength(cols_array, verts_array.Length);
		for (int num14 = 0; num14 < cols_array.Length; num14++)
		{
			cols_array[num14] = base.color;
		}
	}
}
