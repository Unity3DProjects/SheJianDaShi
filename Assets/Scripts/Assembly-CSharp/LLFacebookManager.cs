using System;
using System.Collections.Generic;
using AOT;
using UnityEngine;

public sealed class LLFacebookManager
{
	[Serializable]
	public class UserProfile
	{
		public string id;

		public string cover;

		public string name;

		public string first_name;

		public string last_name;

		public string age_range;

		public string link;

		public string gender;

		public string locale;

		public string picture;

		public string timezone;

		public string updated_time;

		public string verified;
	}

	[Serializable]
	public class UserFriends
	{
		public UserProfile[] data;
	}

	private delegate void LLCalbackBool(bool login);

	private delegate void LLCalbackString(string str);

	public static Action<string> OnTokenGet;

	public static bool _isLogin;

	public static UserProfile _currentUserProfile;

	public static UserFriends _currentUserFriends;

	public static string _currentUserEmail = string.Empty;

	public static string _currentUserAccessToken = string.Empty;

	private const string METHOD_INIT = "LLFacebookInit";

	private const string METHOD_LOGIN = "LLFacebookLogin";

	private const string METHOD_LOGIN_WITH_GET_DATA = "LLFacebookLoginWithGetData";

	private const string METHOD_LOGOUT = "LLFacebookLogout";

	private const string METHOD_IS_LOGIN = "LLFacebookIsLogin";

	private const string METHOD_GET_CURRENT_USER_TOKEN = "LLFacebookGetCurrentUserToken";

	private const string METHOD_LOG_EVENT = "LLFacebookLogEvent";

	private const string METHOD_LOG_PURCHASE = "LLFacebookLogPurchase";

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookInit", LLFacebookSettings.AppID(), LLFacebookSettings.DisplayName(), LLFacebookSettings.ClientToken());
	}

	public static void LogEvent(string eventName, double valueToSum = 0.0, Dictionary<string, string> parameters = null)
	{
		int num = 0;
		string[] array = null;
		string[] array2 = null;
		if (parameters != null)
		{
			num = parameters.Count;
			array = new string[num];
			parameters.Keys.CopyTo(array, 0);
			array2 = new string[num];
			parameters.Values.CopyTo(array2, 0);
		}
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookLogEvent", eventName, valueToSum, num, array, array2);
	}

	public static void LogPurchase(double amount, string currency = "USD", Dictionary<string, string> parameters = null)
	{
		int num = 0;
		string[] array = null;
		string[] array2 = null;
		if (parameters != null)
		{
			num = parameters.Count;
			array = new string[num];
			parameters.Keys.CopyTo(array, 0);
			array2 = new string[num];
			parameters.Values.CopyTo(array2, 0);
		}
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookLogPurchase", amount, currency, num, array, array2);
	}

	public static void Login()
	{
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookLogin", LLAndroidJavaCallback.ProxyCallback(ActivatedFacebookLogin));
	}

	public static void LoginWithGetData()
	{
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookLoginWithGetData", LLAndroidJavaCallback.ProxyCallback(ActivatedFacebookLogin), null, null, null, LLAndroidJavaCallback.ProxyCallback(CurrentUserToken));
	}

	public static void Logout()
	{
		LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookLogout", LLAndroidJavaCallback.ProxyCallback(ActivatedFacebookLogin));
		_currentUserProfile = null;
		_currentUserEmail = string.Empty;
		_currentUserFriends = null;
	}

	public static bool IsLogin()
	{
		_isLogin = LLAndroidJavaSingletone<LLTapdaqManager>.CallStatic<bool>("LLFacebookIsLogin", new object[0]);
		return _isLogin;
	}

	public static void GetUserProfile()
	{
	}

	public static void GetUserEmail()
	{
	}

	public static void GetUserFriends()
	{
	}

	public static string GetCurrentUserToken()
	{
		if (_isLogin)
		{
			LLAndroidJavaSingletone<LLFacebookManager>.CallStatic("LLFacebookGetCurrentUserToken", LLAndroidJavaCallback.ProxyCallback(CurrentUserToken));
		}
		return _currentUserAccessToken;
	}

	[MonoPInvokeCallback(typeof(LLCalbackBool))]
	private static void ActivatedFacebookLogin(bool login)
	{
		_isLogin = login;
	}

	[MonoPInvokeCallback(typeof(LLCalbackString))]
	private static void CurrentUserToken(string token)
	{
		_currentUserAccessToken = token;
		if (OnTokenGet != null)
		{
			OnTokenGet(token);
		}
	}

	[MonoPInvokeCallback(typeof(LLCalbackString))]
	private static void CurrentUserEmail(string email)
	{
		_currentUserEmail = email;
	}

	[MonoPInvokeCallback(typeof(LLCalbackString))]
	private static void CurrentUserProfile(string profile)
	{
		if (profile != null && profile != string.Empty)
		{
			_currentUserProfile = JsonUtility.FromJson<UserProfile>(profile);
		}
	}

	[MonoPInvokeCallback(typeof(LLCalbackString))]
	private static void CurrentUserFriends(string friends)
	{
		if (friends != null && friends != string.Empty)
		{
			_currentUserFriends = JsonUtility.FromJson<UserFriends>(friends);
		}
	}
}
