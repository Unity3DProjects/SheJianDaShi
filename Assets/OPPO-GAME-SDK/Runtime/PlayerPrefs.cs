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
            QG.StorageSetItem(key, value.ToString());
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
            return HasKey(key) && int.TryParse(QG.StorageGetItem(key), out var result) ? result : defaultValue;
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
            QG.StorageSetItem(key, value);
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
            var result = QG.StorageGetItem(key);
            return !string.IsNullOrEmpty(result) ? result : defaultValue;
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
            QG.StorageSetItem(key, value.ToString());
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
            return HasKey(key) && float.TryParse(QG.StorageGetItem(key), out var result) ? result : defaultValue;
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
            QG.StorageClear();
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
            QG.StorageRemoveItem(key);
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
            return !string.IsNullOrEmpty(QG.StorageGetItem(key));
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