using System;
using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.BlackBag;
using AD.Runtime.Tools.Tools;
using UnityEngine;
using System.Runtime.InteropServices;

namespace AD.Runtime.AdAdapter
{
    public class OppoAdAdapter : IAdAdapter
    {
        private ADNormalParam _adNormalParam;

        public void Init(IADParam normalParam)
        {
            if (normalParam is ADNormalParam adNormalParam)
            {
                _adNormalParam = adNormalParam;
                InitRewardAdInOPPO(_adNormalParam.rewardPosIdStr);
            }
            else
            {
                throw new Exception("AD参数初始化错误");
            }
        }

        public void ShowWhiteAD()
        {
            this.Log($"{nameof(OppoAdAdapter)}:尝试展示白包广告");
            ShowCustomAdInOPPO(_adNormalParam.customPosIdStr, _adNormalParam.showDateStr, _adNormalParam.Orientation);
            if (BlackBagManager.Instance.CanShowAD)
            {
                if (string.IsNullOrEmpty(_adNormalParam.bannerPosIdStr))
                    return;
                ShowBannerAdInOPPO(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
            }
        }

        public void ShowBlackAD()
        {
            this.Log($"{nameof(OppoAdAdapter)}:尝试展示黑包广告", ELogColor.Green);
            if (BlackBagManager.Instance.CanShowAD)
            {
                if (!string.IsNullOrEmpty(_adNormalParam.blackCustomPosIdStr))
                {
                    //黑包参数
                    ShowCustomAdInOPPO(_adNormalParam.blackCustomPosIdStr, _adNormalParam.showDateStr,
                        _adNormalParam.Orientation);
                    if (!string.IsNullOrEmpty(_adNormalParam.bannerPosIdStr))
                    {
                        ShowBannerAdInOPPO(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
                    }
                }
                else
                {
                    ShowCustomAdInOPPO(_adNormalParam.customPosIdStr, _adNormalParam.showDateStr, _adNormalParam.Orientation);
                    if (!string.IsNullOrEmpty(_adNormalParam.bannerPosIdStr))
                    {
                        ShowBannerAdInOPPO(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
                    }
                }
            }
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            ShowRewardAdInOPPO(callBackObjectName, callBackMethodName, param);
        }


        public void AddToDesktop()
        {
            CreateShortcutButton();
        }

        public void Quit()
        {
            ExitApplicationInOPPO();
        }

#if UNITY_EDITOR

        /// <summary>
        /// 初始化激励广告,主要用来提前load一个视频备用,避免首次展示激励时提示无可用广告
        /// 该函数不需要手动调用,已在构造函数中自动调用
        /// </summary>
        /// <param name="adUnitIdStr">激励广告id</param>
        private static void InitRewardAdInOPPO(string adUnitIdStr)
        {
            // Debug.Log("初始化激励");
        }

        /// <summary>
        /// 展示banner
        /// </summary>
        /// <param name="bannerAdUnitIdStr">banner广告id</param>
        /// <param name="showDate">屏蔽时间</param>
        private static void ShowBannerAdInOPPO(string bannerAdUnitIdStr, string showDate)
        {
            ("调用banner").Log(ELogColor.Green);
        }

        /// <summary>
        /// 展示模板原生,并在原生关闭时自动展示banner
        /// </summary>
        /// <param name="adUnitIdStr">模板原生广告id</param>
        /// <param name="showDate">屏蔽时间</param>
        /// <param name="orientation">横竖屏</param>
        private static void ShowCustomAdInOPPO(string adUnitIdStr, string showDate,
            int orientation)
        {
            ("调用原生").Log(ELogColor.Green);
        }

        /// <summary>
        /// 展示激励广告
        /// </summary>
        /// <param name="callBackObjectName">回调函数脚本所在的gameobject名</param>
        /// <param name="callBackMethodName">回调函数名</param>
        /// <param name="callBackParam">回调函数参数</param>
        private static void ShowRewardAdInOPPO(string callBackObjectName, string callBackMethodName,
            string callBackParam = null)
        {
            ("调用激励").Log(ELogColor.Green);
            GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
        }

        private static void ExitApplicationInOPPO()
        {
            ("Quit").Log(ELogColor.Green);
            Application.Quit();
        }

        private static void CreateShortcutButton()
        {
            ("添加到桌面").Log(ELogColor.Green);
        }

        public static void IsArea(string GameObjectName, string MethodName)
        {
            // (
            //     $"使用地区屏蔽请求--接受信息游戏物体名字：{GameObjectName}----接受信息方法名字：{MethodName}｝").Log();
        }

        public static void ZDTX(string BaoMing, string QuDaoHao)
        {
            // Debug.Log($"传输的包名：{BaoMing},传输的渠道号：{QuDaoHao}");
        }

#else
    [DllImport("__Internal")]
    public static extern void ShowCustomAdInOPPO(string adUnitIdStr , string showDateStr , int orientation );

    // [DllImport("__Internal")]
    // public static extern void ShowCustomAdZoneInOPPO(string adUnitIdStr = customAdUnitIdStr_2, string showDateStr =
    //     showDateStr, int orientation = Orientation);

    [DllImport("__Internal")]
    public static extern void ShowBannerAdInOPPO(string bannerAdUnitIdStr , string showDateStr );

    [DllImport("__Internal")]
    public static extern void InitRewardAdInOPPO(string adUnitIdStr);

    [DllImport("__Internal")]
    public static extern void ShowRewardAdInOPPO(string callBackObjectName, string callBackMethodName, string callBackParam =
        null);

    [DllImport("__Internal")]
    public static extern void IsArea(string GameObjectName, string MethodName);

    [DllImport("__Internal")]
    public static extern void ZDTX(string BaoMing, string QuDiaoHao);

    [DllImport("__Internal")]
    public static extern void ExitApplicationInOPPO();

    [DllImport("__Internal")]
    public static extern void CreateShortcutButton();
#endif
    }
}