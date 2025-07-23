using AD.Runtime.Attributes;
using AD.Runtime.BlackBag.Operation;
using UnityEngine;

namespace AD.Runtime.BlackBag
{
    /// <summary>
    /// 运营类型
    /// </summary>
    public enum OperationType
    {
        [Operation(typeof(XiaMen)), InspectorName("厦门-吴其乐")]
        XiaMen,

        [Operation(typeof(BiaoYue)), InspectorName("标越-李一帆")]
        BiaoYue,

        [Operation(typeof(WanBan)), InspectorName("玩伴-王勇")]
        WanBan,

        [Operation(typeof(WanBianTi)), InspectorName("玩变体-王灵义")]
        WanBianTi,

        [Operation(typeof(ZhiDianTianXia)), InspectorName("指点天下-樊总")]
        ZhiDianTianXia,

        [Operation(typeof(BoHen)), InspectorName("博恒-孙波")]
        BoHen,

        [Operation(typeof(ChengZhiQing)), InspectorName("陈志晴")]
        ChengZhiQing,

        [Operation(typeof(WanShiRong)), InspectorName("万世荣")]
        WanShiRong,

        [Operation(typeof(YeZi)), InspectorName("烨子")]
        YeZi,

        [Operation(typeof(LuZong)), InspectorName("路总")]
        LuZong,

        [Operation(typeof(ChenZhiWen)), InspectorName("陈志文")]
        ChenZhiWen,
        
        [Operation(typeof(LaiRiFangChang)), InspectorName("来日方长")]
        LaiRiFangChang,

        [Operation(typeof(XiaoMi)), InspectorName("小米")]
        XiaoMi,

        [Operation(typeof(HuaWei)), InspectorName("华为")]
        HuaWei,

        [Operation(typeof(RongYao)), InspectorName("荣耀")]
        RongYao,
    }
}