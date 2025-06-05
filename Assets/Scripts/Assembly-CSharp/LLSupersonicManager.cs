using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;
using UnityEngine;
using Debug = UnityEngine.Debug;

public sealed class LLSupersonicManager
{
	private delegate void LLSupersonicNativeDelegate();

	private delegate void LLSupersonicNativeDelegateStatus(bool result);

	private delegate void LLSupersonicNativeDelegateLoad(string errorText);

	private const string METHOD_INIT = "LLSupersonicInit";

	private const string METHOD_SET_INTERSTITIAL_CALLBACKS = "LLSupersonicSetInterstitialCallbacks";

	private const string METHOD_SET_VIDEO_CALLBACKS = "LLSupersonicSetVideoCallbacks";

	private const string METHOD_IS_VIDEO_AVAILABLE = "LLSupersonicIsVideoAvailable";

	private const string METHOD_IS_INTERSTITIAL_AVAILABLE = "LLSupersonicIsInterstitialAvailable";

	private const string METHOD_SHOW_VIDEO_AD = "LLSupersonicShowVideoAd";

	private const string METHOD_SHOW_INTERSTITIAL = "LLSupersonicShowInterstitial";

	private const string METHOD_FETCH_INTERSTITIAL = "LLSupersonicFetchInterstitial";

	private const string METHOD_GET_VERSION = "LLSupersonicGetVersion";

	private const string METHOD_VALIDATE_INTEGRATION = "LLSupersonicValidateIntegration";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSupersonicVideoInitialized;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSupersonicVideoShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSupersonicVideoFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSupersonicInterstitialInitialized;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSupersonicInterstitialShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnSupersonicInterstitialFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnSupersonicInterstitialClicked;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnSupersonicBannerDidLoad;

	public static bool IsVideoAvailable
	{
		get
		{
			bool flag = true;
			return LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic<bool>("LLSupersonicIsVideoAvailable", new object[0]);
		}
	}

	public static bool IsInterstitialAvailable
	{
		get
		{
			bool flag = true;
			flag = MCMoPubManager.Instance.IsInterstitialReady();
			return LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic<bool>("LLSupersonicIsInterstitialAvailable", new object[0]);
		}
	}

	public static event Action<bool> OnSupersonicVideoInitialized
	{
		add
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoInitialized;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoInitialized, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoInitialized;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoInitialized, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSupersonicVideoShow
	{
		add
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSupersonicVideoFinish
	{
		add
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoFinish, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicVideoFinish, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSupersonicInterstitialInitialized
	{
		add
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicInterstitialInitialized;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialInitialized, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicInterstitialInitialized;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialInitialized, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSupersonicInterstitialShow
	{
		add
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSupersonicManager.m_OnSupersonicInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnSupersonicInterstitialFinish
	{
		add
		{
			Action action = LLSupersonicManager.m_OnSupersonicInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialFinish, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLSupersonicManager.m_OnSupersonicInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialFinish, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnSupersonicInterstitialClicked
	{
		add
		{
			Action action = LLSupersonicManager.m_OnSupersonicInterstitialClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLSupersonicManager.m_OnSupersonicInterstitialClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicInterstitialClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnSupersonicBannerDidLoad
	{
		add
		{
			Action<string> action = LLSupersonicManager.m_OnSupersonicBannerDidLoad;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicBannerDidLoad, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLSupersonicManager.m_OnSupersonicBannerDidLoad;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSupersonicManager.m_OnSupersonicBannerDidLoad, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
	}

	public static void ShowVideo()
	{
		MCMoPubManager.Instance.ShowRewardedVideoAd();
		if (LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic<bool>("LLSupersonicIsVideoAvailable", new object[0]))
		{
			LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic("LLSupersonicShowVideoAd");
		}
	}

	public static void ShowInterstitial()
	{
		MCMoPubManager.Instance.ShowInterstitial();
		if (LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic<bool>("LLSupersonicIsInterstitialAvailable", new object[0]))
		{
			LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic("LLSupersonicShowInterstitial");
		}
	}

	public static void ShowBanner()
	{
	}

	public static void HideBanner()
	{
	}

	public static string GetVersion()
	{
		string empty = string.Empty;
		return LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic<string>("LLSupersonicGetVersion", new object[0]);
	}

	public static void RequestInterstitial()
	{
		LLAndroidJavaSingletone<LLSupersonicManager>.CallStatic("LLSupersonicFetchInterstitial");
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateStatus))]
	public static void InterstitialDidLoad(bool result)
	{
		Debug.Log("Interstitial loaded = " + result);
		LLSupersonicManager.m_OnSupersonicInterstitialInitialized?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateStatus))]
	public static void InterstitialDidPresent(bool result)
	{
		LLSupersonicManager.m_OnSupersonicInterstitialShow?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegate))]
	public static void InterstitialDidDismiss()
	{
		LLSupersonicManager.m_OnSupersonicInterstitialFinish?.Invoke();
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegate))]
	private static void InterstitialClicked()
	{
		LLSupersonicManager.m_OnSupersonicInterstitialClicked?.Invoke();
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateStatus))]
	public static void VideoDidLoad(bool result)
	{
		Debug.Log("Video loaded = " + result);
		LLSupersonicManager.m_OnSupersonicVideoInitialized?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateStatus))]
	public static void VideoDidDismiss(bool result)
	{
		Debug.Log("Video closed" + result);
		LLSupersonicManager.m_OnSupersonicVideoFinish?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateStatus))]
	public static void VideoDidPresent(bool result)
	{
		LLSupersonicManager.m_OnSupersonicVideoShow?.Invoke(result);
	}

	[MonoPInvokeCallback(typeof(LLSupersonicNativeDelegateLoad))]
	private static void BannerDidLoad(string errorText)
	{
		CustomDebug.LogFormat("BannerDidLoad error = {0}", errorText);
		LLSupersonicManager.m_OnSupersonicBannerDidLoad?.Invoke(errorText);
	}
}
