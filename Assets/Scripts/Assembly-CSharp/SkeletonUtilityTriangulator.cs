using System.Collections.Generic;
using UnityEngine;

public static class SkeletonUtilityTriangulator
{
	private static List<Vector2> meshPoints = new List<Vector2>();

	private static List<int> meshIndices = new List<int>();

	public static Mesh CreateMesh(Vector2[] poly, float extrusion)
	{
		meshPoints.Clear();
		meshPoints.AddRange(poly);
		int[] array = Triangulate();
		Mesh mesh = new Mesh();
		Vector3[] array2 = new Vector3[poly.Length * 2];
		for (int i = 0; i < poly.Length; i++)
		{
			array2[i].x = poly[i].x;
			array2[i].y = poly[i].y;
			array2[i].z = 0f - extrusion;
			array2[i + poly.Length].x = poly[i].x;
			array2[i + poly.Length].y = poly[i].y;
			array2[i + poly.Length].z = extrusion;
		}
		int[] array3 = new int[array.Length * 2 + poly.Length * 6];
		int num = 0;
		for (int j = 0; j < array.Length; j += 3)
		{
			array3[j] = array[j];
			array3[j + 1] = array[j + 1];
			array3[j + 2] = array[j + 2];
		}
		num += array.Length;
		for (int k = 0; k < array.Length; k += 3)
		{
			array3[num + k] = array[k + 2] + poly.Length;
			array3[num + k + 1] = array[k + 1] + poly.Length;
			array3[num + k + 2] = array[k] + poly.Length;
		}
		Vector2[] array4 = new Vector2[array2.Length];
		for (int l = 0; l < array4.Length; l++)
		{
			array4[l] = new Vector2(array2[l].x, array2[l].z);
		}
		mesh.vertices = array2;
		mesh.triangles = array3;
		mesh.uv = array4;
		mesh = SideExtrusion(mesh);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	private static int[] Triangulate()
	{
		meshIndices.Clear();
		int count = meshPoints.Count;
		if (count < 3)
		{
			return meshIndices.ToArray();
		}
		int[] array = new int[count];
		if (Area() > 0f)
		{
			for (int i = 0; i < count; i++)
			{
				array[i] = i;
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				array[j] = count - 1 - j;
			}
		}
		int num = count;
		int num2 = 2 * num;
		int num3 = 0;
		int num4 = num - 1;
		while (num > 2)
		{
			if (num2-- <= 0)
			{
				return meshIndices.ToArray();
			}
			int num5 = num4;
			if (num <= num5)
			{
				num5 = 0;
			}
			num4 = num5 + 1;
			if (num <= num4)
			{
				num4 = 0;
			}
			int num6 = num4 + 1;
			if (num <= num6)
			{
				num6 = 0;
			}
			if (Snip(num5, num4, num6, num, array))
			{
				int item = array[num5];
				int item2 = array[num4];
				int item3 = array[num6];
				meshIndices.Add(item);
				meshIndices.Add(item2);
				meshIndices.Add(item3);
				num3++;
				int num7 = num4;
				for (int k = num4 + 1; k < num; k++)
				{
					array[num7] = array[k];
					num7++;
				}
				num--;
				num2 = 2 * num;
			}
		}
		meshIndices.Reverse();
		return meshIndices.ToArray();
	}

	private static float Area()
	{
		int count = meshPoints.Count;
		float num = 0f;
		int index = count - 1;
		int num2 = 0;
		while (num2 < count)
		{
			Vector2 vector = meshPoints[index];
			Vector2 vector2 = meshPoints[num2];
			num += vector.x * vector2.y - vector2.x * vector.y;
			index = num2++;
		}
		return num * 0.5f;
	}

	private static bool Snip(int u, int v, int w, int n, int[] V)
	{
		Vector2 a = meshPoints[V[u]];
		Vector2 b = meshPoints[V[v]];
		Vector2 c = meshPoints[V[w]];
		if (Mathf.Epsilon > (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x))
		{
			return false;
		}
		for (int i = 0; i < n; i++)
		{
			if (i != u && i != v && i != w)
			{
				Vector2 p = meshPoints[V[i]];
				if (InsideTriangle(a, b, c, p))
				{
					return false;
				}
			}
		}
		return true;
	}

	private static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
	{
		float num = C.x - B.x;
		float num2 = C.y - B.y;
		float num3 = A.x - C.x;
		float num4 = A.y - C.y;
		float num5 = B.x - A.x;
		float num6 = B.y - A.y;
		float num7 = P.x - A.x;
		float num8 = P.y - A.y;
		float num9 = P.x - B.x;
		float num10 = P.y - B.y;
		float num11 = P.x - C.x;
		float num12 = P.y - C.y;
		float num13 = num * num10 - num2 * num9;
		float num14 = num5 * num8 - num6 * num7;
		float num15 = num3 * num12 - num4 * num11;
		return num13 >= 0f && num15 >= 0f && num14 >= 0f;
	}

	private static Mesh SideExtrusion(Mesh mesh)
	{
		List<int> list = new List<int>(mesh.triangles);
		int num = mesh.vertices.Length / 2;
		for (int i = 0; i < num; i++)
		{
			int num2 = i;
			int num3 = (num2 + 1) % num;
			int item = num2 + num;
			int item2 = num3 + num;
			list.Add(item2);
			list.Add(item);
			list.Add(num2);
			list.Add(num3);
			list.Add(item2);
			list.Add(num2);
		}
		mesh.triangles = list.ToArray();
		return mesh;
	}
}
