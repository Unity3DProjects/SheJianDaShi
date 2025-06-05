using UnityEngine;

[AddComponentMenu("Inventain/Tween/Position")]
public class TweenPosition : Tweener
{
	public Vector3 beginPosition = Vector3.zero;

	public Vector3 endPosition = Vector3.one;

	public bool useAbsolutePosValues;

	public bool needRecalculatePositions;

	protected override void Awake()
	{
		if (!useAbsolutePosValues)
		{
			if (needRecalculatePositions)
			{
				beginPosition = SizeHelper.RecalculatePosition(beginPosition, SizeFactor.TwoFactor);
				endPosition = SizeHelper.RecalculatePosition(endPosition, SizeFactor.TwoFactor);
			}
			else if (tk2dSystem.IsRetina)
			{
				beginPosition = new Vector3(beginPosition.x * 2f, beginPosition.y * 2f, beginPosition.z);
				endPosition = new Vector3(endPosition.x * 2f, endPosition.y * 2f, endPosition.z);
			}
		}
		base.Awake();
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		base.CachedTransform.localPosition = beginPosition * (1f - factor) + endPosition * factor;
	}

	public static TweenPosition SetPosition(GameObject go, Vector3 position, float duration = 1f)
	{
		TweenPosition tweenPosition = Tweener.InitGO<TweenPosition>(go, duration);
		tweenPosition.beginPosition = tweenPosition.CachedTransform.localPosition;
		tweenPosition.endPosition = position;
		tweenPosition.Play(true);
		return tweenPosition;
	}
}
