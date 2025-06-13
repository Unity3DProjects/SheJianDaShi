using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;


public class ADManager : ADManager_L.SingletonMono<ADManager>
{
     [Header("删除存档")] [L_AD.Button("清除PlayerPrefs", "ClearPlayerPrefs")]
    public bool clearPlayerPrefs;

  
    [Header("一、填写隐私政策")]
    public PrivacyParam privacyData;

  
    [SerializeField, Header("二、渠道（VIVO、OPPO）")]
    public Channel_E channelType;

  
    [Header("三、屏蔽方式")]
    public BlockType_E blockTypeE;

 
    [SerializeField, Header("四、广告参数(参数为空就不填)")]
    private ADParam adParam;

    //TODO:--------------深圳路总的专属参数--------------
    [ConditionalHeaderField("设置签到", "blockTypeE", BlockType_E.深圳路总)]
    public string custom_SheZhiQianDao;

    [ConditionalHeaderField("结算模板", "blockTypeE", BlockType_E.深圳路总)]
    public string custom_JieSuanMoBan;

    [ConditionalHeaderField("宝箱自渲染", ConditionalLogic.All, "channelType", Channel_E.Vivo, "blockTypeE",
        BlockType_E.深圳路总)]
    public string BaoXiangZiXuanRan;

    //TODO:--------------烨子的专属参数--------------
    [ConditionalHeaderField("结算原生烨子", "blockTypeE", BlockType_E.烨子)]
    public string custom_JieSuan_YeZi;

    [ConditionalHeaderField("自弹原生烨子", "blockTypeE", BlockType_E.烨子)]
    public string custom_ZiTan_YeZi;

    [Header("横竖屏参数")] public Orientation_E orientation = Orientation_E.横屏;

    [SerializeField, ConditionalHeaderField("指点天下_包名_渠道号", "blockTypeE", BlockType_E.指点天下服务器)]
    private Channel_ZDTX zdtxParam;

    
    [Header("五、屏蔽时间")]
    [L_AD.Button("（可选）快捷设置屏蔽时间（不准确的话则需要手填）", "AutoSetTime")]
    public bool initButton;

    [SerializeField, Header("黑包屏蔽时间")] public BlockTime blockTime_Black;

    [ConditionalHeaderField("白包屏蔽时间", "channelType", Channel_E.Oppo)]
    public BlockTime blockTime_White = new BlockTime();


    public SwitchState switchState = new SwitchState(); //开关：白包、黑包、宝箱

    
    [HideInInspector,Header("(不用改)定时原生(45s_黑),默认开")] public bool timedCustom_Black = true;
    [HideInInspector,Header("(不用改)定时护眼提示弹窗(45s_黑),默认关")] public bool timedShowEye;
    [HideInInspector,Header("(不用改)定时添加桌面图标弹窗(35s_黑),默认关")] public bool timedShowAddIcon;
    
   
    [Header("2、总开关（不用改）_需要关闭时取消勾选")]
    public bool 总开关_宝箱;
    public bool 总开关_定时_原生;
    public bool 总开关_定时_护眼提示;
    public bool 总开关_定时_添加桌面弹窗;

    public void Init_TotalSwitch()
    {
        总开关_宝箱 = true;
        总开关_定时_护眼提示 = true;
        总开关_定时_添加桌面弹窗 = true;
        总开关_定时_原生 = true;
    }

