using AD.AdAdapter;
using AD.NodeTree;
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.NodeTree
{
    public class VivoMaiLiangNode : OnceGlobalInitNode<VivoMaiLiangNode>
    {
        private const string Key = nameof(VivoMaiLiangNode);

        public override void InitTree(Tree tree)
        {
        }

        protected override void OnceGlobalInit()
        {
            ADManager.Instance.BindReceiver(Key, msg =>
            {
                switch (msg)
                {
                    case "1":
                        Debug.Log("买量用户");
                        Complete(true);
                        break;
                    default:
                        Complete(false);
                        Debug.Log("非买量用户");
                        break;
                }
            });
            VivoAdAdapter.CheckMaiLiangInVivo(Key);
        }

        protected override void OnceInit()
        {
        }


        protected override void OnCall(AdSourceType type)
        {
        }
    }
}