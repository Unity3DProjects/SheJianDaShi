using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using AOT;

public sealed class LLSharingManager
{
	private delegate void LLCallbackString(string activity);

	private delegate void LLCallbackBool(bool success);

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnShareComplete;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnShareDismiss;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSeriveOpened;

	private const string METHOD_INIT = "LLSharingManagerInit";

	private const string METHOD_SHARE = "LLSharingManagerShare";

	private const string METHOD_INSTAGRAM = "LLSharingOpenInstagramLink";

	public static event Action<int> OnShareComplete
	{
		add
		{
			Action<int> action = LLSharingManager.m_OnShareComplete;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnShareComplete, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = LLSharingManager.m_OnShareComplete;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnShareComplete, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<int> OnShareDismiss
	{
		add
		{
			Action<int> action = LLSharingManager.m_OnShareDismiss;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnShareDismiss, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = LLSharingManager.m_OnShareDismiss;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnShareDismiss, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSeriveOpened
	{
		add
		{
			Action<bool> action = LLSharingManager.m_OnSeriveOpened;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnSeriveOpened, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSharingManager.m_OnSeriveOpened;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSharingManager.m_OnSeriveOpened, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLSharingManager>.CallStatic("LLSharingManagerInit");
	}

	public static bool AvaliableService(string scheme)
	{
		bool result = false;
		if (scheme.Contains("instagram"))
		{
			return true;
		}
		return result;
	}

	public static bool OpenService(string scheme)
	{
		bool result = false;
		if (scheme.Contains("instagram"))
		{
			OpenInstagramLink(scheme);
		}
		return result;
	}

	public static void ShareService(string activity, string text, string link)
	{
	}

	public static void ShareAll(string text, string link)
	{
		byte[] bytes = Encoding.BigEndianUnicode.GetBytes(text);
		LLAndroidJavaSingletone<LLSharingManager>.CallStatic("LLSharingManagerShare", LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnShareComplete), LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnSeriveOpened), LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnShareDismiss), bytes, link);
	}

	public static void ShareAll(string text, string link, byte[] image)
	{
		LLAndroidJavaSingletone<LLSharingManager>.CallStatic("LLSharingManagerShare", LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnShareComplete), LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnSeriveOpened), LLAndroidJavaCallback.ProxyCallback(LLSharingManager.OnShareDismiss), text, link, image);
	}

	public static void OpenInstagramLink(string link)
	{
		LLAndroidJavaSingletone<LLSharingManager>.CallStatic("LLSharingOpenInstagramLink", link);
	}

	[MonoPInvokeCallback(typeof(LLCallbackString))]
	private static void ShareServiceComplete(string activity)
	{
		if (LLSharingManager.OnShareComplete != null)
		{
			LLSharingManager.OnShareComplete(0);
		}
	}

	[MonoPInvokeCallback(typeof(LLCallbackString))]
	private static void ShareServiceDismiss(string activity)
	{
		if (LLSharingManager.OnShareDismiss != null)
		{
			LLSharingManager.OnShareDismiss(0);
		}
	}

	[MonoPInvokeCallback(typeof(LLCallbackBool))]
	private static void OpenService(bool success)
	{
		if (LLSharingManager.OnSeriveOpened != null)
		{
			LLSharingManager.OnSeriveOpened(success);
		}
	}
}
