using System;
using MiniJSON;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LLLocalUserGPGS : LLUserProfileGPGS, ILocalUser, IUserProfile
{
	private static AndroidJavaClass classLocalUser = new AndroidJavaClass("com.lllibset." + typeof(LLSocialGPGS).Name + "." + typeof(LLLocalUserGPGS).Name);

	private const string METHOD_AUTHENTICATE = "LLLocalUserGPGSAuthenticate";

	private const string METHOD_LOAD_FRIENDS = "LLLocalUserGPGSLoadFriends";

	private const string METHOD_GET_FRIENDS = "LLLocalUserGPGSFriends";

	private const string METHOD_IS_AUTHENTICATED = "LLLocalUserGPGSIsAuthenticated";

	private const string METHOD_IS_UNDERAGE = "LLLocalUserGPGSIsUnderage";

	private const string METHOD_GET_CURRENT_PLAYER_JSON = "LLLocalUserGPGSGetCurrentPlayerJSON";

	private static LLLocalUserGPGS instance = null;

	public static LLLocalUserGPGS Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LLLocalUserGPGS();
			}
			return instance;
		}
	}

	public bool authenticated
	{
		get
		{
			return classLocalUser.CallStatic<bool>("LLLocalUserGPGSIsAuthenticated", new object[0]);
		}
	}

	public IUserProfile[] friends
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public bool underage
	{
		get
		{
			return classLocalUser.CallStatic<bool>("LLLocalUserGPGSIsUnderage", new object[0]);
		}
	}

	private LLLocalUserGPGS()
	{
	}

	public void Authenticate(Action<bool> callback)
	{
		AndroidJavaProxy androidJavaProxy = HandlerAuthentication(callback);
		classLocalUser.CallStatic("LLLocalUserGPGSAuthenticate", androidJavaProxy);
	}

	public void Authenticate(Action<bool, string> callback)
	{
		throw new NotImplementedException();
	}

	public void LoadFriends(Action<bool> callback)
	{
		throw new NotImplementedException();
	}

	private AndroidJavaProxy HandlerAuthentication(Action<bool> callback)
	{
		return LLAndroidJavaCallback.ProxyCallback(delegate(bool success)
		{
			if (success)
			{
				string text = classLocalUser.CallStatic<string>("LLLocalUserGPGSGetCurrentPlayerJSON", new object[0]);
				if (!string.IsNullOrEmpty(text))
				{
					object obj = JSONParser.Parse(text, typeof(LLNativeUserProfile));
					if (obj is LLNativeUserProfile)
					{
						CopyFields((LLNativeUserProfile)obj);
					}
				}
			}
			if (callback != null)
			{
				callback(success);
			}
		});
	}
}
