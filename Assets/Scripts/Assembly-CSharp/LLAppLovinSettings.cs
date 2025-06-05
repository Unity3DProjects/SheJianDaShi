using UnityEngine;

[CreateAssetMenu(fileName = "LLAppLovinSettings")]
public class LLAppLovinSettings : ScriptableSingleton<LLAppLovinSettings>
{
	[Header("Key")]
	public string sdkKey = string.Empty;

	[Header("Add framework to project")]
	public bool linkFramework;

	[Header("Debug")]
	public bool enableDebug;

	[Header("Android")]
	public AndroidManifestElement[] androidManifestElements;

	public static string SDKKey()
	{
		return ScriptableSingleton<LLAppLovinSettings>.Instance.sdkKey;
	}

	public static bool LinkFramework()
	{
		return ScriptableSingleton<LLAppLovinSettings>.Instance.linkFramework;
	}

	public static bool IsDebug()
	{
		bool flag = ScriptableSingleton<LLAppLovinSettings>.Instance.enableDebug;
		if (CustomDebug.Enable)
		{
			return flag;
		}
		if (flag)
		{
			CustomDebug.LogWarning("You can't enable debug in AppLovin in not development build!");
		}
		return false;
	}

	public static AndroidManifestElement[] AndroidManifestElements()
	{
		string text = SDKKey();
		AndroidManifestElement[] array = ScriptableSingleton<LLAppLovinSettings>.Instance.androidManifestElements;
		if (array != null && array.Length > 0)
		{
			array[0].SetValues(text);
		}
		return array;
	}
}
