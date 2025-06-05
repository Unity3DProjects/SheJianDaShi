using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

public class GUITimer
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<int> m_OnTimerChanged;

	private float timerInSeconds;

	private float saveTimerCounter;

	public bool IsTimerActive { get; set; }

	public float TimerInSeconds
	{
		get
		{
			return timerInSeconds;
		}
		set
		{
			if (timerInSeconds != value)
			{
				timerInSeconds = value;
			}
		}
	}

	public event Action<int> OnTimerChanged
	{
		add
		{
			Action<int> action = this.m_OnTimerChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTimerChanged, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = this.m_OnTimerChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTimerChanged, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void InitializeTimer(float defaultTimerStartValue, bool isTimerActive = false)
	{
		IsTimerActive = isTimerActive;
		TimerInSeconds = defaultTimerStartValue;
	}

	public void UpdateTimer(float deltaTime)
	{
		if (IsTimerActive && TimerInSeconds > 0f)
		{
			float num = TimerInSeconds;
			TimerInSeconds -= deltaTime;
			if (TimerInSeconds <= 0f)
			{
				num = 0f;
				TimerInSeconds = 0f;
				IsTimerActive = false;
			}
			if (this.OnTimerChanged != null && ((int)num != (int)TimerInSeconds || TimerInSeconds <= 0f))
			{
				this.OnTimerChanged((int)num);
			}
		}
	}

	public void ForceTimeChangeEvent()
	{
		if (this.OnTimerChanged != null)
		{
			this.OnTimerChanged((int)TimerInSeconds);
		}
	}
}
