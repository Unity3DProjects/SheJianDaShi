using System;
using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.BlackBag;
using AD.Runtime.BlackBag.Operation;
using AD.Runtime.Tools.Tools;
using UnityEngine;
using System.Runtime.InteropServices;

namespace AD.Runtime.AdAdapter
{
    public class VivoAdAdapter : IAdAdapter
    {
        private ADNormalParam _adNormalParam;

        public void Init(IADParam normalParam)
        {
            if (normalParam is ADNormalParam adNormalParam)
            {
                _adNormalParam = adNormalParam;
                InitRewardInVivo(_adNormalParam.rewardPosIdStr);
                InitWindowResizeInVivo();
                if (!string.IsNullOrWhiteSpace(adNormalParam.boxId))
                {
                    CheckCanShowBox(adNormalParam.boxId);
                }
            }
            else
            {
                throw new Exception("AD参数初始化错误");
            }
        }

        public void ShowWhiteAD()
        {
            this.Log($"{nameof(VivoAdAdapter)}:尝试展示白包广告");
            ShowCustomAdInVivo(_adNormalParam.customPosIdStr, "", _adNormalParam.showDateStr,
                _adNormalParam.Orientation);
            if (BlackBagManager.Instance.CanShowAD)
            {
                ShowBannerAdInVivo(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
            }
        }

        public void ShowBlackAD()
        {
            this.Log($"{nameof(VivoAdAdapter)}:尝试展示黑包广告", ELogColor.Green);
            if (BlackBagManager.Instance.CanShowAD)
            {
                if (!string.IsNullOrEmpty(_adNormalParam.blackCustomPosIdStr))
                {
                    //黑包参数
                    ShowCustomAdInVivo(_adNormalParam.blackCustomPosIdStr, "", _adNormalParam.showDateStr,
                        _adNormalParam.Orientation);
                    if (!string.IsNullOrEmpty(_adNormalParam.bannerPosIdStr))
                    {
                        ShowBannerAdInVivo(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
                    }
                }
                else
                {
                    ShowCustomAdInVivo(_adNormalParam.customPosIdStr, "", _adNormalParam.showDateStr,
                        _adNormalParam.Orientation);
                    ShowBannerAdInVivo(_adNormalParam.bannerPosIdStr, _adNormalParam.showDateStr);
                }
            }
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            ShowRewardAdInVivo(callBackObjectName, callBackMethodName, param, _adNormalParam.rewardPosIdStr);
        }

        public void AddToDesktop()
        {
            CreateShortcutButtonInVivo();
        }

        public void Quit()
        {
            ExitApplicationInVivo();
        }


#if UNITY_EDITOR
        private static void ShowBannerAdInVivo(string posIdStr, string showDate)
        {
            ("调用banner").Log(ELogColor.Green);
        }

        private static void ShowCustomAdInVivo(string posIdStr, string leftPosIdStr,
            string showDate,
            int orientation)
        {
            ("调用原生").Log(ELogColor.Green);
        }

        private static void CreateShortcutButtonInVivo()
        {
            "添加到桌面".Log(ELogColor.Green);
        }

        /// <summary>
        /// 激励广告接口,在编辑器中立刻发放奖励
        /// </summary>
        /// <param name="callBackObjectName">挂载回调函数的物体在场景中的名称</param>
        /// <param name="callBackMethodName">需要调用的回调函数方法名</param>
        /// <param name="callBackParam">回调函数参数,仅限字符串</param>
        /// <param name="posIdStr">激励广告ID</param>
        private static void ShowRewardAdInVivo(string callBackObjectName, string callBackMethodName,
            string callBackParam,
            string posIdStr)
        {
            ("调用激励").Log(ELogColor.Green);
            GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
        }

        private static void InitRewardInVivo(string posIdStr)
        {
            // Debug.Log("初始化激励");
        }

        private static void CheckCanShowBox(string posIdStr)
        {
            LuZong.CanShowBox = true;
        }

        private static void ExitApplicationInVivo()
        {
            ("Quit").Log(ELogColor.Green);
            Application.Quit();
        }

        private static void InitWindowResizeInVivo()
        {
        }

#else
    [DllImport("__Internal")]
    public static extern void ShowBannerAdInVivo(string posIdStr, string showDate);

    [DllImport("__Internal")]
    public static extern void HideBannerAdInVivo();

    [DllImport("__Internal")]
    public static extern void ShowCustomAdInVivo(string posIdStr, string leftPosIdStr, string showDate, int orientation);

    [DllImport("__Internal")]
    public static extern void HideCustomAdInVivo();

    [DllImport("__Internal")]
    public static extern void CreateShortcutButtonInVivo();

    [DllImport("__Internal")]
    public static extern void ShowRewardAdInVivo(string callBackObjectName, string callBackMethodName, string callBackParam,
        string posIdStr);

    [DllImport("__Internal")]
    public static extern void InitRewardInVivo(string posIdStr); 

    [DllImport("__Internal")]
    public static extern void CheckCanShowBox(string posIdStr);


    [DllImport("__Internal")]
    public static extern void ExitApplicationInVivo();

    [DllImport("__Internal")]
    public static extern void InitWindowResizeInVivo();
#endif
    }
}