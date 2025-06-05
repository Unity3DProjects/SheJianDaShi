using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TweenCameraSize : Tweener
{
	public float beginSize;

	public float endSize;

	private Camera targetCamera;

	protected override void Awake()
	{
		targetCamera = GetComponent<Camera>();
		base.Awake();
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		targetCamera.orthographicSize = beginSize * (1f - factor) + endSize * factor;
	}
}