    public void ClearPlayerPrefs()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        // Debug.Log("----已清除PlayerPrefs----");
    }

    private IAdAdapter _adAdapter;

    private IAdAdapter AdAdapter
    {
        get { return _adAdapter; }
        set { _adAdapter = value; }
    }

    private PanelManager _panelManager;

    public PanelManager Panel
    {
        get
        {
            if (_panelManager == null)
            {
                _panelManager = base.gameObject.GetComponentInChildren<PanelManager>();
            }

            return _panelManager;
        }
    }

    private BlockingManager _blockingManager;

    public BlockingManager BlockManager
    {
        get
        {
            if (_blockingManager == null)
            {
                _blockingManager = base.gameObject.AddComponent<BlockingManager>();
            }

            return _blockingManager;
        }
    }

    public void Start()
    {
        //初始化AdAdapter，传入参数
        InitAdAdapter();
        //根据渠道和屏蔽方式获取开关状态
        SelectBlockType();
    }


    public void InitAdAdapter()
    {
        //选择渠道
        switch (channelType)
        {
            case Channel_E.Vivo:
                AdAdapter = new VivoAdAdapter(adParam, blockTime_Black.ToFormattedString(), (int)orientation);
                break;
            case Channel_E.Oppo:
                AdAdapter = new OppoAdAdapter(adParam, blockTime_White.ToFormattedString(),
                    blockTime_Black.ToFormattedString(), (int)orientation);
                break;
            //其他...
        }

        //初始化激励
        AdAdapter.Init();
    }

    /// <summary>
    ///策略选择 --根据运营的不同来选用不同的策略
    /// </summary>
    public void SelectBlockType()
    {
#if UNITY_EDITOR
        //编辑器测试不走黑包时间，方便看开关策略是否正常
#else
        //路总啥屏蔽都不要，所以不走这步
        if (blockTypeE != BlockType_E.深圳路总)
        {
            //没过黑包时间，return掉
            if(!BlockManager.IsExpired(blockTime_Black)) return;
        }


#endif

        Debug.Log($"运营_渠道：{blockTypeE}-{channelType}".FL1_HotPink());

        timedCustom_Black = true;
        switch (blockTypeE)
        {
            case BlockType_E.地区和时间屏蔽:
                BlockManager.GetSwitchState_Area();
                break;
            case BlockType_E.指点天下服务器:
                BlockManager.GetSwitchState_ZDTX(zdtxParam);
                break;
            case BlockType_E.玩伴服务器:
                BlockManager.GetSwitchState_WanBan();
                break;
            case BlockType_E.工作日屏蔽:
                BlockManager.GetSwitchState_Holiday();
                break;
            case BlockType_E.审核时间_黑包时间:
                BlockManager.GetSwitchState_Time();
                break;
            //其他...
            case BlockType_E.玩伴_王勇:
            case BlockType_E.玩变体_王灵义:
                if (channelType == Channel_E.Vivo)
                {
                    BlockManager.GetSwitchState_WanBan();
                }
                else
                {
                    BlockManager.GetSwitchState_Time();
                }

                timedShowAddIcon = true;
                timedCustom_Black = false;
                break;
            case BlockType_E.标越_李一帆:
                BlockManager.GetSwitchState_Area();
                break;
            case BlockType_E.厦门_吴其乐:
                BlockManager.GetSwitchState_Area();
                break;
            case BlockType_E.陈志晴:
                BlockManager.GetSwitchState_Area();
                BlockManager.callBack += delegate
                {
                    if (channelType == Channel_E.Vivo)
                    {
                        //不要宝箱
                        switchState.Box = false;
                    }
                };
                //定时原生 换成  护眼提示弹窗
                timedCustom_Black = false;
                timedShowEye = true;
                break;
            case BlockType_E.博恒_孙波:
                BlockManager.GetSwitchState_Area();
                BlockManager.callBack += delegate
                {
                    //vivo不要宝箱  请求完开关把宝箱关掉
                    switchState.Box = false;
                };
                //oppo不要宝箱
                是否接收oppojs里的宝箱回调 = false;
                break;
            case BlockType_E.万世荣:
                if (channelType == Channel_E.Vivo)
                {
                    BlockManager.GetSwitchState_Area();
                    BlockManager.callBack += delegate
                    {
                        if (channelType == Channel_E.Vivo)
                        {
                            //vivo不要宝箱  请求完开关把宝箱关掉
                            switchState.Box = false;
                        }
                    };
                }
                else
                {
                    BlockManager.GetSwitchState_Time();
                }

                break;
            case BlockType_E.烨子:
                BlockManager.GetSwitchState_Time();
                BlockManager.callBack += delegate
                {
                    if (channelType == Channel_E.Vivo)
                    {
                        Debug.Log($"烨子宝箱初始{switchState.Box}");
                        //宝箱做工作日屏蔽
                        switchState.Box = BlockManager.NonTimeLock();
                        Debug.Log($"烨子宝箱设置后{switchState.Box}");
                    }
                    else
                        switchState.Box = false;
                };

                timedShowAddIcon = true;
                timedCustom_Black = false;
                break;
            case BlockType_E.深圳路总:
                //路总用参数控制开关
                BlockManager.GetSwitchState_LuZong();

                break;
        }

        DebLogEvent();
        //初始化定时任务
        Panel.InitTimedMothed();
        
        StartCoroutine(MyCoroutine());
        IEnumerator MyCoroutine()
        {
            yield return new WaitForSeconds(3);
           // Debug.Log("打包工具修改日志："+ADManager_ModifyLog);
        }
    }

    public void DebLogEvent() 
    {
        // Debug.Log($"AD开关汇总：白包:{switchState.White},黑包:{switchState.Black},宝箱:{switchState.Box}".FH6_Aqua());
        Debug.Log($"其他开关汇总:宝箱总开关:{总开关_宝箱},定时原生(黑):{timedCustom_Black},定时护眼:{timedShowEye},定时添加桌面:{timedShowAddIcon}"
            .FH6_Aqua());
    }

    #region 常用策略----快捷调用

    /// <summary>
    /// 显示白包--
    /// 正规原生广告，需要屏蔽的Banner广告
    /// </summary>
    public void ShowAD_White(CustomType_E customType = CustomType_E.其他模板)
    {
        Debug.Log("-----白包广告-----".FL1_HotPink());
        if (switchState.White) AdAdapter.ShowCustomAd(customType);
        if (switchState.Black) AdAdapter.ShowBannerAd();
    }

    /// <summary>
    /// 显示黑包--
    /// 显示有屏蔽的原生广告、Banner广告
    /// </summary>
    public void ShowAD_Black(CustomType_E customType = CustomType_E.其他模板)
    {
        Debug.Log("-----黑包广告-----".FL1_HotPink());
        if (switchState.Black)
        {
            AdAdapter.ShowCustomAd(customType);
            AdAdapter.ShowBannerAd();
        }
    }

    /// <summary>
    /// 显示激励
    /// </summary>
    /// <param name="objName">奖励方法所在脚本 挂载的对象(物体)名</param>
    /// <param name="methodName">获得奖励的方法名</param>
    public void ShowAD_Reward(string objName, string methodName)
    {
        AdAdapter.ShowRewardAd(objName, methodName);
    }

    /// <summary>
    /// 开始宝箱、结算宝箱（陈志晴vivo使用护眼提示）
    /// </summary>
    public void ShowAD_Box()
    {
        Panel.ShowTreasureBox(false);
    }

    /// <summary>
    /// 添加桌面图标——黑包
    /// </summary>
    public void ShowAD_AddDesktopIcon()
    {
        Debug.Log("-----黑包添加桌面图标-----".FL1_HotPink());
        if (switchState.Black)
        {
            if (blockTypeE == BlockType_E.深圳路总)
            {
                if (switchState.Box)
                    AdAdapter.CreateShortcutButton();
            }
            else
            {
                AdAdapter.CreateShortcutButton();
            }
        }
    }

    /// <summary>
    /// 显示提示文本(用于道具、金币不足)，1.5s后会自动关闭
    /// </summary>
    /// <param name="str">需要显示的内容</param>
    public void Show_TipText(string str)
    {
        Panel.ShowTipText(str);
    }

    /// <summary>
    /// 直接添加桌面
    /// </summary>
    public void AddDesktopIcon_Directly()
    {
        AdAdapter.CreateShortcutButton();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitApplication()
    {
        AdAdapter.ExitApplication();
    }

    #endregion


    #region jslib的回调

    /// <summary>
    /// jslib关闭激励的原生回调
    /// </summary>
    public void ShowADBack()
    {
        Time.timeScale = 1;
        ShowAD_Black();
    }


    //路总的宝箱回调
    private bool LuZongBoxisOpen;

    public void ChackLuZongBox(string str)
    {
        if (str == "Open")
        {
            LuZongBoxisOpen = true;
            switchState.Box = true;
        }
        else
        {
            switchState.Box = false;
            LuZongBoxisOpen = false;
            Debug.Log(str);
        }

        Debug.Log($"路总宝箱开关回调{LuZongBoxisOpen} ");
    }


    //oppo  js的的oppo地区开关回调
    [HideInInspector] public bool isReceive; //oppo使用了地区屏蔽的标识
    [HideInInspector] public bool 是否接收oppojs里的宝箱回调 = true;

    public void ReceiveArea(string args)
    {
        if (args == "NoAd")
        {
        }
        else if (args == "HaveAd")
        {
            if (isReceive)
            {
                switchState.Black = true;
                if (是否接收oppojs里的宝箱回调) switchState.Box = true;
                Debug.Log($"Oppo地区屏蔽开关回调{switchState.Box} ");
            }
        }
    }

    #endregion

    /// <summary>
    /// js里的时间缩放设置为1（正常）
    /// </summary>
    public void Play()
    {
        Time.timeScale = 1;
    }

    #region 自动设置时间方法

    public void AutoSetTime()
    {
        DateTime now = DateTime.Now;
        DateTime tmp = default;
        if (channelType == Channel_E.Vivo)
        {
            if (now.DayOfWeek == DayOfWeek.Friday)
            {
                tmp = now.AddDays(4);
            }
            else if (now.DayOfWeek == DayOfWeek.Thursday)
            {
                tmp = now.AddDays(1);
            }
            else if (now.DayOfWeek == DayOfWeek.Saturday)
            {
                tmp = now.AddDays(3);
            }
            else
            {
                tmp = now.AddDays(2);
            }
             //黑包和白包时间一样，赋值
            CustomSetTime(tmp, blockTime_Black);
            CustomSetTime(tmp, blockTime_White);
        }
        else if (channelType == Channel_E.Oppo)
        {
            switch (blockTypeE)
            {
                //只做时间屏蔽的，屏蔽后两天
                case BlockType_E.玩伴_王勇:
                case BlockType_E.玩变体_王灵义:
                case BlockType_E.审核时间_黑包时间:
                case BlockType_E.烨子:
                case BlockType_E.万世荣:
                    if (now.DayOfWeek == DayOfWeek.Friday)
                    {
                        tmp = now.AddDays(4);
                    }
                    else if (now.DayOfWeek == DayOfWeek.Thursday)
                    {
                        tmp = now.AddDays(1);
                    }
                    else if (now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tmp = now.AddDays(3);
                    }
                    else
                    {
                        tmp = now.AddDays(2);
                    }

                    CustomSetTime(tmp, blockTime_Black);
                    CustomSetTime(tmp, blockTime_White);
                    break;
                default:
                   


                    //白包则设置为打包时间的第二天
                    if (now.DayOfWeek == DayOfWeek.Friday)
                    {
                        tmp = now.AddDays(3);
                    }
                    else if (now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tmp = now.AddDays(2);
                    }
                    else
                    {
                        tmp = now.AddDays(1);
                    }

                    CustomSetTime(tmp, blockTime_White);
                    //其他OPPO的黑包时间是后五天
                    tmp = tmp.AddDays(4);
                    //屏蔽五天后，如果是周末，则延后到周一
                    if (tmp.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tmp = tmp.AddDays(2);
                    }
                    else if (tmp.DayOfWeek == DayOfWeek.Sunday)
                    {
                        tmp = tmp.AddDays(1);
                    }

                    CustomSetTime(tmp, blockTime_Black);
                    break;
            }
        }
    }

    public void CustomSetTime(DateTime timeA, BlockTime timeB)
    {
        timeB.year = timeA.Year;
        timeB.month = timeA.Month;
        timeB.day = timeA.Day;
        // Debug.Log($"{timeA.Year},{timeA.Month},{timeA.Day}");
    }

    #endregion

    #region 特性测试用例（无需理会）

//    public PrivacyParam PrivacyData3;
//    public bool toggle;
//    [ConditionalField("toggle")]
//    public int hiddenField;
//    public void Test()
//    {
//       
//    }
//    public PrivacyParam PrivacyData2;
//    
//    public enum WeaponType { Melee, Ranged  } // 测试非常规枚举值
//
//    [SerializeField]
//    private WeaponType weaponType;
//
//    [ConditionalField("weaponType", 1)]
//    [SerializeField]
//    private float projectileSpeed;
//
//    [ConditionalField("weaponType", WeaponType.Ranged)]
//    [SerializeField]
//    private int meleeDamage;
//    
// //    // 单条件布尔
//    [SerializeField] private bool _enable;
//    [ConditionalField("_enable")] 
//    public int priority;
//
//    public bool isBoss;
//
//    public int level;
//    // 多条件组合
//    [ConditionalField(ConditionalLogic.All, 
//       "isBoss", true,
//       "level", 10)]
//    public float attackPower;
//
//    // 枚举条件
//    public enum State { Idle, Running }
//    [SerializeField] private State _state;
//    [ConditionalField("_state", State.Running)]
//    public float moveSpeed;
//
//    public string language;
//    // 字符串匹配
//    [ConditionalField("language", "CN")]
//    public Font chineseFont;
//    
//    // [ConditionalField("weapon.stats.isReady")]
//    // public Vector3 attackOffset;
//    
//    [Header("-----------------------------按钮-----------------------")]
//    [Button("初始化按钮", "InitializeCharacter")]
//    public bool initButton;
//
//    [Header("战斗系统")]
//    public bool isArmed;
//    public int ammoCount;
//
    // [Button(
    //    label: "装填弹药", 
    //    methodName: "ReloadWeapon",
    //    conditionPath: "isArmed",
    //    targetValue: true
    // )]
    // public bool reloadButton;
//
//    private void InitializeCharacter()
//    {
//       Debug.Log("角色初始化完成");
//       isArmed = true;
//       ammoCount = 30;
//    }
//
    // private void ReloadWeapon()
    // {
    // ammoCount = 30;
    // Debug.Log($"弹药已装填，当前弹药：{ammoCount}");
    // }
//
//    [Button("切换武装状态")]
//    public bool toggleArmed;
//
//    [Button("保存配置", "SaveConfig", "ammoCount", 0)]
//    public bool saveButton;
//
//    private void SaveConfig()
//    {
//       Debug.Log("保存当前配置...");
//    }

    #endregion
}

