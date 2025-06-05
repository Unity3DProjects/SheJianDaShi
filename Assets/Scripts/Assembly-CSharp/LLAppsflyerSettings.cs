using UnityEngine;

[CreateAssetMenu(fileName = "LLAppsflyerSettings")]
public class LLAppsflyerSettings : ScriptableSingleton<LLAppsflyerSettings>
{
	[Header("App ID")]
	public string appID = string.Empty;

	[Header("Dev Key")]
	public string devKey = string.Empty;

	[Header("Debug")]
	public bool enableDebug;

	public static string AppID()
	{
		return ScriptableSingleton<LLAppsflyerSettings>.Instance.appID;
	}

	public static string DevKey()
	{
		return ScriptableSingleton<LLAppsflyerSettings>.Instance.devKey;
	}

	public static bool EnableDebug()
	{
		return ScriptableSingleton<LLAppsflyerSettings>.Instance.enableDebug;
	}
}
