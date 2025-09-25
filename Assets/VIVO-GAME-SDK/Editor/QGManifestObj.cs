using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace QGMiniGame
{
    [Serializable]
    public class SubpackagesItem
    {
        public string name;
        public string root;
    }

    [Serializable]
    public class NetworkTimeout
    {
        public int request;
        public int connectSocket;
        public int uploadFile;
        public int downloadFile;
    }

    [Serializable]
    public class Config
    {
        public string logLevel;
    }

    [Serializable]
    public class ManifesJSONObj
    {
        public string package;
        public string name;
        public string icon;
        public string versionName;
        public string versionCode;
        public string minPlatformVersion;
        public string deviceOrientation;
        public string type;
        public List<SubpackagesItem> subpackages;
        public string isUnity;
        public NetworkTimeout networkTimeout;
        public Config config;
        public string buildType;

        public static ManifesJSONObj CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ManifesJSONObj>(jsonString);
        }


        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class EnvConfJSONObj
    {
        public string icon;
        public string package;
        public string name;
        public string versionName;
        public string versionCode;
        public string minPlatformVersion;
        public string deviceOrientation;
        public string bgImageSrc;
        public string wasmUrl;
        public string streamingAssetsUrl;
        public string preloadUrl;
        public bool subUnityPkg;


        public static EnvConfJSONObj CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<EnvConfJSONObj>(jsonString);
        }


        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }


}