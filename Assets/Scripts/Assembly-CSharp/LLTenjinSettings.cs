using UnityEngine;

[CreateAssetMenu(fileName = "LLTenjinSettings")]
public class LLTenjinSettings : ScriptableSingleton<LLTenjinSettings>
{
	[Header("App ID")]
	public string apiKey = string.Empty;

	public static string ApiKey()
	{
		return ScriptableSingleton<LLTenjinSettings>.Instance.apiKey;
	}
}