/// <summary>
/// 渠道
/// </summary>
public enum Channel_E
{
    Vivo,
    Oppo
}

/// <summary>
/// 广告参数类
/// </summary>
[Serializable]
public class ADParam
{
    [Tooltip("Banner广告参数")] public string banner = "";
    [Tooltip("原生广告参数")] public string custom;
    [Tooltip("激励广告参数")] public string reward;
    [Tooltip("原生左图参数")] public string leftCustom;

    //其他参数
}

/// <summary>
/// 隐私政策参数类
/// </summary>
[Serializable]
public class PrivacyParam
{
    [Header("隐私政策公司名")] public string company;
    [SerializeField, Header("健康忠告是否为图片")] public bool isUseImage;
    [ConditionalField("isUseImage")] public Sprite splashImage;

    [ConditionalHeaderField("著作权人、登记号", "isUseImage", false)]
    public string Copyrightcompany;

    [ConditionalField("isUseImage", false)]
    public string Registrationcode;

    [Header("联系邮箱,不填默认为gamekf_666@sina.com")] public string Contact = "gamekf_666@sina.com";

    public void ChackIsImage()
    {
    }
}

public enum Orientation_E
{
    横屏,
    竖屏
}

/// <summary>
/// 屏蔽时间类
/// </summary>
[Serializable]
public class BlockTime
{
    // [HideInInspector]
    public int year = 2025;
    public int month = 3;
    public int day = 10;
    public int hour = 19;
    [HideInInspector] public int minute = 00;
    [HideInInspector] public int second = 00;

