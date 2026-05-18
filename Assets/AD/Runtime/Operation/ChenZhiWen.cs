using System.Collections.Generic;
using AD.NodeTree;

namespace AD.Operation
{
    [UnityEngine.Scripting.Preserve]
    public class ChenZhiWen : BaseOperation
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
                        @true = new AreaShieldNode(
                            new ParallelNode()
                                .AddNode(SetupTiming.Box())
                                .AddNode(SetupTiming.AD())
                                .AddNode(new SetupBlack())
                                .AddNode(new SetupBox())
                                .AddNode(new ADDefault()),
                            new AdSourceTypeNode(ADUtils.IsWhite, new ADDefault())),
                        @false = new EmptyNode()
                    };
                case EPlatform.OPPO:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterBlackShieldTimeNode()),
                        @true = new AreaShieldNode(
                            new ParallelNode()
                                .AddNode(SetupTiming.Box())
                                .AddNode(SetupTiming.AD())
                                .AddNode(new SetupBlack())
                                .AddNode(new SetupBox())
                                .AddNode(new ADDefault()),
                            new AdSourceTypeNode(ADUtils.IsWhite, new ADDefault())),
                        @false = new AdSourceTypeNode(ADUtils.IsWhite, new ADDefault())
                    };
                case EPlatform.RongYao:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new NotNode()
                                .SetNode(new IsHolidayNode()))
                            .AddNode(new NotNode()
                                .SetNode(new IsWorkingTimeNode())),
                        @true = new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox()),
                        @false = new AdSourceTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.XiaoMi:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterShieldTimeNode()),
                        @true = new ParallelNode()
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox()),
                        @false = new AdSourceTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.HuaWei:
                    return new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox())
                        ;
                case EPlatform.None:
                default:
                    return new NoAD();
            }
        }
    }
}