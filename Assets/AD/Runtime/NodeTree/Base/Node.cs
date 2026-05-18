using System.Collections.Generic;
using AD.NodeTree;
using AD.Tools;
using UnityEngine;

namespace AD.NodeTree
{
    public enum NodeState
    {
        None,
        Waiting,
        Running,
        Success,
        Failure
    }

    public interface INode
    {
        int Id { get; }

        NodeState State { get; }
        void Init();

        void Call(AdSourceType type);

        void Reset();

        void InitTree(Tree tree);
    }


    public abstract class BaseNode : INode
    {
        private static int Counter = 0;
        private int id;

        public int Id => id;

        protected BaseNode()
        {
            id = Counter++;
        }

        private NodeState state = NodeState.None;

        public NodeState State => state;

        public void Init()
        {
            if (state == NodeState.None)
            {
                OnInit();
            }
        }

        public void Call(AdSourceType type)
        {
#if UNITY_EDITOR
            if (type == AdSourceType.None)
            {
                Debug.LogWarning("State Error");
            }
#endif
            if (state == NodeState.Waiting)
                state = NodeState.Running;
            if (state == NodeState.Running)
            {
                OnCall(type);
            }
        }


        public void Reset()
        {
            state = NodeState.None;
            OnReset();
        }

        protected void CompleteInit()
        {
            if (state == NodeState.None)
                state = NodeState.Waiting;
        }


        protected void Complete(bool result)
        {
            if (this.IsDone()) return;
            state = result ? NodeState.Success : NodeState.Failure;
        }

        public abstract void InitTree(Tree tree);

        protected virtual void OnInit()
        {
            CompleteInit();
        }


        protected abstract void OnCall(AdSourceType type);

        protected virtual void OnReset()
        {
        }
    }

    public static class NodeExtensions
    {
        public static void For(this Tree tree, IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
                node.InitTree(tree);
        }

        public static void For(this Tree tree, params INode[] nodes)
        {
            foreach (var node in nodes)
                node.InitTree(tree);
        }

        public static void Init(this IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
                node.Init();
        }

        public static void Init(params INode[] nodes)
        {
            foreach (var node in nodes)
                node.Init();
        }

        public static bool IsDone(this INode node)
        {
            switch (node.State)
            {
                case NodeState.Success:
                case NodeState.Failure:
                    return true;
                default:
                    return false;
            }
        }

        public static bool TryCall(this INode node, AdSourceType type, out bool result)
        {
            node.Call(type);
            return node.TryGetResult(out result);
        }

        private static bool TryGetResult(this INode node, out bool result)
        {
            switch (node.State)
            {
                case NodeState.Success:
                    result = true;
                    return true;
                case NodeState.Failure:
                    result = false;
                    return true;
                default:
                    result = default;
                    return false;
            }
        }
    }
}