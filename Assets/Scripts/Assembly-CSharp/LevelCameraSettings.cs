using System;
using UnityEngine;

[Serializable]
public struct LevelCameraSettings
{
	public float size;

	public bool isMoving;

	public float minScale;

	public float maxScale;

	public float minY;

	public float maxY;

	public float minX;

	public float maxX;

	public Vector2 characterOffset;
}
