using UnityEngine;

[AddComponentMenu("Inventain/Tween/Scale")]
public class TweenScale : Tweener
{
	private Transform targetTransform;

	[SerializeField]
	private Vector3 endScale = Vector3.one;

	[SerializeField]
	private Vector3 beginScale = Vector3.zero;

	public Vector3 EndScale
	{
		get
		{
			return endScale;
		}
		set
		{
			endScale = value;
		}
	}

	public Vector3 BeginScale
	{
		get
		{
			return beginScale;
		}
		set
		{
			beginScale = value;
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
		set
		{
			if (value != targetTransform)
			{
				targetTransform = value;
			}
		}
	}

	public Vector3 CurrentScale
	{
		get
		{
			return TargetTransform.localScale;
		}
		set
		{
			TargetTransform.localScale = value;
		}
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentScale = BeginScale + (EndScale - BeginScale) * factor;
	}

	protected override void TweenUpdateEditor(float factor)
	{
		CurrentScale = BeginScale + (EndScale - BeginScale) * factor;
	}

	public static TweenScale SetScale(GameObject go, Vector3 scale, float duration = 1f)
	{
		TweenScale tweenScale = Tweener.InitGO<TweenScale>(go);
		tweenScale.BeginScale = tweenScale.CurrentScale;
		tweenScale.EndScale = scale;
		tweenScale.duration = duration;
		tweenScale.Play(true);
		return tweenScale;
	}
}
