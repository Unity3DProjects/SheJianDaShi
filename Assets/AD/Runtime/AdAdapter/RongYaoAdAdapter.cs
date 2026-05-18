using System;
using AD.Decorator;
using AD.Tools;
using UnityEngine;
#if RongYao && !UNITY_EDITOR
using QGMiniGame;

#endif

namespace AD.AdAdapter
{
    [UnityEngine.Scripting.Preserve]
    public class RongYaoAdAdapter : ADNormalParamAdapter
    {
        protected override void OnInit(ADNormalParam param)
        {
            InitInRangYao();
        }

        protected override void ShowBannerAD_Internal(string id)
        {
            ShowBannerAdInRangYao(id);
        }

        protected override void ShowCustomAD_Internal(string id)
        {
            ShowCustomAdInRangYao(id);
        }

        protected override void ShowInterstitialAD_Internal(string id)
        {
            ShowInterstitialAdInRangYao(id);
        }

        protected override void ShowRewardAD_Internal(string id, string receiverKey)
        {
            ShowRewardAdInRangYao(id, receiverKey);
        }

        public override void AddToDesktop()
        {
            this.Log("添加桌面");
            AddToDesktopInRangYao();
        }

        public override void Quit()
        {
            this.Log("退出");
            QuitInRangYao();
        }


#if RongYao && !UNITY_EDITOR
        private QGSystemInfo systemInfo;
        private QGBannerAd qgBannerAd;
        private QGCustomAd qgCustomAd;
        private QGRewardedVideoAd qgRewardedVideoAd;
        private QGInterstitialAd qgInterstitialAd;

        private void InitInRangYao()
        {
            systemInfo = QG.GetSystemInfoSync();
            qgRewardedVideoAd = CreateRewardViedeoAd(Param.rewardPosIdStr);
            Debug.Log($"设备宽度:{systemInfo.screenWidth}");
            Debug.Log($"设备高度:{systemInfo.screenHeight}");
        }

        private void ShowBannerAdInRangYao(string id)
        {
            CreateBanner(id);
        }

        private void ShowCustomAdInRangYao(string id)
        {
            try
            {
                if (Param.Orientation == 0)
                {
                    //横屏
                    CreateCustomAd(id,
                        (int) ((systemInfo.screenWidth - 1480) / 2),
                        0
                    );
                }
                else if (Param.Orientation == 1)
                {
                    //竖屏
                    CreateCustomAd(id,
                        0,
                        (int) (systemInfo.screenHeight * 0.5)
                    );
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"抛出:{e.Message}");
            }
        }

        private void ShowInterstitialAdInRangYao(string id)
        {
            CreateInterstitialAd(id);
        }

        private void ShowRewardAdInRangYao(string id, string receiverKey)
        {
            qgRewardedVideoAd.onCloseRewardedVideoAction = respons =>
            {
                if (respons.isEnded)
                {
                    "播放完毕，发送奖励".Log();
                    SendReward(receiverKey, true);
                }
                else
                {
                    SendReward(receiverKey, false);
                }
            };
            qgRewardedVideoAd.Show();
        }

        private void AddToDesktopInRangYao()
        {
            QG.InstallShortcut(
                "",
                (msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
                (msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
            );
        }

        private void QuitInRangYao()
        {
            QG.ExitApplication();
        }

        private void CreateBanner(string id, int left = 0, int top = 0)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            try
            {
                if (qgBannerAd != null)
                {
                    qgBannerAd.Destroy();
                }

                qgBannerAd = QG.CreateBannerAd(
                    new QGCreateBannerAdParam()
                    {
                        adUnitId = id,
                        style = new Style()
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
            this.Log("调用原生");
            if (qgCustomAd != null)
            {
                qgCustomAd.Destroy();
                this.Log("销毁上一次广告");
            }

            qgCustomAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
            {
                adUnitId = id,
                style = new Style()
                {
                    left = left,
                    top = top
                }
            });
            this.Log("创建原生广告");
            qgCustomAd.Show(s => { Debug.Log("CustomAd展示成功"); }
                , s => { Debug.Log($"CustomAd展示失败:{s.errMsg}"); });
            this.Log("显示原生广告");
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
        private void InitInRangYao()
        {
        }

        private void ShowBannerAdInRangYao(string id)
        {
        }

        private void ShowCustomAdInRangYao(string id)
        {
        }

        private void ShowInterstitialAdInRangYao(string id)
        {
        }

        private void ShowRewardAdInRangYao(string id, string receiverKey)
        {
            SendReward(receiverKey, true);
        }

        private void AddToDesktopInRangYao()
        {
        }

        private void QuitInRangYao()
        {
        }
#endif
    }
}