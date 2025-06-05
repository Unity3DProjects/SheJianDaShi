using System;
using System.Collections.Generic;
using UnityEngine;

public class CubicFunction : MonoBehaviour
{
	public static List<float> Solutions(float a, float b, float c, float d)
	{
		List<float> list = new List<float>();
		float num = b / a;
		float num2 = c / a;
		float num3 = d / a;
		float num4 = (Mathf.Pow(num, 2f) - 3f * num2) / 9f;
		float num5 = (2f * Mathf.Pow(num, 3f) - 9f * num * num2 + 27f * num3) / 54f;
		float num6 = Mathf.Pow(num4, 3f);
		float num7 = Mathf.Pow(num5, 2f);
		if (num7 < num6)
		{
			float num8 = Mathf.Acos(num5 / Mathf.Sqrt(Mathf.Pow(num4, 3f))) / 3f;
			list.Add(-2f * Mathf.Sqrt(num4) * Mathf.Cos(num8) - num / 3f);
			list.Add(-2f * Mathf.Sqrt(num4) * Mathf.Cos(num8 + (float)Math.PI * 2f / 3f) - num / 3f);
			list.Add(-2f * Mathf.Sqrt(num4) * Mathf.Cos(num8 - (float)Math.PI * 2f / 3f) - num / 3f);
		}
		else
		{
			float num9 = (0f - Mathf.Sign(num5)) * Mathf.Pow(Mathf.Abs(num5) + Mathf.Sqrt(num7 - num6), 1f / 3f);
			float num10 = 0f;
			if (num9 != 0f)
			{
				num10 = num4 / num9;
			}
			list.Add(num9 + num10 - num / 3f);
			if (num9 == num10 && num9 > 0f)
			{
				list.Add(0f - num9 - num / 3f);
			}
		}
		return list;
	}
}
