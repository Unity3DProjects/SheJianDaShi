using AD.AdAdapter;
using AD.NodeTree;
using UnityEngine;

namespace AD.NodeTree
{
    public class LuZongVivoNode : OnceGlobalInitNode<LuZongVivoNode>
    {
        private const string Key = "路总VIVO宝箱id";
        private StringValue idValue = new StringValue();

        private INode hasBox, noBox;

        public LuZongVivoNode(INode hasBox, INode noBox)
        {
            this.hasBox = hasBox;
            this.noBox = noBox;
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key(Key), idValue);
            hasBox.InitTree(tree);
            noBox.InitTree(tree);
        }

        private bool has;

        protected override void OnCall(AdSourceType type)
        {
            bool result;
            if (has)
            {
                if (hasBox.TryCall(type, out result)) 
                    Complete(result);
            }
            else
            {
                if (noBox.TryCall(type, out result))
                    Complete(result);
            }
        }

        private void OpenBox(string data)
        {
            has = data == "1";
        }

        protected override void OnceGlobalInit()
        {
            if (!string.IsNullOrWhiteSpace(idValue.value))
            {
                ADManager.Instance.BindReceiver(Key, OpenBox);
                VivoAdAdapter.CheckCanShowBox(idValue.value, Key);
            }
            else
            {
                has = false;
            }
        }

        protected override void OnceInit()
        {
            hasBox.Init();
            noBox.Init();
        }
    }
}