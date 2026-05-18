using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD.Tools.OnEnable
{
    public class ShowSource : MonoBehaviour
    {
        /// <summary>
        /// 有些组件的逻辑在Start里,在场景加载的那一帧<see cref="OnEnable"/>里不调用广告
        /// </summary>
        private static readonly ISet<int> LoadedScene = new HashSet<int>();

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

        public AdSourceType type = AdSourceType.GameEnd;

        protected void CallAd()
        {
            ADManager.Instance.Show(type);
        }
    }
}