using AD.Runtime.AdAdapter;
using AD.Runtime.Attributes;
using UnityEngine;

public enum EPlatform
{
    [GamePlatform(typeof(TestAdAdapter), "测试")]
    None,

    [GamePlatform(typeof(VivoAdAdapter), "VIVO")]
    VIVO,

    [GamePlatform(typeof(OppoAdAdapter), "OPPO")]
    OPPO,

    [GamePlatform(typeof(XiaoMiAdAdapter), "小米")] [InspectorName("小米")]
    XiaoMi,

    [GamePlatform(typeof(HuaWeiAdAdapter), "华为")] [InspectorName("华为")]
    HuaWei,

    [GamePlatform(typeof(RongYaoAdAdapter), "荣耀")] [InspectorName("荣耀")]
    RongYao
}