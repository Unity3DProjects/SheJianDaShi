using System;
using UnityEngine;

[Serializable]
public class TargetInfo
{
	public TargetType targetType;

	public string appName;

	public string bundleID;

	public string iconsPath;

	[Header("IDs")]
	public string tapjoyID;

	public string supersonicAppKey;

	public string appLovinSDKKey;

	public string appsflyerAppID;

	public string appsflyerDevKey;

	public string facebookAppID;

	public string tapdaqAppID;

	public string tapdaqClientKey;

	public string tapdaqPlacementTag;

	public string tenjinApiKey;
}
