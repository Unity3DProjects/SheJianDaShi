using AD.BlackBag;
using AD.Decorator;
using AD.Tools;

namespace AD.AdAdapter
{
    public abstract class ADNormalParamAdapter : BaseAdAdapter<ADNormalParam>
    {
        public override void ShowWhiteAD()
        {
            this.Log($"{GetType().Name}:尝试展示白包广告");
            if (BlackBagManager.Instance.CanShowWhiteAD)
            {
                if (!TryShowCustomAD())
                    TryShowInterstitialAD();

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
                if (!TryShowBlackCustomAD())
                    if (!TryShowBlackInterstitialAD())
                        if (!TryShowCustomAD())
                            TryShowInterstitialAD();
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
            id = id ?? Param?.bannerPosIdStr;
            if (!string.IsNullOrWhiteSpace(id) && BlackBagManager.Instance.NeedBannerAD)
            {
                if (Param.Pass(nameof(Param.bannerPosIdStr)))
                {
                    ShowBannerAD_Internal(id);
                }

                return true;
            }

            return false;
        }

        private bool TryShowCustomAD(string id = null)
        {
            id = id ?? Param?.customPosIdStr;

            if (!string.IsNullOrWhiteSpace(Param?.customPosIdStr) && BlackBagManager.Instance.NeedCustomAD)
            {
                if (Param.Pass(nameof(Param.customPosIdStr)))
                {
                    ShowCustomAD_Internal(id);
                }

                return true;
            }

            return false;
        }

        private bool TryShowBlackCustomAD()
        {
            if (!string.IsNullOrWhiteSpace(Param?.blackCustomPosIdStr) && BlackBagManager.Instance.NeedBlackCustomAD)
            {
                if (Param.Pass(nameof(Param.blackCustomPosIdStr)))
                {
                    ShowCustomAD_Internal(Param.blackCustomPosIdStr);
                }

                return true;
            }

            return false;
        }

        private bool TryShowInterstitialAD()
        {
            if (!string.IsNullOrWhiteSpace(Param?.interstitialAdId)
                && BlackBagManager.Instance.NeedInterstitialAD)
            {
                if (Param.Pass(nameof(Param.interstitialAdId)))
                {
                    ShowInterstitialAD_Internal(Param.interstitialAdId);
                }

                return true;
            }

            return false;
        }

        private bool TryShowBlackInterstitialAD()
        {
            if (!string.IsNullOrWhiteSpace(Param?.blackInterstitialAdId)
                && BlackBagManager.Instance.NeedBlackInterstitialAD)
            {
                if (Param.Pass(nameof(Param.blackInterstitialAdId)))
                {
                    ShowInterstitialAD_Internal(Param.blackInterstitialAdId);
                }

                return true;
            }

            return false;
        }

        private bool TryShowRewardAD(string receiverKey, string id = null)
        {
            id = id ?? Param?.rewardPosIdStr;

            if (!string.IsNullOrWhiteSpace(id))
            {
                ShowRewardAD_Internal(id, receiverKey);
                return true;
            }

            return false;
        }

        protected abstract void ShowBannerAD_Internal(string id);
        protected abstract void ShowCustomAD_Internal(string id);

        protected abstract void ShowInterstitialAD_Internal(string id);
        protected abstract void ShowRewardAD_Internal(string id, string receiverKey);
    }
}