using System;

public sealed class LLActivity
{
	private const string METHOD_SET_DEBUG_ENABLE = "LLActivitySetDebugEnable";

	private const string METHOD_SET_CALLBACK_CHANGE_VISIBLE_STATE = "LLActivitySetCallbackChangeVisibleState";

	private static Action<bool> callbackChangeVisibleState;

	public static event Action<bool> OnChangeVisibleState
	{
		add
		{
			if (callbackChangeVisibleState == null)
			{
				LLAndroidJavaSingletone<LLActivity>.CallStatic("LLActivitySetCallbackChangeVisibleState", LLAndroidJavaCallback.ProxyCallback(InvokeOnChangeVisibleState));
			}
			callbackChangeVisibleState = (Action<bool>)Delegate.Combine(callbackChangeVisibleState, value);
		}
		remove
		{
			callbackChangeVisibleState = (Action<bool>)Delegate.Remove(callbackChangeVisibleState, value);
		}
	}

	public static void SetDebugEnable(bool value)
	{
		LLAndroidJavaSingletone<LLActivity>.CallStatic("LLActivitySetDebugEnable", value);
	}

	private static void InvokeOnChangeVisibleState(bool isVisible)
	{
		if (callbackChangeVisibleState != null)
		{
			callbackChangeVisibleState(isVisible);
		}
	}
}
