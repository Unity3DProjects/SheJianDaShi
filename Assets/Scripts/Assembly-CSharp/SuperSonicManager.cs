using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

public class SuperSonicManager : SingletonMonoBehaviour<SuperSonicManager>
{
	public const string VIDEO_FINISH = "finished";

	public const string YES_STRING = "1";

	public const string NO_STRING = "0";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoFinish;

	private bool isVideoAvailable = true;

	private bool isInterstitialAvailable = true;

	private Action<bool> callbackVideo;

	public bool IsVideoAvailable
	{
		get
		{
			if (!IsVideoSuccessfullyInit)
			{
				LLSupersonicManager.Initialize();
			}
			return isVideoAvailable && LLSupersonicManager.IsVideoAvailable;
		}
	}

	public bool IsInterstitialAvailable
	{
		get
		{
			if (!IsInterstitialSuccessfullyInit)
			{
				LLSupersonicManager.Initialize();
			}
			return isInterstitialAvailable && LLSupersonicManager.IsInterstitialAvailable;
		}
	}

	public bool IsInterstitialSuccessfullyInit { get; private set; }

	public bool IsVideoSuccessfullyInit { get; private set; }

	public static event Action<bool> OnVideoFinish
	{
		add
		{
			Action<bool> action = SuperSonicManager.m_OnVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperSonicManager.m_OnVideoFinish, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = SuperSonicManager.m_OnVideoFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperSonicManager.m_OnVideoFinish, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		LLSupersonicManager.Initialize();
	}

	private void OnEnable()
	{
		LLSupersonicManager.OnSupersonicVideoInitialized += LLSupersonicManager_OnSupersonicVideoInitialized;
		LLSupersonicManager.OnSupersonicVideoShow += LLSupersonicManager_OnSupersonicVideoShow;
		LLSupersonicManager.OnSupersonicVideoFinish += LLSupersonicManager_OnSupersonicVideoFinish;
		LLSupersonicManager.OnSupersonicInterstitialInitialized += LLSupersonicManager_OnSupersonicInterstitialInitialized;
		LLSupersonicManager.OnSupersonicInterstitialShow += LLSupersonicManager_OnSupersonicInterstitialShow;
		LLSupersonicManager.OnSupersonicInterstitialFinish += LLSupersonicManager_OnSupersonicInterstitialFinish;
	}

	private void OnDisable()
	{
		LLSupersonicManager.OnSupersonicVideoInitialized -= LLSupersonicManager_OnSupersonicVideoInitialized;
		LLSupersonicManager.OnSupersonicVideoShow -= LLSupersonicManager_OnSupersonicVideoShow;
		LLSupersonicManager.OnSupersonicVideoFinish -= LLSupersonicManager_OnSupersonicVideoFinish;
		LLSupersonicManager.OnSupersonicInterstitialInitialized -= LLSupersonicManager_OnSupersonicInterstitialInitialized;
		LLSupersonicManager.OnSupersonicInterstitialShow -= LLSupersonicManager_OnSupersonicInterstitialShow;
		LLSupersonicManager.OnSupersonicInterstitialFinish -= LLSupersonicManager_OnSupersonicInterstitialFinish;
	}

	public void ShowVideo(Action<bool> callback = null)
	{
		StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
		Analytics.CustomEvent("Videos_Watch_Started", new Dictionary<string, object>
		{
			{ "WatchedVideosCount", cachedStatisticsManager.VideosWatched },
			{ "MatchesCount", cachedStatisticsManager.MathesCount },
			{
				"GameTimeMinutes",
				cachedStatisticsManager.TotalGameTime.Minutes
			}
		});
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.NoInternet, true, null, true);
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		Action action = delegate
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
				SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.VideoNotAvailablePopup, true, null, true);
				if (callback != null)
				{
					callback(false);
				}
			}
		};
		if (IsVideoAvailable)
		{
			action();
		}
		else
		{
			SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.LoadingVideos, true, null, true);
		}
	}

	public void RequestInterstitial(MCAdsManager.InterstitialLocation location)
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
			LLSupersonicManager.ShowInterstitial();
		}
	}

	private void LLSupersonicManager_OnSupersonicVideoInitialized(bool result)
	{
		IsVideoSuccessfullyInit = result;
	}

	private void LLSupersonicManager_OnSupersonicInterstitialInitialized(bool result)
	{
		IsInterstitialSuccessfullyInit = result;
	}

	private void LLSupersonicManager_OnSupersonicVideoShow(bool result)
	{
		if (!result)
		{
			SingletonMonoBehaviour<AudioManager>.Instance.UnMuteSounds();
			SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = false;
			if (SuperSonicManager.OnVideoFinish != null)
			{
				SuperSonicManager.OnVideoFinish(false);
			}
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

	private void LLSupersonicManager_OnSupersonicVideoFinish(bool result)
	{
		SingletonMonoBehaviour<AudioManager>.Instance.UnMuteSounds();
		SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = false;
		if (SuperSonicManager.OnVideoFinish != null)
		{
			SuperSonicManager.OnVideoFinish(result);
		}
		if (callbackVideo != null)
		{
			callbackVideo(result);
		}
		isVideoAvailable = true;
		if (result)
		{
			StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
			Analytics.CustomEvent("Videos_Watch_Successfully_Finished", new Dictionary<string, object>
			{
				{ "WatchedVideosCount", cachedStatisticsManager.VideosWatched },
				{ "MatchesCount", cachedStatisticsManager.MathesCount },
				{
					"GameTimeMinutes",
					cachedStatisticsManager.TotalGameTime.Minutes
				}
			});
		}
	}

	private void LLSupersonicManager_OnSupersonicInterstitialShow(bool result)
	{
		if (result)
		{
			SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = true;
			SingletonMonoBehaviour<AudioManager>.Instance.MuteSounds();
			isInterstitialAvailable = false;
			StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
			Analytics.CustomEvent("Interstitials_Showed", new Dictionary<string, object>
			{
				{ "MatchesCount", cachedStatisticsManager.MathesCount },
				{
					"GameTimeMinutes",
					cachedStatisticsManager.TotalGameTime.Minutes
				}
			});
		}
		else
		{
			SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = false;
			SingletonMonoBehaviour<AudioManager>.Instance.UnMuteSounds();
		}
	}

	private void LLSupersonicManager_OnSupersonicInterstitialFinish()
	{
		SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = false;
		SingletonMonoBehaviour<AudioManager>.Instance.UnMuteSounds();
		isInterstitialAvailable = true;
	}
}
