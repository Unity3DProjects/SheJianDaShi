using AD.Runtime.BlackBag.Shield;

namespace AD.Runtime.BlackBag.Operation
{
    public class XiaoMi : BaseOperation
    {
        public override bool CheckCanShowAD(EPlatform platform)
        {
            return TimeCheck.TimeReach;
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            return false;
        }
    }
}