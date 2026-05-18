using System.Collections.Generic;
using AD.NodeTree;

namespace AD.Operation
{
    [UnityEngine.Scripting.Preserve]
    public class BoHen : BaseOperation
    {

        public override INode CreateRule(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterShieldTimeNode()),
                        @true = new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox()),
                        @false = new AdTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.OPPO:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterBlackShieldTimeNode()),
                        @true = new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox()),
                        @false = new AdTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.RongYao:
                case EPlatform.HuaWei:
                    return new ParallelNode()
                        .AddNode(SetupTiming.AD())
                        .AddNode(new SetupBlack())
                        .AddNode(new ADWithoutBox());
                case EPlatform.XiaoMi:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterShieldTimeNode()),
                        @true = new ParallelNode()
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox()),
                        @false = new AdTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.None:
                default:
                    return new ADNone();
            }
        }
    }
}