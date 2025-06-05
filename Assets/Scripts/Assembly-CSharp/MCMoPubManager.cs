using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mc.Ads;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class MCMoPubManager : MonoBehaviour
{
	private class MoPubManagerListener : ConfiguratorListener
	{
		private MCMoPubManager baseClass;

		public MoPubManagerListener(MCMoPubManager mcMoPubManager)
		{
			baseClass = mcMoPubManager;
		}

		public void OnMoPubInitialized()
		{
			MCMoPubManager.m_OnSdkInitializedEvent?.Invoke();
			baseClass.RequestInterstitial(MCAdsManager.InterstitialLocation.GAMEPLAY);
		}

		public void OnTapJoyInitialized(bool success)
		{
		}
	}

	private class InterstitialsAdListener : InterstitialsListener
	{
		public void OnInterstitialLoaded(InterstitialsPlacement placement, string providerName, bool success)
		{
			if (success)
			{
				UnityEngine.Debug.Log("### MoPubManager::onInterstitialLoadedEvent: " + providerName);
				GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.INTERTITIAL, GoliathIds.AdPhase.LOAD);
			}
			else
			{
				UnityEngine.Debug.Log("### MoPubManager::onInterstitialFailedLoadEvent: " + providerName);
			}
		}

		public void OnInterstitialShown(InterstitialsPlacement placement, string providerName, bool success)
		{
			if (success)
			{
				UnityEngine.Debug.Log("### MoPubManager::onInterstitialShownEvent: " + providerName);
			}
			else
			{
				UnityEngine.Debug.Log("### MoPubManager::onInterstitialFailedShownEvent: " + providerName);
			}
		}

		public void OnInterstitialDismissed(InterstitialsPlacement placement, string providerName)
		{
			UnityEngine.Debug.Log("### MoPubManager::onInterstitialDismissedEvent: " + providerName);
			MCAdsManager.InterstitialEventForwarder.OnInterstitialFinish();
		}

		public void OnInterstitialClicked(InterstitialsPlacement placement, string providerName)
		{
			UnityEngine.Debug.Log("### MoPubManager::onInterstitialClickedEvent: " + providerName);
			GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.INTERTITIAL, GoliathIds.AdPhase.CLICKED);
		}
	}

	private class RewardedVideoAdsListener : RewardedVideosListener
	{
		public void OnRewardedVideoLoaded(RewardedVideosPlacement placement, string providerName, bool success)
		{
			if (success)
			{
				UnityEngine.Debug.Log("Rewarded video loaded.");
				if (videoLoadedCallback != null)
				{
					videoLoadedCallback(true);
				}
			}
			else
			{
				if (videoLoadedCallback != null)
				{
					videoLoadedCallback(false);
				}
				UnityEngine.Debug.Log("Rewarded video failed to load.");
			}
		}

		public void OnRewardedVideoShown(RewardedVideosPlacement placement, string providerName, bool success)
		{
			if (success)
			{
				MCAdsManager.RewardedVideoAdEventForwarder.OnRewardedVideoAdFinish();
				UnityEngine.Debug.Log("Rewarded video shown.");
			}
			else
			{
				MCAdsManager.RewardedVideoAdEventForwarder.OnRewardedVideoAdFailed();
				UnityEngine.Debug.Log("Rewarded video failed to show.");
			}
			videoLoadedCallback = null;
		}

		public void OnRewardedVideoDismissed(RewardedVideosPlacement placement, string providerName)
		{
			UnityEngine.Debug.Log("Rewarded video dismissed.");
		}

		public void OnRewardedVideoRewarded(RewardedVideosPlacement placement, string providerName, string currency, int amount)
		{
			UnityEngine.Debug.LogFormat("Rewarded video rewarded {0} {1}.", amount, currency);
		}
	}

	protected static MCMoPubManager _instance;

	private bool isMoPubInitialized;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnSdkInitializedEvent;

	private InterstitialsPlacement interstitialPlacement;

	private RewardedVideosPlacement videoAdsPlacement;

	[SerializeField]
	private string _interstitialAdUnit;

	[SerializeField]
	private string _videoAdUnit;

	[SerializeField]
	private string _bannerAdUnit;

	private string _createdBannerAdUnit;

	private bool _bannerLoaded;

	private bool _bannerIsShowing;

	private static Action<bool> videoLoadedCallback;

	public static MCMoPubManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool IsMoPubInitialized
	{
		get
		{
			return isMoPubInitialized;
		}
	}

	public bool IsBannerVisible
	{
		get
		{
			return _bannerLoaded && _bannerIsShowing;
		}
	}

	public static event Action OnSdkInitializedEvent
	{
		add
		{
			Action action = MCMoPubManager.m_OnSdkInitializedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCMoPubManager.m_OnSdkInitializedEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MCMoPubManager.m_OnSdkInitializedEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MCMoPubManager.m_OnSdkInitializedEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool IsBannerCached()
	{
		return true;
	}

	private string getInterstitialAdUnit()
	{
		return _interstitialAdUnit;
	}

	private string getRewardedVideoAdUnit()
	{
		return _videoAdUnit;
	}

	private string getBannerAdUnit()
	{
		return _bannerAdUnit;
	}

	public void SetTargetedAdsConsent(bool consentWasGiven)
	{
		Configurator.SetUserConsentGDPR(consentWasGiven);
	}

	private void Start()
	{
		MoPubAndroid.ReportApplicationOpen();
		string[] bannerAdUnitIds = new string[1] { getBannerAdUnit() };
		MoPubAndroid.LoadBannerPluginsForAdUnits(bannerAdUnitIds);
		InitializeSdk();
	}

	private void Awake()
	{
		_instance = this;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void InitializeSdk()
	{
		Configurator.SetLoggingEnabled(true);
		Configurator.SetUserId(MiniclipUtils.GetAdvertisingId());
		new MoPubManagerListener(this);
		InterstitialsListener listener = new InterstitialsAdListener();
		Interstitials.SetListener(listener);
		RewardedVideosListener listener2 = new RewardedVideoAdsListener();
		RewardedVideos.SetListener(listener2);
		isMoPubInitialized = Configurator.SetupMoPub(getInterstitialAdUnit());
		if (isMoPubInitialized)
		{
			MCMoPubManager.m_OnSdkInitializedEvent?.Invoke();
		}
		else
		{
			CustomDebug.LogError("Couldn't setup Mopub correctly", this);
		}
	}

	public void InitializeInterstitials()
	{
		interstitialPlacement = Interstitials.CreateMoPubPlacement(getInterstitialAdUnit());
		LLSupersonicManager.InterstitialDidLoad(true);
	}

	public bool IsInterstitialReady()
	{
		if (interstitialPlacement != null && interstitialPlacement.IsReady())
		{
			return true;
		}
		return false;
	}

	public void RequestInterstitial(MCAdsManager.InterstitialLocation location)
	{
		if (!MiniclipUtils.isNetworkAvailable())
		{
			CustomDebug.LogError("Can't request an interstitial because there is no network connectivity", this);
		}
		else if (interstitialPlacement != null)
		{
			interstitialPlacement.Preload();
		}
	}

	public void ShowInterstitial()
	{
		if (interstitialPlacement.IsReady())
		{
			interstitialPlacement.Show();
		}
	}

	public void InitializeVideoAds()
	{
		videoAdsPlacement = RewardedVideos.CreteMoPubPlacement(getRewardedVideoAdUnit());
	}

	public bool IsVideoAdReady()
	{
		if (videoAdsPlacement != null && videoAdsPlacement.IsReady())
		{
			return true;
		}
		return false;
	}

	public void RequestRewardedVideoAd(Action<bool> callback = null)
	{
		videoLoadedCallback = callback;
		if (!MiniclipUtils.isNetworkAvailable())
		{
			CustomDebug.LogError("Can't request an interstitial because there is no network connectivity", this);
		}
		else if (videoAdsPlacement != null)
		{
			videoAdsPlacement.Preload();
		}
	}

	public void ShowRewardedVideoAd(Action<bool> callback = null)
	{
		videoLoadedCallback = callback;
		if (videoAdsPlacement.IsReady())
		{
			videoAdsPlacement.Show();
		}
	}

	public void CreateBanner()
	{
		if (!MiniclipUtils.isNetworkAvailable())
		{
			Log("Can't create banner because there is no network connectivity");
			return;
		}
		if (!string.IsNullOrEmpty(_createdBannerAdUnit))
		{
			Log("Banner was already created, destroying and recreating...");
			MoPubAndroid.DestroyBanner(_createdBannerAdUnit);
		}
		_createdBannerAdUnit = getBannerAdUnit();
		UnityEngine.Debug.Log("BannerAdUnit: " + _createdBannerAdUnit);
		if (string.IsNullOrEmpty(_createdBannerAdUnit))
		{
			Log("Banner ad unit is null!");
			return;
		}
		_bannerLoaded = false;
		_bannerIsShowing = true;
		MoPubAndroid.CreateBanner(_createdBannerAdUnit, MoPubBase.AdPosition.BottomCenter);
		GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.BANNER, GoliathIds.AdPhase.REQUEST);
		Log(string.Format("Banner successfully created - AdUnit[{0}], IsShowing[{1}]", _createdBannerAdUnit, _bannerIsShowing));
	}

	public void SetBannerVisibility(bool isVisible)
	{
		if (string.IsNullOrEmpty(_createdBannerAdUnit))
		{
			Log("No banner created, cannot set visibility to " + isVisible);
			return;
		}
		MoPubAndroid.ShowBanner(_createdBannerAdUnit, isVisible);
		_bannerIsShowing = isVisible;
		if (isVisible)
		{
			GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.BANNER, GoliathIds.AdPhase.SHOW);
		}
		Log(string.Format("Setting banner visibility - AdUnit[{0}], IsShowing[{1}]", _createdBannerAdUnit, _bannerIsShowing));
	}

	public void DestroyBanner()
	{
		if (string.IsNullOrEmpty(_createdBannerAdUnit))
		{
			Log("There is no banner created... Doing nothing.");
			return;
		}
		Log(string.Format("Destroying banner - AdUnit[{0}], IsShowing[{1}]]", _createdBannerAdUnit, _bannerIsShowing));
		MoPubAndroid.DestroyBanner(_createdBannerAdUnit);
		_createdBannerAdUnit = null;
		_bannerLoaded = (_bannerIsShowing = false);
	}

	private void onAdLoadedEvent(string banner, float height)
	{
		Log("Banner loaded with height = " + height);
		GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.BANNER, GoliathIds.AdPhase.LOAD);
		_bannerLoaded = true;
	}

	private void onAdFailedEvent(string errorMsg, string somethind)
	{
		Log("Banner failed to load with message = " + errorMsg);
		_bannerLoaded = false;
	}

	private void onAdClickedEvent(string adUnitId)
	{
		Log(string.Format("Banner with adunit '{0}' was clicked", adUnitId));
		GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.BANNER, GoliathIds.AdPhase.CLICKED);
	}

	private void Log(string message)
	{
	}
}
