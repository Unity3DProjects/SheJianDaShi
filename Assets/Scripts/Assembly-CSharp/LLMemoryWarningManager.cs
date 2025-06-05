using System;
using AOT;

public static class LLMemoryWarningManager
{
	private delegate void LLMemoryManagerDelegate();

	private static Action OnMemoryWarning;

	private static bool initialized;

	public static void RegisterMemoryWarning(Action callback)
	{
		Initialize();
		OnMemoryWarning = (Action)Delegate.Combine(OnMemoryWarning, callback);
	}

	public static void UnregisterMemoryWarning(Action callback)
	{
		Initialize();
		OnMemoryWarning = (Action)Delegate.Remove(OnMemoryWarning, callback);
	}

	private static void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
		}
	}

	[MonoPInvokeCallback(typeof(LLMemoryManagerDelegate))]
	private static void MemoryWarningCallback()
	{
		if (OnMemoryWarning != null)
		{
			OnMemoryWarning();
		}
	}
}
