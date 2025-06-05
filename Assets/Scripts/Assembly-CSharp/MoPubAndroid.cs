using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubAndroid : MoPubBase
{
	public enum LocationAwareness
	{
		TRUNCATED = 0,
		DISABLED = 1,
		NORMAL = 2
	}

	public static class PartnerApi
	{
		public static Uri CurrentConsentPrivacyPolicyUrl
		{
			get
			{
				return MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", new object[1] { MoPubBase.ConsentLanguageCode }));
			}
		}

		public static Uri CurrentVendorListUrl
		{
			get
			{
				return MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentVendorListLink", new object[1] { MoPubBase.ConsentLanguageCode }));
			}
		}

		public static string CurrentConsentIabVendorListFormat
		{
			get
			{
				return PluginClass.CallStatic<string>("getCurrentVendorListIabFormat", new object[0]);
			}
		}

		public static string CurrentConsentPrivacyPolicyVersion
		{
			get
			{
				return PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion", new object[0]);
			}
		}

		public static string CurrentConsentVendorListVersion
		{
			get
			{
				return PluginClass.CallStatic<string>("getCurrentVendorListVersion", new object[0]);
			}
		}

		public static string PreviouslyConsentedIabVendorListFormat
		{
			get
			{
				return PluginClass.CallStatic<string>("getConsentedVendorListIabFormat", new object[0]);
			}
		}

		public static string PreviouslyConsentedPrivacyPolicyVersion
		{
			get
			{
				return PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion", new object[0]);
			}
		}

		public static string PreviouslyConsentedVendorListVersion
		{
			get
			{
				return PluginClass.CallStatic<string>("getConsentedVendorListVersion", new object[0]);
			}
		}

		public static void GrantConsent()
		{
			PluginClass.CallStatic("grantConsent");
		}

		public static void RevokeConsent()
		{
			PluginClass.CallStatic("revokeConsent");
		}
	}

	private static readonly AndroidJavaClass PluginClass;

	private static readonly Dictionary<string, MoPubAndroidBanner> BannerPluginsDict;

	private static readonly Dictionary<string, MoPubAndroidInterstitial> InterstitialPluginsDict;

	private static readonly Dictionary<string, MoPubAndroidRewardedVideo> RewardedVideoPluginsDict;

	public static bool IsSdkInitialized
	{
		get
		{
			return PluginClass.CallStatic<bool>("isSdkInitialized", new object[0]);
		}
	}

	public static bool AllowLegitimateInterest
	{
		get
		{
			return PluginClass.CallStatic<bool>("shouldAllowLegitimateInterest", new object[0]);
		}
		set
		{
			PluginClass.CallStatic("setAllowLegitimateInterest", value);
		}
	}

	public static LogLevel SdkLogLevel
	{
		get
		{
			return (LogLevel)PluginClass.CallStatic<int>("getLogLevel", new object[0]);
		}
		set
		{
			PluginClass.CallStatic<int>("setLogLevel", new object[1] { (int)value });
			MoPubBase.logLevel = value;
		}
	}

	public static bool CanCollectPersonalInfo
	{
		get
		{
			return PluginClass.CallStatic<bool>("canCollectPersonalInfo", new object[0]);
		}
	}

	public static Consent.Status CurrentConsentStatus
	{
		get
		{
			return Consent.FromString(PluginClass.CallStatic<string>("getPersonalInfoConsentState", new object[0]));
		}
	}

	public static bool ShouldShowConsentDialog
	{
		get
		{
			return PluginClass.CallStatic<bool>("shouldShowConsentDialog", new object[0]);
		}
	}

	public static bool IsConsentDialogReady
	{
		get
		{
			return PluginClass.CallStatic<bool>("isConsentDialogReady", new object[0]);
		}
	}

	[Obsolete("Use the property name IsConsentDialogReady instead.")]
	public static bool IsConsentDialogLoaded
	{
		get
		{
			return IsConsentDialogReady;
		}
	}

	public static bool? IsGdprApplicable
	{
		get
		{
			int num = PluginClass.CallStatic<int>("gdprApplies", new object[0]);
			return (num == 0) ? ((bool?)null) : ((num <= 0) ? new bool?(false) : new bool?(true));
		}
	}

	static MoPubAndroid()
	{
		PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");
		BannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();
		InterstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();
		RewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();
		MoPubBase.InitManager();
	}

	public static void InitializeSdk(string anyAdUnitId)
	{
		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
		InitializeSdk(new SdkConfiguration
		{
			AdUnitId = anyAdUnitId
		});
	}

	public static void InitializeSdk(SdkConfiguration sdkConfiguration)
	{
		MoPubBase.logLevel = sdkConfiguration.LogLevel;
		MoPubLog.Log("InitializeSdk", "SDK initialization started");
		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
		PluginClass.CallStatic("initializeSdk", sdkConfiguration.AdUnitId, sdkConfiguration.AdditionalNetworksString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.AllowLegitimateInterest, (int)sdkConfiguration.LogLevel, sdkConfiguration.NetworkConfigurationsJson, sdkConfiguration.MoPubRequestOptionsJson);
	}

	public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
	{
		foreach (string text in bannerAdUnitIds)
		{
			BannerPluginsDict[text] = new MoPubAndroidBanner(text);
		}
		Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
	}

	public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
	{
		foreach (string text in interstitialAdUnitIds)
		{
			InterstitialPluginsDict[text] = new MoPubAndroidInterstitial(text);
		}
		Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
	}

	public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			RewardedVideoPluginsDict[text] = new MoPubAndroidRewardedVideo(text);
		}
		Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
	}

	public static void EnableLocationSupport(bool shouldUseLocation)
	{
		PluginClass.CallStatic("setLocationAwareness", LocationAwareness.NORMAL.ToString());
	}

	public static void ReportApplicationOpen(string iTunesAppId = null)
	{
		PluginClass.CallStatic("reportApplicationOpen");
	}

	protected static string GetSdkName()
	{
		return "Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion", new object[0]);
	}

	public static void AddFacebookTestDeviceId(string hashedDeviceId)
	{
		PluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
	}

	public static void CreateBanner(string adUnitId, AdPosition position)
	{
		MoPubLog.Log("CreateBanner", "Attempting to load ad");
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.CreateBanner(position);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowBanner(string adUnitId, bool shouldShow)
	{
		MoPubLog.Log("ShowBanner", "Attempting to show ad");
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.ShowBanner(shouldShow);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
	{
		MoPubLog.Log("RefreshBanner", "Attempting to show ad");
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.RefreshBanner(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void SetAutorefresh(string adUnitId, bool enabled)
	{
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.SetAutorefresh(enabled);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void ForceRefresh(string adUnitId)
	{
		MoPubLog.Log("ForceRefresh", "Attempting to show ad");
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.ForceRefresh();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void DestroyBanner(string adUnitId)
	{
		MoPubAndroidBanner value;
		if (BannerPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.DestroyBanner();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
	{
		MoPubLog.Log("RequestInterstitialAd", "Attempting to load ad");
		MoPubAndroidInterstitial value;
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.RequestInterstitialAd(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowInterstitialAd(string adUnitId)
	{
		MoPubLog.Log("ShowInterstitialAd", "Attempting to show ad");
		MoPubAndroidInterstitial value;
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.ShowInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public bool IsInterstialReady(string adUnitId)
	{
		MoPubAndroidInterstitial value;
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			return value.IsInterstitialReady;
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public void DestroyInterstitialAd(string adUnitId)
	{
		MoPubAndroidInterstitial value;
		if (InterstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.DestroyInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestRewardedVideo(string adUnitId, List<LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		MoPubLog.Log("RequestRewardedVideo", "Attempting to load ad");
		MoPubAndroidRewardedVideo value;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowRewardedVideo(string adUnitId, string customData = null)
	{
		MoPubLog.Log("ShowRewardedVideo", "Attempting to show ad");
		MoPubAndroidRewardedVideo value;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.ShowRewardedVideo(customData);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static bool HasRewardedVideo(string adUnitId)
	{
		MoPubAndroidRewardedVideo value;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			return value.HasRewardedVideo();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public static List<Reward> GetAvailableRewards(string adUnitId)
	{
		MoPubAndroidRewardedVideo value;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			return value.GetAvailableRewards();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return null;
	}

	public static void SelectReward(string adUnitId, Reward selectedReward)
	{
		MoPubAndroidRewardedVideo value;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.SelectReward(selectedReward);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void LoadConsentDialog()
	{
		MoPubLog.Log("LoadConsentDialog", "Attempting to load consent dialog");
		PluginClass.CallStatic("loadConsentDialog");
	}

	public static void ShowConsentDialog()
	{
		MoPubLog.Log("ShowConsentDialog", "Consent dialog attempting to show");
		PluginClass.CallStatic("showConsentDialog");
	}

	public static void ForceGdprApplicable()
	{
		PluginClass.CallStatic("forceGdprApplies");
	}
}
