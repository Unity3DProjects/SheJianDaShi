using UnityEngine;

[CreateAssetMenu(fileName = "LLGPGSSettings")]
public class LLGPGSSettings : ScriptableSingleton<LLGPGSSettings>
{
	[Header("App id")]
	public string appId = string.Empty;

	[Header("Web client id (optionaly)")]
	public string webClientId = string.Empty;

	[Header("Manifest elements")]
	public AndroidManifestElement[] androidManifestElements;

	public static string AppId()
	{
		return ScriptableSingleton<LLGPGSSettings>.Instance.appId;
	}

	public static string WebClientId()
	{
		return ScriptableSingleton<LLGPGSSettings>.Instance.webClientId;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		string text = AppId();
		AndroidManifestElement[] array = ScriptableSingleton<LLGPGSSettings>.Instance.androidManifestElements;
		if (array != null && array.Length > 0)
		{
			array[0].SetValues(text);
		}
		return array;
	}
}
