using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;

public sealed class LLAppLovinManager
{
	private delegate void LLAppLovinNativeDelegate();

	private delegate void LLAppLovinNativeDelegateStatus(bool result);

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnAppLovinVideoShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAppLovinVideoFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAppLovinVideoClicked;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnAppLovinInterstitialShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAppLovinInterstitialFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAppLovinInterstitialClicked;

	private const string STRING_EMPTY = "";

	private const string METHOD_INIT = "LLApplovinInit";

	private const string METHOD_IS_VIDEO_READY = "LLApplovinIsVideoReady";

	private const string METHOD_IS_INTERSTITIAL_READY = "LLApplovinIsInterstitialReady";

	private const string METHOD_SHOW_INTERSTITIAL = "LLApplovinShowInterstitial";

	private const string METHOD_SHOW_VIDEO = "LLApplovinShowVideo";

	public static event Action<bool> OnAppLovinVideoShow
	{
		add
		{
			Action<bool> action = LLAppLovinManager.m_OnAppLovinVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLAppLovinManager.m_OnAppLovinVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAppLovinVideoFinish
	{
		add
		{
			Action action = LLAppLovinManager.m_OnAppLovinVideoFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoFinish, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLAppLovinManager.m_OnAppLovinVideoFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoFinish, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAppLovinVideoClicked
	{
		add
		{
			Action action = LLAppLovinManager.m_OnAppLovinVideoClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLAppLovinManager.m_OnAppLovinVideoClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinVideoClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnAppLovinInterstitialShow
	{
		add
		{
			Action<bool> action = LLAppLovinManager.m_OnAppLovinInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLAppLovinManager.m_OnAppLovinInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAppLovinInterstitialFinish
	{
		add
		{
			Action action = LLAppLovinManager.m_OnAppLovinInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialFinish, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLAppLovinManager.m_OnAppLovinInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialFinish, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAppLovinInterstitialClicked
	{
		add
		{
			Action action = LLAppLovinManager.m_OnAppLovinInterstitialClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLAppLovinManager.m_OnAppLovinInterstitialClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAppLovinManager.m_OnAppLovinInterstitialClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLAppLovinManager>.CallStatic("LLApplovinInit");
	}

	public static bool IsInterstitialAvailable(string zoneIdentifier = "")
	{
		bool flag = true;
		return LLAndroidJavaSingletone<LLAppLovinManager>.CallStatic<bool>("LLApplovinIsInterstitialReady", new object[0]);
	}

	public static bool IsVideoAvailable(string zoneIdentifier = "")
	{
		bool flag = true;
		return LLAndroidJavaSingletone<LLAppLovinManager>.CallStatic<bool>("LLApplovinIsVideoReady", new object[0]);
	}

	public static void ShowInterstitial(string zoneIdentifier = "")
	{
		if (IsInterstitialAvailable(zoneIdentifier))
		{
			LLAndroidJavaSingletone<LLAppLovinManager>.CallStatic("LLApplovinShowInterstitial", LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinInterstitialShow), LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinInterstitialClicked), LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinInterstitialFinish));
		}
	}

	public static void ShowVideo(string zoneIdentifier = "")
	{
		if (IsVideoAvailable(zoneIdentifier))
		{
			LLAndroidJavaSingletone<LLAppLovinManager>.CallStatic("LLApplovinShowVideo", LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinVideoShow), LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinVideoClicked), LLAndroidJavaCallback.ProxyCallback(LLAppLovinManager.OnAppLovinVideoFinish));
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegateStatus))]
	private static void InterstitialDidPresent(bool result)
	{
		if (result)
		{
			CustomDebug.Log("AppLovin interstitial opened");
		}
		if (LLAppLovinManager.OnAppLovinInterstitialShow != null)
		{
			LLAppLovinManager.OnAppLovinInterstitialShow(result);
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegate))]
	private static void InterstitialDidDismiss()
	{
		CustomDebug.Log("AppLovin interstitial closed");
		if (LLAppLovinManager.OnAppLovinInterstitialFinish != null)
		{
			LLAppLovinManager.OnAppLovinInterstitialFinish();
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegate))]
	private static void InterstitialClicked()
	{
		CustomDebug.Log("AppLovin interstitial clicked");
		if (LLAppLovinManager.OnAppLovinInterstitialClicked != null)
		{
			LLAppLovinManager.OnAppLovinInterstitialClicked();
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegateStatus))]
	private static void VideoDidPresent(bool result)
	{
		if (result)
		{
			CustomDebug.Log("AppLovin video opened");
		}
		if (LLAppLovinManager.OnAppLovinVideoShow != null)
		{
			LLAppLovinManager.OnAppLovinVideoShow(result);
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegate))]
	private static void VideoDidDismiss()
	{
		CustomDebug.Log("AppLovin video closed");
		if (LLAppLovinManager.OnAppLovinVideoFinish != null)
		{
			LLAppLovinManager.OnAppLovinVideoFinish();
		}
	}

	[MonoPInvokeCallback(typeof(LLAppLovinNativeDelegate))]
	private static void VideoClicked()
	{
		CustomDebug.Log("AppLovin video clicked");
		if (LLAppLovinManager.OnAppLovinVideoClicked != null)
		{
			LLAppLovinManager.OnAppLovinVideoClicked();
		}
	}
}
