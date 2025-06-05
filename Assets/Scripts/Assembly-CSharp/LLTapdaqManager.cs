using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;

public sealed class LLTapdaqManager
{
	private delegate void LLTapdaqNativeDelegate();

	private delegate void LLTapdaqNativeDelegateStatus(bool result);

	private const string METHOD_INIT = "LLTapdaqInit";

	private const string METHOD_SET_CALLBACKS = "LLTapdaqSetCallbacks";

	private const string METHOD_IS_INTERSTITIAL_AVAILABLE = "LLTapdaqIsInterstitialAvailable";

	private const string METHOD_SHOW_INTERSTITIAL = "LLTapdaqShowInterstitial";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnTapdaqInterstitialShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTapdaqInterstitialFinish;

	public static bool IsInterstitialAvailable
	{
		get
		{
			bool flag = true;
			return LLAndroidJavaSingletone<LLTapdaqManager>.CallStatic<bool>("LLTapdaqIsInterstitialAvailable", new object[0]);
		}
	}

	public static event Action<bool> OnTapdaqInterstitialShow
	{
		add
		{
			Action<bool> action = LLTapdaqManager.m_OnTapdaqInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLTapdaqManager.m_OnTapdaqInterstitialShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLTapdaqManager.m_OnTapdaqInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLTapdaqManager.m_OnTapdaqInterstitialShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnTapdaqInterstitialFinish
	{
		add
		{
			Action action = LLTapdaqManager.m_OnTapdaqInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLTapdaqManager.m_OnTapdaqInterstitialFinish, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLTapdaqManager.m_OnTapdaqInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLTapdaqManager.m_OnTapdaqInterstitialFinish, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLTapdaqManager>.CallStatic("LLTapdaqSetCallbacks", LLAndroidJavaCallback.ProxyCallback(LLTapdaqManager.m_OnTapdaqInterstitialShow), LLAndroidJavaCallback.ProxyCallback(LLTapdaqManager.m_OnTapdaqInterstitialFinish));
		LLAndroidJavaSingletone<LLTapdaqManager>.CallStatic("LLTapdaqInit", LLTapdaqSettings.AppID(), LLTapdaqSettings.ClientKey(), LLTapdaqSettings.PlacementTag(), LLTapdaqSettings.IsPortrait(), LLTapdaqSettings.EnableTestMode());
	}

	public static void ShowInterstitial()
	{
		LLAndroidJavaSingletone<LLTapdaqManager>.CallStatic("LLTapdaqShowInterstitial");
	}

	[MonoPInvokeCallback(typeof(LLTapdaqNativeDelegateStatus))]
	private static void VideoDidShow(bool result)
	{
		LLTapdaqManager.m_OnTapdaqInterstitialShow?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLTapdaqNativeDelegate))]
	private static void VideoDidDismiss()
	{
		LLTapdaqManager.m_OnTapdaqInterstitialFinish?.Invoke();
	}
}
