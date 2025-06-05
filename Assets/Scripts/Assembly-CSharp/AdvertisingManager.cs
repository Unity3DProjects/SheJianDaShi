using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using MiniJSON;
using UnityEngine;
using UnityEngine.Analytics;

public class AdvertisingManager : SingletonMonoBehaviour<AdvertisingManager>
{
	private enum AdType
	{
		SuperSonic = 0,
		AppLovin = 1
	}

	private const string AD_PRIORITY_LINK = "https://s3.eu-central-1.amazonaws.com/game-data-ratio/archer.txt";

	private const string APPSFLYER_SUPERSONIC_AD_EVENT = "af_supersonic_show";

	private const string APPSFLYER_APPLOVIN_AD_EVENT = "af_applovin_show";

	private const string TENJIN_SUPERSONIC_AD_EVENT = "tj_supersonic_show";

	private const string TENJIN_APPLOVIN_AD_EVENT = "tj_applovin_show";

	private const string APPLOVIN_SHOW_COUNT = "applovin_show_count";

	private const string SUPERSONIC_SHOW_COUNT = "supersonic_show_count";

	private const string VIDEO_AD_TYPE = "video";

	private const string INTERSTITIAL_AD_TYPE = "interstitial";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoAdFinish;

	private bool isVideoShowing;

	private bool isInterstitialShowing;

	private Action<bool> callbackVideo;

	private List<AdType> adPriority;

	private Managers cachedManagers;

	private bool IsVideoAvailable
	{
		get
		{
			return SingletonMonoBehaviour<MCAdsManager>.Instance.IsVideoAvailable;
		}
	}

	public bool IsInterstitialAvailable
	{
		get
		{
			return SingletonMonoBehaviour<MCAdsManager>.Instance.IsInterstitialAvailable;
		}
	}

	public bool IsAdsShowing
	{
		get
		{
			return isInterstitialShowing || isVideoShowing;
		}
	}

	private List<AdType> AdPriorityList
	{
		get
		{
			string text = CustomPlayerPrefs.GetString("ad_priority_list", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				return Json.Deserialize<List<AdType>>(text);
			}
			return null;
		}
		set
		{
			CustomPlayerPrefs.SetString("ad_priority_list", Json.Serialize(value));
		}
	}

