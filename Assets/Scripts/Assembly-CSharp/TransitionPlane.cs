using UnityEngine;

public class TransitionPlane : MonoBehaviour
{
	[SerializeField]
	private TweenScale transitionTweener;

	public bool IsTransitionPlaneOnScreen
	{
		get
		{
			return transitionTweener.Factor >= 1f;
		}
	}

	public void ChangeTransitionScale(bool isTransitionAppear, TweenCallback onTransitionScaleFinish = null)
	{
		if ((IsTransitionPlaneOnScreen && !isTransitionAppear) || (isTransitionAppear && !IsTransitionPlaneOnScreen))
		{
			transitionTweener.transform.parent.gameObject.SetActive(true);
			transitionTweener.SetOnFinishedDelegate(onTransitionScaleFinish);
			if (!isTransitionAppear)
			{
				transitionTweener.AddOnFinishedDelegate(delegate
				{
					transitionTweener.transform.parent.gameObject.SetActive(false);
				});
			}
			transitionTweener.IsReversed = !isTransitionAppear;
			transitionTweener.ResetTween();
			transitionTweener.enabled = true;
		}
		else if (onTransitionScaleFinish != null)
		{
			onTransitionScaleFinish(null);
		}
	}
}
