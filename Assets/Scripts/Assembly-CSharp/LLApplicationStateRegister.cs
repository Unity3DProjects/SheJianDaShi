using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;
using UnityEngine;

public class LLApplicationStateRegister : MonoBehaviour
{
	private delegate void LLNativeDelegate(bool isBackgroundEntered);

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnApplicationEnteredBackground;

	public static event Action<bool> OnApplicationEnteredBackground
	{
		add
		{
			Action<bool> action = LLApplicationStateRegister.m_OnApplicationEnteredBackground;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLApplicationStateRegister.m_OnApplicationEnteredBackground, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLApplicationStateRegister.m_OnApplicationEnteredBackground;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLApplicationStateRegister.m_OnApplicationEnteredBackground, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
	}

	[MonoPInvokeCallback(typeof(LLNativeDelegate))]
	private static void ApplicationEnteredBackground(bool isBackgroundEntered)
	{
		LLApplicationStateRegister.m_OnApplicationEnteredBackground?.Invoke(isBackgroundEntered);
	}
}
