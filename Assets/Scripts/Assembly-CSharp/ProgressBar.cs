using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ProgressBar : GUILayoutCell
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnProgressBarEnded;

	[SerializeField]
	private tk2dProgressBar progressBar;

	[SerializeField]
	private float fillTime;

	[SerializeField]
	private float minFill = 0.1f;

	[SerializeField]
	private float maxFill = 0.9f;

	private float fillBar = 1f;

	private GameScreen gamescreen;

	private float ActualFill
	{
		get
		{
			return (progressBar.fillAmount - minFill) / (maxFill - minFill);
		}
	}

	private float SpeedFill
	{
		get
		{
			float b = 0f;
			if (fillTime > 0f)
			{
				b = (maxFill - minFill) / fillTime;
			}
			return Mathf.Max(0.01f, b);
		}
	}

	public bool IsIgnoreTimeScale { get; set; }

	private float UsedTime
	{
		get
		{
			return (!IsIgnoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime;
		}
	}

	public static event Action OnProgressBarEnded
	{
		add
		{
			Action action = ProgressBar.m_OnProgressBarEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ProgressBar.m_OnProgressBarEnded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = ProgressBar.m_OnProgressBarEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ProgressBar.m_OnProgressBarEnded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void OnEnable()
	{
		gamescreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.GameScreen;
		if (gamescreen != null && gamescreen.IsBarInterrup)
		{
			StopAllCoroutines();
			StartCoroutine(FillCoroutine(fillBar, null));
		}
	}

	public void SetFillAmount(float f, bool immediately = false, Action onFinished = null)
	{
		f = Mathf.Clamp01(f);
		if (!Mathf.Approximately(f, ActualFill))
		{
			if (immediately)
			{
				UpdateFill(f);
				if (onFinished != null)
				{
					onFinished();
				}
			}
			else if (base.gameObject.activeInHierarchy)
			{
				StopAllCoroutines();
				StartCoroutine(FillCoroutine(f, onFinished));
			}
			else
			{
				UpdateFill(f);
				if (onFinished != null)
				{
					onFinished();
				}
			}
		}
		else if (onFinished != null)
		{
			onFinished();
		}
	}

	private void UpdateFill(float f, bool isIntermediateValue = false)
	{
		float fillAmount = progressBar.fillAmount;
		progressBar.fillAmount = minFill + (maxFill - minFill) * f;
		if (progressBar.fillAmount > fillAmount && isIntermediateValue)
		{
			AudioManager.PlayAudioWithKey("progress_bar_filling");
		}
	}

	private IEnumerator FillCoroutine(float f, Action onFinished)
	{
		if (gamescreen != null)
		{
			gamescreen.IsBarInterrup = true;
		}
		float currentTime = 0f;
		float needTime = Mathf.Abs((f - ActualFill) / SpeedFill);
		fillBar = f;
		yield return null;
		for (currentTime += UsedTime; currentTime < needTime; currentTime += UsedTime)
		{
			float newFill = ActualFill + Mathf.Sign(f - ActualFill) * SpeedFill * UsedTime;
			UpdateFill(newFill, true);
			yield return null;
		}
		if (gamescreen != null && currentTime > needTime)
		{
			gamescreen.IsBarInterrup = false;
			ProgressBar.m_OnProgressBarEnded?.Invoke();
		}
		UpdateFill(f);
		if (onFinished != null)
		{
			onFinished();
		}
	}
}
