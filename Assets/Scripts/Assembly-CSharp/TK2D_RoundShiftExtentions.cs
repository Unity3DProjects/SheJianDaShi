using UnityEngine;

public static class TK2D_RoundShiftExtentions
{
	private static Vector2 resultVector2;

	private static Vector3 resultVector3;

	public static Vector2 FloorVector(this Vector2 vector)
	{
		resultVector2.x = Mathf.Floor(vector.x);
		resultVector2.y = Mathf.Floor(vector.y);
		return resultVector2;
	}

	public static Vector3 FloorVector(this Vector3 vector)
	{
		resultVector3.x = Mathf.Floor(vector.x);
		resultVector3.y = Mathf.Floor(vector.y);
		resultVector3.z = Mathf.Floor(vector.z);
		return resultVector3;
	}

	public static Vector3 RoundVector(this Vector3 vector)
	{
		resultVector3.x = Mathf.Round(vector.x);
		resultVector3.y = Mathf.Round(vector.y);
		resultVector3.z = Mathf.Round(vector.z);
		return resultVector3;
	}

	public static Vector2 RoundShift(this Transform transform, bool useSelfShift = true)
	{
		resultVector2.x = 0f;
		resultVector2.y = 0f;
		Transform transform2 = ((!useSelfShift) ? transform.parent : transform);
		while (transform2 != null)
		{
			Vector2 vector = transform2.localPosition - transform2.localPosition.FloorVector();
			resultVector2 += vector;
			transform2 = transform2.parent;
		}
		return resultVector2;
	}
}
