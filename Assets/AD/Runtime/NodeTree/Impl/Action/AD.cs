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

    /// <summary>
    /// 接受全部广告
    /// </summary>
    public class ADDefault : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            ADManager.Instance.Call(type);
        }
    }
    /// <summary>
    /// 宝箱以外
    /// </summary>
    public class ADWithoutBox : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (!type.IsBox())
                ADManager.Instance.Call(type);
        }
    }
    /// <summary>
    /// 白包
    /// </summary>
    public class ADWhite : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (type.IsWhite())
                ADManager.Instance.Call(type);
        }
    }
    /// <summary>
    /// 黑包
    /// </summary>
    public class ADBlack : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (type.IsBlack())
                ADManager.Instance.Call(type);
        }
    }
    /// <summary>
    /// 只有宝箱
    /// </summary>
    public class ADBox : ADBaseNode
    {
        protected override void OnCall(AdSourceType type)
        {
            if (type.IsBox())
                ADManager.Instance.Call(type);
        }
    }
    /// <summary>
    /// 无广告
    /// </summary>
    public class ADNone : ADBaseNode
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

    public class AdTypeNode : BaseNode
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

        public AdTypeNode(Func<AdSourceType, bool> condition, INode node)
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

    public class AdTypeConditionNode : BaseNode
    {
        private Func<AdSourceType, bool> condition;

        public AdTypeConditionNode(Func<AdSourceType, bool> condition)
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