using System;
using System.Globalization;
using System.Reflection;
using AD.Runtime.AdAdapter;
using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.Attributes;
using AD.Runtime.BlackBag;
using AD.Runtime.Tools.Tools;
using AD.Runtime.TreasureBox.Scripts;
using UnityEngine;

[DefaultExecutionOrder(-2000)]
public partial class ADManager : SingletonMonoBase<ADManager>
{
    [Header("平台")] [SerializeField] private EPlatform mPlatform = EPlatform.None;

    [Header("广告参数")] [SerializeReference] public IADParam ADParam;

    private IAdAdapter AdAdapter { get; set; }

    public string Date
    {
        get
        {
            if (ADParam is ADNormalParam adNormalParam)
            {
                return adNormalParam.showDateStr;
            }

            if (ADParam is ADHuaWeiParam adHuaWeiParam)
            {
                return adHuaWeiParam.date;
            }

            throw new Exception("初始化广告参数错误，请检查广告参数");
        }
    }

    public string BlackDate
    {
        get
        {
            if (ADParam is ADNormalParam adNormalParam)
            {
                return adNormalParam.blackDate;
            }

            if (ADParam is ADHuaWeiParam adHuaWeiParam)
            {
                return adHuaWeiParam.blackDate;
            }

            throw new Exception("初始化广告参数错误，请检查广告参数");
        }
    }

    public EPlatform Platform => mPlatform;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Initialize()
    {
        SetPlatform();
        if (AdAdapter == null)
        {
            throw new NullReferenceException($"{nameof(AdAdapter)}is null");
        }

        if (ADParam is ADNormalParam adNormalParam)
        {
            CheckParam(ref adNormalParam);
        }

        AdAdapter.Init(ADParam);
    }

    /// <summary>
    /// 设置平台
    /// </summary>
    private void SetPlatform()
    {
        var fieldInfo = typeof(EPlatform).GetField(Platform.ToString());
        var GamePlatformAttribute = fieldInfo.GetCustomAttribute<GamePlatformAttribute>();
        if (GamePlatformAttribute == null) throw new Exception($"当前平台未添加初始化类型!");
        AdAdapter = Activator.CreateInstance(GamePlatformAttribute.Type) as IAdAdapter;
        if (AdAdapter == null) throw new Exception($"{nameof(ADManager)}:初始化失败!");
        this.Log($"当前平台:{GamePlatformAttribute.Desc}");
    }

    /// <summary>
    /// 显示白包广告
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowWhiteAd()
    {
        AdAdapter.ShowWhiteAD();
    }


    /// <summary>
    /// 显示黑包广告
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowBlackAd()
    {
        AdAdapter.ShowBlackAD();
    }

    /// <summary>
    /// 显示激励广告
    /// </summary>
    /// <param name="callBackObjectName"></param>
    /// <param name="callBackMethodName"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param = null)
    {
        AdAdapter.ShowRewardAD(callBackObjectName, callBackMethodName, param);
    }

    /// <summary>
    /// 显示激励广告
    /// </summary>
    /// <param name="callBackObjectName"></param>
    /// <param name="callBackMethodName"></param>
    /// <param name="callBackParam"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowBlackRewardAD(string callBackObjectName, string callBackMethodName, string param = null)
    {
        if (!BlackBagManager.Instance.CanShowAD)
            return;
        AdAdapter.ShowRewardAD(callBackObjectName, callBackMethodName, param);
    }

    /// <summary>
    /// 关闭应用
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ExitApplication()
    {
        AdAdapter.Quit();
    }

    /// <summary>
    /// 添加到桌面
    /// </summary>
    public void CreateShortcutButton()
    {
        AdAdapter.AddToDesktop();
    }

    /// <summary>
    /// 显示宝箱
    /// </summary>
    /// <param name="OnFinish">打开宝箱时的回调</param>
    public void ShowBox(Action OnFinish = null)
    {
        TimingManager.Instance.ShowTreasureBox(OnFinish);
    }


    /// <summary>
    /// 参数校验
    /// </summary>
    private void CheckParam(ref ADNormalParam adNormalParam)
    {
        if (string.IsNullOrWhiteSpace(adNormalParam.customPosIdStr) ||
            string.IsNullOrWhiteSpace(adNormalParam.rewardPosIdStr))
        {
            throw new Exception("请设置原生广告参数或者激励广告参数");
        }

        //广告参数校验
        adNormalParam.customPosIdStr = SpaceTrim(adNormalParam.customPosIdStr);
        adNormalParam.customPosIdStr = SpaceTrim(adNormalParam.customPosIdStr);
        adNormalParam.rewardPosIdStr = SpaceTrim(adNormalParam.rewardPosIdStr);
        adNormalParam.blackCustomPosIdStr = SpaceTrim(adNormalParam.blackCustomPosIdStr);

        //日期格式校验
        string format = "yyyy-M-d HH:mm:ss";
        bool isValid = DateTime.TryParseExact(adNormalParam.showDateStr, format, CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var result);
        if (!isValid)
        {
            throw new InvalidCastException("字符串无法转换为 DateTime");
        }
    }

    /// <summary>
    /// 空格校验和清理
    /// </summary>
    /// <param name="msg"></param>
    private string SpaceTrim(string msg)
    {
        if (msg.Contains(" "))
        {
            Debug.Log($"{msg}中包含空格");
            return msg.Trim();
        }

        return msg;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Platform == EPlatform.HuaWei)
        {
            if (ADParam is ADNormalParam)
                ADParam = new ADHuaWeiParam();
        }
        else
        {
            if (ADParam is ADHuaWeiParam)
                ADParam = new ADNormalParam();
        }
    }
#endif
}