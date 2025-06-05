using System;
using UnityEngine;

public class LLAndroidCallback : ScriptableObject
{
	private Delegate proxyCallback;

	private float time;

	public Delegate ProxyCallback
	{
		get
		{
			return proxyCallback;
		}
	}

	public void Setup(Delegate callback, float t)
	{
		proxyCallback = callback;
		time = t;
	}

	public void DynamicInvoke(params object[] args)
	{
		if ((object)ProxyCallback == null)
		{
			return;
		}
		if (time >= 0f)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				ProxyCallback.DynamicInvoke(args);
			}, time);
		}
		else
		{
			ProxyCallback.DynamicInvoke(args);
		}
	}
}
