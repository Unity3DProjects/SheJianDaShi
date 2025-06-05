using UnityEngine;

[CreateAssetMenu(fileName = "LLNotificationSettings")]
public class LLNotificationSettings : ScriptableSingleton<LLNotificationSettings>
{
	[Header("Icon")]
	public string iconFileName = string.Empty;

	public static string IconFileName()
	{
		return ScriptableSingleton<LLNotificationSettings>.Instance.iconFileName;
	}
}
