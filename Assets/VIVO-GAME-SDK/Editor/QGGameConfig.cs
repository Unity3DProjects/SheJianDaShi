using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace QGMiniGame
{
  
    [Serializable]
    public class RunCmdConfig
    {
        public string command;
        public string workSrc;
        public bool startTerminal;
    }

    [Serializable]
    public class UpdateResonse
    {
        [SerializeField] public UpdateConfig data;
    }

    [Serializable]
    public class UpdateConfig
    {
        public string pluginUrl = "";
        public int version;
    }

    [Serializable]
    public class EnvConfig
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
    }

    [Serializable]
    public class WebGlConfig
    {
        public string companyName = "DefaultCompany";
        public string productName = "minigame-package";
        public string productVersion = "0.1";
        public string dataUrl = "webgl.data.unityweb";
        public string wasmCodeUrl = "webgl.wasm.code.unityweb";
        public string wasmFrameworkUrl = "webgl.wasm.framework.unityweb";
        public string[] graphicsAPI = { "WebGL 1.0" };
        public WebglContext webglContextAttributes = new WebglContext();
        public string splashScreenStyle = "Dark";
        public string backgroundColor = "#231F20";
        public bool developmentBuild = false;
        public bool multithreading = false;
        public string unityVersion = Application.unityVersion;
        public int unityPluginVersion = QG.SDK_VERSION;
    }

    [Serializable]
    public class WebglContext
    {
        public bool preserveDrawingBuffer = false;
    }

    [Serializable]
    public class RenameFile
    {
        public string oldName;
        public string newName;
    }

    [Serializable]
    public class QGGameConfig : ScriptableObject
    {
        public EnvConfig envConfig;
        public string buildSrc = "";
        public string cliSrc = "";
        public bool useSelfLoading;
        public bool useSubPkgLoading;
        public bool useAddressable;
        public bool usePreAsset;
        public bool useWebgl2;
        public bool useCodeSize;
        public int usTargetBgType;

        public static string webglDir = "webgl";       //构建的webgl文件夹
        public static string vivoWebglDir = "webgl_vivo";

        internal string GetSubWasmUrl()
        {
            return useSelfLoading ? envConfig.wasmUrl : "";
        }

        internal string GetWebGlPath()
        {
            return Path.Combine(buildSrc, webglDir);
        }

        internal string GetVivoWebGlPath()
        {
            return Path.Combine(buildSrc, vivoWebglDir);
        }

        internal string GetPreloadUrl()
        {
            string preloadUrl = "";
            if (usePreAsset)
            {
                string[] result = envConfig.preloadUrl.Split(';');
                for (int i = 0; i < Math.Min(5, result.Length); i++)
                {
                    preloadUrl += result[i] + ";";
                }
            }
            return preloadUrl;
        }

        internal string GetAbsBgImagePath()
        {
            string bgImageSrc = envConfig.bgImageSrc.Replace("Assets/", "");
            return Path.Combine(Application.dataPath, bgImageSrc);
        }

    }

    enum SubWasmPkgType
    {
        NONE = 0,
        CDN = 1,
        VIVO = 2
    }

    enum Orientation
    {
        Portrait = 0,
        Landscape = 1
    }
}



