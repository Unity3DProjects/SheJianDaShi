using System;
using AD.Attributes;
using UnityEngine;

[Serializable]
public class ADXMParam : BaseADParam
{
    private const float Space = 4;

    [AdDecorator(Name = "Banner参数", Space = Space), AdParamBind(AdType.Banner)]
    public string bannerPosIdStr;

    [AdDecorator(Name = "原生参数", Space = Space), AdParamBind(AdType.Custom)]
    public string customPosIdStr;

    [AdDecorator(Name = "激励参数", Space = Space), AdParamBind(AdType.Reward)]
    public string rewardPosIdStr = "0";
    

    [Header("横竖屏, 0:横屏,1:竖屏"), AdDecorator(Display = false)]
    public int Orientation;

    public override int GameOrientation
    {
        get => Orientation;
        set => Orientation = value;
    }
}