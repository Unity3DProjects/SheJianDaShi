using System;
using System.Collections.Generic;

namespace AD.NodeTree
{
    public class EmptyNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }

        protected override void OnCall(AdSourceType type)
        {
        }
    }

    public abstract class SingleNode : BaseNode
    {
        public INode child;

        public override void InitTree(Tree tree)
        {
            child.InitTree(tree);
        }

        protected override void OnInit()
        {
            child.Init();
            base.OnInit();
        }

        protected override void OnCall(AdSourceType type)
        {
            if (child.TryCall(type, out var result))
                Complete(Consume(result));
        }

        protected abstract bool Consume(bool v);

        public SingleNode SetNode(INode node)
        {
            child = node;
            return this;
        }
    }

    public abstract class MultiNode : BaseNode
    {
        public List<INode> children = new List<INode>();

        public override void InitTree(Tree tree)
        {
            tree.For(children);
        }

        protected override void OnInit()
        {
            children.Init();
            base.OnInit();
        }

        public MultiNode AddNode(INode node)
        {
            children.Add(node);
            return this;
        }
    }

    public class IfElseNode : BaseNode
    {
        public INode conditionNode, @true, @false;

        public override void InitTree(Tree tree)
        {
            tree.For(conditionNode, @true, @false);
        }

        protected override void OnInit()
        {
            NodeExtensions.Init(conditionNode, @true, @false);
            base.OnInit();
        }

        protected sealed override void OnCall(AdSourceType type)
        {
            if (conditionNode.TryCall(type, out var result))
            {
                bool actionResult;
                if (result)
                {
                    if (@true.TryCall(type, out actionResult))
                        Complete(actionResult);
                }
                else
                {
                    if (@false.TryCall(type, out actionResult))
                        Complete(actionResult);
                }
            }
        }
    }

    public class NotNode : SingleNode
    {
        protected override bool Consume(bool v) => !v;
    }

    public class AndNode : MultiNode
    {
        protected override void OnCall(AdSourceType type)
        {
            int childCount = children.Count, completeCount = 0;
            foreach (var child in children)
            {
                if (child.TryCall(type, out var result))
                {
                    completeCount++;
                    if (!result)
                    {
                        Complete(false);
                        break;
                    }
                }
            }

            if (completeCount == childCount)
                Complete(true);
        }
    }

    public class OrNode : MultiNode
    {
        protected override void OnCall(AdSourceType type)
        {
            int childCount = children.Count, completeCount = 0;
            foreach (var child in children)
            {
                if (child.TryCall(type, out var result))
                {
                    completeCount++;
                    if (result)
                    {
                        Complete(true);
                        break;
                    }
                }
            }

            if (completeCount == childCount)
                Complete(false);
        }
    }

    public abstract class OnceInitNode : BaseNode
    {
        private bool run = false;

        protected sealed override void OnInit()
        {
            if (run) return;
            OnceInit();
            base.OnInit();
            run = true;
        }

        protected abstract void OnceInit();
    }

    public abstract class OnceGlobalInitNode<T> : BaseNode
        where T : OnceGlobalInitNode<T>
    {
        private static bool runGlobalInit = false;
        private bool run;
        protected static bool OnlyOnceInit { private get; set; } = true;

        protected sealed override void OnInit()
        {
            if (!runGlobalInit)
            {
                OnceGlobalInit();
                runGlobalInit = true;
            }

            if (OnlyOnceInit)
            {
                if (!run)
                {
                    OnceInit();
                    base.OnInit();
                    run = true;
                }
            }
            else
            {
                OnceInit();
                base.OnInit();
            }
        }

        protected abstract void OnceGlobalInit();
        protected abstract void OnceInit();
    }

    public class ParallelNode : MultiNode
    {
        protected override void OnCall(AdSourceType type)
        {
            foreach (var child in children)
            {
                child.Call(type);
            }
        }
    }

    public class ActionNode : BaseNode
    {
        public Action initAction;
        public Action<AdSourceType> callAction;

        public ActionNode(Action initAction, Action<AdSourceType> callAction)
        {
            this.callAction = callAction;
        }

        public override void InitTree(Tree tree)
        {
        }

        protected override void OnInit()
        {
            initAction?.Invoke();
            base.OnInit();
        }

        protected override void OnCall(AdSourceType type)
        {
            callAction?.Invoke(type);
        }
    }
}