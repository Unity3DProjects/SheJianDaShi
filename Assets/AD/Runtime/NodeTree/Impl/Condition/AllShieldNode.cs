using AD.BlackBag;
using AD.NodeTree;

namespace AD.NodeTree
{
    public class AllShieldNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }

        protected override void OnInit()
        {
            BlackBagManager.Instance.CanShowBlackAD
                = BlackBagManager.Instance.CanShowWhiteAD
                    = BlackBagManager.Instance.CanShowBox
                        = false;
            Complete(true);
        }

        protected override void OnCall(AdSourceType type)
        {
        }
    }
}