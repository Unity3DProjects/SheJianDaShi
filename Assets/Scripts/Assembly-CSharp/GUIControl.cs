using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControl : MonoBehaviour
{
	[Serializable]
	public abstract class ShowableObject
	{
		public float showDelay;

		public float showDelayVariance;

		public float hideDelay;

		public float hideDelayVariance;
	}

	[Serializable]
	public class TweenData : ShowableObject
	{
		public Tweener tween;

		public float showDurationScale = 1f;

		public float hideDurationScale = 1f;
	}

	[Serializable]
	public class ControlData : ShowableObject
	{
		public GUIControl control;
	}

	public struct ActionWithInvokes
	{
		public Action<GUIControl> onFinished;

		public bool callInvokes;

		public ActionWithInvokes(Action<GUIControl> action, bool call)
		{
			onFinished = action;
			callInvokes = call;
		}
	}

	[HideInInspector]
	[SerializeField]
	private List<TweenData> tweensData = new List<TweenData>();

	[HideInInspector]
	[SerializeField]
	private List<ControlData> controlsData = new List<ControlData>();

	[SerializeField]
	private bool notUsedForScreenAnimatingCondition;

	[SerializeField]
	private bool isAnimatingIgnoreTimeScale;

	[SerializeField]
	private GameObject[] deactivateOnHideObjects;

	private Transform cachedTransform;

	public Transform CachedTransform
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

	public bool NotUsedForScreenAnimatingCondition
	{
		get
		{
			return notUsedForScreenAnimatingCondition;
		}
		set
		{
			notUsedForScreenAnimatingCondition = value;
		}
	}

	public List<TweenData> TweensData
	{
		get
		{
			return tweensData;
		}
	}

	public List<ControlData> ControlsData
	{
		get
		{
			return controlsData;
		}
	}

	public bool IsShown
	{
		get
		{
			for (int i = 0; i < tweensData.Count; i++)
			{
				TweenData tweenData = tweensData[i];
				if (tweenData.tween != null && !tweenData.tween.NotUsedForScreenAnimatingCondition && (!tweenData.tween.IsEndStateSet || tweenData.tween.enabled))
				{
					return false;
				}
			}
			for (int j = 0; j < controlsData.Count; j++)
			{
				ControlData controlData = controlsData[j];
				if (controlData.control != null && !controlData.control.NotUsedForScreenAnimatingCondition && !controlData.control.IsShown)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsHidden
	{
		get
		{
			int i = 0;
			for (int count = tweensData.Count; i < count; i++)
			{
				TweenData tweenData = tweensData[i];
				if (tweenData.tween != null && !tweenData.tween.NotUsedForScreenAnimatingCondition && (!tweenData.tween.IsBeginStateSet || tweenData.tween.enabled))
				{
					return false;
				}
			}
			int j = 0;
			for (int count2 = controlsData.Count; j < count2; j++)
			{
				ControlData controlData = controlsData[j];
				if (controlData.control != null && !controlData.control.NotUsedForScreenAnimatingCondition && !controlData.control.IsHidden)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsAnimating
	{
		get
		{
			return !IsHidden && !IsShown;
		}
	}

	public float ShowDuration
	{
		get
		{
			float num = 0f;
			int i = 0;
			for (int count = tweensData.Count; i < count; i++)
			{
				TweenData tweenData = tweensData[i];
				if (tweenData != null && (tweenData.showDelay + tweenData.tween.duration) * tweenData.showDurationScale > num)
				{
					num = tweenData.showDelay + tweenData.tween.duration * tweenData.showDurationScale;
				}
			}
			int j = 0;
			for (int count2 = controlsData.Count; j < count2; j++)
			{
				ControlData controlData = controlsData[j];
				if (controlData != null && controlData.showDelay + controlData.control.ShowDuration > num)
				{
					num = controlData.showDelay + controlData.control.ShowDuration;
				}
			}
			return num;
		}
	}

	public float HideDuration
	{
		get
		{
			float num = 0f;
			int i = 0;
			for (int count = tweensData.Count; i < count; i++)
			{
				TweenData tweenData = tweensData[i];
				if (tweenData != null && (tweenData.hideDelay + tweenData.tween.duration) * tweenData.showDurationScale > num)
				{
					num = tweenData.hideDelay + tweenData.tween.duration * tweenData.showDurationScale;
				}
			}
			int j = 0;
			for (int count2 = controlsData.Count; j < count2; j++)
			{
				ControlData controlData = controlsData[j];
				if (controlData != null && controlData.control != null && controlData.hideDelay + controlData.control.HideDuration > num)
				{
					num = controlData.hideDelay + controlData.control.HideDuration;
				}
			}
			return num;
		}
	}

	public void ActivateControl()
	{
		base.gameObject.SetActive(true);
		int i = 0;
		for (int count = tweensData.Count; i < count; i++)
		{
			TweenData tweenData = tweensData[i];
			if (tweenData != null)
			{
				tweenData.tween.gameObject.SetActive(true);
			}
		}
		int j = 0;
		for (int count2 = controlsData.Count; j < count2; j++)
		{
			ControlData controlData = controlsData[j];
			if (controlData != null)
			{
				controlData.control.ActivateControl();
			}
		}
	}

	public void DeactivateControl()
	{
		base.gameObject.SetActive(false);
		int i = 0;
		for (int count = tweensData.Count; i < count; i++)
		{
			TweenData tweenData = tweensData[i];
			if (tweenData != null)
			{
				tweenData.tween.gameObject.SetActive(false);
			}
		}
		int j = 0;
		for (int count2 = controlsData.Count; j < count2; j++)
		{
			ControlData controlData = controlsData[j];
			if (controlData != null)
			{
				controlData.control.DeactivateControl();
			}
		}
	}

	public virtual void Show()
	{
		Show(null);
	}

	public virtual void ShowImmediately()
	{
		Show(null, true);
	}

	public virtual void Show(Action<GUIControl> onFinished)
	{
		Show(onFinished, false);
	}

	public virtual void Show(Action<GUIControl> onFinished, bool immediately, bool callInvokes = true)
	{
		Show(onFinished, immediately, 0f, callInvokes);
	}

	public virtual void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.gameObject.SetActive(true);
		int i = 0;
		for (int num = deactivateOnHideObjects.Length; i < num; i++)
		{
			GameObject gameObject = deactivateOnHideObjects[i];
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		if (!base.gameObject.activeInHierarchy && !immediately)
		{
			Show(onFinished, true, delay, callInvokes);
			return;
		}
		ShowTweenData(immediately, delay);
		int j = 0;
		for (int count = controlsData.Count; j < count; j++)
		{
			ControlData controlData = controlsData[j];
			if (controlData != null)
			{
				controlData.control.Show(null, immediately, controlData.showDelay + UnityEngine.Random.Range((0f - controlData.showDelayVariance) * 0.5f, controlData.showDelayVariance * 0.5f) + delay, callInvokes);
			}
		}
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(StartAnimation(new ActionWithInvokes(onFinished, callInvokes)));
		}
	}

	public virtual void Hide()
	{
		Hide(null);
	}

	public virtual void HideImmediately()
	{
		Hide(null, true);
	}

	public virtual void Hide(Action<GUIControl> onFinished)
	{
		Hide(onFinished, false);
	}

	public virtual void Hide(Action<GUIControl> onFinished, bool immediately, bool callInvokes = true)
	{
		Hide(onFinished, immediately, 0f, callInvokes);
	}

	public virtual void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		if (!base.gameObject.activeInHierarchy && !immediately)
		{
			Hide(onFinished, true, delay, callInvokes);
			return;
		}
		HideControls(immediately, delay, callInvokes);
		if (immediately)
		{
			DeactivateObjectsToHide();
			if (onFinished != null)
			{
				onFinished(this);
			}
		}
		else if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(StartAnimation(new ActionWithInvokes(onFinished, callInvokes)));
		}
	}

	public virtual void HideControls()
	{
		HideControls(false, 0f, true);
	}

	public virtual void HideControls(bool immediately, float delay, bool callInvokes)
	{
		HideTweenData(immediately, delay);
		int i = 0;
		for (int count = controlsData.Count; i < count; i++)
		{
			ControlData controlData = controlsData[i];
			if (controlData != null)
			{
				if (controlData.control == null)
				{
					CustomDebug.LogError(base.gameObject.name + ": Child control on GUIControl component not set!");
				}
				else
				{
					controlData.control.Hide(null, immediately, controlData.hideDelay + UnityEngine.Random.Range((0f - controlData.showDelayVariance) * 0.5f, controlData.showDelayVariance * 0.5f) + delay, callInvokes);
				}
			}
		}
	}

	private void DeactivateObjectsToHide()
	{
		int i = 0;
		for (int num = deactivateOnHideObjects.Length; i < num; i++)
		{
			GameObject gameObject = deactivateOnHideObjects[i];
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	protected virtual IEnumerator StartAnimation(ActionWithInvokes actionWithInvokes)
	{
		while (IsAnimating)
		{
			if (!isAnimatingIgnoreTimeScale)
			{
				yield return new WaitForSeconds(0.1f);
			}
			else
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
		}
		if (actionWithInvokes.onFinished != null)
		{
			actionWithInvokes.onFinished(this);
		}
		if (IsHidden)
		{
			DeactivateObjectsToHide();
			if (CachedTransform.localScale.x < 0.01f || CachedTransform.localScale.y < 0.01f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	protected virtual IEnumerator WaitForRealTimeSeconds(float time)
	{
		float endTime = Time.realtimeSinceStartup + time;
		yield return new WaitWhile(() => Time.realtimeSinceStartup < endTime);
	}

	protected virtual void ShowTweenData(bool immediately, float delay)
	{
		int i = 0;
		for (int count = tweensData.Count; i < count; i++)
		{
			TweenData tweenData = tweensData[i];
			if (tweenData != null)
			{
				if (immediately)
				{
					tweenData.tween.SetEndStateImmediately();
					tweenData.tween.enabled = false;
				}
				else
				{
					tweenData.tween.scaleDuration = tweenData.showDurationScale;
					tweenData.tween.SetBeginStateImmediately();
					tweenData.tween.SetEndState(tweenData.showDelay + UnityEngine.Random.Range((0f - tweenData.showDelayVariance) * 0.5f, tweenData.showDelayVariance * 0.5f) + delay);
				}
			}
		}
	}

	protected virtual void HideTweenData(bool immediately, float delay)
	{
		int i = 0;
		for (int count = tweensData.Count; i < count; i++)
		{
			TweenData tweenData = tweensData[i];
			if (tweenData != null)
			{
				if (tweenData.tween == null)
				{
					CustomDebug.LogError(base.gameObject.name + ": Tween on GUIControl component not set!", this);
				}
				else if (immediately)
				{
					tweenData.tween.SetBeginStateImmediately();
					tweenData.tween.enabled = false;
				}
				else
				{
					tweenData.tween.scaleDuration = tweenData.hideDurationScale;
					tweenData.tween.SetBeginState(tweenData.hideDelay + UnityEngine.Random.Range((0f - tweenData.showDelayVariance) * 0.5f, tweenData.showDelayVariance * 0.5f) + delay);
				}
			}
		}
	}

	public virtual void BackButtonPressed()
	{
	}
}
