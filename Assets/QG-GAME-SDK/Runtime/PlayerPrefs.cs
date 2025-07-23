using QGMiniGame;
using UnityEngine;

//覆盖unity的PlayerPrefs
public static class PlayerPrefs
{
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
    public static int GetInt(string key, int defaultValue = 0)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return QG.StorageGetIntSync(key, defaultValue);
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
        }
        
    }
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
    public static string GetString(string key, string defaultValue = "")
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return QG.StorageGetStringSync(key, defaultValue);
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
        }
    }
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
    public static float GetFloat(string key, float defaultValue = 0)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return QG.StorageGetFloatSync(key, defaultValue);
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
        }
    }
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
    public static void Save() {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
           
        }
        else
        {
            UnityEngine.PlayerPrefs.Save();
        }
    }
}
