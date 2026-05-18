using System;
using AD.BlackBag;
using AD.NodeTree;
using UnityEngine.Scripting;

namespace AD.NodeTree
{
    [Preserve]
    public class CustomShieldNode : BaseNode
    {
        private const string Key = "自定义";
        private CustomValue customValue = new CustomValue();


        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key(Key), customValue);
        }

        protected override void OnInit()
        {
            BlackBagManager.Instance.CanShowBlackAD = customValue.showBlackAd;
            BlackBagManager.Instance.CanShowWhiteAD = customValue.showWhiteAd;
            BlackBagManager.Instance.CanShowBox = customValue.showBoxAd;
            Complete(true);
        }

        protected override void OnCall(AdSourceType type)
        {
        }

        [Serializable, Preserve]
        public class CustomValue : Tree.Value
        {
            public bool showBlackAd, showWhiteAd, showBoxAd;

            public override void Load(Tree.Value other)
            {
                if (other is CustomValue v)
                {
                    showBlackAd = v.showBlackAd;
                    showWhiteAd = v.showWhiteAd;
                    showBoxAd = v.showBoxAd;
                }
            }
        }
    }
}