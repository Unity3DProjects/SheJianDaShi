using System;
using UnityEngine;

public class SchedulerTask
{
	public UnityEngine.Object Target;

	public Action Method;

	public float Interval;

	public float Elapsed;

	public bool Autoremove;

	public bool Paused;

	public SchedulerTask(UnityEngine.Object pTarget, Action pName, float pInterval)
		: this(pTarget, pName, pInterval, false)
	{
	}

	public SchedulerTask(UnityEngine.Object pTarget, Action pName, float pInterval, bool pAutoremove)
	{
		Target = pTarget;
		Method = pName;
		Interval = pInterval;
		Autoremove = pAutoremove;
		Elapsed = 0f;
		Paused = false;
	}

	public void Update(float dt)
	{
		if (!(Target != null) || Method == null || Paused)
		{
			return;
		}
		Elapsed += dt;
		if (Elapsed >= Interval)
		{
			Elapsed -= Interval;
			Method();
			if (Autoremove)
			{
				SingletonMonoBehaviour<Scheduler>.Instance.UnscheduleTask(this);
			}
		}
	}

	public float GetRemainingTime()
	{
		return Mathf.Clamp(Interval - Elapsed, 0f, Interval);
	}
}