    public string ToFormattedString()
    {
        ValidateInputs();
        return $"{year:D4}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";
    }

    private void ValidateInputs()
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "月份必须在1~12之间");
        if (day < 1 || day > 31)
            throw new ArgumentOutOfRangeException(nameof(day), "日期必须在1~31之间");
        if (hour < 0 || hour > 23)
            throw new ArgumentOutOfRangeException(nameof(hour), "小时必须在0~23之间");
        if (minute < 0 || minute > 59)
            throw new ArgumentOutOfRangeException(nameof(minute), "分钟必须在0~59之间");
        if (second < 0 || second > 59)
            throw new ArgumentOutOfRangeException(nameof(second), "秒必须在0~59之间");
    }

    public static BlockTime Parse(string timeStr)
    {
        string[] dateTimeParts = timeStr.Split(' ');
        string[] dateParts = dateTimeParts[0].Split('-');
        string[] timeParts = dateTimeParts[1].Split(':');

        BlockTime blockTime = new BlockTime
        {
            year = int.Parse(dateParts[0]),
            month = int.Parse(dateParts[1]),
            day = int.Parse(dateParts[2]),
            hour = int.Parse(timeParts[0]),
            minute = int.Parse(timeParts[1]),
            second = int.Parse(timeParts[2])
        };

        blockTime.ValidateInputs();
        return blockTime;
    }
}

/// <summary>
/// 指点天下专用参数  结构体
/// 包名  渠道号
/// </summary>
[Serializable]
public struct Channel_ZDTX
{
    public string packageName;
    public string channelNum;
}

/// <summary>
/// 屏蔽方式——每个运营都有不同的屏蔽策略
/// </summary>
public enum BlockType_E
{
    //运营
    指点天下服务器,
    玩伴_王勇,
    玩变体_王灵义,
    厦门_吴其乐,
    标越_李一帆,
    博恒_孙波,
    陈志晴,
    万世荣,
    烨子,

    //其他
    地区和时间屏蔽,
    玩伴服务器,
    工作日屏蔽,
    审核时间_黑包时间,
    深圳路总
}