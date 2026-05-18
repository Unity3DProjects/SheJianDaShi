using AD.Tools;
using UnityEngine;

#if !UNITY_EDITOR
using System.Runtime.InteropServices;

#endif
namespace AD.AdAdapter
{
    [UnityEngine.Scripting.Preserve]
    public class VivoAdAdapter : ADNormalParamAdapter
    {
        public override bool HasTitle => true;  

        protected override void OnInit(ADNormalParam param)
        {
            InitRewardInVivo(Param.rewardPosIdStr);
            InitWindowResizeInVivo();
        }

        protected override void ShowBannerAD_Internal(string id)
        {
            ShowBannerAdInVivo(id);
        }

        protected override void ShowCustomAD_Internal(string id)
        {
            ShowCustomAdInVivo(id, "",  Param.Orientation);
        }


        protected override void ShowInterstitialAD_Internal(string id)
        {
            ShowInterstitialAdInVivo(id, Param.Orientation);
        }

        protected override void ShowRewardAD_Internal(string id, string receiverKey)
        {
            ShowRewardAdInVivo(id, receiverKey);
        }

        

        public override void AddToDesktop()
        {
            CreateShortcutButtonInVivo();
        }

        public override void Quit()
        {
            ExitApplicationInVivo();
        }


#if UNITY_EDITOR
        private static void ShowBannerAdInVivo(string posIdStr)
        {
            ("调用banner").Log(ELogColor.Green);
        }

        private static void ShowCustomAdInVivo(string posIdStr, string leftPosIdStr,
            int orientation)
        {
            ("调用原生").Log(ELogColor.Green);
        }

        private static void ShowInterstitialAdInVivo(string posIdStr, int orientation)
        {
            ("调用插屏").Log(ELogColor.Green);
        }

        private static void CreateShortcutButtonInVivo()
        {
            "添加到桌面".Log(ELogColor.Green);
        }

        /// <summary>
        /// 激励广告接口,在编辑器中立刻发放奖励
        /// </summary>
        private static void ShowRewardAdInVivo(string posIdStr, string receiverKey)
        {
            ("调用激励").Log(ELogColor.Green);
            SendReward(receiverKey, true);
        }

        private static void InitRewardInVivo(string posIdStr)
        {
            // Debug.Log("初始化激励");
        }

        public static void CheckCanShowBox(string posIdStr, string receiverKey)
        {
            ADUtils.SendMsgToADManager(receiverKey, "1");
        }        
        public static void CheckMaiLiangInVivo(string receiverKey)
        {
            ADUtils.SendMsgToADManager(receiverKey, "0");
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
    public static extern void ShowBannerAdInVivo(string posIdStr);

    [DllImport("__Internal")]
    public static extern void HideBannerAdInVivo();

    [DllImport("__Internal")]
    public static extern void ShowCustomAdInVivo(string posIdStr, string leftPosIdStr, int orientation);
    
    [DllImport("__Internal")]
    private static extern void ShowInterstitialAdInVivo(string posIdStr, int orientation);

    [DllImport("__Internal")]
    public static extern void HideCustomAdInVivo();

    [DllImport("__Internal")]
    public static extern void CreateShortcutButtonInVivo();

    [DllImport("__Internal")]
    public static extern void ShowRewardAdInVivo(string posIdStr, string receiverKey);

    [DllImport("__Internal")]
    public static extern void InitRewardInVivo(string posIdStr); 

    [DllImport("__Internal")]
    public static extern void CheckCanShowBox(string posIdStr, string receiverKey);
            
    [DllImport("__Internal")]
    public static extern void CheckMaiLiangInVivo(string receiverKey);


    [DllImport("__Internal")]
    public static extern void ExitApplicationInVivo();

    [DllImport("__Internal")]
    public static extern void InitWindowResizeInVivo();
#endif
    }
}