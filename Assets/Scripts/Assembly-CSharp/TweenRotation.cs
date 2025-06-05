using UnityEngine;

[AddComponentMenu("Inventain/Tween/Rotation")]
public class TweenRotation : Tweener
{
	private Transform targetTransform;

	[SerializeField]
	private Vector3 endRotation = Vector3.one;

	[SerializeField]
	private Vector3 beginRotation = Vector3.zero;

	public Vector3 EndRotation
	{
		get
		{
			return endRotation;
		}
		set
		{
			endRotation = value;
		}
	}

	public Vector3 BeginRotation
	{
		get
		{
			return beginRotation;
		}
		set
		{
			beginRotation = value;
		}
	}

	public Transform TargetTransform
	{
		get
		{
			if (targetTransform == null)
			{
				targetTransform = base.transform;
			}
			return targetTransform;
		}
	}

	public Vector3 CurrentRotation
	{
		get
		{
			return TargetTransform.localRotation.eulerAngles;
		}
		set
		{
			TargetTransform.localRotation = Quaternion.Euler(value);
		}
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentRotation = BeginRotation + (EndRotation - BeginRotation) * factor;
	}

	protected override void TweenUpdateEditor(float factor)
	{
		CurrentRotation = BeginRotation + (EndRotation - BeginRotation) * factor;
	}

	public static TweenRotation SetRatation(GameObject go, Vector3 rotation, float duration = 1f)
	{
		TweenRotation tweenRotation = Tweener.InitGO<TweenRotation>(go);
		tweenRotation.BeginRotation = tweenRotation.CurrentRotation;
		tweenRotation.EndRotation = rotation;
		tweenRotation.duration = duration;
		tweenRotation.Play(true);
		return tweenRotation;
	}
}
