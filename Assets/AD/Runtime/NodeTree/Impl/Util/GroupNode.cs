namespace AD.NodeTree
{
    public class GroupNode : BaseNode
    {
        private string groupName;

        public INode node;

        public GroupNode(string groupName, INode node)
        {
            this.groupName = groupName;
            this.node = node;
        }

        protected override void OnInit()
        {
            node.Init();
            base.OnInit();
        }

        public override void InitTree(Tree tree)
        {
            using (tree.Data.CreateGroup(groupName)) 
                node.InitTree(tree);
        }

        protected override void OnCall(AdSourceType type)
        {
            if (node.TryCall(type,out var result)) 
                Complete(result);
        }
    }
}