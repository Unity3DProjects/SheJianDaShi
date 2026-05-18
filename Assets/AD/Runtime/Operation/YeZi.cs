using System;
using System.Collections.Generic;
using AD.Decorator;
using AD.Instance;
using AD.NodeTree;
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.Operation
{
    [UnityEngine.Scripting.Preserve]
    public class YeZi : BaseOperation
    {
        public override AdInstanceType AllowedInstanceType(EPlatform platform)
        {
            return AdInstanceType.Health | AdInstanceType.Privacy;
        }

        public override AdType AllowedAdType(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                case EPlatform.OPPO:
                    return AdType.Custom | AdType.BlackCustom | AdType.Reward;
                case EPlatform.RongYao:
                    return AdType.Interstitial | AdType.BlackInterstitial | AdType.Reward;
                default:
                    return base.AllowedAdType(platform);
            }
        }

        public override void InitDecorator(EPlatform platform, ADParamDecoratorManager decoratorManager)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    decoratorManager.decorators.AddItem(nameof(ADNormalParam.blackCustomPosIdStr), new TimerDecorator()
                    {
                        intervalSeconds = 30
                    });
                    break;

                case EPlatform.OPPO:
                    decoratorManager.decorators.AddItem(nameof(ADNormalParam.customPosIdStr), new TimerDecorator()
                    {
                        intervalSeconds = 30
                    });
                    decoratorManager.decorators.AddItem(nameof(ADNormalParam.blackCustomPosIdStr), new TimerDecorator()
                    {
                        intervalSeconds = 30
                    });
                    break;
                default:
                    base.InitDecorator(platform, decoratorManager);
                    break;
            }
        }

        public override INode CreateRule(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return new IfElseNode()
                    {
                        conditionNode = new VivoMaiLiangNode(),
                        @true = new GroupNode("买量", new ParallelNode()
                            .AddNode(SetupTiming.Box())
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new SetupBox())
                            .AddNode(new ADDefault())),
                        @false = new IfElseNode()
                        {
                            //if true black
                            conditionNode = new AndNode()
                                .AddNode(new IsAfterShieldTimeNode()),
                            @true = new ParallelNode()
                                .AddNode(SetupTiming.AD())
                                .AddNode(new SetupBlack())
                                .AddNode(new ADWithoutBox())
                                .AddNode(new AdTypeNode(type =>
                                {
                                    switch (type)
                                    {
                                        case AdSourceType.Pause:
                                        case AdSourceType.Setting:
                                        case AdSourceType.GameEnd:
                                        case AdSourceType.White:
                                        case AdSourceType.Black:
                                            return true;
                                        default:
                                            return false;
                                    }
                                }, new IntervalNode(new Tree.Key("添加桌面CD"), 60,
                                    new ActionNode(() => { }, _ => ShortcutHepler.TryInstallShortcut()))))
                                .AddNode(new IfElseNode()
                                {
                                    conditionNode = new IsWorkingTimeNode(),
                                    @true = new EmptyNode(),
                                    @false = new GroupNode("非工作时间",
                                        new ParallelNode()
                                            .AddNode(SetupTiming.Box())
                                            .AddNode(new SetupBox())
                                            .AddNode(new AdTypeNode(ADUtils.IsBox, new ADDefault()))
                                    )
                                }),

                            @false = new AdTypeNode(ADUtils.IsWhite, new ADDefault())
                        }
                    };
                case EPlatform.OPPO:
                    return new IfElseNode()
                    {
                        //if true black
                        conditionNode = new AndNode()
                            .AddNode(new IsAfterBlackShieldTimeNode()),
                        @true = new ParallelNode()
                            // .AddNode(SetupTiming.Box())
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            // .AddNode(new SetupBox())
                            .AddNode(new ADWithoutBox())
                            .AddNode(new AdTypeNode(type =>
                            {
                                switch (type)
                                {
                                    case AdSourceType.Pause:
                                    case AdSourceType.Setting:
                                    case AdSourceType.GameEnd:
                                    case AdSourceType.White:
                                    case AdSourceType.Black:
                                        return true;
                                    default:
                                        return false;
                                }
                            }, new IntervalNode(new Tree.Key("添加桌面CD"), 90,
                                new ActionNode(() => { }, _ => ShortcutHepler.TryInstallShortcut())))),
                        @false = new AdTypeNode(ADUtils.IsWhite, new ADWithoutBox())
                    };
                case EPlatform.HuaWei:
                    return new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox())
                        ;
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
                case EPlatform.RongYao:
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
                case EPlatform.None:
                default:
                    return new ADNone();
            }
        }
    }
}