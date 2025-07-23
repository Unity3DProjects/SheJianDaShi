using AD.Runtime.BlackBag.Shield;

namespace AD.Runtime.BlackBag.Operation
{
    public class ZhiDianTianXia : BaseOperation
    {
        public override bool NeedPrivacy(EPlatform platform)
        {
            if (platform != EPlatform.VIVO) return true;
            return false;
        }

        public override bool CheckCanShowAD(EPlatform platform)
        {
            return ZDTXShield.getGGType >= 2;
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            return ZDTXShield.getGGType >= 3;
        }
    }
}