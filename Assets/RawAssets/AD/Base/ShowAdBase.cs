using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD.Tools
{
    public abstract class ShowAdBase : MonoBehaviour
    {
        /// <summary>
        /// 保证通过OnEnable调广告的方法在场景加载后的Start后才能调用
        /// </summary>
        private static readonly ISet<int> LoadedScene = new HashSet<int>(){-1};

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneLoaded += (s, m) => LoadedScene.Add(s.buildIndex);
            SceneManager.sceneUnloaded += s => LoadedScene.Remove(s.buildIndex);
        }

        protected void OnEnable()
        {
            if (!LoadedScene.Contains(gameObject.scene.buildIndex)) return;
            CallAd();
        }

        protected abstract void CallAd();
    }
}