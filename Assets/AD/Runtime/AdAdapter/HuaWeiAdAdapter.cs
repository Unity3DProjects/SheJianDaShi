using System;
using AD.BlackBag;
using AD.Decorator;
using AD.Instance;
using AD.Tools;
using HWWASM;
using UnityEngine;

namespace AD.AdAdapter
{
    /// <summary>
    /// 不支持自定义广告id
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class HuaWeiAdAdapter : BaseAdAdapter<ADHuaWeiParam>
    {
        protected override void OnInit(ADHuaWeiParam param)
        {
        }

        public override void Login(Action onSuccess, Action onFailure)
        {
            this.Log("华为账号登录");
#if UNITY_EDITOR
            base.Login(onSuccess, onFailure);
#else
            GameLoginOption option = new GameLoginOption();
            option.appid = Param.appid;
            this.Log($"华为id:{Param.appid}");
            option.forceLogin = 1;
            option.success += s =>
            {
                HWSDK.Instance.OnLoginSuccess(Param);
                onSuccess?.Invoke();
                ADUtils.SendMsgToADManager(ADManagerConst.LoginKey, true.ToString());
            };
            option.fail += s =>
            {
                Debug.LogError("华为登录失败错误码:" + s.code + "错误信息" + s.data);
                HWSDK.Instance.OnLoginFail();
                onFailure?.Invoke();
                ADUtils.SendMsgToADManager(ADManagerConst.LoginKey, false.ToString());
            };
            QG.GameLogin(option);
#endif
        }

        public override void ShowWhiteAD()
        {
            this.Log($"{GetType().Name}:尝试展示白包广告");
            if (BlackBagManager.Instance.CanShowWhiteAD)
            {
                TryShowCustomAD();

                if (BlackBagManager.Instance.CanShowBlackAD)
                    TryShowBannerAD();
            }
        }

        public override void ShowBlackAD()
        {
            var canShowBlackAD = BlackBagManager.Instance.CanShowBlackAD;
            this.Log($"{GetType().Name}:尝试展示黑包广告 {canShowBlackAD}", ELogColor.Green);
            if (canShowBlackAD)
            {
                TryShowBannerAD();
                TryShowCustomAD();
            }
        }

        public override void ShowBannerAD(string id = null)
        {
            TryShowBannerAD(id);
        }

        public override void ShowCustomAD(string id = null)
        {
            TryShowCustomAD(id);
        }

        public override void ShowRewardAD(string receiverKey, string id = null)
        {
            this.Log($"{GetType().Name}:尝试展示激励广告");
            TryShowRewardAD(receiverKey, id);
        }


        private bool TryShowBannerAD(string id = null)
        {
            id = id ?? Param?.BannerId;
            if (!string.IsNullOrWhiteSpace(id) && BlackBagManager.Instance.NeedBannerAD)
            {
                ShowBannerAD_Internal(id);
                return true;
            }

            return false;
        }

        private bool TryShowCustomAD(string id = null)
        {
            id = id ?? Param?.CustomId;
            if (!string.IsNullOrWhiteSpace(id) && BlackBagManager.Instance.NeedCustomAD)
            {
                ShowCustomAD_Internal(id);
                return true;
            }

            return false;
        }

        private bool TryShowRewardAD(string receiverKey, string id = null)
        {
            id = id ?? Param?.RewardId;
            if (!string.IsNullOrWhiteSpace(id))
            {
                ShowRewardAD_Internal(receiverKey, id);
                return true;
            }

            return false;
        }

        private void ShowBannerAD_Internal(string id)
        {
        }

        private void ShowCustomAD_Internal(string id)
        {
            if (Param.Pass(nameof(Param.customPosID)))
            {
                HWSDK.Instance.ShowNativeAd();
            }
        }


        private void ShowRewardAD_Internal(string receiverKey, string id)
        {
            HWSDK.ShowRewardAd(Param.RewardId, receiverKey);
        }

        public override void AddToDesktop()
        {
        }

        public override void Quit()
        {
            HWSDK.Instance.ExitApp();
        }
    }
}