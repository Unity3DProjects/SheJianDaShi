using System;
using System.Collections;
using System.Reflection;
using AD.AdAdapter;
using AD.BlackBag;
using AD.BlackBag.Shield;
using AD.Instance;
using AD.Tools;
using AD;
using AD.Attributes;
using AD.Decorator;
using AD.Operation;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[DefaultExecutionOrder(-2000)]
public partial class ADManager : SingletonMonoBase<ADManager>, IADInit
{
    #region serialize

    [Header("平台")] [SerializeField] private EPlatform mPlatform = EPlatform.None;

    [Header("运营类型")] [SerializeField] private OperationType operationType;

    [Header("覆盖广告屏蔽方式")] [SerializeField] private EShieldType overrideShieldType = EShieldType.None;

    [Header("广告参数(不用的会被隐藏)")] [SerializeReference]
    public IADParam ADParam;

    #endregion

    public EPlatform Platform => mPlatform;
    public IAdAdapter AdAdapter { get; private set; }
    public OperationType OperationType => operationType;
    public BaseOperation Operation { get; private set; }

    public EShieldType OverrideShieldType => overrideShieldType;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        InitReceiver();
        InitPlatform();
        InitOperation();
        ADParam.Init();
        AdAdapter.Init(ADParam);
        InitSourceBinder(AdAdapter);
        BlackBagManager.Instance.Init(Operation);
        ADInstanceManager.Instance.Init();
        //
        ShieldManager.Instance.Init(this);
        TimingManager.Instance.Init();
    }


    /// <summary>
    /// 设置平台
    /// </summary>
    private void InitPlatform()
    {
        if (!Platform.TryGetAdAdapterType(out var type))
        {
            throw new Exception($"当前平台未添加初始化类型!");
        }

        if (!(Activator.CreateInstance(type) is IAdAdapter adapter))
        {
            throw new Exception($"{nameof(ADManager)}:初始化失败!");
        }

        AdAdapter = adapter;
        this.Log($"当前平台:{Platform}");
    }

    private void InitOperation()
    {
        var fieldInfo = typeof(OperationType).GetField(OperationType.ToString());
        var operationAtt = fieldInfo.GetAttr<EnumTypeBinderAttribute>();
        if (operationAtt == null) throw new Exception($"当前策略未添加初始化类型!");
        if (Activator.CreateInstance(operationAtt.Type) is BaseOperation operation)
        {
            Operation = operation;
        }
        else
        {
            throw new Exception($"{nameof(Operation)}:初始化失败!");
        }
    }

    public void Login(Action onSuccess = null, Action onFailure = null)
    {
        AdAdapter.Login(onSuccess, onFailure);
    }

    public AdButtonType AllowedButtonType()
    {
        return Operation.AllowedButtonType(Platform);
    }

    /// <summary>
    /// 显示白包广告
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowWhiteAd()
    {
        Show(AdSourceType.White);
        // AdAdapter.ShowWhiteAD();
    }


    /// <summary>
    /// 显示黑包广告
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ShowBlackAd()
    {
        Show(AdSourceType.Black);
        // AdAdapter.ShowBlackAD();
    }

    public void ShowRewardAD(Action onSuccess, Action onFailure = null)
    {
        var key = Guid.NewGuid().ToString("N");
        BindReceiver(key, msg =>
        {
            switch (msg)
            {
                case "Success":
                    onSuccess?.Invoke();
                    break;
                case "Failure":
                    onFailure?.Invoke();
                    break;
                default:
                    break;
            }

            RemoveAllReceiver(key);
        });
        AdAdapter.ShowRewardAD(key);
    }

    /// <summary>
    /// 显示激励广告
    /// </summary>
    public void ShowRewardAD(string objectName, string methodName, string param = null, Action onFailure = null)
    {
        ShowRewardAD(() => GameObject.Find(objectName).SendMessage(methodName, param),
            onFailure);
    }


    /// <summary>
    /// 显示激励广告
    /// </summary>
    public void ShowBlackRewardAD(string objectName, string methodName, string param = null, Action onFailure = null)
    {
        if (!BlackBagManager.Instance.CanShowBlackAD)
            return;
        ShowRewardAD(objectName, methodName, param, onFailure);
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
    /// 添加到桌面
    /// </summary>
    public void CreateShortcutButtonBlack()
    {
        if (BlackBagManager.Instance.CanShowBlackAD)
            AdAdapter.AddToDesktop();
    }

    /// <summary>
    /// 显示宝箱
    /// </summary>
    /// <param name="onFinish">打开宝箱时的回调</param>
    public void ShowBox(Action onFinish = null)
    {
        RemoveAllReceiver(ADManagerConst.ShowBoxCallback);
        BindReceiver(ADManagerConst.ShowBoxCallback, msg => { onFinish?.Invoke(); });
        Show(AdSourceType.Box);

        // TreasureBoxWindow.Instance.ShowTreasureBox();
    }

    /// <summary>
    /// 显示宝箱
    /// </summary>
    /// <param name="onFinish">打开宝箱时的回调</param>
    public void ShowBoxCloseable(Action onFinish = null)
    {
        RemoveAllReceiver(ADManagerConst.ShowBoxCallback);
        BindReceiver(ADManagerConst.ShowBoxCallback, msg => { onFinish?.Invoke(); });
        Show(AdSourceType.CloseableBox);
        // TreasureBoxWindow.Instance.ShowTreasureBoxCloseable();
    }

    public void Show(AdSourceType type)
    {
        Debug.Log($"Current Call {type}");
        ShieldManager.Instance.Rule.Call(type);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (SetupADParam() || KeepOne(gameObject, typeof(ShieldManager)))
        {
            EditorUtility.SetDirty(gameObject);
        }
    }

    internal bool SetupADParam()
    {
        Type adParamType;
        if (!Platform.TryGetAdParamType(out adParamType))
            adParamType = typeof(ADNormalParam);
        if (ADParam != null && ADParam.GetType() == adParamType) return false;
        if (Activator.CreateInstance(adParamType) is IADParam param)
        {
            ADParam = param;
        }
        else
        {
            ADParam = new ADNormalParam();
            Debug.LogError("找不到广告参数类型");
        }

        return true;
    }

    private static bool KeepOne(GameObject go, Type componentType)
    {
        var components = go.GetComponents(componentType);
        if (components.Length == 0)
        {
            go.AddComponent(componentType);
            return true;
        }

        if (components.Length == 1) return false;
        for (var i = 1; i < components.Length; i++)
        {
            var comp = components[i];
            EditorApplication.delayCall += () => { DestroyImmediate(comp); };
        }

        return true;
    }
#endif
}