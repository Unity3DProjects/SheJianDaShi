using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LLTapdaqSettings")]
public class LLTapdaqSettings : ScriptableSingleton<LLTapdaqSettings>
{
	[Header("AppID")]
	public string appID = string.Empty;

	[Header("ClientKey")]
	public string key = string.Empty;

	[Header("Placement Tag")]
	public string placementTag = string.Empty;

	[Header("Portrait")]
	public bool isPortrait;

	[Header("TestMode")]
	public bool enableTestMode;

	[Header("Embedded Binaries")]
	public bool isNeedAddEmbeddedBinariesFramework;

	[Header("Url Schemes")]
	public string[] additionalURLSchemes = new string[0];

	[Header("Queries Schemes")]
	public string[] additionalQueriesSchemes = new string[0];

	private readonly string[] queriesSchemes = new string[25]
	{
		"mercify", "bowmasters", "bowmastersadfree", "hellycopter", "despicablebear", "despicablebearadfree", "wheels", "wheelsadfree", "states", "statesadfree",
		"rubinio", "kittengun", "handofgod", "curveball", "kickthebuddy", "coloric", "tanks", "twerk", "partymasters", "tgr2048",
		"tgrbillball", "tgrhexio", "tgrdictator", "hmff", "hmfromcore"
	};

	[Header("Android")]
	public string deepLinkName = string.Empty;

	public AndroidManifestElement[] androidManifestElements;

	public static string AppID()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.appID;
	}

	public static string ClientKey()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.key;
	}

	public static string PlacementTag()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.placementTag;
	}

	public static bool IsPortrait()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.isPortrait;
	}

	public static bool EnableTestMode()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.enableTestMode;
	}

	public static bool IsNeedEmbeddedBinaries()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.isNeedAddEmbeddedBinariesFramework;
	}

	public static string[] QueriesSchemes()
	{
		List<string> list = new List<string>();
		list.AddRange(ScriptableSingleton<LLTapdaqSettings>.Instance.queriesSchemes);
		list.AddRange(ScriptableSingleton<LLTapdaqSettings>.Instance.additionalQueriesSchemes);
		return list.ToArray();
	}

	public static string[] URLSchemes()
	{
		List<string> list = new List<string>();
		list.AddRange(ScriptableSingleton<LLTapdaqSettings>.Instance.additionalURLSchemes);
		return list.ToArray();
	}

	public static string DeepLinkName()
	{
		return ScriptableSingleton<LLTapdaqSettings>.Instance.deepLinkName;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		string text = DeepLinkName();
		AndroidManifestElement[] array = ScriptableSingleton<LLTapdaqSettings>.Instance.androidManifestElements;
		if (array != null && array.Length > 0)
		{
			array[0].SetValues(text);
		}
		return array;
	}
}
