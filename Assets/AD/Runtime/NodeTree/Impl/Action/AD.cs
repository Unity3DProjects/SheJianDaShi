using System;
using AD.BlackBag;

namespace AD.NodeTree
{
    public abstract class ADBaseNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }
    }

    public class ADDefault : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            ADManager.Instance.Call(type);
        }
    }

    public class ADWithoutBox : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (!type.IsBox())
                ADManager.Instance.Call(type);
        }
    }

    public class ADWhite : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (type.IsWhite())
                ADManager.Instance.Call(type);
        }
    }

    public class ADBlack : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (type.IsBlack())
                ADManager.Instance.Call(type);
        }
    }

    public class NoAD : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
        }
    }

    #region ad

    public class BannerUnitAD : BaseNode
    {
        private Tree.Key key;
        private StringValue idValue = new StringValue();

        public BannerUnitAD(Tree.Key key)
        {
            this.key = key;
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(key, idValue);
        }


        protected override void OnCall(AdSourceType type)
        {
            if (type.HasBanner())
                ADManager.Instance.AdAdapter.ShowBannerAD(idValue.value);
        }
    }

    public class CustomUnitAD : BaseNode
    {
        private Tree.Key key;
        private StringValue idValue = new StringValue();

        public CustomUnitAD(Tree.Key key)
        {
            this.key = key;
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(key, idValue);
        }


        protected override void OnCall(AdSourceType type)
        {
            if (type.HasCustom())
                ADManager.Instance.AdAdapter.ShowCustomAD(idValue.value);
        }
    }

    #endregion

    public class AdSourceTypeNode : BaseNode
    {
        private Func<AdSourceType, bool> condition;
        private INode node;

        public override void InitTree(Tree tree)
        {
            node.InitTree(tree);
        }

        protected override void OnInit()
        {
            node.Init();
            CompleteInit();
        }

        public AdSourceTypeNode(Func<AdSourceType, bool> condition, INode node)
        {
            this.condition = condition;
            this.node = node;
        }

        protected override void OnCall(AdSourceType type)
        {
            if (condition(type))
                node.Call(type);
        }
    }

    public class AdSourceTypeConditionNode : BaseNode
    {
        private Func<AdSourceType, bool> condition;

        public AdSourceTypeConditionNode(Func<AdSourceType, bool> condition)
        {
            this.condition = condition;
        }

        public override void InitTree(Tree tree)
        {
        }

        protected override void OnCall(AdSourceType type)
        {
            Complete(condition(type));
        }
    }

    public class SetupBlack : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }

        protected override void OnCall(AdSourceType type)
        {
            BlackBagManager.Instance.CanShowBlackAD = true;
        }
    }

    public class SetupBox : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }

        protected override void OnCall(AdSourceType type)
        {
            BlackBagManager.Instance.CanShowBox = true;
        }
    }
}