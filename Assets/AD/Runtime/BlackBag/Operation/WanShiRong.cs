using AD.Runtime.BlackBag.Shield;

namespace AD.Runtime.BlackBag.Operation
{
    public class WanShiRong : BaseOperation
    {
        public override bool NeedPrivacy(EPlatform platform)
        {
            if (platform != EPlatform.VIVO) return true;
            return false;
        }

        public override bool CheckCanShowAD(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return TimeCheck.TimeReach && AreaShield.CanShowAd;
                case EPlatform.OPPO:
                    return TimeCheck.BlackTimeReach;
                default:
                    return TimeCheck.TimeReach && AreaShield.CanShowAd;
            }
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return false;
                case EPlatform.OPPO:
                    return TimeCheck.BlackTimeReach;
                default:
                    return false;
            }
        }
    }
}