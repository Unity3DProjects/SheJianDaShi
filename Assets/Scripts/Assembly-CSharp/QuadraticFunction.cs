using System.Collections.Generic;
using UnityEngine;

public class QuadraticFunction : MonoBehaviour
{
	public static List<float> Solutions(float a, float b, float c, float d, float e)
	{
		List<float> list = new List<float>();
		float num = Mathf.Pow(b, 3f) / 8f - b * c * 0.5f + d;
		if (num != 0f)
		{
			float a2 = 1f;
			float b2 = 0f - c;
			float c2 = b * d - 4f * e;
			float d2 = 0f - (e * (Mathf.Pow(b, 2f) - 4f * c) + Mathf.Pow(d, 2f));
			List<float> list2 = CubicFunction.Solutions(a2, b2, c2, d2);
			list.Add(0.5f * (-0.5f * b + Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]) + Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.5f - c - list2[0] - 2f * num / Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]))));
			list.Add(0.5f * (-0.5f * b + Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]) - Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.5f - c - list2[0] - 2f * num / Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]))));
			list.Add(0.5f * (-0.5f * b - Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]) + Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.5f - c - list2[0] + 2f * num / Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]))));
			list.Add(0.5f * (-0.5f * b - Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]) - Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.5f - c - list2[0] + 2f * num / Mathf.Sqrt(Mathf.Pow(b, 2f) * 0.25f - c + list2[0]))));
		}
		else
		{
			float f = (0f - c + Mathf.Sqrt(Mathf.Pow(c, 2f) - 4f * e)) * 0.5f;
			float f2 = (0f - c - Mathf.Sqrt(Mathf.Pow(c, 2f) - 4f * e)) * 0.5f;
			list.Add(Mathf.Sqrt(f));
			list.Add(0f - Mathf.Sqrt(f));
			list.Add(Mathf.Sqrt(f2));
			list.Add(0f - Mathf.Sqrt(f2));
		}
		return list;
	}
}
