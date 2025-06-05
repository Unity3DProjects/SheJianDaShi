using System;
using System.Collections.Generic;
using System.Linq;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubBase
{
	public enum AdPosition
	{
		TopLeft = 0,
		TopCenter = 1,
		TopRight = 2,
		Centered = 3,
		BottomLeft = 4,
		BottomCenter = 5,
		BottomRight = 6
	}

	public static class Consent
	{
		public enum Status
		{
			Unknown = 0,
			Denied = 1,
			DoNotTrack = 2,
			PotentialWhitelist = 3,
			Consented = 4
		}

		private static class Strings
		{
			public const string ExplicitYes = "explicit_yes";

			public const string ExplicitNo = "explicit_no";

			public const string Unknown = "unknown";

			public const string PotentialWhitelist = "potential_whitelist";

			public const string Dnt = "dnt";
		}

		public static Status FromString(string status)
		{
			switch (status)
			{
			case "explicit_yes":
				return Status.Consented;
			case "explicit_no":
				return Status.Denied;
			case "dnt":
				return Status.DoNotTrack;
			case "potential_whitelist":
				return Status.PotentialWhitelist;
			case "unknown":
				return Status.Unknown;
			default:
				try
				{
					return (Status)Enum.Parse(typeof(Status), status);
				}
				catch
				{
					Debug.LogError("Unknown consent status string: " + status);
					return Status.Unknown;
				}
			}
		}
	}

	public enum BannerType
	{
		Size320x50 = 0,
		Size300x250 = 1,
		Size728x90 = 2,
		Size160x600 = 3
	}

	public enum LogLevel
	{
		MPLogLevelDebug = 20,
		MPLogLevelInfo = 30,
		MPLogLevelNone = 70
	}

	public struct SdkConfiguration
	{
		public string AdUnitId;

		public MediatedNetwork[] MediatedNetworks;

		public bool AllowLegitimateInterest;

		private LogLevel _logLevel;

		public LogLevel LogLevel
		{
			get
			{
				return (_logLevel == (LogLevel)0) ? LogLevel.MPLogLevelNone : _logLevel;
			}
			set
			{
				_logLevel = value;
			}
		}

		public string AdditionalNetworksString
		{
			get
			{
				IEnumerable<string> source = from n in MediatedNetworks ?? Enumerable.Empty<MediatedNetwork>()
					where n != null && !(n is SupportedNetwork)
					where !string.IsNullOrEmpty(n.AdapterConfigurationClassName)
					select n.AdapterConfigurationClassName;
				return string.Join(",", source.ToArray());
			}
		}

		public string NetworkConfigurationsJson
		{
			get
			{
				IEnumerable<MediatedNetwork> source = from n in MediatedNetworks ?? Enumerable.Empty<MediatedNetwork>()
					where n.NetworkConfiguration != null
					where !string.IsNullOrEmpty(n.AdapterConfigurationClassName)
					select n;
				return Json.Serialize(source.ToDictionary((MediatedNetwork n) => n.AdapterConfigurationClassName, (MediatedNetwork n) => n.NetworkConfiguration));
			}
		}

		public string MediationSettingsJson
		{
			get
			{
				IEnumerable<MediatedNetwork> source = from n in MediatedNetworks ?? Enumerable.Empty<MediatedNetwork>()
					where n.MediationSettings != null
					where !string.IsNullOrEmpty(n.MediationSettingsClassName)
					select n;
				return Json.Serialize(source.ToDictionary((MediatedNetwork n) => n.MediationSettingsClassName, (MediatedNetwork n) => n.MediationSettings));
			}
		}

		public string MoPubRequestOptionsJson
		{
			get
			{
				IEnumerable<MediatedNetwork> source = from n in MediatedNetworks ?? Enumerable.Empty<MediatedNetwork>()
					where n.MoPubRequestOptions != null
					where !string.IsNullOrEmpty(n.AdapterConfigurationClassName)
					select n;
				return Json.Serialize(source.ToDictionary((MediatedNetwork n) => n.AdapterConfigurationClassName, (MediatedNetwork n) => n.MoPubRequestOptions));
			}
		}
	}

	public class LocalMediationSetting : Dictionary<string, object>
	{
		public class AdColony : LocalMediationSetting
		{
			public AdColony()
				: base("AdColony")
			{
			}
		}

		public class AdMob : LocalMediationSetting
		{
			public AdMob()
				: base("GooglePlayServices", "MPGoogle")
			{
			}
		}

		public class Chartboost : LocalMediationSetting
		{
			public Chartboost()
				: base("Chartboost")
			{
			}
		}

		public class Vungle : LocalMediationSetting
		{
			public Vungle()
				: base("Vungle")
			{
			}
		}

		public string MediationSettingsClassName { get; set; }

		public LocalMediationSetting()
		{
		}

		public LocalMediationSetting(string adVendor)
		{
			MediationSettingsClassName = adVendor;
		}

		public LocalMediationSetting(string android, string ios)
			: this(android)
		{
		}

		public static string ToJson(IEnumerable<LocalMediationSetting> localMediationSettings)
		{
			IEnumerable<LocalMediationSetting> source = (localMediationSettings ?? Enumerable.Empty<LocalMediationSetting>()).Where((LocalMediationSetting n) => n != null && !string.IsNullOrEmpty(n.MediationSettingsClassName));
			return Json.Serialize(source.ToDictionary((LocalMediationSetting n) => n.MediationSettingsClassName, (LocalMediationSetting n) => n));
		}
	}

	public class MediatedNetwork
	{
		public string AdapterConfigurationClassName { get; set; }

		public string MediationSettingsClassName { get; set; }

		public Dictionary<string, object> NetworkConfiguration { get; set; }

		public Dictionary<string, object> MediationSettings { get; set; }

		public Dictionary<string, object> MoPubRequestOptions { get; set; }
	}

	public class SupportedNetwork : MediatedNetwork
	{
		public class AdColony : SupportedNetwork
		{
			public AdColony()
				: base("AdColony")
			{
			}
		}

		public class AdMob : SupportedNetwork
		{
			public AdMob()
				: base("GooglePlayServices", "MPGoogle")
			{
			}
		}

		public class AppLovin : SupportedNetwork
		{
			public AppLovin()
				: base("AppLovin")
			{
			}
		}

		public class Chartboost : SupportedNetwork
		{
			public Chartboost()
				: base("Chartboost")
			{
			}
		}

		public class Facebook : SupportedNetwork
		{
			public Facebook()
				: base("Facebook")
			{
			}
		}

		public class IronSource : SupportedNetwork
		{
			public IronSource()
				: base("IronSource")
			{
			}
		}

		public class OnebyAOL : SupportedNetwork
		{
			public OnebyAOL()
				: base("Millennial", "MPMillennial")
			{
			}
		}

		public class Tapjoy : SupportedNetwork
		{
			public Tapjoy()
				: base("Tapjoy")
			{
			}
		}

		public class Unity : SupportedNetwork
		{
			public Unity()
				: base("Unity", "UnityAds")
			{
			}
		}

		public class Vungle : SupportedNetwork
		{
			public Vungle()
				: base("Vungle")
			{
			}
		}

		protected SupportedNetwork(string adVendor)
		{
			base.AdapterConfigurationClassName = "com.mopub.mobileads." + adVendor + "AdapterConfiguration";
			base.MediationSettingsClassName = adVendor;
		}

		protected SupportedNetwork(string android, string ios)
			: this(android)
		{
		}
	}

	public struct Reward
	{
		public string Label;

		public int Amount;

		public override string ToString()
		{
			return string.Format("\"{0} {1}\"", Amount, Label);
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Label) && Amount > 0;
		}
	}

	public const double LatLongSentinel = 99999.0;

	public static readonly string moPubSDKVersion = "5.5.0";

	private static string _pluginName;

	private static bool _allowLegitimateInterest;

	public static string ConsentLanguageCode { get; set; }

	public static LogLevel logLevel { get; protected set; }

	public static string PluginName
	{
		get
		{
			return _pluginName ?? (_pluginName = "MoPub Unity Plugin v" + moPubSDKVersion);
		}
	}

	protected MoPubBase()
	{
	}

	public static int CompareVersions(string a, string b)
	{
		int[] array = VersionStringToInts(a);
		int[] array2 = VersionStringToInts(b);
		for (int i = 0; i < Mathf.Max(array.Length, array2.Length); i++)
		{
			if (VersionPiece(array, i) < VersionPiece(array2, i))
			{
				return -1;
			}
			if (VersionPiece(array, i) > VersionPiece(array2, i))
			{
				return 1;
			}
		}
		return 0;
	}

	protected static void ValidateAdUnitForSdkInit(string adUnitId)
	{
		if (string.IsNullOrEmpty(adUnitId))
		{
			Debug.LogError("A valid ad unit ID is needed to initialize the MoPub SDK.");
		}
	}

	protected static void ReportAdUnitNotFound(string adUnitId)
	{
		Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
	}

	protected static Uri UrlFromString(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}
		try
		{
			return new Uri(url);
		}
		catch
		{
			Debug.LogError("Invalid URL: " + url);
			return null;
		}
	}

	private static int VersionPiece(IList<int> versionInts, int pieceIndex)
	{
		return (pieceIndex < versionInts.Count) ? versionInts[pieceIndex] : 0;
	}

	private static int[] VersionStringToInts(string version)
	{
		int piece;
		return (from v in version.Split('.')
			select int.TryParse(v, out piece) ? piece : 0).ToArray();
	}

	protected static void InitManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		MoPubManager component = new GameObject("MoPubManager", typeFromHandle).GetComponent<MoPubManager>();
		if (MoPubManager.Instance != component)
		{
			Debug.LogWarning("It looks like you have the " + typeFromHandle.Name + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}
}
