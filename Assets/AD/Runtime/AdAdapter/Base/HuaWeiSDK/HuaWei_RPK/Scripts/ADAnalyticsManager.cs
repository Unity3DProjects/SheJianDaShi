using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AD.Tools;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 行为上报组件-放到游戏主菜单场景
/// </summary>
public class ADAnalyticsManager : SingletonMonoBase<ADAnalyticsManager>
{
    [Header("是否开启行为上报")] [SerializeField] private bool isActive;
    [Header("请求接口，默认即可")] [SerializeField] private string m_URL = "https://hwmail.slykji.com/zsfz.php/game/config?ref=addtabs";
    [Header("包名")] [SerializeField] private string m_Pkg = "";

    [Header("APPID")] [SerializeField] private string m_App_Id = "112034173";

    private string _OAID = "";

    private AnalyticsData _analyticsData = new AnalyticsData();
    private HashSet<int> reportedTimes = new HashSet<int>();


    int[] arr_Time = new int[] {60, 300, 600, 900, 1200, 1500, 1800};
    int[] nativeShows = new int[] {2, 4, 6, 8, 10};
    int[] nativeClicks = new int[] {1, 2, 3, 4, 5, 6};
    int[] videoClicks = new int[] {1, 2, 3, 4, 5};
    int[] level = new int[] {1, 2, 3, 4, 5};

    public void Awake()
    {
        Load();
        Console.WriteLine($"_Storage_Data : {JsonUtility.ToJson(_analyticsData)}");
        DontDestroyOnLoad(gameObject);
        GetOAID();
    }

    void Update()
    {
        _analyticsData.gameRuntime += Time.unscaledDeltaTime;
        int currentTimeInSeconds = Mathf.FloorToInt(_analyticsData.gameRuntime);

        if (System.Array.IndexOf(arr_Time, currentTimeInSeconds) >= 0 &&
            !reportedTimes.Contains(currentTimeInSeconds))
        {
            ReportAction(EPlayerAction.GameTime, currentTimeInSeconds / 60);
            reportedTimes.Add(currentTimeInSeconds);
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetString(nameof(_analyticsData), JsonUtility.ToJson(_analyticsData));
    }

    private void Load()
    {
        var data = JsonUtility.FromJson<AnalyticsData>(PlayerPrefs.GetString(nameof(_analyticsData)));
        _analyticsData = data == null ? new AnalyticsData() : data;
    }


    public void ReportAction(EPlayerAction action, object data = null)
    {
        if (!isActive)
            return;
        Save();
        Debug.Log("开始上报");
        WWWForm param = null;
        switch (action)
        {
            case EPlayerAction.ACTIVE:
                param = BuildRequestData("active");
                break;
            case EPlayerAction.GameTime:
                param = BuildRequestData("time", data);
                break;
            case EPlayerAction.VIDEO_CLICK:
                param = BuildRequestData("vclick", data);
                break;
            case EPlayerAction.NATIVE_SHOW:
                param = BuildRequestData("screen", data);
                break;
            case EPlayerAction.NATIVE_CLICK:
                param = BuildRequestData("click", data);
                break;
        }

        if (param != null)
        {
            Instance.StartCoroutine(SendPostRequestCoroutine(param));
        }
    }

    private WWWForm BuildRequestData(string action, object data = null)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("pkg", m_Pkg);
        wwwForm.AddField("imei", "");
        wwwForm.AddField("oaid", _OAID);
        wwwForm.AddField("action", data != null ? $"{action}{data}" : action);
        wwwForm.AddField("APPID", m_App_Id.StartsWith("C") ? m_App_Id : $"C{m_App_Id}");
        return wwwForm;
    }

    private IEnumerator SendPostRequestCoroutine(WWWForm wwwForm)
    {
        var request = UnityWebRequest.Post(m_URL, wwwForm);
        request.SetRequestHeader("Content-type", "application/x-www-form-urlencoded");
        // 等待请求
        yield return request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error))
        {
            // Post请求执行成功 // Post请求的返回参数
            var data = request.downloadHandler.text;
            Debug.Log("成功:" + data);
        }
        else
        {
            // Post请求执行失败
            Debug.Log("失败");
        }

        Save();
    }

    private void GetOAID(string OAID)
    {
        Console.WriteLine("get_OAID ===================================");
        _OAID = OAID;
        if (!_analyticsData.isActive)
        {
            _analyticsData.isActive = true;
            Instance.ReportAction(EPlayerAction.ACTIVE);
        }

        Debug.Log(OAID);
    }


#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void GetOAID();
#else
    public static void GetOAID()
    {
    }
#endif


    #region 外部调用

    public void OnNativeShow()
    {
        _analyticsData.Native_Show_Count++;
        if (nativeShows.Contains(_analyticsData.Native_Show_Count))
        {
            Debug.Log($"{EPlayerAction.NATIVE_SHOW.ToString()}:{_analyticsData.Native_Show_Count}");
            ReportAction(EPlayerAction.NATIVE_SHOW, _analyticsData.Native_Show_Count);
        }
    }


    public void OnNativeClick()
    {
        _analyticsData.Native_Click_Count++;
        if (nativeClicks.Contains(_analyticsData.Native_Click_Count))
        {
            Debug.Log($"{EPlayerAction.NATIVE_CLICK.ToString()}:{_analyticsData.Native_Click_Count}");
            ReportAction(EPlayerAction.NATIVE_CLICK, _analyticsData.Native_Click_Count);
        }
    }


    public void OnVideoClick()
    {
        _analyticsData.Video_Click_Count++;
        if (videoClicks.Contains(_analyticsData.Video_Click_Count))
        {
            Debug.Log($"{EPlayerAction.VIDEO_CLICK.ToString()}:{_analyticsData.Video_Click_Count}");
            ReportAction(EPlayerAction.VIDEO_CLICK, _analyticsData.Video_Click_Count);
        }
    }

    public void OnLevelFinish()
    {
        _analyticsData.Level++;
        if (level.Contains(_analyticsData.Level))
        {
            Debug.Log($"{EPlayerAction.LEVEL.ToString()}:{_analyticsData.Level}");
            ReportAction(EPlayerAction.LEVEL, _analyticsData.Level);
        }
    }

    #endregion

    [Serializable]
    private class AnalyticsData
    {
        public bool isActive;
        public float gameRuntime;
        public int Level;
        public int Native_Show_Count;
        public int Native_Click_Count;
        public int Video_Click_Count;
    }
}

/// <summary>
/// 统计行为
/// </summary>
public enum EPlayerAction
{
    /// <summary>
    /// 激活(登录时调)
    /// </summary>
    ACTIVE = 0,

    /// <summary>
    /// 关卡
    /// </summary>
    LEVEL = 1,

    /// <summary>
    /// 原生展示
    /// </summary>
    NATIVE_SHOW = 2,

    /// <summary>
    /// 原生点击次数
    /// </summary>
    NATIVE_CLICK = 3,

    /// <summary>
    /// 视频点击次数
    /// </summary>
    VIDEO_CLICK = 4,

    /// <summary>
    /// 游戏时长
    /// </summary>
    GameTime = 5
}