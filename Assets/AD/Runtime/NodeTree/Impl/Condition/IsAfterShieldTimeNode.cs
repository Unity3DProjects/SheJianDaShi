using System;
using AD.NodeTree;
using AD.Tools;

namespace AD.NodeTree
{
    public class IsAfterShieldTimeNode : OnceGlobalInitNode<IsAfterShieldTimeNode>
    {
        private const string Key = nameof(IsAfterShieldTimeNode);

        private WhiteTimeSelectValue timeValue = new WhiteTimeSelectValue()
        {
            date = "2025-05-01 19:00:00"
        };

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key(Key, "白包屏蔽时间"), timeValue);
        }

        protected override void OnCall(AdSourceType type)
        {
            Complete(TimeCheck.IsExpired(timeValue.date));
        }

        protected override void OnceGlobalInit()
        {
        }

        protected override void OnceInit()
        {
        }
    }

    public class IsAfterBlackShieldTimeNode : OnceGlobalInitNode<IsAfterBlackShieldTimeNode>
    {
        private const string Key = nameof(IsAfterBlackShieldTimeNode);

        private BlackTimeSelectValue timeValue = new BlackTimeSelectValue()
        {
            date = "2025-05-01 19:00:00"
        };

        protected override void OnCall(AdSourceType type)
        {
            Complete(TimeCheck.IsExpired(timeValue.date));
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key(Key, "黑包屏蔽时间"), timeValue);
        }

        protected override void OnceGlobalInit()
        {
        }

        protected override void OnceInit()
        {
        }
    }

    [Serializable]
    public abstract class TimeSelectValue : Tree.Value
    {
        public string date = "2025-05-01 19:00:00";

        public sealed override void Load(Tree.Value other)
        {
            if (other is TimeSelectValue t)
                date = t.date;
        }
    }

    [Serializable]
    public class WhiteTimeSelectValue : TimeSelectValue
    {
    }

    [Serializable]
    public class BlackTimeSelectValue : TimeSelectValue
    {
    }
}