using System;
using UnityEngine;

public class TouchInfo
{
	public enum TouchMoveDirection
	{
		UP = 0,
		RIGHT = 1,
		DOWN = 2,
		LEFT = 3,
		OTHER = 4
	}

	public static float StationaryDelta = 0f;

	public static float MaxAlphaAngle = (float)Math.PI / 4f;

	public static float MaxSnapDelta = Mathf.Sin(MaxAlphaAngle);

	public int id;

	public TouchPhase phase;

	public Vector2 delta = Vector2.zero;

	public Vector2 position = Vector2.zero;

	public Vector2 previousPosition = Vector2.zero;

	public Vector2 startPosition = Vector2.zero;

	public bool IsStationary
	{
		get
		{
			return (startPosition - position).magnitude < GetDPI() * 0.2f;
		}
	}

	public bool IsTap
	{
		get
		{
			return (startPosition - position).magnitude < GetDPI() * 0.2f;
		}
	}

	public TouchInfo()
	{
	}

	public TouchInfo(TouchInfo touch)
	{
		id = touch.id;
		phase = touch.phase;
		delta = touch.delta;
		position = touch.position;
		previousPosition = touch.previousPosition;
		startPosition = touch.startPosition;
	}

	public TouchInfo(Touch touch)
	{
		id = touch.fingerId;
		phase = touch.phase;
		position = touch.position;
		previousPosition = touch.position;
		startPosition = touch.position;
	}

	public void UpdatePosition(Vector2 newPosition)
	{
		previousPosition = position;
		position = newPosition;
		delta = previousPosition - position;
	}

	private static float GetDPI()
	{
		return (Screen.dpi != 0f) ? Screen.dpi : 160f;
	}

	public TouchMoveDirection GetTouchMoveDirection()
	{
		if (delta.magnitude >= StationaryDelta)
		{
			Vector2 normalized = delta.normalized;
			if (Mathf.Abs(normalized.y) + MaxSnapDelta > 1f && Mathf.Abs(normalized.x) < MaxSnapDelta)
			{
				if (normalized.y > 0f)
				{
					return TouchMoveDirection.DOWN;
				}
				return TouchMoveDirection.UP;
			}
			if (Mathf.Abs(normalized.x) + MaxSnapDelta > 1f && Mathf.Abs(normalized.y) < MaxSnapDelta)
			{
				if (normalized.x > 0f)
				{
					return TouchMoveDirection.LEFT;
				}
				return TouchMoveDirection.RIGHT;
			}
		}
		return TouchMoveDirection.OTHER;
	}
}
