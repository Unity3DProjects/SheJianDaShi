using UnityEngine;

public abstract class IgnoreTimeScale : MonoBehaviour
{
	private float realTime;

	private float lastRealTime;

	private float timeDelta;

	private float actual;

	private bool timeStarted;

	protected float RealTime
	{
		get
		{
			return realTime;
		}
	}

	protected virtual void OnEnable()
	{
		timeStarted = true;
		timeDelta = 0f;
		lastRealTime = Time.realtimeSinceStartup;
	}

	protected float UpdateRealDeltaTime()
	{
		realTime = Time.realtimeSinceStartup;
		if (timeStarted)
		{
			float num = realTime - lastRealTime;
			actual += ((!(num < 0f)) ? num : 0f);
			timeDelta = 1E-05f * (float)(int)(actual * 100000f);
			actual -= timeDelta;
			if (timeDelta > 1f)
			{
				timeDelta = 1f;
			}
		}
		else
		{
			timeStarted = true;
			timeDelta = 0f;
		}
		lastRealTime = realTime;
		return timeDelta;
	}
}
