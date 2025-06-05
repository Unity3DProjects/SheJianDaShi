using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class NativeEventsWrapper : SingletonMonoBehaviour<NativeEventsWrapper>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnApplicationDidReceiveMemoryWarning;

	[Button]
	public string Unload = "UnloadUnusedAssets";

	public static event Action OnApplicationDidReceiveMemoryWarning
	{
		add
		{
			Action action = NativeEventsWrapper.m_OnApplicationDidReceiveMemoryWarning;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref NativeEventsWrapper.m_OnApplicationDidReceiveMemoryWarning, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = NativeEventsWrapper.m_OnApplicationDidReceiveMemoryWarning;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref NativeEventsWrapper.m_OnApplicationDidReceiveMemoryWarning, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void ReceiveMemoryWarning(string message)
	{
		NativeEventsWrapper.m_OnApplicationDidReceiveMemoryWarning?.Invoke();
		UnloadUnusedAssets();
	}

	private void UnloadUnusedAssets()
	{
		Resources.UnloadUnusedAssets();
	}
}
