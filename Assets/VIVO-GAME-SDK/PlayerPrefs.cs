using QGMiniGame;
using UnityEngine;

[UnityEngine.Scripting.Preserve]

//覆盖unity的PlayerPrefs
public static class PlayerPrefs
{
    [UnityEngine.Scripting.Preserve]
    public static void SetInt(string key, int value)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QG.StorageSetIntSync(key, value);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt(key, value);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static int GetInt(string key, int defaultValue = 0)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return HasKey(key) ? QG.StorageGetIntSync(key, defaultValue) : defaultValue;
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static void SetString(string key, string value)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QG.StorageSetStringSync(key, value);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetString(key, value);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static string GetString(string key, string defaultValue = "")
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return HasKey(key) ? QG.StorageGetStringSync(key, defaultValue) : defaultValue;
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static void SetFloat(string key, float value)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QG.StorageSetFloatSync(key, value);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetFloat(key, value);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static float GetFloat(string key, float defaultValue = 0)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return HasKey(key) ? QG.StorageGetFloatSync(key, defaultValue) : defaultValue;
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static void DeleteAll()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QG.StorageDeleteAllSync();
        }
        else
        {
            UnityEngine.PlayerPrefs.DeleteAll();
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static void DeleteKey(string key)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QG.StorageDeleteKeySync(key);
        }
        else
        {
            UnityEngine.PlayerPrefs.DeleteKey(key);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static bool HasKey(string key)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return QG.StorageHasKeySync(key);
        }
        else
        {
            return UnityEngine.PlayerPrefs.HasKey(key);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public static void Save()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
        }
        else
        {
            UnityEngine.PlayerPrefs.Save();
        }
    }
}