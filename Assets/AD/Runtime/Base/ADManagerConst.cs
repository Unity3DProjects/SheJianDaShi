using System;
using System.Runtime.CompilerServices;
using AD;
using AD.AdAdapter;
using AD.Attributes;
using AD.Operation;
using AD.BlackBag.Shield;
using AD.NodeTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: InternalsVisibleTo("ADEditor")]

public static class ADManagerConst
{
    public static string LoginKey = "AD_LOGIN";

    public static string ShowBoxCallback =
        nameof(ADManager) + nameof(ADManager.ShowBox) + nameof(ShowBoxCallback);

    public static bool IsPlaying
    {
        get
        {
#if UNITY_EDITOR
            return isPlaying;
#else
            return true;
#endif
        }
    }
#if UNITY_EDITOR
    private static bool isPlaying;
    [InitializeOnLoadMethod]
    private static void EditorInit()
    {
        EditorApplication.playModeStateChanged += m =>
        {
            switch (m)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    isPlaying = true;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    isPlaying = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(m), m, null);
            }
        };
    }
#endif
}

[Preserve]
public enum EPlatform
{
    [EnumTypeBinder(typeof(TestAdAdapter))] [InspectorName("测试")]
    None,

    [EnumTypeBinder(typeof(VivoAdAdapter))]
    VIVO,

    [EnumTypeBinder(typeof(OppoAdAdapter))]
    OPPO,

    [EnumTypeBinder(typeof(XiaoMiAdAdapter))] [InspectorName("小米")]
    XiaoMi,

    [EnumTypeBinder(typeof(HuaWeiAdAdapter))] [InspectorName("华为")]
    HuaWei,

    [EnumTypeBinder(typeof(RongYaoAdAdapter))] [InspectorName("荣耀")]
    RongYao
}

[Preserve]
[Flags]
public enum EShieldType
{
    [InspectorName("不使用覆盖屏蔽"), OverrideableShieldType(true)]
    None = 0,

    [InspectorName("展示全部广告"), EnumTypeBinder(typeof(NoShieldNode)), OverrideableShieldType(true)]
    NoShield = 1 << 0,

    [InspectorName("白包时间屏蔽"), EnumTypeBinder(typeof(IsAfterShieldTimeNode)), OverrideableShieldType]
    Time = 1 << 1,

    [InspectorName("黑包时间屏蔽"), EnumTypeBinder(typeof(IsAfterBlackShieldTimeNode)), OverrideableShieldType]
    BlackTime = 1 << 2,

    [InspectorName("节假日屏蔽"), EnumTypeBinder(typeof(IsHolidayNode)), OverrideableShieldType]
    Holaday = 1 << 3,

    // [InspectorName("地区屏蔽"), EnumTypeBinder(typeof(AreaShieldNode)), OverrideableShieldType]
    // Area = 1 << 4,

    // [InspectorName("玩伴屏蔽"), EnumTypeBinder(typeof(IsWanBanOnNode))]
    // WanBan = 1 << 5,
    //
    // [InspectorName("指点天下屏蔽"), EnumTypeBinder(typeof(ZDTXNode))]
    // ZDTX = 1 << 6,
    //
    // [InspectorName("路总屏蔽"), EnumTypeBinder(typeof(LuZongVivoNode))]
    // LuZong = 1 << 7,

    [InspectorName("黑包全部屏蔽"), EnumTypeBinder(typeof(NoBlackShieldNode)), OverrideableShieldType(true)]
    NoBlackAd = 1 << 8,

    [InspectorName("自定义屏蔽"), EnumTypeBinder(typeof(CustomShieldNode)), OverrideableShieldType(true)]
    Custom = 1 << 9,

    [InspectorName("工作时间屏蔽"), EnumTypeBinder(typeof(IsWorkingTimeNode)), OverrideableShieldType]
    WorkTime = 1 << 11,

    [InspectorName("全部屏蔽"), EnumTypeBinder(typeof(AllShieldNode)), OverrideableShieldType(true)]
    NoAd = 1 << 30,
}

