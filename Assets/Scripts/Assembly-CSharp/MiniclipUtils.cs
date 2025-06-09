using System;
using System.Text;
using UnityEngine;

public static class MiniclipUtils
{
	public static long GetUnixTimeSeconds()
	{
		return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static long GetUnixTimeMilliseconds()
	{
		return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
	}

	public static bool IsAdvertisingIdInitialized()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		return androidJavaClass.CallStatic<bool>("isAdvertisingIdInitialized", new object[0]);
	}

	public static string GetAdvertisingId()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		return androidJavaClass.CallStatic<string>("getAdvertisingID", new object[0]);
	}

	public static string GetPlatform()
	{
		return "Google";
	}

	public static string GetTimeZone()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		return androidJavaClass.CallStatic<string>("getTimeZone", new object[0]);
	}

	public static string GetDeviceType()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.mcgoliath.MCGoliathWrapper");
		return androidJavaClass.CallStatic<string>("deviceType", new object[0]);
	}

	public static string GetSystemArchitecture()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.mcgoliath.MCGoliathWrapper");
		return androidJavaClass.CallStatic<string>("deviceArchitecture", new object[0]);
	}

	public static int IsDeviceJailbroken()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		return androidJavaClass.CallStatic<bool>("isDeviceRooted", new object[0]) ? 1 : 0;
	}

	public static string GetStartNotificationId()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.LNBindings");
		return androidJavaClass.GetStatic<string>("localNotificationID");
	}

	public static void ShowReceiptValidatingPopup(string title, string text)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		androidJavaClass.CallStatic("ShowProgressDialog", title, text);
	}

	public static void DismissReceiptValidatingPopup()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		androidJavaClass.CallStatic("DismissProgressDialog");
	}

	public static bool isNetworkAvailable()
	{
		// return Application.internetReachability != NetworkReachability.NotReachable;
		return true;
	}

	public static void SendToast(string message)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		androidJavaClass.CallStatic("sendToast", message);
	}

	public static void ShareAll(string message, string link)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		androidJavaClass.CallStatic("shareToAll", message, link);
	}

	public static void SetGDPRConsentForThirdPartyProviders(bool isRelevant, bool targetedAds)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.miniclip.Utils");
		androidJavaClass.CallStatic("setGDPRConsentForThirdPartyProviders", isRelevant, targetedAds);
	}

	public static string ConvertStringToEncriptedHex(string text, string key)
	{
		try
		{
			char[] array = text.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= key[i % key.Length];
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array.Length; j++)
			{
				stringBuilder.AppendFormat("{0:X2}", Convert.ToUInt32(array[j]));
			}
			return stringBuilder.ToString();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return null;
		}
	}

	public static string ConvertEncriptedHexToString(string hexValue, string key)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder((int)((float)hexValue.Length * 0.5f));
			int num = (int)((float)hexValue.Length * 0.5f);
			for (int i = 0; i < num; i++)
			{
				char c = (char)Convert.ToUInt32(hexValue.Substring(2 * i, 2), 16);
				c ^= key[i % key.Length];
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return null;
		}
	}
}
