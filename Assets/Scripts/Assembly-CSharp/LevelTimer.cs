using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float> m_OnTimerChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTimeAdd;

	[SerializeField]
	private float time;

	public float Time
	{
		get
		{
			return time;
		}
	}

	public static event Action<float> OnTimerChanged
	{
		add
		{
			Action<float> action = LevelTimer.m_OnTimerChanged;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelTimer.m_OnTimerChanged, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float> action = LevelTimer.m_OnTimerChanged;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelTimer.m_OnTimerChanged, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnTimeAdd
	{
		add
		{
			Action action = LevelTimer.m_OnTimeAdd;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelTimer.m_OnTimeAdd, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LevelTimer.m_OnTimeAdd;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelTimer.m_OnTimeAdd, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		if (LevelTimer.OnTimerChanged != null)
		{
			LevelTimer.OnTimerChanged(time);
		}
	}

	public void AddTime(float addTime)
	{
		if (addTime > 0f)
		{
			time += addTime;
			if (LevelTimer.OnTimeAdd != null)
			{
				LevelTimer.OnTimeAdd();
			}
		}
	}

	public void UpdateTimer(float deltaTime, LevelBirdsHunt level)
	{
		if (time > 0f)
		{
			time -= deltaTime;
			if (time <= 0f)
			{
				time = 0f;
				level.TimeIsOver();
			}
			if (LevelTimer.OnTimerChanged != null)
			{
				LevelTimer.OnTimerChanged(time);
			}
		}
	}
}
