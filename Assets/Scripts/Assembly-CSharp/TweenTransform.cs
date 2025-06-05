using UnityEngine;

[AddComponentMenu("Inventain/Tween/Transform")]
public class TweenTransform : Tweener
{
	public bool ignoreZ;

	public bool ignoreRotation;

	public Transform beginTransform;

	public Transform endTransform;

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		if (ignoreZ)
		{
			float z = base.CachedTransform.position.z;
			base.CachedTransform.position = beginTransform.position * (1f - factor) + endTransform.position * factor;
			base.CachedTransform.SetGlobalPositionZ(z);
		}
		else
		{
			base.CachedTransform.position = beginTransform.position * (1f - factor) + endTransform.position * factor;
		}
		if (ignoreRotation)
		{
			base.CachedTransform.rotation = beginTransform.rotation;
		}
		else
		{
			base.CachedTransform.rotation = Quaternion.Euler(beginTransform.rotation.eulerAngles * (1f - factor) + endTransform.rotation.eulerAngles * factor);
		}
	}

	public static TweenTransform SetTransform(GameObject go, Transform transform, float duration = 1f)
	{
		TweenTransform tweenTransform = Tweener.InitGO<TweenTransform>(go, duration);
		tweenTransform.beginTransform = tweenTransform.CachedTransform;
		tweenTransform.endTransform = transform;
		tweenTransform.Play(true);
		return tweenTransform;
	}
}
