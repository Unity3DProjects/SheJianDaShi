using UnityEngine;

public static class TransformExtension
{
	public static void SetLocalPositionX(this Transform t, float x)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.x = x;
		t.localPosition = localPosition;
	}

	public static void SetLocalPositionY(this Transform t, float y)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.y = y;
		t.localPosition = localPosition;
	}

	public static void SetLocalPositionZ(this Transform t, float z)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.z = z;
		t.localPosition = localPosition;
	}

	public static string FullTransformName(this Transform t)
	{
		Transform transform = t;
		string text = string.Empty;
		while (transform != null)
		{
			text = "/" + transform.name + text;
			transform = transform.parent;
		}
		return text;
	}

	public static void SetGlobalPositionX(this Transform t, float x)
	{
		Vector3 position = t.position;
		position.x = x;
		t.position = position;
	}

	public static void SetGlobalPositionY(this Transform t, float y)
	{
		Vector3 position = t.position;
		position.y = y;
		t.position = position;
	}

	public static void SetGlobalPositionZ(this Transform t, float z)
	{
		Vector3 position = t.position;
		position.z = z;
		t.position = position;
	}
}
