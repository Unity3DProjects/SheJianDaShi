using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class MCAdsManager : SingletonMonoBehaviour<MCAdsManager>
{
	public enum InterstitialLocation
	{
		GAMEPLAY = 0,
		ACHIEVEMENTS = 1,
		SETTINGS = 2
	}

	public enum BannerLocation
	{
		GAMEPLAY = 0
	}

	public static class InterstitialEventForwarder
	{
		public static void OnInterstitialFinish()
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.InternalOnInterstitialFinish();
		}
	}

	public static class RewardedVideoAdEventForwarder
	{
		public static void OnRewardedVideoAdFinish()
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.InternalOnVideoFinish(true);
		}

		public static void OnRewardedVideoAdFailed()
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.InternalOnVideoFinish(false);
		}
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoTryLoad;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnInterstitialShow;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnInterstitialFinish;

	private bool isVideoAvailable = true;

	private bool isInterstitialAvailable = true;

	private Action<bool> callbackVideo;

	public bool IsVideoAvailable
	{
		get
		{
			return isVideoAvailable && MCMoPubManager.Instance.IsVideoAdReady();
		}
	}

	public bool IsInterstitialAvailable
	{
		get
		{
			return isInterstitialAvailable && MCMoPubManager.Instance.IsInterstitialReady();
		}
	}

	public static event Action<bool> OnVideoTryLoad
	{
		add
		{
			Action<bool> action = MCAdsManager.m_OnVideoTryLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoTryLoad, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MCAdsManager.m_OnVideoTryLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoTryLoad, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnVideoShow
	{
		add
		{
			Action<bool> action = MCAdsManager.m_OnVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MCAdsManager.m_OnVideoShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnVideoFinish
	{
		add
		{
			Action<bool> action = MCAdsManager.m_OnVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoFinish, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MCAdsManager.m_OnVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnVideoFinish, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnInterstitialShow
	{
		add
		{
			Action<bool> action = MCAdsManager.m_OnInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnInterstitialShow, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MCAdsManager.m_OnInterstitialShow;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnInterstitialShow, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnInterstitialFinish
	{
		add
		{
			Action action = MCAdsManager.m_OnInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnInterstitialFinish, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MCAdsManager.m_OnInterstitialFinish;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCAdsManager.m_OnInterstitialFinish, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void Initialize()
	{
		MCMoPubManager.Instance.InitializeInterstitials();
		MCMoPubManager.Instance.InitializeVideoAds();
	}

	public void ShowVideo(Action<bool> callback = null)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.NoInternet, true, null, true);
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		if (!SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent)
		{
			callbackVideo = callback;
			ShowTargetedAdsPopup();
			return;
		}
		Action showVideoAction = delegate
		{
			if (IsVideoAvailable)
			{
				callbackVideo = callback;
				SingletonMonoBehaviour<AudioManager>.Instance.MuteSounds();
				SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = true;
				LLSupersonicManager.ShowVideo();
			}
			else
			{
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.NoVideo, true, null, true);
				}, 0f);
				if (callback != null)
				{
					callback(false);
				}
			}
		};
		if (IsVideoAvailable)
		{
			showVideoAction();
			return;
		}
		SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.LoadingVideos, true, null, true);
		MCMoPubManager.Instance.RequestRewardedVideoAd(delegate
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				SingletonMonoBehaviour<GUIPopupManager>.Instance.HidePopup(PopupType.LoadingVideos);
			}, 0f);
			showVideoAction();
		});
	}

	public void RequestInterstitial(InterstitialLocation location)
	{
		if (SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager.SpentBucks == 0 && SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager.TotalSeesionCount > 1)
		{
			MCMoPubManager.Instance.RequestInterstitial(location);
		}
	}

	public void ShowInterstitial()
	{
		if (IsInterstitialAvailable)
		{
			MCMoPubManager.Instance.ShowInterstitial();
		}
	}

	public void ShowTargetedAdsPopup()
	{
		GDPRPopup gDPRPopup = SingletonMonoBehaviour<GDPRManager>.Instance.ShowSettingsTargetedAdsPopup();
		gDPRPopup.OnAgreedClicked += GDPRPopup_OnAgreedClicked;
		gDPRPopup.OnRefusedClicked += GDPRPopup_OnRefusedClicked;
	}

	public void GDPRPopup_OnAgreedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.VIDEO_AD, GoliathIds.GDPRClickResult.GAVE_AD_PERMISSION);
		ShowVideo(callbackVideo);
	}

	public void GDPRPopup_OnRefusedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.VIDEO_AD, GoliathIds.GDPRClickResult.DENIED_AD_PERMISSION);
		if (callbackVideo != null)
		{
			callbackVideo(false);
		}
		InternalOnVideoTryLoad(false);
	}

	private void InternalOnVideoTryLoad(bool result)
	{
		MCAdsManager.m_OnVideoTryLoad?.Invoke(result);
	}

	private void InternalOnVideoShow(bool result)
	{
		if (!result)
		{
			MCAdsManager.m_OnVideoFinish?.Invoke(false);
			if (callbackVideo != null)
			{
				callbackVideo(false);
			}
		}
		else
		{
			isVideoAvailable = false;
		}
	}

	private void InternalOnVideoFinish(bool result)
	{
		MCAdsManager.m_OnVideoFinish?.Invoke(result);
		if (callbackVideo != null)
		{
			callbackVideo(result);
		}
		isVideoAvailable = true;
	}

	private void OnInternalInterstitialShow(bool result)
	{
		MCAdsManager.m_OnInterstitialShow?.Invoke(result);
	}

	private void InternalOnInterstitialFinish()
	{
		MCAdsManager.m_OnInterstitialFinish?.Invoke();
	}
}