/// <summary>
/// 运营类型
/// </summary>
[Preserve]
public enum OperationType
{
    [EnumTypeBinder(typeof(XiaMen)), InspectorName("厦门-吴其乐")]
    XiaMen,

    [EnumTypeBinder(typeof(BiaoYue)), InspectorName("标越-李一帆")]
    BiaoYue,

    [EnumTypeBinder(typeof(WanBan)), InspectorName("玩伴-王勇")]
    WanBan,

    [EnumTypeBinder(typeof(WanBianTi)), InspectorName("玩变体-王灵义")]
    WanBianTi,

    [EnumTypeBinder(typeof(ZDTX_Default)), InspectorName("指点天下-默认")]
    ZDTX_Default,

    [EnumTypeBinder(typeof(ZDTX_邓嘉印)), InspectorName("指点天下-邓嘉印")]
    ZDTX_邓嘉印,

    [EnumTypeBinder(typeof(BoHen)), InspectorName("博恒-孙波")]
    BoHen,

    [EnumTypeBinder(typeof(ChengZhiQing)), InspectorName("陈志晴")]
    ChengZhiQing,

    [EnumTypeBinder(typeof(WanShiRong)), InspectorName("万世荣")]
    WanShiRong,

    [EnumTypeBinder(typeof(YeZi)), InspectorName("烨子")]
    YeZi,

    [EnumTypeBinder(typeof(LuZong)), InspectorName("路总")]
    LuZong,

    [EnumTypeBinder(typeof(ChenZhiWen)), InspectorName("陈志文")]
    ChenZhiWen,

    [EnumTypeBinder(typeof(LaiRiFangChang)), InspectorName("来日方长")]
    LaiRiFangChang,

    [EnumTypeBinder(typeof(LongXiangYou)), InspectorName("龙翔游")]
    LongXiangYou,
}

[Preserve]
[Flags]
public enum AdButtonType
{
    None,
    隐私政策 = 1,
    添加桌面 = 1 << 1,
    注销账号 = 1 << 2,
}

[Preserve]
[Flags]
public enum AdInstanceType
{
    None,
    Privacy = 1,
    Health = 1 << 1,
}

/// <summary>
/// 定时的组件类型
/// </summary>
[Flags]
[Preserve]
public enum TimingComponentType
{
    None = 0,

    [EnumTypeBinder(typeof(TreasureBoxTiming)), InspectorName("定时宝箱")]
    Box = 1,

    [EnumTypeBinder(typeof(EyeCareTiming)), InspectorName("定时护眼")]
    Eye = 1 << 1,

    [EnumTypeBinder(typeof(AddDeskTiming)), InspectorName("定时添加桌面")]
    AddDesk = 1 << 2,

    [EnumTypeBinder(typeof(AddDeskWindowTiming)), InspectorName("定时添加桌面弹窗")]
    AddDeskPanel = 1 << 3,

    [EnumTypeBinder(typeof(ADTiming)), InspectorName("定时广告")]
    Ad = 1 << 4,
}

#region privacy

public enum ECopyrightType
{
    [InspectorName("文本")] Text,
    [InspectorName("图片")] Picture
}

public enum EAgeType
{
    [InspectorName("16+")] Sixteen,
    [InspectorName("12+")] Twelve
}

public enum EPrivacyType
{
    [InspectorName("Http")] Http,
    [InspectorName("内容")] Content,
    [InspectorName("模板")] Pattern
}

#endregion

[Flags]
public enum PrivacyButtonType
{
    None = 0,
    Confirm = 1,
    Decline = 1 << 1,
    Agree = 1 << 2,
}

[Flags]
public enum AdType
{
    None = 0,
    Banner = 1,
    Custom = 1 << 1,
    Interstitial = 1 << 2,
    Reward = 1 << 3,
    BlackCustom = 1 << 4,
    BlackInterstitial = 1 << 6,
}

public enum AdSourceType
{
    None,


    Pause,
    Setting,
    GameEnd,
    Callback,
    White,
    Black,
    TimingAd,

    #region Box

    Box,
    CloseableBox,
    TimingBox,

    #endregion

    #region Direct

    Banner,
    Custom,

    #endregion

    Other,

    SetupTiming,
}