using AD.BlackBag;
using AD.NodeTree;

namespace AD.NodeTree
{
    public class NoBlackShieldNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }
        protected override void OnInit()
        {
            BlackBagManager.Instance.CanShowBlackAD
                = BlackBagManager.Instance.CanShowBox
                    = false;
            BlackBagManager.Instance.CanShowWhiteAD = true;
            Complete(true);
        }

        protected override void OnCall(AdSourceType type)
        {
        }
    }
}