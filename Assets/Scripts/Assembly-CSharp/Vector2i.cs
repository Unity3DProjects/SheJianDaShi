using System;
using UnityEngine;

[Serializable]
public struct Vector2i
{
	public int x;

	public int y;

	public static Vector2i zero
	{
		get
		{
			return new Vector2i(0, 0);
		}
	}

	public static Vector2i one
	{
		get
		{
			return new Vector2i(1, 1);
		}
	}

	public static Vector2i left
	{
		get
		{
			return new Vector2i(-1, 0);
		}
	}

	public static Vector2i right
	{
		get
		{
			return new Vector2i(1, 0);
		}
	}

	public static Vector2i up
	{
		get
		{
			return new Vector2i(0, 1);
		}
	}

	public static Vector2i down
	{
		get
		{
			return new Vector2i(0, -1);
		}
	}

	public Vector2i(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Vector3 ConvertToV3()
	{
		return new Vector3(x, y);
	}

	public static int Distance(Vector2i v1, Vector2i v2)
	{
		return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
	}

	public static bool operator ==(Vector2i v1, Vector2i v2)
	{
		return v1.x == v2.x && v1.y == v2.y;
	}

	public static bool operator !=(Vector2i v1, Vector2i v2)
	{
		return v1.x != v2.x || v1.y != v2.y;
	}

	public static Vector2i operator +(Vector2i v1, Vector2i v2)
	{
		return new Vector2i(v1.x + v2.x, v1.y + v2.y);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector2i))
		{
			return false;
		}
		Vector2i vector2i = (Vector2i)other;
		return x.Equals(vector2i.x) && y.Equals(vector2i.y);
	}

	public override int GetHashCode()
	{
		return x ^ y;
	}

	public override string ToString()
	{
		return string.Empty + x + "  " + y;
	}
}
