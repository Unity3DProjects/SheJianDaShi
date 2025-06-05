using System;
using UnityEngine;

[Serializable]
public class ParallaxSettings
{
	public Transform targetTransform;

	[Range(0f, 1f)]
	public float targetMultiplier;
}
