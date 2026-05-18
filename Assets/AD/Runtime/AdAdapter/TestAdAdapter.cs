using AD.Tools;

namespace AD.AdAdapter
{
    [UnityEngine.Scripting.Preserve]
    public class TestAdAdapter : ADNormalParamAdapter
    {
        protected override void OnInit(ADNormalParam param)
        {
        }

        protected override void ShowBannerAD_Internal(string id)
        {
        }

        protected override void ShowCustomAD_Internal(string id)
        {
        }

        protected override void ShowInterstitialAD_Internal(string id)
        {
        }

        protected override void ShowRewardAD_Internal(string id, string receiverKey)
        {
            SendReward(receiverKey, true);
        }

        public override void AddToDesktop()
        {
            this.Log("添加桌面");
        }

        public override void Quit()
        {
            this.Log("退出");
        }
    }
}