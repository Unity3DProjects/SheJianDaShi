using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;
using UnityEngine;

public class LLICloudManager : MonoBehaviour
{
	private delegate void LLICloudNativeDelegateVoid();

	private delegate void LLICloudNativeDelegateString(string result);

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnProgressDownloaded;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnICloudLoginFailed;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnProgressDownloadFailed;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnProgressUploaded;

	public static bool IsLocalPlayerAuthenticated
	{
		get
		{
			return false;
		}
	}

	public static event Action<string> OnProgressDownloaded
	{
		add
		{
			Action<string> action = LLICloudManager.m_OnProgressDownloaded;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressDownloaded, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLICloudManager.m_OnProgressDownloaded;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressDownloaded, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnICloudLoginFailed
	{
		add
		{
			Action action = LLICloudManager.m_OnICloudLoginFailed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnICloudLoginFailed, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLICloudManager.m_OnICloudLoginFailed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnICloudLoginFailed, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnProgressDownloadFailed
	{
		add
		{
			Action action = LLICloudManager.m_OnProgressDownloadFailed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressDownloadFailed, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLICloudManager.m_OnProgressDownloadFailed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressDownloadFailed, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnProgressUploaded
	{
		add
		{
			Action action = LLICloudManager.m_OnProgressUploaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressUploaded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLICloudManager.m_OnProgressUploaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLICloudManager.m_OnProgressUploaded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
	}

	public static void DownloadData()
	{
	}

	public static void UploadData(string downloadedProgress)
	{
	}

	public static void OpenGameCenter()
	{
	}

	public static void OpenICloud()
	{
	}

	[MonoPInvokeCallback(typeof(LLICloudNativeDelegateString))]
	private static void ProgressDownloaded(string result)
	{
		if (LLICloudManager.OnProgressDownloaded != null)
		{
			LLICloudManager.OnProgressDownloaded(result);
		}
	}

	[MonoPInvokeCallback(typeof(LLICloudNativeDelegateVoid))]
	private static void LoginFailed()
	{
		if (LLICloudManager.OnICloudLoginFailed != null)
		{
			LLICloudManager.OnICloudLoginFailed();
		}
	}

	[MonoPInvokeCallback(typeof(LLICloudNativeDelegateVoid))]
	private static void ProgressDownloadFailed()
	{
		if (LLICloudManager.OnProgressDownloadFailed != null)
		{
			LLICloudManager.OnProgressDownloadFailed();
		}
	}

	[MonoPInvokeCallback(typeof(LLICloudNativeDelegateVoid))]
	private static void ProgressUploaded()
	{
		if (LLICloudManager.OnProgressUploaded != null)
		{
			LLICloudManager.OnProgressUploaded();
		}
	}
}
