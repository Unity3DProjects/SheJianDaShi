using System;
using System.Collections.Generic;
using AD.NodeTree;

namespace AD.Operation
{
    [UnityEngine.Scripting.Preserve]
    public class LuZong : BaseOperation
    {
        public override AdType AllowedAdType(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return AdType.Banner | AdType.Custom | AdType.Reward;
                case EPlatform.OPPO:
                    return AdType.Banner | AdType.Custom | AdType.BlackCustom | AdType.Reward;
                default:
                    return base.AllowedAdType(platform);
            }
        }

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
                            .AddNode(new IfElseNode()
                            {
                                conditionNode = new AdSourceTypeConditionNode(e => e == AdSourceType.GameEnd),
                                @true = new CustomUnitAD(new Tree.Key("结算模板")),
                                @false = new ADWithoutBox()
                            })
                            .AddNode(new SetupBlack())
                            .AddNode(new LuZongVivoNode(
                                new ParallelNode()
                                    .AddNode(SetupTiming.Box())
                                    .AddNode(new SetupBox())
                                    .AddNode(new AdSourceTypeNode(ADUtils.IsBox, new ADDefault())),
                                new EmptyNode())),
                        @false = new AdSourceTypeNode(ADUtils.IsWhite, new ADDefault())
                    };
                case EPlatform.OPPO:
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
                        @false = new AdSourceTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.RongYao:
                case EPlatform.HuaWei:
                    return new ParallelNode()
                        .AddNode(SetupTiming.AD())
                        .AddNode(new SetupBlack())
                        .AddNode(new ADWithoutBox());
                case EPlatform.None:
                default:
                    return new NoAD();
            }
        }
    }
}