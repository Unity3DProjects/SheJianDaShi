
namespace AD.Runtime.BlackBag.Operation
{
    public class LuZong : BaseOperation
    {
        public static bool CanShowBox;

        public override bool CheckCanShowAD(EPlatform platform)
        {
            return true;
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return CanShowBox;
                case EPlatform.OPPO:
                    return false;
                default:
                    return false;
            }
        }
    }
}