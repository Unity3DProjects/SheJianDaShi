using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TweenCameraHeight : Tweener
{
	public float beginHeight;

	public float endHeight;

	private Camera targetCamera;

	protected override void Awake()
	{
		targetCamera = GetComponent<Camera>();
		base.Awake();
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		float num = beginHeight * (1f - factor) + endHeight * factor;
		targetCamera.rect = new Rect(0f, (1f - num) * 0.5f, 1f, num);
	}
}
