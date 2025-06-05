using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TargetSettings : ScriptableSingleton<TargetSettings>
{
	public List<TargetInfo> availableInfo;

	public static TargetInfo InfoByType(TargetType type)
	{
		return ScriptableSingleton<TargetSettings>.Instance.availableInfo.Find((TargetInfo currentInfo) => currentInfo.targetType == type);
	}
}
