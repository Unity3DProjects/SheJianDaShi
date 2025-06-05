using UnityEngine;

public static class ObjectExtension
{
	public static bool IsNull(this object obj)
	{
		if (obj is Object)
		{
			return !(obj as Object);
		}
		return obj == null;
	}
}
