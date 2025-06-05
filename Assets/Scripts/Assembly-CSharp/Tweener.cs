using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tweener : IgnoreTimeScale, ITweener, ITweenerMini
{
	private const float TWEEN_FACTOR_EPSILON = 0.001f;

	public Method method;

	public Style style;

	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	public bool useCurve;

	public bool ignoreTimeScale = true;

	public float duration = 1f;

	public float scaleDuration = 1f;

	public bool steeperCurves = true;

	public InvokeData invokeWhenFinished = new InvokeData();

	public bool shouldSetBeginStateAtStart;

	[SerializeField]
	private string _animationCurveId = "default";

	[SerializeField]
	private bool notUsedForScreenAnimatingCondition;

	private float cachedDuration = -1f;

	private float cachedScaleDuration = -1f;

	private float amountPerDelta = 1f;

	private float factor;

	private TweenCallback OnFinished;

	private List<KeyValuePair<string, TimedCallback>> timedCallbacks = new List<KeyValuePair<string, TimedCallback>>();

	private Transform cachedTransform;

	private bool started;

	private float startTime;

	private float delay;

	private bool isReversed;

	private WaitAction WaitAndDoCallback;

	public float AmountPerDelta
	{
		get
		{
			if (cachedDuration != duration || cachedScaleDuration != scaleDuration)
			{
				cachedDuration = duration;
				cachedScaleDuration = scaleDuration;
				float num = duration * scaleDuration;
				amountPerDelta = ((!(num > 0f)) ? 10000f : (1f / num));
			}
			return amountPerDelta * (float)((!isReversed) ? 1 : (-1));
		}
	}

	public float Factor
	{
		get
		{
			return factor;
		}
	}

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

	public float Delay
	{
		set
		{
			delay = value;
			started = false;
		}
	}

	public virtual string TargetName
	{
		get
		{
			return base.gameObject.name;
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

	public bool IsReversed
	{
		get
		{
			return isReversed;
		}
		set
		{
			isReversed = value;
		}
	}

	public virtual bool IsBeginStateSet
	{
		get
		{
			return !base.enabled && factor < 0.001f;
		}
	}

	public virtual bool IsEndStateSet
	{
		get
		{
			return !base.enabled && factor > 0.999f;
		}
	}

	public bool IsRun
	{
		get
		{
			return base.enabled;
		}
	}

	protected virtual void Awake()
	{
		if (shouldSetBeginStateAtStart)
		{
			SetBeginStateImmediately();
		}
		base.enabled = false;
	}

	public virtual void Update()
	{
		float num = ((!ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
		float num2 = ((!ignoreTimeScale) ? Time.time : Time.unscaledTime);
		if (!started)
		{
			started = true;
			startTime = num2 + delay;
		}
		if (num2 < startTime)
		{
			return;
		}
		float num3 = factor;
		factor += AmountPerDelta * num;
		if (timedCallbacks.Count > 0)
		{
			for (int i = 0; i < timedCallbacks.Count; i++)
			{
				TimedCallback value = timedCallbacks[i].Value;
				if (value.callback != null && num3 < value.time && factor > value.time)
				{
					value.callback(this);
				}
			}
		}
		if (style == Style.Loop)
		{
			if (factor > 1f)
			{
				factor -= Mathf.Floor(factor);
			}
			else if (factor < 0f)
			{
				factor = 1f - factor;
			}
		}
		else if (style == Style.PingPong)
		{
			if (factor > 1f)
			{
				factor = 1f - (factor - Mathf.Floor(factor));
				amountPerDelta = 0f - AmountPerDelta;
			}
			else if (factor < 0f)
			{
				factor = 0f - factor;
				factor -= Mathf.Floor(factor);
				amountPerDelta = 0f - AmountPerDelta;
			}
		}
		if (style == Style.Once && (factor > 1f || factor < 0f))
		{
			factor = Mathf.Clamp01(factor);
			Sample(factor, true);
			base.enabled = false;
			if (OnFinished != null)
			{
				OnFinished(this);
			}
			invokeWhenFinished.Call();
		}
		else
		{
			Sample(factor, false);
		}
	}

	public void Sample(float value, bool isFinished)
	{
		value = Mathf.Clamp01(value);
		switch (method)
		{
		case Method.EaseIn:
			value = 1f - Mathf.Sin((float)Math.PI / 2f * (1f - value));
			if (steeperCurves)
			{
				value *= value;
			}
			break;
		case Method.EaseOut:
			value = Mathf.Sin((float)Math.PI / 2f * value);
			if (steeperCurves)
			{
				value = 1f - value;
				value = 1f - value * value;
			}
			break;
		case Method.EaseInOut:
			value -= Mathf.Sin(value * ((float)Math.PI * 2f)) / ((float)Math.PI * 2f);
			if (steeperCurves)
			{
				value = value * 2f - 1f;
				float num = ((!(value < 0f)) ? 1f : (-1f));
				value = 1f - num * value;
				value = 1f - value * value;
				value = num * value * 0.5f + 0.5f;
			}
			break;
		}
		if (Application.isPlaying)
		{
			TweenUpdateRuntime((!useCurve || animationCurve == null) ? value : animationCurve.Evaluate(value), isFinished);
		}
		else
		{
			TweenUpdateEditor((!useCurve || animationCurve == null) ? value : animationCurve.Evaluate(value));
		}
	}

	public void SwitchOnCurve()
	{
		useCurve = true;
	}

	public void SwitchOffCurve()
	{
		useCurve = false;
	}

	public void Play(bool forward)
	{
		IsReversed = !forward;
		base.enabled = true;
	}

	public void Reverse()
	{
		isReversed = !isReversed;
	}

	public void ResetScaleDuration()
	{
		scaleDuration = 1f;
	}

	public void ResetTween()
	{
		ResetTween(false);
	}

	protected void ResetTween(bool isFinished)
	{
		factor = ((!IsReversed) ? 0f : 1f);
		Sample(factor, isFinished);
	}

	public void SetBeginStateDefault()
	{
		SetBeginState();
	}

	public virtual void SetBeginState()
	{
		SetBeginState(0f);
	}

	public virtual void SetBeginState(float delay)
	{
		SetBeginState(delay, null);
	}

	public virtual void SetBeginState(float delay, TweenCallback del)
	{
		if (!base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			SetBeginStateImmediately();
			if (del != null)
			{
				del(this);
			}
		}
		else
		{
			Delay = delay;
			SetOnFinishedDelegate(del);
			Play(false);
		}
	}

	public void SetEndStateDefault()
	{
		SetEndState();
	}

	public virtual void SetEndState(float delay = 0f, TweenCallback del = null)
	{
		if (!base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			SetEndStateImmediately();
			if (del != null)
			{
				del(this);
			}
		}
		else
		{
			Delay = delay;
			SetOnFinishedDelegate(del);
			Play(true);
		}
	}

	public virtual void SetBeginStateImmediately()
	{
		factor = 0f;
		Sample(0f, true);
	}

	public virtual void SetEndStateImmediately()
	{
		factor = 1f;
		Sample(1f, true);
	}

	public void AddTimedCallback(string key, float time, TweenCallback call)
	{
		if (!string.IsNullOrEmpty(key))
		{
			AddTimedCallback(key, new TimedCallback(call, time));
		}
	}

	public void AddTimedCallback(string key, TimedCallback timedCallback)
	{
		if (string.IsNullOrEmpty(key) || timedCallback.callback == null)
		{
			return;
		}
		for (int i = 0; i < timedCallbacks.Count; i++)
		{
			if (timedCallbacks[i].Key.Equals(key))
			{
				return;
			}
		}
		timedCallbacks.Add(new KeyValuePair<string, TimedCallback>(key, timedCallback));
	}

	public void RemoveTimedCallback(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < timedCallbacks.Count; i++)
		{
			KeyValuePair<string, TimedCallback> item = timedCallbacks[i];
			if (item.Key.Equals(key))
			{
				timedCallbacks.Remove(item);
				break;
			}
		}
	}

	public void ClearAllTimedCallbacks()
	{
		timedCallbacks.Clear();
	}

	public void SetTimedCallback(float time, TweenCallback call)
	{
		if (call != null)
		{
			for (int i = 0; i < timedCallbacks.Count; i++)
			{
				if (timedCallbacks[i].Key.Equals("oldVersionDefaultKey"))
				{
					timedCallbacks.Add(new KeyValuePair<string, TimedCallback>("oldVersionDefaultKey", new TimedCallback(call, time)));
					break;
				}
			}
			return;
		}
		for (int j = 0; j < timedCallbacks.Count; j++)
		{
			KeyValuePair<string, TimedCallback> item = timedCallbacks[j];
			if (item.Key.Equals("oldVersionDefaultKey"))
			{
				timedCallbacks.Remove(item);
				break;
			}
		}
	}

	public void AddOnFinishedDelegate(TweenCallback del)
	{
		if (del != null)
		{
			OnFinished = (TweenCallback)Delegate.Combine(OnFinished, del);
		}
	}

	public void RemoveOnFinishedDelegate(TweenCallback del)
	{
		if (del != null)
		{
			OnFinished = (TweenCallback)Delegate.Remove(OnFinished, del);
		}
	}

	public void SetOnFinishedDelegate(TweenCallback del)
	{
		OnFinished = del;
	}

	public static T InitGO<T>(GameObject go) where T : Tweener
	{
		T val = go.GetComponent<T>() ?? go.AddComponent<T>();
		val.factor = 0f;
		return val;
	}

	public static T InitGO<T>(GameObject go, float duration) where T : Tweener
	{
		T val = go.GetComponent<T>() ?? go.AddComponent<T>();
		val.duration = duration;
		val.factor = 0f;
		return val;
	}

	protected abstract void TweenUpdateRuntime(float factor, bool isFinished);

	protected virtual void TweenUpdateEditor(float factor)
	{
		TweenUpdateRuntime(factor, false);
	}

	private IEnumerator WaitAndDo(float wait)
	{
		float start = Time.time;
		while (Time.time - start < wait)
		{
			yield return null;
		}
		if (WaitAndDoCallback != null)
		{
			WaitAndDoCallback();
		}
	}

	protected void RunWaitTimer(float waitTime, WaitAction todo)
	{
		WaitAndDoCallback = todo;
		StartCoroutine("WaitAndDo", waitTime);
	}

	protected void ResetWaitTimer()
	{
		WaitAndDoCallback = null;
		StopCoroutine("WaitAndDo");
	}
}
