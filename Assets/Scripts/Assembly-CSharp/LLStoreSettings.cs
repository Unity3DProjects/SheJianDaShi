using UnityEngine;

[CreateAssetMenu(fileName = "LLStoreSettings")]
public class LLStoreSettings : ScriptableSingleton<LLStoreSettings>
{
	[Header("Android")]
	public string googlePlayAppPublicKey = string.Empty;

	public static string GooglePlayAppPublicKey()
	{
		return ScriptableSingleton<LLStoreSettings>.Instance.googlePlayAppPublicKey;
	}
}
