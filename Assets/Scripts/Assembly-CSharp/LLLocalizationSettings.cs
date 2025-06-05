using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LLLocalizationSettings")]
public class LLLocalizationSettings : ScriptableSingleton<LLLocalizationSettings>
{
	[Header("Languages")]
	public List<SystemLocalization> availableLocalizations;

	public static List<SystemLocalization> AvailableLocalizations()
	{
		return ScriptableSingleton<LLLocalizationSettings>.Instance.availableLocalizations;
	}
}
