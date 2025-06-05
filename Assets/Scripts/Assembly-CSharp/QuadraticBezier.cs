using UnityEngine;

public class QuadraticBezier
{
	public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = num2 * num;
		float num4 = t * t;
		float num5 = num4 * t;
		Vector3 vector = num3 * p0;
		vector += 3f * num2 * t * p1;
		vector += 3f * num * num4 * p2;
		return vector + num5 * p3;
	}
}
