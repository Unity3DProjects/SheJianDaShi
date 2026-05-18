using System;
using System.Collections.Generic;
using AD.Instance;
using AD.NodeTree;
using AD.Tools;

namespace AD.Operation
{
    [UnityEngine.Scripting.Preserve]
    public class ZDTX_Default : BaseOperation
    {
        public override string GetBlackShieldTime(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return TimeCheck.GetNotShieldTime();
                case EPlatform.OPPO:
                    return TimeCheck.GetOppoTime();
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                case EPlatform.VIVO:
                    return TimeCheck.GetVIVOBlockTime();
                default:
                    return TimeCheck.GetVIVOBlockTime();
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
                            .AddNode(SetupTiming.AD())
                            .AddNode(SetupTiming.Box())
                            .AddNode(new SetupBlack())
                            .AddNode(new SetupBox())
                            .AddNode(new ADDefault())),
                        @false = new ZDTXNode(
                            gg1: new ADWhite()
                            // new AdSourceTypeNode(ADUtils.IsWhite,
                            //     new ParallelNode()
                            //         .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 90, new ADDefault()))
                            //     )
                            ,
                            gg2: new ParallelNode()
                                .AddNode(SetupTiming.AD(true, 40, 40))
                                .AddNode(new SetupBlack())
                                .AddNode(new ADWithoutBox())
                            // .AddNode(new AdSourceTypeNode(e => e == AdSourceType.TimingAd, new ADDefault()))
                            // .AddNode(new AdSourceTypeNode(ADUtils.IsWhite, new ADDefault()))
                            ,
                            gg3: new ParallelNode()
                                .AddNode(SetupTiming.AD(true, 40, 40))
                                .AddNode(SetupTiming.Box())
                                .AddNode(new SetupBlack())
                                .AddNode(new SetupBox())
                                .AddNode(new ADDefault())
                        )
                    };
                case EPlatform.OPPO:
                    return new ZDTXNode(
                        gg1: new AdTypeNode(ADUtils.IsWhite,
                            new ParallelNode()
                                .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 90, new ADWhite()))
                        ),
                        gg2: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 90, 90))
                            .AddNode(new SetupBlack())
                            .AddNode(new AdTypeNode(e => e == AdSourceType.TimingAd, new ADDefault()))
                        ,
                        gg3: new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(SetupTiming.Box())
                            .AddNode(new SetupBox())
                            .AddNode(new SetupBlack())
                            .AddNode(new ADDefault())
                    );
                case EPlatform.XiaoMi:
                    return new ZDTXNode(
                        gg1: new ADWhite(),
                        gg2: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 60, 60))
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox())
                        ,
                        gg3: new ParallelNode()
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox())
                    );
                case EPlatform.RongYao:
                    return new ZDTXNode(
                        gg1: new ADNone(),
                        gg2: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 60, 60))
                            .AddNode(new SetupBlack())
                            .AddNode(new ADWithoutBox())
                        ,
                        gg3: new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new SetupBlack())
                            .AddNode(new SetupBox())
                            .AddNode(new ADDefault())
                    );
                case EPlatform.HuaWei:
                    return new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new ADWithoutBox())
                            .AddNode(new SetupBlack())
                        ;
                case EPlatform.None:
                default:
                    return new ADNone();
            }
        }
    }

    [UnityEngine.Scripting.Preserve]
    public class ZDTX_邓嘉印 : BaseOperation
    {
        public override string GetBlackShieldTime(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return TimeCheck.GetNotShieldTime();
                case EPlatform.OPPO:
                    return TimeCheck.GetOppoTime();
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                case EPlatform.VIVO:
                    return TimeCheck.GetVIVOBlockTime();
                default:
                    return TimeCheck.GetVIVOBlockTime();
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
                            .AddNode(SetupTiming.AD())
                            .AddNode(SetupTiming.Box())
                            .AddNode(new SetupBlack())
                            .AddNode(new SetupBox())
                            .AddNode(new ADDefault())),
                        @false = new ZDTXNode(
                            gg1: new AdTypeNode(e => e.IsWhite(),
                                new ParallelNode()
                                    .AddNode(new ADWhite())
                                    .AddNode(new ActionNode(() => { }, _ => ADManager.Instance.CreateShortcutButton()))
                            )
                            ,
                            gg2: new ParallelNode()
                                .AddNode(SetupTiming.AD(true, 40, 40))
                                .AddNode(new SetupBlack())
                                .AddNode(new AdTypeNode(e => e.IsWhite(),
                                    new ParallelNode()
                                        .AddNode(new ADWhite())
                                        .AddNode(new ActionNode(() => { },
                                            _ => ADManager.Instance.CreateShortcutButton()))
                                ))
                                // .AddNode(new AdSourceTypeNode(e => e == AdSourceType.TimingAd, new ADDefault()))
                                .AddNode(new ADBlack())
                            ,
                            gg3: new ParallelNode()
                                .AddNode(SetupTiming.AD(true, 30, 30))
                                .AddNode(new SetupBlack())
                                .AddNode(new SetupBox())
                                .AddNode(new ActionNode(() =>
                                {
                                    const string key =
                                            nameof(ZDTX_邓嘉印)
                                            + "." + nameof(EPlatform.VIVO)
                                            + "." + "gg3"
                                            + "." + "BindEvent"
                                        ;
                                    if (GlobalValuePool.Get<bool>(key)) return;
                                    GlobalValuePool.Set(key, true);
                                    HealthManager.Instance.OnHealthEnd += () => ADManager.Instance.ShowBox();
                                }, _ => { }))
                                .AddNode(new ADBox())
                                .AddNode(new AdTypeNode(e => e.IsWhite() || e.IsBlack(),
                                    new ParallelNode()
                                        .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 15, new ADDefault()))
                                ))
                        )
                    };
                case EPlatform.OPPO:
                    return new ZDTXNode(
                        gg1: new ADWhite(),
                        gg2: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 100, 100))
                            .AddNode(new SetupBlack())
                            .AddNode(new AdTypeNode(e => e.IsWhite() || e.IsBlack(),
                                new ParallelNode()
                                    .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 30, new ADDefault()))
                            ))
                        ,
                        gg3: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 30, 30))
                            .AddNode(new SetupBlack())
                            .AddNode(new AdTypeNode(e => e.IsWhite() || e.IsBlack(),
                                new ParallelNode()
                                    .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 15, new ADDefault()))
                            ))
                    );
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                    return new ZDTXNode(
                        gg1: new AdTypeNode(e => e.IsWhite(),
                            new ParallelNode()
                                .AddNode(new ADWhite())
                                .AddNode(new ActionNode(() => { }, _ => ADManager.Instance.CreateShortcutButton()))
                        )
                        ,
                        gg2: new ParallelNode()
                            .AddNode(SetupTiming.AD(true, 100, 100))
                            .AddNode(new SetupBlack())
                            .AddNode(new AdTypeNode(e => e.IsWhite() || e.IsBlack(),
                                new ParallelNode()
                                    .AddNode(new IntervalNode(new Tree.Key("插屏CD"), 30, new ADDefault()))
                            ))
                        ,
                        gg3: new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new ADDefault())
                            .AddNode(new SetupBlack())
                            .AddNode(new SetupBox())
                    );
                case EPlatform.HuaWei:
                    return new ParallelNode()
                            .AddNode(SetupTiming.AD())
                            .AddNode(new ADWithoutBox())
                            .AddNode(new SetupBlack())
                        ;
                case EPlatform.None:
                default:
                    return new ADNone();
            }
        }

        private static void BindEnterGameAction()
        {
        }
    }
}