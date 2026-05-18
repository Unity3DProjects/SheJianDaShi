using System.Runtime.InteropServices;
using AD.Tools;
using UnityEngine;

namespace AD.AdAdapter
{
    [UnityEngine.Scripting.Preserve]
    public class OppoAdAdapter : ADNormalParamAdapter
    {
        protected override void OnInit(ADNormalParam param)
        {
            InitRewardAdInOPPO(Param.rewardPosIdStr);
        }

        protected override void ShowBannerAD_Internal(string id)
        {
            ShowBannerAdInOPPO(id);
        }

        protected override void ShowCustomAD_Internal(string id)
        {
            ShowCustomAdInOPPO(id, Param.Orientation);
        }

        protected override void ShowInterstitialAD_Internal(string id)
        {
            ShowCustomAdInOPPO(id, Param.Orientation);
        }

        protected override void ShowRewardAD_Internal(string id, string receiverKey)
        {
            ADManager.Instance.BindReceiver(receiverKey, msg => { ADManager.Instance.Show(AdSourceType.Callback); });
            ShowRewardAdInOPPO(id, receiverKey);
        }


        public override void AddToDesktop()
        {
            CreateShortcutButton();
        }

        public override void Quit()
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
        private static void ShowBannerAdInOPPO(string bannerAdUnitIdStr)
        {
            ("调用banner").Log(ELogColor.Green);
        }

        /// <summary>
        /// 展示模板原生,并在原生关闭时自动展示banner
        /// </summary>
        /// <param name="adUnitIdStr">模板原生广告id</param>
        /// <param name="showDate">屏蔽时间</param>
        /// <param name="orientation">横竖屏</param>
        private static void ShowCustomAdInOPPO(string adUnitIdStr, int orientation)
        {
            ("调用原生").Log(ELogColor.Green);
        }

        /// <summary>
        /// 展示激励广告
        /// </summary>
        private static void ShowRewardAdInOPPO(string posIdStr, string receiverKey)
        {
            ("调用激励").Log(ELogColor.Green);
            SendReward(receiverKey, true);
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

        public static void IsArea(string receiverKey)
        {
            ADUtils.SendMsgToADManager(receiverKey, "HaveAd");
        }

        public static void ZDTX(string BaoMing, string QuDaoHao, string receiverKey)
        {
            ADUtils.SendMsgToADManager(receiverKey, "2");
        }

#else
        [DllImport("__Internal")]
        public static extern void ShowCustomAdInOPPO(string adUnitIdStr, int orientation);

        // [DllImport("__Internal")]
        // public static extern void ShowCustomAdZoneInOPPO(string adUnitIdStr = customAdUnitIdStr_2 =
        //     showDateStr, int orientation = Orientation);

        [DllImport("__Internal")]
        public static extern void ShowBannerAdInOPPO(string bannerAdUnitIdStr);

        [DllImport("__Internal")]
        public static extern void InitRewardAdInOPPO(string adUnitIdStr);

        [DllImport("__Internal")]
        public static extern void ShowRewardAdInOPPO(string posIdStr, string receiverKey);

        [DllImport("__Internal")]
        public static extern void IsArea(string receiverKey);

        [DllImport("__Internal")]
        public static extern void ZDTX(string BaoMing, string QuDiaoHao, string receiverKey);

        [DllImport("__Internal")]
        public static extern void ExitApplicationInOPPO();

        [DllImport("__Internal")]
        public static extern void CreateShortcutButton();
#endif
    }
}