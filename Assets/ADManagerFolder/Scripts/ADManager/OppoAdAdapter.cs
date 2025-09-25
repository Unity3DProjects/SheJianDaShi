using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

public  class OppoAdAdapter:IAdAdapter
{
    // const string bannerAdUnitIdStr = "1665604";
    // const string customAdUnitIdStr = "1665609";
    // const string rewardAdUnitIdStr = "1665610";

    //额外添加参数时用，一般只用上面三个
    public const string customAdUnitIdStr_JieSuan = "";
    const string customAdUnitIdStr_2 = "";
     string rewardAdUnitIdStrBox = "";

    // 屏蔽时间 格式为:yyyy-MM-dd HH:mm:ss
     string _showDateStrWhite = "2024-08-13 19:00:00";
     string _showDateStrBlack = "2024-08-13 19:00:00";

    // 横竖屏, 0:横屏,1:竖屏
    int Orientation = 0;
    private ADParam _adParam;
    private string CustomTemp;
    public OppoAdAdapter(ADParam adParam,string showDateStrWhite,string showDateStrBlack,int orientation)
    {
        _adParam = adParam;
        _showDateStrWhite = showDateStrWhite;
        _showDateStrBlack = showDateStrBlack;
        Orientation = orientation;
        Debug.Log("目前使用的都是：OppoAdAdapter".FH5_Yellow());
        Debug.Log($"白包屏蔽时间：{showDateStrWhite}".FL6_Khaki());
        Debug.Log($"黑包屏蔽时间：{showDateStrBlack}".FL6_Khaki());
    }
    public static void L_Init()
    {
        // Init_Oppo(_adParam.reward);
    }

//宝箱初始化
    public  void L_InitBoxReward()
    {
        // InitBoxReward_Oppo(rewardAdUnitIdStrBox);
    }


    public  bool Is_Show_Ad()
    {
        DateTime Time_Now = DateTime.Now;
        DateTime Ad_Time = Convert.ToDateTime(_showDateStrWhite);
        TimeSpan hs = new TimeSpan(Time_Now.Ticks - Ad_Time.Ticks);
        return hs.TotalMilliseconds > 0;
    }
    public void Init()
    {
        Init_Oppo(_adParam.reward);
    }

    public void ShowBannerAd()
    {
        ShowBannerAd_Oppo(_adParam.banner,_showDateStrWhite);
    }

    public void ShowCustomAd(CustomType_E customType=CustomType_E.其他模板)
    {
        if (ADManager.Instance.blockTypeE == BlockType_E.深圳路总)
        {
            switch (customType)
            {
                case CustomType_E.设置签到:
                    CustomTemp = ADManager.Instance.custom_SheZhiQianDao;
                    break;
                case CustomType_E.结算模板:
                    CustomTemp = ADManager.Instance.custom_JieSuanMoBan;
                    break; 
                case CustomType_E.其他模板:
                case CustomType_E.正常模板:
                    CustomTemp = _adParam.custom;
                    break;
            }
            ShowCustomAd_Oppo(CustomTemp,_showDateStrBlack,Orientation);
        }
        else if (ADManager.Instance.blockTypeE == BlockType_E.烨子)
        {
            switch (customType)
            {
                
                case CustomType_E.结算模板:
                    CustomTemp = ADManager.Instance.custom_JieSuan_YeZi;
                    break; 
                case CustomType_E.自弹模板:
                    CustomTemp = ADManager.Instance.custom_ZiTan_YeZi;
                    break; 
                case CustomType_E.设置签到:
                case CustomType_E.其他模板:
                case CustomType_E.正常模板:
                    CustomTemp = _adParam.custom;
                    break;
            }
            ShowCustomAd_Oppo(CustomTemp,_showDateStrBlack,Orientation);
        }else
        {
            ShowCustomAd_Oppo(_adParam.custom, _showDateStrWhite, Orientation);
        }
    }

    public void ShowRewardAd(string gameObjectName, string methodName)
    {
        ShowRewardAd_Oppo(gameObjectName,methodName);
    }

    public void CreateShortcutButton()
    {
        CreateShortcutButton_Oppo();
    }

    public void ExitApplication()
    {
        ExitApplication_Oppo();
    }

#if UNITY_EDITOR

    /// <summary>
    /// 初始化激励广告,主要用来提前load一个视频备用,避免首次展示激励时提示无可用广告
    /// 该函数不需要手动调用,已在构造函数中自动调用
    /// </summary>
    /// <param name="adUnitIdStr">激励广告id</param>
    private static void Init_Oppo(string adUnitIdStr)
    {
        Debug.Log("Oppo初始化激励");
    }
    
    private static void InitBoxReward_Oppo(string adUnitIdStrBox )
    {
        Debug.Log("初始化宝箱激励");
    }
    
    /// <summary>
    /// 展示模板原生,并在原生关闭时自动展示banner
    /// </summary>
    /// <param name="adUnitIdStr">模板原生广告id</param>
    /// <param name="bannerAdUnitIdStr">banner广告id</param>
    /// <param name="showDate">屏蔽时间</param>
    /// <param name="orientation">横竖屏</param>
    public static void ShowCustomAd_Oppo(string adUnitIdStr,
         string showDate , int orientation )
    {
        Debug.Log("调用原生");
    }
    
    public static void ShowCustomAdZone_Oppo(string adUnitIdStr , string showDate ,
        int orientation )
    {
        Debug.Log("调用 30s 原生");
    }
    
    public static void ShowBannerAd_Oppo(string bannerAdUnitIdStr , string showDateStr)
    {
        Debug.Log("调用 Banner");
    }
    
    /// <summary>
    /// 展示激励广告
    /// </summary>
    /// <param name="callBackObjectName">回调函数脚本所在的gameobject名</param>
    /// <param name="callBackMethodName">回调函数名</param>
    /// <param name="callBackParam">回调函数参数</param>
    public static void ShowRewardAd_Oppo(string callBackObjectName, string callBackMethodName, string callBackParam = null)
    {
        Debug.Log("调用激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
    }
    
    public static void ShowRewardAdBox_Oppo(string callBackObjectName, string callBackMethodName,
        string callBackParam = null)
    {
        Debug.Log("调用宝箱激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
    }
    
    public static void CreateShortcutButton_Oppo()
    {
        Debug.Log("添加到桌面");
    }
    
    public static void ExitApplication_Oppo()
    {
        Application.Quit();
    }
    

#else
    [DllImport("__Internal")]
    public static extern void Init_Oppo(string adUnitIdStr );
    [DllImport("__Internal")]
    public static extern void ShowCustomAd_Oppo(string adUnitIdStr , string showDateStr , int orientation );

    [DllImport("__Internal")]
    public static extern void ShowCustomAdZone_Oppo(string adUnitIdStr , string showDateStr , int orientation );

    [DllImport("__Internal")]
    public static extern void ShowBannerAd_Oppo(string bannerAdUnitIdStr , string showDateStr );

   

  [DllImport("__Internal")]
    public static extern void InitBoxReward_Oppo(string adUnitIdStrBox );

    [DllImport("__Internal")]
    public static extern void ShowRewardAd_Oppo(string callBackObjectName, string callBackMethodName,string callBackParam =
 null);
    [DllImport("__Internal")]
    public static extern void ShowRewardAdBox_Oppo(string callBackObjectName, string callBackMethodName,string callBackParam
 =
 null);
  [DllImport("__Internal")]
    public static extern void CreateShortcutButton_Oppo();

    [DllImport("__Internal")]
    public static extern void ExitApplication_Oppo();

#endif
}