#if HuaWei
using HWWASM;

#if UNITY_WEBGL && !UNITY_EDITOR
[UnityEngine.Scripting.Preserve]
public class PlayerPrefs
{
	[UnityEngine.Scripting.Preserve]
    public static void SetInt(string key, int value)
    {
        QG.LocalStorage.SetItem(key, value.ToString());
    }

    [UnityEngine.Scripting.Preserve]
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

    [UnityEngine.Scripting.Preserve]
    public static void SetString(string key, string value)
    {
        QG.LocalStorage.SetItem(key, value);
    }

    [UnityEngine.Scripting.Preserve]
    public static string GetString(string key, string defaultValue = "")
    {
        string value = QG.LocalStorage.GetItem(key);
        return value ?? defaultValue;
    }

    [UnityEngine.Scripting.Preserve]
    public static void SetFloat(string key, float value)
    {
        QG.LocalStorage.SetItem(key, value.ToString());
    }

    [UnityEngine.Scripting.Preserve]
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


    [UnityEngine.Scripting.Preserve]
    public static bool HasKey(string key)
    {
        return QG.LocalStorage.GetItem(key) != null;
    }

    [UnityEngine.Scripting.Preserve]
    public static void DeleteKey(string key)
    {
        QG.LocalStorage.RemoveItem(key);
    }

    [UnityEngine.Scripting.Preserve]
    public static void DeleteAll()
    {
        QG.LocalStorage.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    public static void Save()
    {
    }
}
#else
[UnityEngine.Scripting.Preserve]
public class PlayerPrefs
{
	[UnityEngine.Scripting.Preserve]
	public static void SetInt(string key, int value)
	{
		UnityEngine.PlayerPrefs.SetInt(key, value);
	}

	[UnityEngine.Scripting.Preserve]
	public static int GetInt(string key, int defaultValue = 0)
	{
		return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
	}

	[UnityEngine.Scripting.Preserve]
	public static void SetString(string key, string value)
	{
		UnityEngine.PlayerPrefs.SetString(key, value);
	}

	[UnityEngine.Scripting.Preserve]
	public static string GetString(string key, string defaultValue = "")
	{
		return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
	}

	[UnityEngine.Scripting.Preserve]
	public static void SetFloat(string key, float value)
	{
		UnityEngine.PlayerPrefs.SetFloat(key, value);
	}

	[UnityEngine.Scripting.Preserve]
	public static float GetFloat(string key, float defaultValue = 0.0f)
	{
		return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
	}

	[UnityEngine.Scripting.Preserve]
	public static bool HasKey(string key)
	{
		return UnityEngine.PlayerPrefs.HasKey(key);
	}

	[UnityEngine.Scripting.Preserve]
	public static void DeleteKey(string key)
	{
		UnityEngine.PlayerPrefs.DeleteKey(key);
	}

	[UnityEngine.Scripting.Preserve]
	public static void DeleteAll()
	{
		UnityEngine.PlayerPrefs.DeleteAll();
	}

	[UnityEngine.Scripting.Preserve]
	public static void Save()
	{
		UnityEngine.PlayerPrefs.Save();
	}
}
#endif
#endif