	private List<int> AdPriorityWeights
	{
		get
		{
			string text = CustomPlayerPrefs.GetString("ad_priority_weights", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				return Json.Deserialize<List<int>>(text);
			}
			return null;
		}
		set
		{
			CustomPlayerPrefs.SetString("ad_priority_weights", Json.Serialize(value));
		}
	}

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public int ApplovinShowCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("applovin_show_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("applovin_show_count", value);
			SendLoadApplovinEvents(value);
		}
	}

	public int SupersonicShowCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("supersonic_show_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("supersonic_show_count", value);
			SendLoadSupersonicEvents(value);
		}
	}

	public static event Action<bool> OnVideoAdFinish
	{
		add
		{
			Action<bool> action = AdvertisingManager.m_OnVideoAdFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AdvertisingManager.m_OnVideoAdFinish, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = AdvertisingManager.m_OnVideoAdFinish;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AdvertisingManager.m_OnVideoAdFinish, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		adPriority = new List<AdType> { AdType.SuperSonic };
	}

	private void Start()
	{
		SingletonMonoBehaviour<MCAdsManager>.Instance.Initialize();
	}

	private void OnEnable()
	{
		MCAdsManager.OnVideoFinish += OnVideoFinish;
		MCAdsManager.OnVideoShow += OnVideoShow;
		MCAdsManager.OnInterstitialShow += OnInterstitialShow;
		MCAdsManager.OnInterstitialFinish += OnInterstitialFinish;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
	}

	private void OnDisable()
	{
		MCAdsManager.OnVideoFinish += OnVideoFinish;
		MCAdsManager.OnVideoShow += OnVideoShow;
		MCAdsManager.OnInterstitialShow += OnInterstitialShow;
		MCAdsManager.OnInterstitialFinish += OnInterstitialFinish;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
	}

	public void ShowVideo(Action<bool> callback = null)
	{
		SingletonMonoBehaviour<MCAdsManager>.Instance.ShowVideo(callback);
	}

	public void RequestInterstitial(MCAdsManager.InterstitialLocation location = MCAdsManager.InterstitialLocation.GAMEPLAY)
	{
		if (!IsInterstitialAvailable)
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.RequestInterstitial(location);
		}
	}

	public void ShowInterstitial(MCAdsManager.InterstitialLocation location = MCAdsManager.InterstitialLocation.GAMEPLAY)
	{
		if (IsInterstitialAvailable)
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.ShowInterstitial();
		}
		else
		{
			SingletonMonoBehaviour<MCAdsManager>.Instance.RequestInterstitial(location);
		}
	}

	private void FillUserAdPriority(List<int> receivedAdWeights = null)
	{
		List<int> list = AdPriorityWeights;
		List<AdType> adPriorityList = AdPriorityList;
		int num = Enum.GetNames(typeof(AdType)).Length;
		bool flag = false;
		if (adPriorityList == null)
		{
			flag = true;
		}
		else if (list == null)
		{
			flag = true;
		}
		else if (receivedAdWeights != null)
		{
			if (receivedAdWeights.Count != list.Count)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != receivedAdWeights[i])
					{
						flag = true;
						break;
					}
				}
			}
		}
		if (flag)
		{
			if (receivedAdWeights == null)
			{
				if (list == null)
				{
					list = new List<int>();
					if (num > 0)
					{
						list.Add(1);
					}
				}
			}
			else
			{
				list = receivedAdWeights;
			}
			AdPriorityWeights = list;
			adPriorityList = new List<AdType>();
			int num2 = 0;
			int num3 = 0;
			int num4 = Mathf.Min(list.Count, num);
			for (int j = 0; j < num4; j++)
			{
				num2 += list[j];
			}
			if (num2 > 0)
			{
				List<float> list2 = new List<float>(num4);
				for (int k = 0; k < num4; k++)
				{
					num3 += list[k];
					list2.Add((float)num3 / (float)num2);
				}
				list2[num4 - 1] = 1f;
				float value = UnityEngine.Random.value;
				for (int l = 0; l < list2.Count; l++)
				{
					if (value <= list2[l])
					{
						adPriorityList.Add((AdType)l);
						break;
					}
				}
				List<AdType> list3 = new List<AdType>();
				for (int m = 0; m < num4; m++)
				{
					AdType adType = (AdType)m;
					if (adType != adPriorityList[0])
					{
						list3.Add(adType);
					}
				}
				int count = list3.Count;
				for (int n = 0; n < count - 1; n++)
				{
					int index = UnityEngine.Random.Range(n, count);
					AdType value2 = list3[n];
					list3[n] = list3[index];
					list3[index] = value2;
				}
				for (int num5 = 0; num5 < count; num5++)
				{
					adPriorityList.Add(list3[num5]);
				}
			}
			AdPriorityList = adPriorityList;
			adPriority = adPriorityList;
		}
		else
		{
			adPriority = AdPriorityList;
		}
	}

	private bool IsInterstitialAvailableForAd(AdType currentAdType)
	{
		bool result = false;
		if (currentAdType == AdType.SuperSonic)
		{
			result = LLSupersonicManager.IsInterstitialAvailable;
		}
		return result;
	}

	private bool IsVideoAvailableForAd(AdType currentAdType)
	{
		bool result = false;
		if (currentAdType == AdType.SuperSonic)
		{
			result = LLSupersonicManager.IsVideoAvailable;
		}
		return result;
	}

	private void ShowInterstitialForAd(AdType currentAdType)
	{
		isInterstitialShowing = true;
		if (currentAdType == AdType.SuperSonic)
		{
			LLSupersonicManager.ShowInterstitial();
		}
	}

	private void ShowVideoForAd(AdType currentAdType)
	{
		isVideoShowing = true;
		if (currentAdType == AdType.SuperSonic)
		{
			LLSupersonicManager.ShowVideo();
		}
	}

	private IEnumerator LoadAdPriorityFromServer()
	{
		WWW downloader = new WWW("https://s3.eu-central-1.amazonaws.com/game-data-ratio/archer.txt");
		yield return downloader;
		if (downloader.error != null)
		{
			CustomDebug.Log("Fail to download ad priority with error " + downloader.error);
			yield break;
		}
		string[] array = CSVReader.SplitCsvGridByRows(downloader.text);
		List<int> list = new List<int>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(int.Parse(array[i]));
		}
		FillUserAdPriority(list);
	}

	private void SendLoadSupersonicEvents(int loadCount)
	{
		string text = string.Empty;
		switch (loadCount)
		{
		case 10:
			text = "ironsource_show_10_times";
			break;
		case 20:
			text = "ironsource_show_20_times";
			break;
		case 30:
			text = "ironsource_show_30_times";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			LLFacebookManager.LogEvent(text);
			LLTenjinManager.LogEvent(text, string.Empty);
		}
	}

	private void SendLoadApplovinEvents(int loadCount)
	{
		string text = string.Empty;
		switch (loadCount)
		{
		case 10:
			text = "applovin_show_10_times";
			break;
		case 20:
			text = "applovin_show_20_times";
			break;
		case 30:
			text = "applovin_show_30_times";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			LLFacebookManager.LogEvent(text);
			LLTenjinManager.LogEvent(text, string.Empty);
		}
	}

	private void OnSupersonicVideoShow(bool result)
	{
		OnVideoShow(result);
	}

	private void OnAppLovinVideoShow(bool result)
	{
		OnVideoShow(result);
	}

	private void OnSupersonicInterstitialShow(bool result)
	{
		OnInterstitialShow(result);
	}

	private void OnAppLovinInterstitialShow(bool result)
	{
		OnInterstitialShow(result);
	}

	private void OnVideoShow(bool result)
	{
		if (!result)
		{
			CachedManagers.CachedAudioManager.UnMuteSounds();
			CachedManagers.CachedGameManager.IsGamePaused = false;
			if (AdvertisingManager.m_OnVideoAdFinish != null)
			{
				AdvertisingManager.m_OnVideoAdFinish(false);
			}
			if (callbackVideo != null)
			{
				callbackVideo(false);
			}
		}
		isVideoShowing = result;
	}

	private void OnAppLovinVideoFinish()
	{
		OnVideoFinish(true);
	}

	private void OnVideoFinish(bool result)
	{
		CachedManagers.CachedAudioManager.UnMuteSounds();
		CachedManagers.CachedGameManager.IsGamePaused = false;
		if (AdvertisingManager.m_OnVideoAdFinish != null)
		{
			AdvertisingManager.m_OnVideoAdFinish(result);
		}
		if (callbackVideo != null)
		{
			callbackVideo(result);
		}
		isVideoShowing = false;
		if (result)
		{
			StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
			Analytics.CustomEvent("Videos_Watch_Successfully_Finished", new Dictionary<string, object>
			{
				{ "WatchedVideosCount", cachedStatisticsManager.VideosWatched },
				{ "MatchesCount", cachedStatisticsManager.MathesCount },
				{
					"GameTimeMinutes",
					cachedStatisticsManager.TotalGameTime.Minutes
				}
			});
			CachedManagers.CachedStatisticsManager.SendEvent("video_closed");
		}
	}

	private void OnInterstitialShow(bool result)
	{
		if (result)
		{
			CachedManagers.CachedGameManager.IsGamePaused = true;
			CachedManagers.CachedAudioManager.MuteSounds();
			isInterstitialShowing = true;
			StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
			Analytics.CustomEvent("Interstitials_Showed", new Dictionary<string, object>
			{
				{ "MatchesCount", cachedStatisticsManager.MathesCount },
				{
					"GameTimeMinutes",
					cachedStatisticsManager.TotalGameTime.Minutes
				}
			});
			CachedManagers.CachedStatisticsManager.SendEvent("interstitial_show");
		}
		else
		{
			CachedManagers.CachedGameManager.IsGamePaused = false;
			CachedManagers.CachedAudioManager.UnMuteSounds();
		}
		isInterstitialShowing = result;
	}

	private void OnInterstitialFinish()
	{
		SingletonMonoBehaviour<AdvertisingManager>.Instance.RequestInterstitial();
		CachedManagers.CachedGameManager.IsGamePaused = false;
		CachedManagers.CachedAudioManager.UnMuteSounds();
		isInterstitialShowing = false;
	}

	private void SendAdEvents(string eventName)
	{
		LLFacebookManager.LogEvent(eventName);
		LLAppsflyerManager.LogEvent(eventName);
		LLTenjinManager.LogEvent(eventName, string.Empty);
		Analytics.CustomEvent(eventName);
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 100, 0f, 100f, 100f), "Inter"))
		{
			ShowInterstitial();
		}
		if (GUI.Button(new Rect(Screen.width - 100, 100f, 100f, 100f), "Video"))
		{
			ShowVideo();
		}
	}
}
