using System;
using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.BlackBag;
using AD.Runtime.Tools.Tools;
using UnityEngine;
#if XiaoMi
using System.Collections;
using mi;
using SystemInfo = mi.SystemInfo;
#endif

namespace AD.Runtime.AdAdapter
{
    public class XiaoMiAdAdapter : IAdAdapter
    {
#if XiaoMi
        private ADNormalParam adParam;

        private mi.SystemInfo systemInfo = new SystemInfo();

        int _BannerHeight = 150;

        /// <summary>
        /// 小米30秒内不能重复弹广告
        /// </summary>
        private bool canShowAD = true;

        public void Init(IADParam param)
        {
            if (param is ADNormalParam adNormalParam)
            {
                MiBridge.Instance.Init();
                adParam = adNormalParam;
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
            else
            {
                throw new Exception("AD参数初始化错误");
            }
        }

        private IEnumerator CoolDownAD(float time, Action OnStart, Action OnEnd)
        {
            OnStart?.Invoke();
            yield return new WaitForSecondsRealtime(time);
            OnEnd?.Invoke();
        }

        public void ShowWhiteAD()
        {
            $"尝试显示白包".Log();
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!canShowAD)
                return;
#endif
            ShowBanner();
            ADManager.Instance.StartCoroutine(CoolDownAD(30f, () => { canShowAD = false; },
                () => { canShowAD = true; }));
            if (BlackBagManager.Instance.CanShowAD)
            {
                ShowNativeAd();
            }
        }

        public void ShowBlackAD()
        {
            $"尝试显示黑包".Log();
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!canShowAD)
                return;
#endif
            if (!BlackBagManager.Instance.CanShowAD) return;
            ADManager.Instance.StartCoroutine(CoolDownAD(30f, () => { canShowAD = false; },
                () => { canShowAD = true; }));
            ShowNativeAd();
            ShowBanner();
        }

        private bool canShowReward = true;

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            $"尝试展示激励".Log();
#if UNITY_EDITOR
            GameObject.Find(callBackObjectName)?.SendMessage(callBackMethodName, param);
#endif
            MiBridge.Instance.DestroyAd(adParam.rewardPosIdStr);
            MiBridge.Instance.CreateRewardedVideoAd(adParam.rewardPosIdStr, new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog("c# reward ad error"); },

                    onAdClose = (end) =>
                    {
                        string info = "success";
                        if (!end)
                        {
                            // end == true 说明激励视频播放完毕，是正常结束
                            info = "failure";
                        }
                        else
                        {
                            //发奖励
                            if (canShowReward)
                            {
                                MiBridge.Instance.SendMessage(callBackObjectName, callBackMethodName, param);
                                ADManager.Instance.StartCoroutine(CoolDownAD(1f, () => { canShowReward = false; },
                                    () => { canShowReward = true; }));
                            }
                        }

                        MiBridge.Instance.QGLog($"c# reward ad close end={info}");
                    },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog("c# reward ad load"); },
                }
            );
            MiBridge.Instance.LoadAd(adParam.rewardPosIdStr);
            MiBridge.Instance.ShowAd(adParam.rewardPosIdStr);
        }

        public void AddToDesktop()
        {
            $"尝试添加桌面".Log();
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

        public void Quit()
        {
            $"尝试退出游戏".Log();
            MiBridge.Instance.ExitApp((data) => { });
        }

        private void ShowBanner()
        {
            MiBridge.Instance.CreateBannerAd(adParam.bannerPosIdStr,
                (int)(systemInfo.screenWidth * 0.3),
                0,
                (int)(systemInfo.screenWidth * 0.5),
                _BannerHeight,
                new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog($"创建Banner失败,code:{code}"); },

                    onAdClose = (isEnd) => { MiBridge.Instance.QGLog("c# ad close"); },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog("c# Banner ad load"); },
                }
            );

            MiBridge.Instance.ShowAd(adParam.bannerPosIdStr);
            $"显示Banner".Log();
        }

        private void ShowNativeAd()
        {
            MiBridge.Instance.CreateNativeAd(adParam.customPosIdStr,
                (int)(systemInfo.screenWidth * 0.3),
                (int)(systemInfo.screenHeight * 0.1),
                (int)(systemInfo.screenWidth * 0.5),
                (int)(systemInfo.screenHeight * 0.8),
                new AdEventListener
                {
                    onAdError = (code, msg) => { MiBridge.Instance.QGLog("原生广告加载失败"); },

                    onAdClose = (isEnd) => { MiBridge.Instance.QGLog("c# ad close"); },

                    onAdLoad = (info) => { MiBridge.Instance.QGLog($"c# 原生广告加载:{info}"); },
                }
            );

            MiBridge.Instance.LoadAd(adParam.customPosIdStr);
            MiBridge.Instance.ShowAd(adParam.customPosIdStr);
            $"显示原生".Log();
        }
#else
        public void Init(IADParam normalParam)
        {
        }

        public void ShowWhiteAD()
        {
        }

        public void ShowBlackAD()
        {
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
        }

        public void AddToDesktop()
        {
        }

        public void Quit()
        {
        }
#endif
    }
}