using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using AD.Runtime.AdAdapter;
using AD.Runtime.Tools.Tools;
using HWWASM;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HWAdAdapter : MonoBehaviour
{
    private NativeInfo _info;
    private static HWAdAdapter _instance;

    public static HWAdAdapter Instance
    {
        get { return _instance; }
    }

    [Header("是否是测试包")] public bool isTestRPK;

    [Header("原生广告")] public GameObject NativeAdPanel;
    public Text NativeTitle;
    public RectTransform closeBtn;
    public RawImage NativeTexture;
    public Text NativeDescText;
    public Text NativeBtnText;
    private bool needShow;


    [Header("登录")] public GameObject LoginFailPanel;
    public GameObject LogoutPanel;

    [Header("六要素")] public Text appName;
    public Text developerName;
    public Text BanBen;
    public Button JieShao_Btn;
    public Button YinSi_Btn;
    public Button QuanXian_Btn;

    private static string jieShao_url = "", yinSi_url = "", quanXian_url = "";

    public RectTransform RootRext;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        // closeBtn.localScale = isTestRPK ? Vector3.one : new Vector3(0.45f, 0.45f, 1);
        DontDestroyOnLoad(this);
        _instance = this;
        needShow = false;


        // 六要素三按钮注册时间
        JieShao_Btn.onClick.AddListener(() => { OnClick_JieShao(jieShao_url); });
        YinSi_Btn.onClick.AddListener(() => { OnClick_YinSi(yinSi_url); });
        QuanXian_Btn.onClick.AddListener(() => { OnClick_QuanXian(quanXian_url); });
        NativeAdPanel.SetActive(false);
    }

    private NativeInfo info1;


    public void OnLoginSuccess(bool isAuLoadNextScene = true)
    {
        Debug.Log("登录成功");
        // 初始化,预加载广告;华为禁止同意隐私政策前预加载,因此放在登陆成功后

        var param = HuaWeiAdAdapter.HuaWeiADParam;
        if (param.isTestPackage)
        {
            InitAd(HuaWeiAdAdapter.HuaWeiADParam.testCustomPosID, HuaWeiAdAdapter.HuaWeiADParam.testBannerID,
                HuaWeiAdAdapter.HuaWeiADParam.testRewardPosID, HuaWeiAdAdapter.HuaWeiADParam.date);
            "使用测试ID".Log();
        }
        else
        {
            InitAd(HuaWeiAdAdapter.HuaWeiADParam.customPosID, HuaWeiAdAdapter.HuaWeiADParam.bannerID,
                HuaWeiAdAdapter.HuaWeiADParam.rewardPosID, HuaWeiAdAdapter.HuaWeiADParam.date);
            "使用正式ID".Log();
        }

        LoginFailPanel.SetActive(false);
        // 跳转到下一个场景
        if (isAuLoadNextScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OnLoginFail()
    {
        Debug.Log("登录失败");
        LoginFailPanel.SetActive(true);
    }

    bool isShowBanner = true;
    float timer1;


    private void Update()
    {
        if (!isShowBanner)
        {
            timer1 += Time.deltaTime;
            if (timer1 >= 30f)
            {
                isShowBanner = true;
                timer1 = 0;
            }
        }
    }


#if UNITY_EDITOR

    public static void OnClick_JieShao(string jieShao)
    {
        Debug.Log("介绍");
    }

    public static void OnClick_YinSi(string yinSi)
    {
        Debug.Log("隐私");
    }

    public static void OnClick_QuanXian(string quanXian)
    {
        Debug.Log("权限");
    }


    /// <summary>
    /// 显示原生广告
    /// </summary>
    public void ShowNativeAd()
    {
        Debug.Log("显示原生广告与banner");
        NativeAdPanel.SetActive(true);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(BanBen.gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(RootRext);
        ADAnalyticsManager.Instance.OnNativeShow();
    }

    /// <summary>
    /// 原生广告关闭按钮点击事件
    /// </summary>
    public void NativeAdClick()
    {
        Debug.Log("原生广告点击事件触发");
        CloseNativeAd();
        ADAnalyticsManager.Instance.OnNativeClick();
    }

    public void CloseNativeAd()
    {
        Debug.Log("原生广告关闭");
        NativeAdPanel.SetActive(false);
    }

    /// <summary>
    /// 展示激励广告
    /// </summary>
    /// <param name="callBackObjectName">回调函数脚本所在的gameobject名</param>
    /// <param name="callBackMethodName">回调函数名</param>
    /// <param name="callBackParam">回调函数参数</param>
    public static void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null)
    {
        Debug.Log("调用激励");
        GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, callBackParam);
        ADAnalyticsManager.Instance.OnVideoClick();
    }

    /// <summary>
    /// 账号登陆,登陆成功会自动跳转到下一个场景
    /// </summary>
    public void Login()
    {
        Debug.Log("华为账号登录");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// 打开确认注销账号提示框
    /// </summary>
    public void OpenLogoutPanel()
    {
        LogoutPanel.SetActive(true);
    }

    /// <summary>
    /// 账号注销
    /// </summary>
    public void Logout()
    {
        Debug.Log("华为账号注销");
        PlayerPrefs.DeleteAll();
        ExitApp();
    }

    /// <summary>
    /// 初始化广告组件
    /// </summary>
    /// <param name="nativeAdUnitStr">原生广告id</param>
    /// <param name="bannerAdUnitIdStr">banner广告id</param>
    /// <param name="rewardAdUnitIdStr">激励广告id</param>
    /// <param name="showDateStr">屏蔽时间</param>
    static void InitAd(string nativeAdUnitStr, string bannerAdUnitIdStr, string rewardAdUnitIdStr, string showDateStr)
    {
        Debug.Log("华为广告初始化");
    }

    /// <summary>
    /// 退出应用
    /// </summary>
    public void ExitApp()
    {
        Debug.Log("退出应用");
        Application.Quit();
    }

    /// <summary>
    /// 打开超链接
    /// </summary>
    /// <param name="url">http链接</param>
    public static void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// 数据存储
    /// </summary>
    public void SaveStr(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string LoadStr(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public float LoadFloat(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

#else
    [DllImport("__Internal")]
    static extern void OnClick_JieShao(string jieShao);

    [DllImport("__Internal")]
    static extern void OnClick_YinSi(string yinSi);

    [DllImport("__Internal")]
    static extern void OnClick_QuanXian(string quanXian);


    // 原生广告

    public void ShowNativeAd()
    {
        if (isShowBanner)
        {
          ShowBannerAd();
            isShowBanner = false;
        }
        Debug.Log("请求原生信息");
        LoadNativeAdInfo();

//额外更新一次版本号文本，处理第一次打开原生页面时版本号自动布局没刷新的问题
        BanBen.text = "版本 " +info1.versionName;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(BanBen.gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(RootRext);
        needShow = true;
         ADAnalyticsManager.Instance.OnNativeShow();
    }

    public void NativeAdClick()
    {
        ReportNativeAdClick();
        ADAnalyticsManager.Instance.OnNativeClick();
        CloseNativeAd();
    }

    public void CloseNativeAd()
    {
        _info = null;
        OnCloseNativeAd();
        NativeAdPanel.SetActive(false);
    }

    void OnLoadNativeAdInfoSuccess(string infoJson)
    {
        NativeInfo info = JsonUtility.FromJson<NativeInfo>(infoJson);
        if (info == null)
        {
            Debug.Log("加载到的原生Info为空");
            return;
        }

        _info = info;
         info1 = info;

         if(_info.appName=="")
{
   NativeTitle.text = "鲸鸿动能";
   }
 else
 {
  NativeTitle.text = _info.title;
}
       
        NativeDescText.text = _info.source;
        NativeBtnText.text = _info.clickBtnText;

        //L_测试六要素----------
        
       
            BanBen.text = "版本 " + _info.versionName;
            Debug.Log("加载版本号");
       
            if(_info.appName=="")
            {
            appName.text = "华为商城";
            }else{
              appName.text = _info.appName;
            }

              if(_info.developerName=="")
               {
                developerName.text = "运营者有限公司";
            BanBen.text = "版本 " + "1.0.0";
            
               }
               else
               {
                 developerName.text = _info.developerName;
               }

      // appName.text = _info.appName;
      
       // developerName.text = _info.developerName;
        jieShao_url = _info.appDetailUrl;
        yinSi_url = _info.privacyUrl;
        quanXian_url = _info.permissionUrl;
        //L_测试六要素----------
 

        StopAllCoroutines();
        StartCoroutine("LoadTexture");
    }

    IEnumerator LoadTexture()
    {
        using (UnityWebRequest req1 = UnityWebRequestTexture.GetTexture("https://baidu.com"))
        {
            req1.SendWebRequest();
        }

        using (UnityWebRequest req2 = UnityWebRequestTexture.GetTexture(_info.imgUrl))
        {
            yield return req2.SendWebRequest();
            while (!req2.isDone)
            {
                Debug.Log("当前的下载进度为：" + req2.downloadProgress);
                yield return 0;
            }

            if (!req2.isHttpError && !req2.isNetworkError)
            {
                NativeTexture.texture = (req2.downloadHandler as DownloadHandlerTexture).texture;

                if (needShow)
                {
                    ReportNativeAdShow();
                    NativeAdPanel.SetActive(true);
                    needShow = false;
                }
            }
        }
    }

    void OnGameSwitchToBackground()
    {
        _info = null;
        NativeAdPanel.SetActive(false);
    }

    [DllImport("__Internal")]
    static extern void LoadNativeAdInfo();

    [DllImport("__Internal")]
    static extern void OnCloseNativeAd();

    [DllImport("__Internal")]
    static extern void ReportNativeAdShow();

    [DllImport("__Internal")]
    static extern void ReportNativeAdClick();

    // banner广告

    [DllImport("__Internal")]
    static extern void ShowBannerAd();

    [DllImport("__Internal")]
    static extern void HideBannerAd();

    // 激励广告

    [DllImport("__Internal")]
    public static extern void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam =
        null);


    public void OpenLogoutPanel()
    {
        LogoutPanel.SetActive(true);
    }

    public void Logout()
    {
        ClearAllData();
        Logout_JS();
        ExitApplication();
    }



    [DllImport("__Internal")]
    static extern void Login_JS(string appidStr);

    [DllImport("__Internal")]
    static extern void Logout_JS();

    // 其他

    [DllImport("__Internal")]
    static extern void InitAd(string nativeAdUnitStr, string bannerAdUnitIdStr, string rewardAdUnitIdStr,
        string showDateStr);

    public void ExitApp()
    {
        ExitApplication();
    }

    [DllImport("__Internal")]
    static extern void ExitApplication();

    [DllImport("__Internal")]
    public static extern void OpenUrl(string url);

    // 数据存储
    public void SaveStr(string key, string value)
    {
        SaveStrData(key, value);
    }

    public string LoadStr(string key, string defaultValue = "")
    {
        return LoadStrData(key, defaultValue);
    }

    public void SaveInt(string key, int value)
    {
        SaveStrData(key, value.ToString());
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        string value = LoadStrData(key, defaultValue.ToString());
        return Int32.Parse(value);
    }

    public void SaveFloat(string key, float value)
    {
        SaveStrData(key, value.ToString());
    }

    public float LoadFloat(string key, float defaultValue = 0)
    {
        string value = LoadStrData(key, defaultValue.ToString());
        return Single.Parse(value);
    }

    public void ClearAllData()
    {
        ClearData();
        PlayerPrefs.DeleteAll();
    }

    [DllImport("__Internal")]
    static extern void SaveStrData(string key, string value);

    [DllImport("__Internal")]
    static extern string LoadStrData(string key, string defaultValue);

    [DllImport("__Internal")]
    static extern void ClearData();

#endif

    [Serializable]
    class NativeInfo
    {
        public string title;
        public string source;
        public string imgUrl;
        public string clickBtnText;

        //六要素
        public string appName; //应用包名
        public string developerName; //运营者信息
        public string versionName; //版本
        public string appDetailUrl; //介绍
        public string permissionUrl; //权限
        public string privacyUrl; //隐私
    }
}