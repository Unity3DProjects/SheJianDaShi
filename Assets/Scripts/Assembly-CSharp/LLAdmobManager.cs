using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;

public sealed class LLAdmobManager
{
	private delegate void LLNativeDelegate();

	private delegate void LLNativeDelegateStatus(bool result);

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnBannerClick;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnBannerLoad;

	private static BannerSize currentSizeType;

	private static BannerPosition currentPositionType = BannerPosition.Bottom;

	private const string METHOD_INIT = "LLAdmobInit";

	private const string METHOD_SET_TEST_DEVICES_IDS = "LLAdmobSetTestDevicesIDs";

	private const string METHOD_REGISTER_CALLBACKS = "LLAdmobRegisterCallbacks";

	private const string METHOD_IS_BANNER_AVALAIBLE = "LLAdmobIsBannerAvalaible";

	private const string METHOD_RESIZE = "LLAdmobResize";

	private const string METHOD_SHOW_BANNER = "LLAdmobShowBanner";

	private const string METHOD_HIDE_BANNER = "LLAdmobHideBanner";

	private const string METHOD_SET_POSITION = "LLAdmobSetPosition";

	public static BannerSize CurrentSizeType
	{
		get
		{
			return currentSizeType;
		}
	}

	public static BannerPosition CurrentPositionType
	{
		get
		{
			return currentPositionType;
		}
	}

	public static bool IsBannerAvailable
	{
		get
		{
			bool flag = true;
			return LLAndroidJavaSingletone<LLAdmobManager>.CallStatic<bool>("LLAdmobIsBannerAvalaible", new object[0]);
		}
	}

	public static event Action OnBannerClick
	{
		add
		{
			Action action = LLAdmobManager.m_OnBannerClick;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAdmobManager.m_OnBannerClick, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLAdmobManager.m_OnBannerClick;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAdmobManager.m_OnBannerClick, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnBannerLoad
	{
		add
		{
			Action<bool> action = LLAdmobManager.m_OnBannerLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAdmobManager.m_OnBannerLoad, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLAdmobManager.m_OnBannerLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLAdmobManager.m_OnBannerLoad, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobRegisterCallbacks", LLAndroidJavaCallback.ProxyCallback(LLAdmobManager.m_OnBannerClick), LLAndroidJavaCallback.ProxyCallback(LLAdmobManager.m_OnBannerLoad));
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobSetTestDevicesIDs", LLAdmobSettings.IsTestDevicesNeeded(), LLAdmobSettings.TestDevicesIDs());
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobInit", LLAdmobSettings.AppKey(), LLAdmobSettings.UnitID(), LLAdmobSettings.SizeType(), LLAdmobSettings.PositionType());
	}

	public static void Resize(BannerSize size)
	{
		Resize(size, currentPositionType);
	}

	public static void SetPositionType(BannerPosition position)
	{
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobSetPosition", (int)position);
	}

	public static void Resize(BannerSize size, BannerPosition position)
	{
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobResize", (int)size, (int)position);
	}

	public static void ShowBanner()
	{
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobShowBanner");
	}

	public static void HideBanner()
	{
		LLAndroidJavaSingletone<LLAdmobManager>.CallStatic("LLAdmobHideBanner");
	}

	[MonoPInvokeCallback(typeof(LLNativeDelegate))]
	private static void OnBannerClickCallback()
	{
		LLAdmobManager.m_OnBannerClick?.Invoke();
	}

	[MonoPInvokeCallback(typeof(LLNativeDelegateStatus))]
	private static void OnLoadAd(bool result)
	{
		LLAdmobManager.m_OnBannerLoad?.Invoke(result);
	}
}
