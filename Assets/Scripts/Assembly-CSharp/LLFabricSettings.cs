using UnityEngine;

[CreateAssetMenu(fileName = "LLFabricSettings")]
public class LLFabricSettings : ScriptableSingleton<LLFabricSettings>
{
	[Header("Key")]
	public string apiKey = string.Empty;

	[Header("Debug")]
	public bool enableDebug;

	[Header("Android")]
	public AndroidManifestElement[] androidManifestElements;

	public static string ApiKey()
	{
		return ScriptableSingleton<LLFabricSettings>.Instance.apiKey;
	}

	public static bool EnableDebug()
	{
		return ScriptableSingleton<LLFabricSettings>.Instance.enableDebug;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		string text = ApiKey();
		AndroidManifestElement[] array = ScriptableSingleton<LLFabricSettings>.Instance.androidManifestElements;
		if (array != null && array.Length > 0)
		{
			array[0].SetValues(text);
		}
		return array;
	}
}
