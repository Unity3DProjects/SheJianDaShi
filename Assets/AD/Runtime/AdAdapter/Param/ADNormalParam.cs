using System;
using System.Globalization;
using AD.Attributes;
using AD.Decorator;
using AD.Tools;
using UnityEngine;

[Serializable]
public class ADNormalParam : BaseADParam
{
    private const float Space = 4;

    [AdDecorator(Name = "Banner参数", Space = Space), AdParamBind(AdType.Banner), Decoratable]
    public string bannerPosIdStr;

    [AdDecorator(Name = "原生参数", Space = Space), AdParamBind(AdType.Custom), Decoratable]
    public string customPosIdStr;

    [AdDecorator(Name = "黑包原生参数", Space = Space), AdParamBind(AdType.BlackCustom), Decoratable]
    public string blackCustomPosIdStr;

    [AdDecorator(Name = "插屏参数", Space = Space), AdParamBind(AdType.Interstitial), Decoratable]
    public string interstitialAdId;

    [AdDecorator(Name = "黑包插屏参数", Space = Space), AdParamBind(AdType.BlackInterstitial), Decoratable]
    public string blackInterstitialAdId;

    [AdDecorator(Name = "激励参数", Space = Space), AdParamBind(AdType.Reward)]
    public string rewardPosIdStr = "0";
    
    

    [Header("横竖屏, 0:横屏,1:竖屏"), AdDecorator(Display = false)]
    public int Orientation;

    [AdDecorator(Display = false)] public ADParamDecoratorManager decorator = new ADParamDecoratorManager();
    

    public override int GameOrientation
    {
        get => Orientation;
        set => Orientation = value;
    }
    

    public override ADParamDecoratorManager Decorator
    {
        get => decorator;
        set => decorator = value;
    }

    public override void Init()
    {
        if (string.IsNullOrWhiteSpace(customPosIdStr) ||
            string.IsNullOrWhiteSpace(rewardPosIdStr))
        {
            Debug.LogError("请设置原生广告参数或者激励广告参数");
        }

        //广告参数校验
        customPosIdStr = SpaceTrim(customPosIdStr);
        rewardPosIdStr = SpaceTrim(rewardPosIdStr);
        blackCustomPosIdStr = SpaceTrim(blackCustomPosIdStr);

        base.Init();
    }

    /// <summary>
    /// 空格校验和清理
    /// </summary>
    /// <param name="msg"></param>
    private string SpaceTrim(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg)) return "";
        if (msg.Contains(" "))
        {
            Debug.LogError($"{msg}中包含空格");
            return msg.Trim();
        }

        return msg;
    }
}