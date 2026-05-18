using System;
using System.Collections;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;

#if XiaoMi&&!UNITY_EDITOR
using mi;
using SystemInfo = mi.SystemInfo;
#endif

namespace AD.AdAdapter
{
    [UnityEngine.Scripting.Preserve]
    public class XiaoMiAdAdapter : BaseAdAdapter<ADXMParam>
    {
        public override bool HasTitle => true;

        protected override void OnInit(ADXMParam param)
        {
            Init_Internal();
        }

        /// <summary>
        /// 小米30秒内不能重复弹广告
        /// </summary>
        private bool canShowAD = true;

        public override void ShowWhiteAD()
        {
            if (!canShowAD)
                return;
            ADManager.Instance.StartCoroutine(CoolDownAD(30f, () => { canShowAD = false; },
                () => { canShowAD = true; }));
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
            if (!canShowAD)
                return;
            ADManager.Instance.StartCoroutine(CoolDownAD(30f, () => { canShowAD = false; },
                () => { canShowAD = true; }));
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
            TryShowRewardAD(receiverKey, id);
        }

        private bool TryShowBannerAD(string id = null)
        {
            id = id ?? Param?.bannerPosIdStr;
            if (!string.IsNullOrWhiteSpace(id) && BlackBagManager.Instance.NeedBannerAD)
            {
                ShowBannerAD_Internal(id);
                return true;
            }

            return false;
        }

        private bool TryShowCustomAD(string id = null)
        {
            id = id ?? Param?.customPosIdStr;
            if (!string.IsNullOrWhiteSpace(id) && BlackBagManager.Instance.NeedCustomAD)
            {
                ShowCustomAD_Internal(id);
                return true;
            }

            return false;
        }

        private bool TryShowRewardAD(string receiverKey, string id = null)
        {
            id = id ?? Param?.rewardPosIdStr;
            if (!string.IsNullOrWhiteSpace(id))
            {
                ShowRewardAD_Internal(receiverKey, id);
                return true;
            }

            return false;
        }

        public override void AddToDesktop()
        {
            this.Log("添加桌面");
            AddToDesktop_Internal();
        }

        public override void Quit()
        {
            $"尝试退出游戏".Log();
            Quit_Internal();
        }

        private IEnumerator CoolDownAD(float time, Action OnStart, Action OnEnd)
        {
            OnStart?.Invoke();
            yield return new WaitForSecondsRealtime(time);
            OnEnd?.Invoke();
        }

#if XiaoMi&&!UNITY_EDITOR
        private mi.SystemInfo systemInfo = new SystemInfo();

        int _BannerHeight = 150;

        private void Init_Internal()
        {
            MiBridge.Instance.Init();
            MiBridge.Instance.GetSystemInfo((isSucces, data) =>
            {
                $"小米适配器初始化".Log();
                if (isSucces)
                {
                    systemInfo = data;
                    MiBridge.Instance.QGLog("获取设备信息成功");
                }
                else
                {
                    MiBridge.Instance.QGLog("获取设备信息失败");
                }
            });
        }

        private void ShowBannerAD_Internal(string id)
        {
            MiBridge.Instance.CreateBannerAd(id,
                (int) (systemInfo.screenWidth * 0.3),
                0,
                (int) (systemInfo.screenWidth * 0.5),
                _BannerHeight,
                new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog($"创建Banner失败,code:{code}"); },

                    onAdClose = (isEnd) => { MiBridge.Instance.QGLog("c# ad close"); },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog("c# Banner ad load"); },
                }
            );

            MiBridge.Instance.ShowAd(Param.bannerPosIdStr);
            $"显示Banner".Log();
        }

        private void ShowCustomAD_Internal(string id)
        {
            ShowNativeAd(id);
        }
        

        private bool canShowReward = true;

        private void ShowRewardAD_Internal(string receiverKey, string id)
        {
            MiBridge.Instance.DestroyAd(id);
            MiBridge.Instance.CreateRewardedVideoAd(id, new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog("c# reward ad error"); },

                    onAdClose = (end) =>
                    {
                        string info = "success";
                        if (end)
                        {
                            //发奖励
                            if (canShowReward)
                            {
                                SendReward(receiverKey, true);
                                ADManager.Instance.StartCoroutine(CoolDownAD(1f, () => { canShowReward = false; },
                                    () => { canShowReward = true; }));
                            }
                        }
                        else
                        {
                            // end == true 说明激励视频播放完毕，是正常结束
                            info = "failure";
                            SendReward(receiverKey, false);
                        }

                        MiBridge.Instance.QGLog($"c# reward ad close end={info}");
                    },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog("c# reward ad load"); },
                }
            );
            MiBridge.Instance.LoadAd(id);
            MiBridge.Instance.ShowAd(id);
        }

        private void AddToDesktop_Internal()
        {
            MiBridge.Instance.HasShortcut(has =>
            {
                if (has)
                {
                    MiBridge.Instance.QGLog("has short cut");
                }
                else
                {
                    MiBridge.Instance.QGLog("has not short cut");
                    MiBridge.Instance.CreateShortcut("便于打开游戏",
                        (success, code, msg) =>
                        {
                            MiBridge.Instance.QGLog("create short cut {0}, {1}, {2}", success, code, msg);
                        });
                }
            });
        }

        private void Quit_Internal()
        {
            MiBridge.Instance.ExitApp((data) => { });
        }
        

        private void ShowNativeAd(string id)
        {
            MiBridge.Instance.CreateNativeAd(id,
                (int) (systemInfo.screenWidth * 0.3),
                (int) (systemInfo.screenHeight * 0.1),
                (int) (systemInfo.screenWidth * 0.5),
                (int) (systemInfo.screenHeight * 0.8),
                new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog("原生广告加载失败"); },

                    onAdClose = (isEnd) => { MiBridge.Instance.QGLog("c# ad close"); },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog($"c# 原生广告加载:{info}"); },
                }
            );

            MiBridge.Instance.LoadAd(id);
            MiBridge.Instance.ShowAd(id);
            $"显示原生".Log();
        }
#else
        private void Init_Internal()
        {
        }

        private void ShowBannerAD_Internal(string id)
        {
        }

        private void ShowCustomAD_Internal(string id)
        {
        }


        private void ShowRewardAD_Internal(string receiverKey, string id)
        {
            SendReward(receiverKey, true);
        }

        private void AddToDesktop_Internal()
        {
        }

        private void Quit_Internal()
        {
        }
#endif
    }
}