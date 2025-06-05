using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnSdkInitializedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, float> m_OnAdLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string> m_OnAdFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnAdClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnAdExpandedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnAdCollapsedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnInterstitialLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string> m_OnInterstitialFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnInterstitialDismissedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnInterstitialExpiredEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnInterstitialShownEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnInterstitialClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string> m_OnRewardedVideoFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoExpiredEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoShownEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string> m_OnRewardedVideoFailedToPlayEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string, float> m_OnRewardedVideoReceivedRewardEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoClosedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnRewardedVideoLeavingApplicationEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> m_OnConsentStatusChangedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnConsentDialogLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnConsentDialogFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnConsentDialogShownEvent;

	public static MoPubManager Instance { get; private set; }

	public static event Action<string> OnSdkInitializedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnSdkInitializedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnSdkInitializedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnSdkInitializedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnSdkInitializedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, float> OnAdLoadedEvent
	{
		add
		{
			Action<string, float> action = MoPubManager.m_OnAdLoadedEvent;
			Action<string, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdLoadedEvent, (Action<string, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, float> action = MoPubManager.m_OnAdLoadedEvent;
			Action<string, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdLoadedEvent, (Action<string, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string> OnAdFailedEvent
	{
		add
		{
			Action<string, string> action = MoPubManager.m_OnAdFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdFailedEvent, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = MoPubManager.m_OnAdFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdFailedEvent, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnAdClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnAdClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnAdClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnAdExpandedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnAdExpandedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdExpandedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnAdExpandedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdExpandedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnAdCollapsedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnAdCollapsedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdCollapsedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnAdCollapsedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnAdCollapsedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnInterstitialLoadedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnInterstitialLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialLoadedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnInterstitialLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialLoadedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string> OnInterstitialFailedEvent
	{
		add
		{
			Action<string, string> action = MoPubManager.m_OnInterstitialFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialFailedEvent, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = MoPubManager.m_OnInterstitialFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialFailedEvent, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnInterstitialDismissedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnInterstitialDismissedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialDismissedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnInterstitialDismissedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialDismissedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnInterstitialExpiredEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnInterstitialExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialExpiredEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnInterstitialExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialExpiredEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnInterstitialShownEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnInterstitialShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialShownEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnInterstitialShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialShownEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnInterstitialClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnInterstitialClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnInterstitialClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnInterstitialClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoLoadedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoLoadedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoLoadedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string> OnRewardedVideoFailedEvent
	{
		add
		{
			Action<string, string> action = MoPubManager.m_OnRewardedVideoFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoFailedEvent, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = MoPubManager.m_OnRewardedVideoFailedEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoFailedEvent, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoExpiredEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoExpiredEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoExpiredEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoShownEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoShownEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoShownEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string> OnRewardedVideoFailedToPlayEvent
	{
		add
		{
			Action<string, string> action = MoPubManager.m_OnRewardedVideoFailedToPlayEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoFailedToPlayEvent, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = MoPubManager.m_OnRewardedVideoFailedToPlayEvent;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoFailedToPlayEvent, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string, float> OnRewardedVideoReceivedRewardEvent
	{
		add
		{
			Action<string, string, float> action = MoPubManager.m_OnRewardedVideoReceivedRewardEvent;
			Action<string, string, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoReceivedRewardEvent, (Action<string, string, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string, float> action = MoPubManager.m_OnRewardedVideoReceivedRewardEvent;
			Action<string, string, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoReceivedRewardEvent, (Action<string, string, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoClosedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoClosedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoClosedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoClosedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoClosedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnRewardedVideoLeavingApplicationEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoLeavingApplicationEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoLeavingApplicationEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnRewardedVideoLeavingApplicationEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnRewardedVideoLeavingApplicationEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> OnConsentStatusChangedEvent
	{
		add
		{
			Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> action = MoPubManager.m_OnConsentStatusChangedEvent;
			Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentStatusChangedEvent, (Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> action = MoPubManager.m_OnConsentStatusChangedEvent;
			Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentStatusChangedEvent, (Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnConsentDialogLoadedEvent
	{
		add
		{
			Action action = MoPubManager.m_OnConsentDialogLoadedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogLoadedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MoPubManager.m_OnConsentDialogLoadedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogLoadedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnConsentDialogFailedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_OnConsentDialogFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_OnConsentDialogFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnConsentDialogShownEvent
	{
		add
		{
			Action action = MoPubManager.m_OnConsentDialogShownEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogShownEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MoPubManager.m_OnConsentDialogShownEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_OnConsentDialogShownEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private string[] DecodeArgs(string argsJson, int min)
	{
		bool flag = false;
		List<object> list = Json.Deserialize(argsJson) as List<object>;
		if (list == null)
		{
			Debug.LogError("Invalid JSON data: " + argsJson);
			list = new List<object>();
			flag = true;
		}
		if (list.Count < min)
		{
			if (!flag)
			{
				Debug.LogError("Missing one or more values: " + argsJson + " (expected " + min + ")");
			}
			while (list.Count < min)
			{
				list.Add(string.Empty);
			}
		}
		return list.Select((object v) => v.ToString()).ToArray();
	}

	public void EmitSdkInitializedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubBase.LogLevel logLevel = MoPubBase.LogLevel.MPLogLevelNone;
		if (array.Length > 1)
		{
			try
			{
				logLevel = (MoPubBase.LogLevel)Enum.Parse(typeof(MoPubBase.LogLevel), array[1]);
			}
			catch (ArgumentException)
			{
				Debug.LogWarning("Invalid LogLevel received: " + array[1]);
			}
		}
		else
		{
			Debug.LogWarning("No LogLevel received");
		}
		MoPubLog.Log("EmitSdkInitializedEvent", "SDK initialized and ready to display ads.  Log Level: {0}", logLevel);
		Action<string> onSdkInitializedEvent = MoPubManager.OnSdkInitializedEvent;
		if (onSdkInitializedEvent != null)
		{
			onSdkInitializedEvent(obj);
		}
	}

	public void EmitConsentStatusChangedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 3);
		MoPubBase.Consent.Status arg = MoPubBase.Consent.FromString(array[0]);
		MoPubBase.Consent.Status status = MoPubBase.Consent.FromString(array[1]);
		bool flag = array[2].ToLower() == "true";
		MoPubLog.Log("EmitConsentStatusChangedEvent", "Consent changed to {0} from {1}: PII can{2} be collected. Reason: {3}", status, flag);
		Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> onConsentStatusChangedEvent = MoPubManager.OnConsentStatusChangedEvent;
		if (onConsentStatusChangedEvent != null)
		{
			onConsentStatusChangedEvent(arg, status, flag);
		}
	}

	public void EmitConsentDialogLoadedEvent()
	{
		MoPubLog.Log("EmitConsentDialogLoadedEvent", "Consent dialog loaded");
		Action onConsentDialogLoadedEvent = MoPubManager.OnConsentDialogLoadedEvent;
		if (onConsentDialogLoadedEvent != null)
		{
			onConsentDialogLoadedEvent();
		}
	}

	public void EmitConsentDialogFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string text = array[0];
		MoPubLog.Log("EmitConsentDialogFailedEvent", "Consent dialog failed: ({0}) {1}", text);
		Action<string> onConsentDialogFailedEvent = MoPubManager.OnConsentDialogFailedEvent;
		if (onConsentDialogFailedEvent != null)
		{
			onConsentDialogFailedEvent(text);
		}
	}

	public void EmitConsentDialogShownEvent()
	{
		MoPubLog.Log("EmitConsentDialogShownEvent", "Sucessfully showed consent dialog");
		Action onConsentDialogShownEvent = MoPubManager.OnConsentDialogShownEvent;
		if (onConsentDialogShownEvent != null)
		{
			onConsentDialogShownEvent();
		}
	}

	public void EmitAdLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string s = array[1];
		MoPubLog.Log("EmitAdLoadedEvent", "Ad loaded");
		MoPubLog.Log("EmitAdLoadedEvent", "Ad shown");
		Action<string, float> onAdLoadedEvent = MoPubManager.OnAdLoadedEvent;
		if (onAdLoadedEvent != null)
		{
			onAdLoadedEvent(arg, float.Parse(s));
		}
	}

	public void EmitAdFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string text = array[1];
		MoPubLog.Log("EmitAdFailedEvent", "Ad failed to load: ({0}) {1}", text);
		Action<string, string> onAdFailedEvent = MoPubManager.OnAdFailedEvent;
		if (onAdFailedEvent != null)
		{
			onAdFailedEvent(arg, text);
		}
	}

	public void EmitAdClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitAdClickedEvent", "Ad tapped");
		Action<string> onAdClickedEvent = MoPubManager.OnAdClickedEvent;
		if (onAdClickedEvent != null)
		{
			onAdClickedEvent(obj);
		}
	}

	public void EmitAdExpandedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitAdExpandedEvent", "Ad expanded");
		Action<string> onAdExpandedEvent = MoPubManager.OnAdExpandedEvent;
		if (onAdExpandedEvent != null)
		{
			onAdExpandedEvent(obj);
		}
	}

	public void EmitAdCollapsedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitAdCollapsedEvent", "Ad collapsed");
		Action<string> onAdCollapsedEvent = MoPubManager.OnAdCollapsedEvent;
		if (onAdCollapsedEvent != null)
		{
			onAdCollapsedEvent(obj);
		}
	}

	public void EmitInterstitialLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitInterstitialLoadedEvent", "Ad loaded");
		Action<string> onInterstitialLoadedEvent = MoPubManager.OnInterstitialLoadedEvent;
		if (onInterstitialLoadedEvent != null)
		{
			onInterstitialLoadedEvent(obj);
		}
	}

	public void EmitInterstitialFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string text = array[1];
		MoPubLog.Log("EmitInterstitialFailedEvent", "Ad failed to load: ({0}) {1}", text);
		Action<string, string> onInterstitialFailedEvent = MoPubManager.OnInterstitialFailedEvent;
		if (onInterstitialFailedEvent != null)
		{
			onInterstitialFailedEvent(arg, text);
		}
	}

	public void EmitInterstitialDismissedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitInterstitialDismissedEvent", "Ad did disappear");
		Action<string> onInterstitialDismissedEvent = MoPubManager.OnInterstitialDismissedEvent;
		if (onInterstitialDismissedEvent != null)
		{
			onInterstitialDismissedEvent(obj);
		}
	}

	public void EmitInterstitialDidExpireEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitInterstitialDidExpireEvent", "Ad expired since it was not shown within {0} minutes of it being loaded");
		Action<string> onInterstitialExpiredEvent = MoPubManager.OnInterstitialExpiredEvent;
		if (onInterstitialExpiredEvent != null)
		{
			onInterstitialExpiredEvent(obj);
		}
	}

	public void EmitInterstitialShownEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitInterstitialShownEvent", "Ad shown");
		Action<string> onInterstitialShownEvent = MoPubManager.OnInterstitialShownEvent;
		if (onInterstitialShownEvent != null)
		{
			onInterstitialShownEvent(obj);
		}
	}

	public void EmitInterstitialClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitInterstitialClickedEvent", "Ad tapped");
		Action<string> onInterstitialClickedEvent = MoPubManager.OnInterstitialClickedEvent;
		if (onInterstitialClickedEvent != null)
		{
			onInterstitialClickedEvent(obj);
		}
	}

	public void EmitRewardedVideoLoadedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitRewardedVideoLoadedEvent", "Ad loaded");
		Action<string> onRewardedVideoLoadedEvent = MoPubManager.OnRewardedVideoLoadedEvent;
		if (onRewardedVideoLoadedEvent != null)
		{
			onRewardedVideoLoadedEvent(obj);
		}
	}

	public void EmitRewardedVideoFailedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string text = array[1];
		MoPubLog.Log("EmitRewardedVideoFailedEvent", "Ad failed to load: ({0}) {1}", text);
		Action<string, string> onRewardedVideoFailedEvent = MoPubManager.OnRewardedVideoFailedEvent;
		if (onRewardedVideoFailedEvent != null)
		{
			onRewardedVideoFailedEvent(arg, text);
		}
	}

	public void EmitRewardedVideoExpiredEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitRewardedVideoExpiredEvent", "Ad expired since it was not shown within {0} minutes of it being loaded");
		Action<string> onRewardedVideoExpiredEvent = MoPubManager.OnRewardedVideoExpiredEvent;
		if (onRewardedVideoExpiredEvent != null)
		{
			onRewardedVideoExpiredEvent(obj);
		}
	}

	public void EmitRewardedVideoShownEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitRewardedVideoShownEvent", "Ad shown");
		Action<string> onRewardedVideoShownEvent = MoPubManager.OnRewardedVideoShownEvent;
		if (onRewardedVideoShownEvent != null)
		{
			onRewardedVideoShownEvent(obj);
		}
	}

	public void EmitRewardedVideoClickedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitRewardedVideoClickedEvent", "Ad tapped");
		Action<string> onRewardedVideoClickedEvent = MoPubManager.OnRewardedVideoClickedEvent;
		if (onRewardedVideoClickedEvent != null)
		{
			onRewardedVideoClickedEvent(obj);
		}
	}

	public void EmitRewardedVideoFailedToPlayEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		Action<string, string> onRewardedVideoFailedToPlayEvent = MoPubManager.OnRewardedVideoFailedToPlayEvent;
		if (onRewardedVideoFailedToPlayEvent != null)
		{
			onRewardedVideoFailedToPlayEvent(arg, arg2);
		}
	}

	public void EmitRewardedVideoReceivedRewardEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 3);
		string arg = array[0];
		string arg2 = array[1];
		string s = array[2];
		MoPubLog.Log("EmitRewardedVideoReceivedRewardEvent", "Ad should reward user with {0} {1}");
		Action<string, string, float> onRewardedVideoReceivedRewardEvent = MoPubManager.OnRewardedVideoReceivedRewardEvent;
		if (onRewardedVideoReceivedRewardEvent != null)
		{
			onRewardedVideoReceivedRewardEvent(arg, arg2, float.Parse(s));
		}
	}

	public void EmitRewardedVideoClosedEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		MoPubLog.Log("EmitRewardedVideoClosedEvent", "Ad did disappear");
		Action<string> onRewardedVideoClosedEvent = MoPubManager.OnRewardedVideoClosedEvent;
		if (onRewardedVideoClosedEvent != null)
		{
			onRewardedVideoClosedEvent(obj);
		}
	}

	public void EmitRewardedVideoLeavingApplicationEvent(string argsJson)
	{
		string[] array = DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoLeavingApplicationEvent = MoPubManager.OnRewardedVideoLeavingApplicationEvent;
		if (onRewardedVideoLeavingApplicationEvent != null)
		{
			onRewardedVideoLeavingApplicationEvent(obj);
		}
	}
}
