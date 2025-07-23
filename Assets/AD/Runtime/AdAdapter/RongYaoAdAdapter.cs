using System;
using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.BlackBag;
using AD.Runtime.BlackBag.Shield;
using AD.Runtime.Tools.Tools;
using UnityEngine;

#if RongYao
using QGMiniGame;
#endif

namespace AD.Runtime.AdAdapter
{
    public class RongYaoAdAdapter : IAdAdapter
    {
        private ADNormalParam _adNormalParam;

#if RongYao && !UNITY_EDITOR
        private QGSystemInfo systemInfo;
        private QGBannerAd qgBannerAd;
        private QGCustomAd qgCustomAd;
        private QGRewardedVideoAd qgRewardedVideoAd;
        private QGInterstitialAd qgInterstitialAd;

        public void Init(IADParam normalParam)
        {
            if (normalParam is ADNormalParam adNormalParam)
            {
                _adNormalParam = adNormalParam;
                systemInfo = QG.GetSystemInfoSync();
                Debug.Log($"设备宽度:{systemInfo.screenWidth}");
                Debug.Log($"设备高度:{systemInfo.screenHeight}");
                qgRewardedVideoAd = CreateRewardViedeoAd(_adNormalParam.rewardPosIdStr);
            }
            else
            {
                throw new Exception("AD参数初始化错误");
            }
        }

        public void ShowWhiteAD()
        {
            this.Log("尝试调用白包");
            if(!TimeCheck.TimeReach)return;
            ShowCustomAD();
            // CreateInterstitialAd(_adNormalParam.blackCustomPosIdStr);
            if (!BlackBagManager.Instance.CanShowAD) return;
            CreateBanner(_adNormalParam.bannerPosIdStr);
        }

        public void ShowBlackAD()
        {
            this.Log("尝试调用黑包");

            if (!BlackBagManager.Instance.CanShowAD) return;
            CreateBanner(_adNormalParam.bannerPosIdStr);
            ShowCustomAD();
            // CreateInterstitialAd(_adNormalParam.blackCustomPosIdStr);
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            this.Log("激励广告");
#if RongYao && !UNITY_EDITOR
            qgRewardedVideoAd.onCloseRewardedVideoAction = respons =>
            {
                if (respons.isEnded)
                {
                    "播放完毕，发送奖励".Log();
                    GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, param);
                }
            };
            qgRewardedVideoAd.Show();
#else
            GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, param);
#endif
        }

        public void AddToDesktop()
        {
            this.Log("添加桌面");

            QG.InstallShortcut(
                "",
                (msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
                (msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
            );
        }

        public void Quit()
        {
            this.Log("退出");

            QG.ExitApplication();
        }

        private void ShowCustomAD()
        {
            try
            {
                if (_adNormalParam.Orientation == 0)
                {
                    //横屏
                    CreateCustomAd(_adNormalParam.customPosIdStr,
                        (int)((systemInfo.screenWidth - 1480) / 2),
                        0
                    );
                }
                else if (_adNormalParam.Orientation == 1)
                {
                    //竖屏
                    CreateCustomAd(_adNormalParam.customPosIdStr,
                        0,
                        (int)(systemInfo.screenHeight * 0.5)
                    );
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"抛出:{e.Message}");
            }
        }


        private void CreateBanner(string id, int left = 0, int top = 0)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            try
            {
                // if (qgBannerAd != null)
                // {
                //     if (QGMiniGameManager.Instance.IsShow(qgBannerAd.adId))
                //     {
                //         qgBannerAd.Hide();
                //     }
                //     else
                //     {
                //         Debug.Log("当前广告没有显示");
                //     }
                // }

                qgBannerAd = QG.CreateBannerAd(
                    new QGCreateBannerAdParam()
                    {
                        adUnitId = id,
                        style = new QGMiniGame.Style()
                        {
                            left = left,
                            top = top
                        }
                    }
                );
                qgBannerAd.OnLoad(() => { Debug.Log("加载Banner"); });
                qgBannerAd?.Show(s => { Debug.Log("banner展示成功"); }
                    , s => { Debug.Log($"banner展示失败:{s.errMsg}"); });
            }
            catch (Exception e)
            {
                Debug.LogError($"抛出:{e.Message}");
            }
        }

        private void CreateInterstitialAd(string id)
        {
            if (qgInterstitialAd != null)
            {
                qgInterstitialAd.Hide();
                qgInterstitialAd.Destroy();
            }

            qgInterstitialAd = QG.CreateInterstitialAd(
                new QGCommonAdParam()
                {
                    adUnitId = id
                }
            );
            qgInterstitialAd.Show(s => { Debug.Log("InterstitialAd展示成功"); }
                , s => { Debug.Log($"InterstitialAd展示失败:{s.errMsg}"); });
        }


        private void CreateCustomAd(string id, int left = 0, int top = 0)
        {
            if (qgCustomAd != null)
            {
                if (qgCustomAd.IsShow())
                {
                    qgCustomAd.Hide();
                }
                else
                {
                    Debug.Log("当前广告没有显示");
                }
            }

            qgCustomAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
            {
                adUnitId = id,
                style = new QGMiniGame.Style()
                {
                    left = left,
                    top = top
                }
            });
            qgCustomAd.Show(s => { Debug.Log("CustomAd展示成功"); }
                , s => { Debug.Log($"CustomAd展示失败:{s.errMsg}"); });
        }

        private QGRewardedVideoAd CreateRewardViedeoAd(string id)
        {
            return QG.CreateRewardedVideoAd(
                new QGCommonAdParam()
                {
                    adUnitId = id
                }
            );
        }
#else
        public void Init(IADParam normalParam)
        {
        }

        public void ShowWhiteAD()
        {
            this.Log("尝试调用白包");
        }

        public void ShowBlackAD()
        {
            this.Log("尝试调用黑包");
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            this.Log("激励");
            GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, param);
        }

        public void AddToDesktop()
        {
            this.Log("添加桌面");
        }

        public void Quit()
        {
            this.Log("退出");
        }
#endif
    }

}