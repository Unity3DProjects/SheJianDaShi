using UnityEngine;

[CreateAssetMenu(fileName = "LLTapjoySettings")]
public class LLTapjoySettings : ScriptableSingleton<LLTapjoySettings>
{
	[Header("Key")]
	public string appKey = string.Empty;

	[Header("Debug")]
	public bool enableDebug;

	[Header("Add framework to project")]
	public bool linkFramework;

	[Header("Android")]
	public AndroidManifestElement[] androidManifestElements;

	public static string AppKey()
	{
		return ScriptableSingleton<LLTapjoySettings>.Instance.appKey;
	}

	public static bool EnableDebug()
	{
		return ScriptableSingleton<LLTapjoySettings>.Instance.enableDebug;
	}

	public static bool LinkFramework()
	{
		return ScriptableSingleton<LLTapjoySettings>.Instance.linkFramework;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		return ScriptableSingleton<LLTapjoySettings>.Instance.androidManifestElements;
	}
}
