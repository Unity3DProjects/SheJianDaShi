using UnityEngine;

[SerializeField]
public class ADHuaWeiParam : IADParam
{
    [Header("是否是测试包")] public bool isTestPackage;

    [Header("华为AppID")] public string appid;

    [Header("Banner参数")] public string bannerID;

    [Header("原生参数")] public string customPosID;

    [Header("激励参数")] public string rewardPosID;


    [Header("测试ID")] public string testBannerID = "testw6vs28auh3";
    public string testCustomPosID = "testu7m3hc4gvm";
    public string testRewardPosID = "testx9dtjwj8hp";

    private string blackCustomPosId;

    [Header("屏蔽时间 格式为:yyyy-MM-dd HH:mm:ss")] [SerializeField]
    public string date = "2025-5-1 19:00:00";

    //没用的参数
    [HideInInspector]
    public string blackDate = "2024-10-1 19:00:00";
    [Header("横竖屏, 0:横屏,1:竖屏")] public int Orientation;

}