using System;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : SingletonMonoBehaviour<Scheduler>
{
	private List<SchedulerTask> mSchedulers = new List<SchedulerTask>();

	private List<SchedulerTask> addList = new List<SchedulerTask>();

	private List<SchedulerTask> removeList = new List<SchedulerTask>();

	private void Update()
	{
		if (addList.Count > 0)
		{
			mSchedulers.AddRange(addList);
			addList.Clear();
		}
		int count = removeList.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				SchedulerTask item = removeList[i];
				mSchedulers.Remove(item);
			}
			removeList.Clear();
		}
		int count2 = mSchedulers.Count;
		for (int j = 0; j < count2; j++)
		{
			SchedulerTask schedulerTask = mSchedulers[j];
			schedulerTask.Update(Time.deltaTime);
		}
	}

	public void ScheduleMethod(UnityEngine.Object target, Action selector, float pInterval)
	{
		bool flag = false;
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target && mScheduler.Method == selector)
			{
				CustomDebug.Log(string.Concat("Scheduler: update interval for mathod ", selector, " from: ", mScheduler.Interval, " to: ", pInterval));
				mScheduler.Interval = pInterval;
				mScheduler.Elapsed = 0f;
				flag = true;
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target && add.Method == selector)
			{
				CustomDebug.Log(string.Concat("Scheduler: update interval for mathod ", selector, " from: ", add.Interval, " to: ", pInterval));
				add.Interval = pInterval;
				add.Elapsed = 0f;
				flag = true;
			}
		}
		if (!flag)
		{
			SchedulerTask item = new SchedulerTask(target, selector, pInterval);
			addList.Add(item);
		}
	}

	public void ScheduleMethod(UnityEngine.Object target, Action selector)
	{
		ScheduleMethod(target, selector, 0f);
	}

	public SchedulerTask CallMethodWithDelay(UnityEngine.Object target, Action selector, float delay)
	{
		SchedulerTask schedulerTask = new SchedulerTask(target, selector, delay, true);
		addList.Add(schedulerTask);
		return schedulerTask;
	}

	public void UnscheduleAllMethodForTarget(UnityEngine.Object target)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target)
			{
				removeList.Add(mScheduler);
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target)
			{
				removeList.Add(add);
			}
		}
	}

	public void UnscheduleMethod(UnityEngine.Object target, Action selector)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target && mScheduler.Method == selector)
			{
				removeList.Add(mScheduler);
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target && add.Method == selector)
			{
				removeList.Add(add);
			}
		}
	}

	public void UnscheduleTask(SchedulerTask unTask)
	{
		removeList.Add(unTask);
	}

	public void PauseMethod(UnityEngine.Object target, Action selector)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target && mScheduler.Method == selector)
			{
				mScheduler.Paused = true;
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target && add.Method == selector)
			{
				add.Paused = true;
			}
		}
	}

	public void UnpauseMethod(UnityEngine.Object target, Action selector)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target && mScheduler.Method == selector)
			{
				mScheduler.Paused = false;
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target && add.Method == selector)
			{
				add.Paused = false;
			}
		}
	}

	public void PauseAllMethodForTarget(UnityEngine.Object target)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target)
			{
				mScheduler.Paused = true;
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target)
			{
				add.Paused = true;
			}
		}
	}

	public void UnpauseAllMethodForTarget(UnityEngine.Object target)
	{
		foreach (SchedulerTask mScheduler in mSchedulers)
		{
			if (mScheduler.Target == target)
			{
				mScheduler.Paused = false;
			}
		}
		foreach (SchedulerTask add in addList)
		{
			if (add.Target == target)
			{
				add.Paused = false;
			}
		}
	}
}
