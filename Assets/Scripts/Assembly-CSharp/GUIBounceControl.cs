using System;
using UnityEngine;

[RequireComponent(typeof(TweenScale))]
public class GUIBounceControl : GUIControl
{
	private TweenScale bounceTween;

	[SerializeField]
	private bool loopBounce;

	[SerializeField]
	private float startDelay;

	[SerializeField]
	private float middleDelay;

	[SerializeField]
	private bool randomMiddleDelay;

	[SerializeField]
	private float randomMiddleDelayPercent;

	[SerializeField]
	private float duration = 0.8f;

	[SerializeField]
	private float bounceSessionTime = -1f;

	[SerializeField]
	private float bounceSessionDelay = -1f;

	private float sessionTime;

	public bool IsActive { get; set; }

	public TweenScale BounceTween
	{
		get
		{
			if (bounceTween == null)
			{
				bounceTween = GetComponent<TweenScale>();
			}
			if (bounceTween == null)
			{
				bounceTween = base.gameObject.AddComponent<TweenScale>();
			}
			return bounceTween;
		}
	}

	private void OnEnable()
	{
		if (IsActive)
		{
			BounceTween.enabled = true;
		}
	}

	private void OnDisable()
	{
		BounceTween.enabled = false;
	}

	public void Play()
	{
		BounceTween.duration = duration;
		BounceTween.SetBeginStateImmediately();
		BounceTween.SetEndState(startDelay, BounceDone);
		sessionTime = 0f;
		IsActive = true;
	}

	public void SetLoop(bool looped = true)
	{
		loopBounce = looped;
	}

	public void Stop(bool immediately = false)
	{
		IsActive = false;
		if (immediately)
		{
			BounceTween.SetOnFinishedDelegate(null);
			BounceTween.SetBeginStateImmediately();
			BounceTween.enabled = false;
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
		Play();
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Hide(onFinished, immediately, delay, callInvokes);
		Stop(immediately);
	}

	private void Update()
	{
		if (IsActive)
		{
			sessionTime += Time.deltaTime;
		}
	}

	private void BounceDone(ITweener tw)
	{
		tw.SetOnFinishedDelegate(null);
		if (!IsActive)
		{
			return;
		}
		IsActive = loopBounce && base.gameObject.activeSelf && base.gameObject.activeInHierarchy;
		if (IsActive)
		{
			tw.SetBeginStateImmediately();
			float num = middleDelay + ((!randomMiddleDelay) ? 0f : (middleDelay * UnityEngine.Random.Range(0f - randomMiddleDelayPercent, randomMiddleDelayPercent) / 100f));
			if (bounceSessionTime > 0f && sessionTime > bounceSessionTime)
			{
				sessionTime = 0f - bounceSessionDelay;
				num += bounceSessionDelay;
			}
			tw.SetEndState(num, BounceDone);
		}
	}
}
