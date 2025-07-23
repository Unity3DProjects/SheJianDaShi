using System;
using UnityEngine;

[Serializable]
public class ADNormalParam : IADParam
{
    [Header("Banner参数")] public string bannerPosIdStr;
    [Header("原生参数")] public string customPosIdStr;
    [Header("激励参数")] public string rewardPosIdStr;
    [Header("黑包参数（可选：烨子,路总OPPO）")] public string blackCustomPosIdStr;
    [Header("宝箱开关参数（可选:路总VIVO）")] public string boxId;

    [Header("屏蔽时间 格式为:yyyy-MM-dd HH:mm:ss")]
    public string showDateStr;

    [Header("黑包时间(OPPO和时间屏蔽必填，烨子)")] [SerializeField]
    public string blackDate;

    [Header("横竖屏, 0:横屏,1:竖屏")] public int Orientation;

    public ADNormalParam()
    {
    }

    public ADNormalParam(string bannerPosIdStr, string customPosIdStr, string rewardPosIdStr, string showDateStr,
        int orientation, string blackCustomPosIdStr = "")
    {
        this.bannerPosIdStr = bannerPosIdStr;
        this.customPosIdStr = customPosIdStr;
        this.rewardPosIdStr = rewardPosIdStr;
        this.blackCustomPosIdStr = blackCustomPosIdStr;
        this.showDateStr = showDateStr;
        this.Orientation = orientation;
    }
}