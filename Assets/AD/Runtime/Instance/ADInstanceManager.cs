using AD.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD.Instance
{
    internal class ADInstanceManager : SingletonMonoBase<ADInstanceManager>, IADInit
    {
        [Header("是否跳转下一场景")] [SerializeField] private bool isAutoLoadNextScene = true;

        private AsyncOperation sceneLoadOp;
        private bool tempLoadNext;

        public void Init()
        {
            tempLoadNext = isAutoLoadNextScene;
            HealthManager.Instance.OnHealthStart += () =>
            {
                if (tempLoadNext)
                {
                    sceneLoadOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                    sceneLoadOp.allowSceneActivation = false;
                }
            };
            HealthManager.Instance.OnHealthEnd += () =>
            {
                ADManager.Instance.BindReceiver(ADManagerConst.LoginKey, OnLogin);
                ADManager.Instance.Login();
            };
            PrivacyManager.Instance.OnAgree += HealthManager.Instance.ShowHealthPanel;
            PrivacyManager.Instance.Init();
            HealthManager.Instance.Init();
        }

        private void OnLogin(string msg)
        {
            if (bool.TryParse(msg, out var result) && result)
            {
                ADManager.Instance.RemoveReceiver(ADManagerConst.LoginKey, OnLogin);
                if (tempLoadNext)
                {
                    sceneLoadOp.allowSceneActivation = true;
                }

                ADManager.Instance.CreateShortcutButtonBlack();
            }
        }
    }
}