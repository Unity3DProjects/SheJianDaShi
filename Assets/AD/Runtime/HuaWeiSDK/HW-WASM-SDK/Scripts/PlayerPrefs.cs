#if HuaWei
using HWWASM;
#if UNITY_WEBGL && !UNITY_EDITOR
	public class PlayerPrefs
	{
		public static void SetInt(string key, int value)
		{
			QG.LocalStorage.SetItem(key, value.ToString());
		}

		public static int GetInt(string key, int defaultValue = 0)
		{
			string value = QG.LocalStorage.GetItem(key);
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			int.TryParse(value, out defaultValue);
			return defaultValue;
		}

		public static void SetString(string key, string value)
		{
			QG.LocalStorage.SetItem(key, value);
		}

		public static string GetString(string key, string defaultValue = "")
		{
			string value = QG.LocalStorage.GetItem(key);
			return value ?? defaultValue;
		}

		public static void SetFloat(string key, float value)
		{
			QG.LocalStorage.SetItem(key, value.ToString());
		}

		public static float GetFloat(string key, float defaultValue = 0.0f)
		{
			string value = QG.LocalStorage.GetItem(key);
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			float.TryParse(value, out defaultValue);
			return defaultValue;
		}


		public static bool HasKey(string key)
		{
			return QG.LocalStorage.GetItem(key) != null;
		}

		public static void DeleteKey(string key)
		{
			QG.LocalStorage.RemoveItem(key);
		}

		public static void DeleteAll()
		{
			QG.LocalStorage.Clear();
		}

		public static void Save()
		{
		}
	}
#else
public class PlayerPrefs
{
	public static void SetInt(string key, int value)
	{
		UnityEngine.PlayerPrefs.SetInt(key, value);
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
	}

	public static void SetString(string key, string value)
	{
		UnityEngine.PlayerPrefs.SetString(key, value);
	}

	public static string GetString(string key, string defaultValue = "")
	{
		return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
	}

	public static void SetFloat(string key, float value)
	{
		UnityEngine.PlayerPrefs.SetFloat(key, value);
	}

	public static float GetFloat(string key, float defaultValue = 0.0f)
	{
		return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
	}

	public static bool HasKey(string key)
	{
		return UnityEngine.PlayerPrefs.HasKey(key);
	}

	public static void DeleteKey(string key)
	{
		UnityEngine.PlayerPrefs.DeleteKey(key);
	}

	public static void DeleteAll()
	{
		UnityEngine.PlayerPrefs.DeleteAll();
	}

	public static void Save()
	{
		UnityEngine.PlayerPrefs.Save();
	}
}
#endif
#endif