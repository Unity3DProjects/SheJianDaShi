using System;

namespace AD.NodeTree
{
    public class IntervalNode : BaseNode
    {
        private Tree.Key intervalKey;
        private FloatValue intervalValue = new FloatValue();
        private INode child;

        public IntervalNode(Tree.Key intervalKey, float interval, INode node)
        {
            this.intervalKey = intervalKey;
            this.intervalValue.value = interval;
            this.child = node;
        }


        public override void InitTree(Tree tree)
        {
            tree.Data.Add(intervalKey, intervalValue);
        }

        protected override void OnInit()
        {
            child.Init();
            base.OnInit();
        }

        private DateTime? time;

        protected override void OnCall(AdSourceType type)
        {
            var now = DateTime.Now;
            if (time.HasValue && (now - time.Value).TotalSeconds < intervalValue.value) return;
            time = now;
            child.Call(type);
        }
    }
}