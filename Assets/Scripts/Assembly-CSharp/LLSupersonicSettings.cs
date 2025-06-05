using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "LLSupersonicSettings")]
public class LLSupersonicSettings : ScriptableSingleton<LLSupersonicSettings>
{
	[StructLayout((LayoutKind)0, Size = 1)]
	private struct SupersonicAdapters
	{
		public const string ADAPTER_ADMOB = "AdMobAdapter";

		public const string ADAPTER_APPLOVIN = "AppLovinAdapter";

		public const string ADAPTER_CHARTBOOST = "ChartboostAdapter";

		public const string ADAPTER_FACEBOOK = "FacebookAdapter";

		public const string ADAPTER_INMOBI = "InMobiAdapter";

		public const string ADAPTER_TAPJOY = "TapjoyAdapter";

		public const string ADAPTER_UNITYADS = "UnityAdsAdapter";

		public const string ADAPTER_VUNGLE = "VungleAdapter";
	}

	[Header("Key")]
	public string appKey = string.Empty;

	[Header("Validate Integration")]
	public bool checkValidate;

	[Header("Exclude frameworks from project")]
	[Enum(typeof(SupersonicAdapters))]
	public string[] excludeFrameworks;

	[Header("Modules to be loaded")]
	[EnumFlags]
	public SupersonicModule modules = SupersonicModule.RewardedVideo | SupersonicModule.Interstitial;

	[Header("Banner Settings")]
	public SupersonicBannerSettings bannerSettings;

	[Header("Android")]
	public AndroidManifestElement[] androidManifestElements;

	public static string AppKey()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.appKey;
	}

	public static string[] ExcludeFrameworks()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.excludeFrameworks;
	}

	public static bool ValidateIntegration()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.checkValidate;
	}

	public static SupersonicModule Modules()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.modules;
	}

	public static SupersonicBannerSettings BannerSettings()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.bannerSettings;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		return ScriptableSingleton<LLSupersonicSettings>.Instance.androidManifestElements;
	}
}
