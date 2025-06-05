using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public sealed class LLSocialGPGS : ISocialPlatform
{
	private const string METHOD_INIT = "LLSocialGPGSInit";

	private const string METHOD_UNLOCK_ACHIEVEMENT = "LLSocialGPGSUnlockAchievement";

	private const string METHOD_INCREMENT_ACHIEVEMENT = "LLSocialGPGSIncrementAchievement";

	private const string METHOD_SUBMIT_SCORE = "LLSocialGPGSSubmitScore";

	private const string METHOD_SHOW_ACHIEVEMENTS_UI = "LLSocialGPGSShowAchievementsUI";

	private const string METHOD_SHOW_LEADERBOARD_UI = "LLSocialGPGSShowLeaderboardUI";

	private const string METHOD_LOAD_SCORES = "LLSocialGPGSLoadScores";

	private const string METHOD_LOAD_ACHIEVEMENTS = "LLSocialGPGSLoadAchievements";

	private const string METHOD_GET_SERVER_AUTH_CODE = "LLSocialGPGSGetServerAuthCode";

	private const string METHOD_HAS_PERMISSION = "LLSocialGPGSHasPermission";

	private const string METHOD_REQUEST_PERMISSION = "LLSocialGPGSRequestPermission";

	private const string METHOD_SIGN_OUT = "LLSocialGPGSSignOut";

	private static LLSocialGPGS instance;

	private string webClientId;

	private LeaderboardConfiguration leaderboardConfig;

	public static LLSocialGPGS Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LLSocialGPGS();
			}
			return instance;
		}
	}

	public bool IsRequestAuthCode
	{
		get
		{
			return !string.IsNullOrEmpty(webClientId);
		}
	}

	public LeaderboardConfiguration LeaderboardConfig
	{
		get
		{
			return leaderboardConfig;
		}
		set
		{
			leaderboardConfig = value;
		}
	}

	public ILocalUser localUser
	{
		get
		{
			return LLLocalUserGPGS.Instance;
		}
	}

	private LLSocialGPGS()
	{
		webClientId = null;
		leaderboardConfig = new LeaderboardConfiguration
		{
			leaderboardType = LLLeaderboardType.CurrentPlayer,
			timeScope = TimeScope.AllTime,
			rowCount = 25
		};
	}

	public IAchievement CreateAchievement()
	{
		return new LLAchievementGPGS();
	}

	public void ReportProgress(string achievementId, double progress, Action<bool> callback)
	{
		if (progress == 100.0)
		{
			LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSUnlockAchievement", achievementId, LLAndroidJavaCallback.ProxyCallback(callback));
		}
		else
		{
			callback(false);
		}
	}

	public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
	{
		if (callback != null)
		{
			Action<LLAchievementGPGS[]> callback2 = delegate(LLAchievementGPGS[] achievementsGPGS)
			{
				callback(achievementsGPGS);
			};
			AndroidJavaProxy androidJavaProxy = HandlerJSONAchievements(callback2);
			LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSLoadAchievements", androidJavaProxy);
		}
	}

	public void LoadAchievements(Action<IAchievement[]> callback)
	{
		if (callback != null)
		{
			Action<LLAchievementGPGS[]> callback2 = delegate(LLAchievementGPGS[] achievementsGPGS)
			{
				callback(achievementsGPGS);
			};
			AndroidJavaProxy androidJavaProxy = HandlerJSONAchievements(callback2);
			LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSLoadAchievements", androidJavaProxy);
		}
	}

	public void ShowAchievementsUI()
	{
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSShowAchievementsUI");
	}

	public ILeaderboard CreateLeaderboard()
	{
		return new LLLeaderboardGPGS(leaderboardConfig.leaderboardType, leaderboardConfig.timeScope);
	}

	public void ReportScore(long score, string leaderboardId, Action<bool> callback)
	{
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSSubmitScore", score, leaderboardId, LLAndroidJavaCallback.ProxyCallback(callback));
	}

	public void LoadScores(string leaderboardID, Action<IScore[]> callback)
	{
		LoadScores(leaderboardID, leaderboardConfig.leaderboardType, leaderboardConfig.timeScope, leaderboardConfig.rowCount, callback);
	}

	public void LoadScores(ILeaderboard board, Action<bool> callback)
	{
		board.LoadScores(callback);
	}

	public bool GetLoading(ILeaderboard board)
	{
		return board.loading;
	}

	public void ShowLeaderboardUI()
	{
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSShowLeaderboardUI");
	}

	public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
	{
	}

	public void Authenticate(ILocalUser user, Action<bool> callback)
	{
	}

	public void Authenticate(ILocalUser user, Action<bool, string> callback)
	{
	}

	public void LoadFriends(ILocalUser user, Action<bool> callback)
	{
	}

	public static LLSocialGPGS Activate()
	{
		CustomDebug.Log("Activate social for Android");
		string text = LLGPGSSettings.WebClientId();
		Instance.webClientId = ((!string.IsNullOrEmpty(text)) ? text : null);
		Social.Active = Instance;
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSInit", Instance.webClientId);
		return Instance;
	}

	public void SignOut()
	{
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSSignOut");
	}

	public string GetServerAuthCode()
	{
		string result = string.Empty;
		if (IsRequestAuthCode)
		{
			result = LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic<string>("LLSocialGPGSGetServerAuthCode", new object[0]);
		}
		return result;
	}

	public bool HasPermission(LLGooglePermission permission)
	{
		return LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic<bool>("LLSocialGPGSHasPermission", new object[1] { (int)permission });
	}

	public void RequestPermission(LLGooglePermission permission, Action<bool> callback)
	{
		LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSRequestPermission", (int)permission, LLAndroidJavaCallback.ProxyCallback(callback));
	}

	public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
	{
	}

	public void LoadScores(string leaderboardID, LLLeaderboardType leaderboardType, TimeScope timeScope, int rowCount, Action<IScore[]> callback)
	{
		if (callback != null)
		{
			AndroidJavaProxy androidJavaProxy = HandlerJSONScores(leaderboardID, leaderboardType, rowCount, timeScope, callback);
			LLAndroidJavaSingletone<LLSocialGPGS>.CallStatic("LLSocialGPGSLoadScores", leaderboardID, (int)leaderboardType, (int)GetNativeTimeSpan(timeScope), rowCount, androidJavaProxy);
		}
	}

	private LLLeaderboardTimeSpan GetNativeTimeSpan(TimeScope value)
	{
		switch (value)
		{
		case TimeScope.Week:
			return LLLeaderboardTimeSpan.Weekly;
		case TimeScope.Today:
			return LLLeaderboardTimeSpan.Daily;
		case TimeScope.AllTime:
			return LLLeaderboardTimeSpan.AllTime;
		default:
			return LLLeaderboardTimeSpan.AllTime;
		}
	}

	private AndroidJavaProxy HandlerJSONScores(string leaderboardID, LLLeaderboardType leaderboardType, int rowCount, TimeScope timeScope, Action<IScore[]> callback)
	{
		return LLAndroidJavaCallback.ProxyCallback(delegate(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				callback(null);
			}
			else
			{
				List<LLNativeScore> list = new List<LLNativeScore>();
				if (leaderboardType == LLLeaderboardType.CurrentPlayer)
				{
					object obj = JSONParser.Parse(json, typeof(LLNativeScore));
					if (obj is LLNativeScore)
					{
						list.Add((LLNativeScore)obj);
					}
				}
				else
				{
					List<LLNativeScore> list2 = JSONParser.Parse(json, typeof(List<LLNativeScore>)) as List<LLNativeScore>;
					if (list2 != null)
					{
						list = list2;
					}
				}
				LLScoreGPGS[] array = new LLScoreGPGS[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = new LLScoreGPGS(leaderboardID, list[i]);
				}
				callback(array);
			}
		});
	}

	private AndroidJavaProxy HandlerJSONAchievements(Action<LLAchievementGPGS[]> callback)
	{
		return LLAndroidJavaCallback.ProxyCallback(delegate(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				callback(null);
			}
			else
			{
				List<LLNativeAchievement> list = JSONParser.Parse(json, typeof(List<LLNativeAchievement>)) as List<LLNativeAchievement>;
				if (list != null)
				{
					LLAchievementGPGS[] array = new LLAchievementGPGS[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						array[i] = new LLAchievementGPGS(list[i]);
					}
					callback(array);
				}
				else
				{
					callback(null);
				}
			}
		});
	}
}
