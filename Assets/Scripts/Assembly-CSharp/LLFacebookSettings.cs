using UnityEngine;

[CreateAssetMenu]
public class LLFacebookSettings : ScriptableSingleton<LLFacebookSettings>
{
	private const string FB_PREFIX_URL_SCHEME = "fb";

	[Header("App ID")]
	public string appID = string.Empty;

	[Header("Name (Optionaly)")]
	public string displayName = string.Empty;

	[Header("Client Token (Optionaly)")]
	public string clientToken = string.Empty;

	[Header("Add framework to project (not conflict with ad mediation)")]
	public bool linkFrameworks = true;

	[Header("Android")]
	public AndroidManifestElement[] androidManifestElements;

	public static string AppID()
	{
		return ScriptableSingleton<LLFacebookSettings>.Instance.appID;
	}

	public static string DisplayName()
	{
		return ScriptableSingleton<LLFacebookSettings>.Instance.displayName;
	}

	public static string ClientToken()
	{
		return ScriptableSingleton<LLFacebookSettings>.Instance.clientToken;
	}

	public static bool LinkFrameworks()
	{
		return ScriptableSingleton<LLFacebookSettings>.Instance.linkFrameworks;
	}

	public static string URLScheme()
	{
		return "fb" + ScriptableSingleton<LLFacebookSettings>.Instance.appID;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		string text = AppID();
		AndroidManifestElement[] array = ScriptableSingleton<LLFacebookSettings>.Instance.androidManifestElements;
		if (array != null && array.Length > 0)
		{
			array[0].SetValues(text);
			array[1].SetValues(text);
		}
		return array;
	}
}
