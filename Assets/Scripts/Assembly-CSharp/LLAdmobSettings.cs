using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LLAdmobSettings")]
public class LLAdmobSettings : ScriptableSingleton<LLAdmobSettings>
{
	[Header("APP Key / APP ID")]
	public string appKey = string.Empty;

	[Header("Unit")]
	public string unitID = string.Empty;

	[Header("Banner position type")]
	public BannerPosition positionTypeBanner;

	[Header("Banner size type")]
	public BannerSize sizeTypeBanner;

	[Header("Test devices needed")]
	public bool isTestDevicesNeeded = true;

	[Header("AdMob additional test Devices IDs")]
	public string[] additionalTestDevicesIds;

	private readonly string[] testDevices = new string[36]
	{
		"4b446ac1b3b36d38e3bb1db8d8ff2842", "692bdc9658a7eb3a40c93220da5a5f22", "845a5d69b2a01aba7725b8b964a1bb14", "e6dc5da67bc8497c4977cd5c49ea2e64", "46e8ed81d1c7034cae0a529f094640bc", "0dae6b9de8f5ff56cc2e72d127c917b4", "051a34b4a1b320cde9fdce3f39034710", "e78c9fe499637ed235f3f52dd2eab738", "b454087745093b71bc1ecf632f666010", "6dc737f72a35397848efcfeb4772f23a",
		"8c5444390315b67453492386826eeaa0", "bb282cee705f16a549c47984acfcc345", "2538ec25b285d0d74636d3acc221bd95", "97adbc58d3e9a8ec7d22f6c2193caea4", "c51c457bde1418341a08e92c814c95a1", "11ef02fbcbd876af479046be070fc3f0", "f3f5f03cbd49443e425d96ae5d07ba57", "90d92309d35af1052aeb8c005149402b", "bf4e34344cb00cca55b330f90741af37", "33e817cf7cdba7dc45a29d5e921e7a41",
		"1f28870e59f828cd0449c73437dfaebd", "049b2bfdb9bafabd7b8c5ffbdbc27575", "83a40411aaf46fe1e11c1b342eefd9b3", "b9843291d48298a404fd24e42bcc69d6", "d911729abba145a20773011f533d214c", "817bd4caa8ecdf643eff96d31b19be34", "e7837deaedd57919964c6f8eca4ce937", "2e9deaf3a6d4c9a9a32089760090c76f", "b3cf0877f5455883507117177bfd9630", "e47f7de560948c31bbfdb48f05cb9486",
		"f88dc6ef84ea853fc5ba9757b8cc3bfc", "a52f3b2673ed87093389020bae705c7f", "f3af82c5c03f340adba5c733f96c1fb2", "72455db5825bbd30ed5a01da31966741", "34f3e10f59785e50ab3b87951206195b", "9cc64f5dcff591721b617778245ba740"
	};

	[Header("AppLovin Banner")]
	public string appLovinSDKKey;

	public bool isNeedInitializeAppLovinSDK;

	[Header("Add AdMob framework to project")]
	public bool linkAdMobFramework;

	[Header("Add AppLovin framework to project")]
	public bool linkAppLovinFramework;

	private readonly string[] androidTestDevices = new string[13]
	{
		"5CE15B6A538A483D85FFBAC7AFA8D14C", "BD1DD44A4D6CC73902C3B62547B50054", "F30920DA32BCDB278BE146F8CB1870EF", "4E969EE8D28B260156EDF2A7FE63F60E", "6CB7515D525B90126B99127A7BD9C130", "F411C1B722295FA36DD74C5121B3A7A6", "46D6D71157D96D4C641FCE1E90BAED41", "89CC39D0D7A1F66BD10733CAA25D21FD", "3EBE9088A8354800676DE637B5B12CC6", "14CC456ABAD0A676BDE4517AA940C589",
		"2ECA780E92A885876DF6E28770B75113", "1849B7ECA12D8A6AC56501013E0BAA8B", "3902D0F644C7509F5647D6046CA0FDB7"
	};

	[Header("Android AdMob test Devices IDs")]
	public string[] androidAdditionalTestDevicesIds;

	public int TypePositionBanner
	{
		get
		{
			return (int)positionTypeBanner;
		}
	}

	public int TypeSizeBanner
	{
		get
		{
			return (int)sizeTypeBanner;
		}
	}

	public static string AppKey()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.appKey;
	}

	public static string UnitID()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.unitID;
	}

	public static int PositionType()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.TypePositionBanner;
	}

	public static int SizeType()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.TypeSizeBanner;
	}

	public static string AppLovinSDKKey()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.appLovinSDKKey;
	}

	public static bool IsNeedInitializeAppLovinSDK()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.isNeedInitializeAppLovinSDK;
	}

	public static bool LinkAdMobFramework()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.linkAdMobFramework;
	}

	public static bool LinkAppLovinFramework()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.linkAppLovinFramework;
	}

	public static bool IsTestDevicesNeeded()
	{
		return ScriptableSingleton<LLAdmobSettings>.Instance.isTestDevicesNeeded;
	}

	public static string[] TestDevicesIDs()
	{
		string[] array = null;
		List<string> list = new List<string>();
		list.AddRange(ScriptableSingleton<LLAdmobSettings>.Instance.androidTestDevices);
		list.AddRange(ScriptableSingleton<LLAdmobSettings>.Instance.androidAdditionalTestDevicesIds);
		return list.ToArray();
	}
}
