using System;
using UnityEngine;

public sealed class CustomPlayerPrefs
{
	private static readonly string DefaultStringValue = string.Empty;

	private const int DEFAULT_INT_VALUE = 0;

	private const float DEFAULT_FLOAT_VALUE = 0f;

	private const int TRUE_INT_VALUE = 1;

	private const int FALSE_INT_VALUE = 0;

	public static void SetBool(string key, bool value, bool saveImmediately = false)
	{
		int value2 = (value ? 1 : 0);
		SetInt(key, value2, saveImmediately);
	}

	public static void SetFloat(string key, float value, bool saveImmediately = false)
	{
		PlayerPrefs.SetFloat(key, value);
		if (saveImmediately)
		{
			Save();
		}
	}

	public static void SetInt(string key, int value, bool saveImmediately = false)
	{
		PlayerPrefs.SetInt(key, value);
		if (saveImmediately)
		{
			Save();
		}
	}

	public static void SetDateTime(string key, DateTime value, bool saveImmediately = false)
	{
		PlayerPrefs.SetString(key, value.ToBinary().ToString());
		if (saveImmediately)
		{
			Save();
		}
	}

	public static void SetString(string key, string value, bool saveImmediately = false)
	{
		PlayerPrefs.SetString(key, value);
		if (saveImmediately)
		{
			Save();
		}
	}

	public static void SetEnumValue<T>(string key, T value, bool saveImmediately = false) where T : struct, IConvertible
	{
		SetString(key, value.ToString(), saveImmediately);
	}

	public static bool GetBool(string key)
	{
		return GetInt(key) == 1;
	}

	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	public static int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public static DateTime GetDateTime(string key, DateTime defaultValue)
	{
		string value = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(value))
		{
			long dateData = Convert.ToInt64(value);
			return DateTime.FromBinary(dateData);
		}
		return defaultValue;
	}

	public static string GetString(string key)
	{
		return GetString(key, DefaultStringValue);
	}

	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static T GetEnumValue<T>(string key, T defaultValue = default(T)) where T : struct, IConvertible
	{
		string value = GetString(key);
		if (!string.IsNullOrEmpty(value))
		{
			return (T)Enum.Parse(typeof(T), value);
		}
		return defaultValue;
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
		Save();
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
		Save();
	}

	public static void Save()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.ScheduleMethod(MiniclipManager.Instance.SchedulerObjTag, delegate
		{
			PlayerPrefs.Save();
			SingletonMonoBehaviour<Scheduler>.Instance.UnscheduleAllMethodForTarget(MiniclipManager.Instance.SchedulerObjTag);
		}, 1f);
	}
}
