using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class VivoAdAdapter:IAdAdapter
{
    private ADParam _adParam;
    
    // 屏蔽时间 格式为:yyyy-MM-dd HH:mm:ss
     string _showDateStrBlack = "2024-08-13 19:00:00";

    // 横竖屏, 0:横屏,1:竖屏
     int Orientation = 0;
     
    private string CustomTemp;//临时，路总、烨子  区分原生参数时使用
    public VivoAdAdapter(ADParam adParam,string showDateStrBlack,int orientation)
    {
        _adParam = adParam;
        _showDateStrBlack = showDateStrBlack;
        Orientation = orientation;
        Debug.Log("目前使用：VivoAdAdapter".FH5_Yellow());
        Debug.Log($"黑包屏蔽时间：{showDateStrBlack}".FL6_Khaki());
    }
    public void Init()
    {
        if (ADManager.Instance.blockTypeE == BlockType_E.深圳路总)
        {
            //第二个参数是，路总自渲染宝箱参数，用于判断路总的宝箱是否开
            Init_Vivo(_adParam.reward,ADManager.Instance.BaoXiangZiXuanRan);
        }
        else
        {
          Init_Vivo(_adParam.reward,"");
        }

       
    }

    public void ShowBannerAd()
    {
        ShowBannerAd_Vivo(_adParam.banner,_showDateStrBlack);
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
            ShowCustomAd_Vivo(CustomTemp,_adParam.leftCustom,_showDateStrBlack,Orientation);
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
            ShowCustomAd_Vivo(CustomTemp,_adParam.leftCustom,_showDateStrBlack,Orientation);
        }else
        {
            
          ShowCustomAd_Vivo(_adParam.custom,_adParam.leftCustom,_showDateStrBlack,Orientation);
        }
        
    }

    

    public void ShowRewardAd(string gameObjectName, string methodName)
    {
        ShowRewardAd_Vivo(gameObjectName,methodName,null,_adParam.reward);
    }

    public void CreateShortcutButton()
    {
        CreateShortcutButton_Vivo();
    }

    public void ExitApplication()
    {
        ExitApplication_Vivo();
    }
#if UNITY_EDITOR
    private static void Init_Vivo(string adUnitIdStr, string ZiXuanRan_Box)
    {
        Debug.Log("Vivo初始化激励");
    }
    
    public static void ShowBannerAd_Vivo(string posIdStr , string showDate )
    {
        Debug.Log("调用banner");
    }
    
    public static void ShowCustomAd_Vivo(string posIdStr , string leftPosIdStr ,
        string showDate , int orientation )
    {
        Debug.Log("调用原生");
    }
    
    /// <summary>
    /// 激励广告接口,在编辑器中立刻发放奖励
    /// </summary>
    /// <param name="callBackObjectName">挂载回调函数的物体在场景中的名称</param>
    /// <param name="callBackMethodName">需要调用的回调函数方法名</param>
    /// <param name="callBackParam">回调函数参数,仅限字符串</param>
    /// <param name="posIdStr">激励广告ID</param>
    public static void ShowRewardAd_Vivo(string callBackObjectName, string callBackMethodName, string callBackParam ,
        string posIdStr )
    {
        Debug.Log("调用激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
    }
    
    public static void CreateShortcutButton_Vivo()
    {
        Debug.Log("添加到桌面");
    }
    
    public static void ExitApplication_Vivo()
    {
        Application.Quit();
    }

#else
    [DllImport("__Internal")]
    public static extern void Init_Vivo(string adUnitIdStr, string ZiXuanRan_Box);
    [DllImport("__Internal")]
    public static extern void ShowBannerAd_Vivo(string posIdStr, string showDate );

    [DllImport("__Internal")]
    public static extern void HideBannerAd();

    [DllImport("__Internal")]
    public static extern void ShowCustomAd_Vivo(string posIdStr, string leftPosIdStr, string showDate , int orientation );

    [DllImport("__Internal")]
    public static extern void HideCustomAd();

    [DllImport("__Internal")]
    public static extern void ShowRewardAd_Vivo(string callBackObjectName, string callBackMethodName,string callBackParam , string posIdStr);

  [DllImport("__Internal")]
    public static extern void CreateShortcutButton_Vivo();
    [DllImport("__Internal")]
    public static extern void ExitApplication_Vivo();
#endif
   
}