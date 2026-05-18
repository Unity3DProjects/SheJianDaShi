using System;
using AD.Attributes;
using UnityEngine;

[Serializable]
public class ADHuaWeiParam : BaseADParam
{
    private const float Space = 4;

    [AdDecorator(Name = "是否是测试包", Space = Space)]
    public bool isTestPackage;

    [AdDecorator(Name = "华为AppID", Space = Space)]
    public string appid;

    [AdDecorator(Name = "Banner参数", Space = Space)]
    public string bannerID;

    [AdDecorator(Name = "原生参数", Space = Space), Decoratable]
    public string customPosID;

    [AdDecorator(Name = "激励参数", Space = Space)]
    public string rewardPosID="0";


    [Header("测试ID")] [AdDecorator(Enable = false, Space = Space)]
    public string testBannerID = "testw6vs28auh3";

    [AdDecorator(Enable = false, Space = Space)]
    public string testCustomPosID = "testu7m3hc4gvm";

    [AdDecorator(Enable = false, Space = Space)]
    public string testRewardPosID = "testx9dtjwj8hp";

    [AdDecorator(Name = "白包屏蔽时间", Space = Space + 8), TimeSelector]
    public string date = "2025-5-1 19:00:00";

    [Header("横竖屏, 0:横屏,1:竖屏"), AdDecorator(Display = false)]
    public int Orientation;

    public string BannerId => isTestPackage ? testBannerID : bannerID;
    public string CustomId => isTestPackage ? testCustomPosID : customPosID;
    public string RewardId => isTestPackage ? testRewardPosID : rewardPosID;

    public override int GameOrientation
    {
        get => Orientation;
        set => Orientation = value;
    }
}