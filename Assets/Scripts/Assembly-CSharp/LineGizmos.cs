using UnityEngine;

public class LineGizmos : MonoBehaviour
{
	[SerializeField]
	private LineRenderer lineRenderer;

	private Transform cachedTransform;

	private tk2dCamera cachedUICamera;

	private Vector3 start;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	private tk2dCamera CachedUICamera
	{
		get
		{
			if (cachedUICamera == null)
			{
				cachedUICamera = SingletonMonoBehaviour<GUIScreenManager>.Instance.UICamera;
			}
			return cachedUICamera;
		}
	}

	public void StartLine(Vector3 startPoint)
	{
		Vector3 position = CachedUICamera.ScreenCamera.ScreenToWorldPoint(startPoint);
		position.z = CachedTransform.position.z;
		lineRenderer.SetPosition(0, position);
		lineRenderer.SetPosition(1, position);
		start = position;
	}

	public void FinishLine(Vector3 finishPoint)
	{
		Vector3 position = CachedUICamera.ScreenCamera.ScreenToWorldPoint(finishPoint);
		position.z = CachedTransform.position.z;
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, position);
	}
}
