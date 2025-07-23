using AD.Runtime.BlackBag.Shield;

namespace AD.Runtime.BlackBag.Operation
{
    public class WanBan : BaseOperation
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
                    return WanBanShield.IsOn;
                case EPlatform.OPPO:
                    return TimeCheck.BlackTimeReach;
                default:
                    return WanBanShield.IsOn;
            }
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return WanBanShield.IsOn;
                case EPlatform.OPPO:
                    return TimeCheck.BlackTimeReach;
                default:
                    return WanBanShield.IsOn;
            }
        }
    }
}