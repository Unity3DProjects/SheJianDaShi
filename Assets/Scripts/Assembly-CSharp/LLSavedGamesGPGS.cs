using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public sealed class LLSavedGamesGPGS
{
	private static readonly AndroidJavaClass classSavedGames = new AndroidJavaClass("com.lllibset." + typeof(LLSocialGPGS).Name + "." + typeof(LLSavedGamesGPGS).Name);

	private const string METHOD_INIT = "LLSavedGamesGPGSInit";

	private const string METHOD_SET_CALLBACKS = "LLSavedGamesGPGSSetCallbacks";

	private const string METHOD_LOAD_GAME_PROGRESS = "LLSavedGamesGPGSLoadGameProgress";

	private const string METHOD_SAVE_GAME_PROGRESS = "LLSavedGamesGPGSSaveGameProgress";

	private static readonly string DEFAULT_STRING_VALUE = string.Empty;

	private static readonly DateTime DEFAULT_DATETIME_VALUE = DateTime.MinValue;

	private static bool isInitialize;

	private static Dictionary<string, object> gameProgress = new Dictionary<string, object>();

	private static bool isLoaded;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnSavedGameLoad;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnProgressLoad;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnProgressSaved;

	public static bool IsAuthenticated
	{
		get
		{
			return isInitialize && LLSocialGPGS.Instance.HasPermission(LLGooglePermission.DriveAppFolder) && LLSocialGPGS.Instance.HasPermission(LLGooglePermission.GamesLite);
		}
	}

	public static bool IsLoaded
	{
		get
		{
			return isLoaded;
		}
	}

	private static event Action<string> OnSavedGameLoad
	{
		add
		{
			Action<string> action = LLSavedGamesGPGS.m_OnSavedGameLoad;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnSavedGameLoad, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLSavedGamesGPGS.m_OnSavedGameLoad;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnSavedGameLoad, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnProgressLoad
	{
		add
		{
			Action<bool> action = LLSavedGamesGPGS.m_OnProgressLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnProgressLoad, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSavedGamesGPGS.m_OnProgressLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnProgressLoad, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnProgressSaved
	{
		add
		{
			Action<bool> action = LLSavedGamesGPGS.m_OnProgressSaved;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnProgressSaved, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLSavedGamesGPGS.m_OnProgressSaved;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSavedGamesGPGS.m_OnProgressSaved, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		if (isInitialize)
		{
			return;
		}
		classSavedGames.CallStatic("LLSavedGamesGPGSInit");
		if (LLSocialGPGS.Instance.localUser.authenticated)
		{
			CustomDebug.Log("LLSavedGamesGPGS Initialized");
			isInitialize = true;
			classSavedGames.CallStatic("LLSavedGamesGPGSSetCallbacks", LLAndroidJavaCallback.ProxyCallback(GameProgressSaved), LLAndroidJavaCallback.ProxyCallback(JSONGameProgressLoaded));
			LoadGameProgress();
			return;
		}
		CustomDebug.Log("Try Authenticate for Initialize LLSavedGamesGPGS");
		LLSocialGPGS.Instance.localUser.Authenticate(delegate(bool success)
		{
			if (success)
			{
				Initialize();
			}
		});
	}

	public static void SetBool(string key, bool value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static void SetFloat(string key, float value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static void SetDouble(string key, double value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static void SetInt(string key, int value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static void SetDateTime(string key, DateTime value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static void SetString(string key, string value, bool saveServerImmediately = false)
	{
		SetValue(key, value, saveServerImmediately);
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		return GetValue(key, defaultValue);
	}

	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return GetValue(key, defaultValue);
	}

	public static double GetDouble(string key, double defaultValue = 0.0)
	{
		return GetValue(key, defaultValue);
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		return GetValue(key, defaultValue);
	}

	public static DateTime GetDateTime(string key)
	{
		return GetDateTime(key, DEFAULT_DATETIME_VALUE);
	}

	public static DateTime GetDateTime(string key, DateTime defaultValue)
	{
		return GetValue(key, defaultValue);
	}

	public static string GetString(string key)
	{
		return GetString(key, DEFAULT_STRING_VALUE);
	}

	public static string GetString(string key, string defaultValue)
	{
		return GetValue(key, defaultValue);
	}

	public static bool HasKey(string key)
	{
		return gameProgress.ContainsKey(key);
	}

	public static void DeleteKey(string key)
	{
		if (gameProgress.ContainsKey(key))
		{
			gameProgress.Remove(key);
			Save();
		}
	}

	public static void DeleteAll()
	{
		if (gameProgress.Keys.Count > 0)
		{
			gameProgress.Clear();
			Save();
		}
	}

	public static void Save()
	{
		if (IsAuthenticated)
		{
			string text = JsonConvert.SerializeObject(gameProgress);
			CustomDebug.Log("Save GameProgress " + text);
			classSavedGames.CallStatic("LLSavedGamesGPGSSaveGameProgress", text);
		}
	}

	public static void LoadGameProgress()
	{
		if (IsAuthenticated)
		{
			CustomDebug.Log("Load GameProgress");
			classSavedGames.CallStatic("LLSavedGamesGPGSLoadGameProgress");
		}
	}

	private static void SetValue(string key, object value, bool saveServerImmediately = false)
	{
		if (gameProgress.ContainsKey(key))
		{
			gameProgress[key] = value;
		}
		else
		{
			gameProgress.Add(key, value);
		}
		if (saveServerImmediately)
		{
			Save();
		}
	}

	private static T GetValue<T>(string key, T defaultValue)
	{
		object value;
		if (gameProgress.TryGetValue(key, out value))
		{
			if (value is ValueType)
			{
				return (T)Convert.ChangeType(value, typeof(T));
			}
			if (value is T)
			{
				return (T)value;
			}
		}
		return defaultValue;
	}

	private static void JSONGameProgressLoaded(string json)
	{
		CustomDebug.Log("JSONGameProgressLoaded " + json);
		isLoaded = false;
		gameProgress.Clear();
		if (!string.IsNullOrEmpty(json))
		{
			gameProgress = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (gameProgress != null)
			{
				LLSavedGamesGPGS.m_OnProgressLoad?.Invoke(true);
				isLoaded = true;
				return;
			}
		}
		LLSavedGamesGPGS.m_OnProgressLoad?.Invoke(false);
	}

	private static void GameProgressSaved(bool success)
	{
		CustomDebug.Log("GameProgressSaved " + success);
		if (!success)
		{
			isLoaded = false;
			LoadGameProgress();
		}
		LLSavedGamesGPGS.m_OnProgressSaved?.Invoke(success);
	}
}
