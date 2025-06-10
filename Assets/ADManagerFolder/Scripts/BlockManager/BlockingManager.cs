using System;
using System.Collections;
using System.Collections.Generic;
using _ADD;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 开关
/// </summary>
public class SwitchState
{
    public bool White=true;
    public bool Black=false;
    public bool Box=false;

    public SwitchState()
    {
        
    }
    public SwitchState(bool white,bool black,bool box)
    {
        White = white;
        Black = black;
        Box = box;
        
    }
    public void SetState(bool white,bool black,bool box)
    {
        White = white;
        Black = black;
        Box = box;
    }
}
public class BlockingManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    //请求完开关的需执行的回调
   public  Action callBack;
    public void GetBlockResult(Channel_E channel,BlockType_E blockType)
    {
        
    }

    private SwitchState _switchState=new SwitchState();

    public SwitchState SwitchState
    {
        get
        {
            
            return _switchState;
        }
        set
        {
            _switchState = value;
        }
       
    }




    #region 地区屏蔽

    private const string url = "https://api.live.bilibili.com/client/v1/Ip/getInfoNew";
    //*/ "https://yxapi.tomatojoy.cn/getIp";

    private string[] Are =
    {
        "5bm/5bee", "5Y6m6Zeo", "5YyX5Lqs", "6KW/5rex5Zyz", "6ZW/5rKZ", "5Y2X5Lqs", "5Lic6I6e", "5p2t5bee", "6YeN5bqG", "5oiQ6YO9", "5q2m5rGJ",
        "5LiK5rW3", /*"6Z2S5bKb"*/
    };

    /// <summary>
    /// 地区屏蔽bool
    /// </summary>
    public static bool IsRequestCanShowAd;
    
   
       #region new GetInfo

       public void GetSwitchState_Area()
       {
           
           if(ADManager.Instance.channelType==Channel_E.Vivo)
              StartCoroutine(GetSwitchStateArea());
           else
           {
               ADManager.Instance.isReceive = true;//表示接收oppojslib回调的方法ReceiveArea可以接收回调,因为在Oppo初始化的时候都会调用ReceiveArea
              Debug.Log("Oppo使用jslib里面的地区屏蔽".FH6_Aqua()); 
           }
       }
        /// <summary>
        /// 获取地区信息网络请求
        /// </summary>
        /// <returns></returns>
        private IEnumerator GetSwitchStateArea()
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError("错误" + request.error);
            }

            while (!request.isDone)
            {
                IsRequestCanShowAd = false;
            }

            GetDate(request.downloadHandler.text);
        }

        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="requestData"></param>
        private void GetDate(string requestData )
        {
            // Debug.Log(requestData);
            var data = JsonUtility.FromJson<ResponseRootData>(requestData);
            IsRequestCanShowAd = CheckCity(data,out string str);
            if (IsRequestCanShowAd)
            {
                if (_switchState == null)
                {
                    Debug.Log("kkkkkkk");
                }
                _switchState.SetState(true,IsRequestCanShowAd,IsRequestCanShowAd);
                ADManager.Instance.switchState = _switchState;
                
                //处理完开关，执行特殊需求
                callBack?.Invoke();
                callBack = null;
            }
           
            Debug.Log($"地区屏蔽_{IsRequestCanShowAd}_你在 {str}：白包:{_switchState.White},黑包:{_switchState.Black},宝箱:{_switchState.Box} ".FH6_Aqua());
          
        }


        /// <summary>
        /// 加密Base64
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>城市名称</returns>
        private string NewBaseCode(string cityName)
        {
            byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(cityName);
            return Convert.ToBase64String(byteStr);
        }

        /// <summary>
        /// 检查是否在屏蔽地区
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckCity(ResponseRootData data, out string str)
        {
            var City = Tools.T_Replace(data.data.city, "市");
            // Debug.Log("去除市--------" + City);
            var _city = NewBaseCode(City);
            str = City;
            foreach (var city in Are)
            {
                if (city.Contains(_city))
                {
                    //不能有广告
                    return false;
                }
            }

            return true;
        }

        /*private void CheckNetworkAccessibility()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    Debug.Log("网络不可用");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    Debug.Log("通过移动数据网络连接");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    Debug.Log("通过WiFi或有线网络连接");
                    break;
            }
        }
        private void CheckPro(string pro)
        {
            byte[] strc = Convert.FromBase64String("5bm/5Lic");
            string str4 = System.Text.Encoding.UTF8.GetString(strc);
            if (pro == str4)
            {
                canShowAddThings = true;
                Debug.Log("在屏蔽区域");
            }
        }

        //Base64位对称加密
        public void Base64Code(string cityName)
        {
            string str1 = cityName; //原始字符串
            //加密
            byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(str1);
            string str2 = Convert.ToBase64String(byteStr); //转换后的64位字符串
            Debug.Log("Base64加密：" + str2);
            //解密
            byte[] temp = Convert.FromBase64String(str2);
            string str3 = System.Text.Encoding.UTF8.GetString(temp); //64位恢复字符串
            Debug.Log("Base64解密：" + str3);
        }
        */

        #endregion
    

    #endregion


    #region 指点天下服务器

    [HideInInspector]
    public int getGGType = 1;
    [HideInInspector]
    public int GD = 0;
    private string sfcity = "";
    // public string HKT_PKG ;
    // public string Changle ;
    public static bool CanShowWhite = true;
    public static bool CanShowBlack;
    public static bool CanShowBox;
    private Channel_ZDTX tmpParam;
    public void GetSwitchState_ZDTX(Channel_ZDTX zdtxParam)
    {
        sfcity = "";
        tmpParam = zdtxParam;
        DealTakeNum(zdtxParam);
        // Debug.Log("------------");
    }

    private void DealTakeNum(Channel_ZDTX zdtxParam)
    {
        if (zdtxParam.channelNum == "" || zdtxParam.packageName == "")
        {
            Debug.Log("指点天下；渠道号和包名不能为空".FH3_Red());
            Debug.LogError("指点天下；渠道号和包名不能为空".FH3_Red());
        }
     
        string url = $"http://datacenter.zywxgames.com:15855/api/index/params?pkm={zdtxParam.packageName}&canshu={zdtxParam.channelNum}&url=new&yys=yd";
        // Debug.Log(url);
        StartCoroutine(SendRequest(url));
    }

    private IEnumerator SendRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.isDone &&!request.isHttpError &&!request.isNetworkError)
            {
                string response = request.downloadHandler.text;
                // Debug.Log(response);
                if (response == "error")
                {
                    Debug.Log($"开关{tmpParam.channelNum}未配置".FH3_Red());
                }
                DealMessage(response);
                
            }
            else
            {
                DealMessage("GG1");
                Debug.LogError("Error: " + request.error.FH3_Red());
            }
        }
    }
    

    public void DealMessage(string str1)
    {
        string[] ggarr_b = str1.Split('&');
        if (ggarr_b.Length>1)
        {
            string ggarr = ggarr_b[1];
            // Debug.Log(ggarr.FL3_Pink());
            if (ggarr == "1")
            {
                getGGType = 2;
                Debug.Log($"开关{tmpParam.channelNum} 已开GG2 ".FH4_OrangeRed());
            }
            if (ggarr == "2")
            {
                getGGType = 3;
                Debug.Log($"开关{tmpParam.channelNum} 已开GG3".FH4_OrangeRed());
            }
            CheckAdvertiseType();
        }
        
    }

    public void CheckAdvertiseType()
    {
        if (getGGType == 2)
        {
            //GG2
            // CanShowBlack = true;
            // CanShowWhite = true;
            //宝箱：关
            _switchState.SetState(true,true,false);
        }
        if (getGGType == 3)
        {
            //GG3
            // CanShowBox = true;
            // CanShowBlack = true;
            // CanShowWhite = true;
            //全开
            _switchState.SetState(true,true,true);
            
        } 
        ADManager.Instance.switchState = _switchState;
        Debug.Log($"指点天下服务器 {getGGType}:白包:{_switchState.White},黑包:{_switchState.Black},宝箱:{_switchState.Box}".FH6_Aqua());
      
    }

    #endregion

    #region 玩伴服务器

    private string postUrl = "http://mf777.top/api/mf0202";
    private const string productId = "product_id";
    private const string channelId = "channel_id";
    private const string belonging_province = "belonging_province";
    public static bool IsOn = false;

    public void GetSwitchState_WanBan()
    {
        StartCoroutine(GetSwitchWanBan());
    }
    IEnumerator GetSwitchWanBan()
    {
        WWWForm form = new WWWForm();
        form.AddField(productId, "10578417"); //产品Id
        form.AddField(channelId, "22"); //渠道
        form.AddField(belonging_province, ""); //手机卡地址，没用
        UnityWebRequest request = UnityWebRequest.Post(postUrl, form);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            // Debug.LogError(request.error);
        }
        else
        {
            //Debug.Log(GetjsonText);
            SwitchJsonData jsdata = JsonUtility.FromJson<SwitchJsonData>(request.downloadHandler.text);
            if (jsdata.ifopen == "1")
            {
                IsOn = true;
                // Debug.Log("黑包开启");
                /*this.ShieldAddress = false;*/
                //自弹
                //SelfShow();
                //SelfShow(int.Parse(jsdata.insert_interval));
            }
            else if (jsdata.ifopen == "2")
            {
                IsOn = false;
               
                //Debug.Log("白包开启");
                /*this.ShieldAddress = true;*/
            }
            _switchState.SetState(true,IsOn,IsOn);
            ADManager.Instance.switchState = _switchState;
            Debug.Log($"玩伴服务器 {IsOn}：白包:{_switchState.White},黑包:{_switchState.Black},宝箱:{_switchState.Box}".FH6_Aqua());
           
        }
    }

    #endregion
    
     
  
  
    #region 工作节假日屏蔽

        private static TimeSpan startTime=new TimeSpan(9, 0, 0);
        private static TimeSpan endTime=new TimeSpan(18, 0, 0);
        
        private static TimeSpan noonStartTime=new TimeSpan(9, 0, 0);
        private static TimeSpan noonEndTime=new TimeSpan(9, 0, 0);

        private bool isNonTimeLock ;

        public void GetSwitchState_Holiday()
        {
            isNonTimeLock = NonTimeLock();
            
            _switchState.SetState(true,isNonTimeLock,isNonTimeLock);
            ADManager.Instance.switchState = _switchState;
            Debug.Log($"工作节假日屏蔽 {dayOfWeek}：白包:{_switchState.White},黑包:{_switchState.Black},宝箱:{_switchState.Box}".FH6_Aqua());
        }
        /// <summary>
        /// 时间段
        /// </summary>
        /// <param name="t1">开始时间点</param>
        /// <param name="t2">结束时间点</param>
        /// <returns></returns>
        private  bool CheckTime(TimeSpan t1, TimeSpan t2)
        {
            DateTime now = DateTime.Now;
            TimeSpan currentTime = now.TimeOfDay;
            return currentTime >= t1 && currentTime <= t2;
        }

        private  DayOfWeek dayOfWeek;
        /// <summary>
        /// 工作日
        /// </summary>
        /// <returns></returns>
        private  bool CheckToDay()
        {
            DateTime now = DateTime.Now;
          
                
            dayOfWeek = now.DayOfWeek;
            Debug.Log($"现在时间{now}，{dayOfWeek}");
            return (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday);
        }
        
        private static readonly HashSet<DateTime> holidays = new HashSet<DateTime>()
        {
            new DateTime(2025, 4, 4), // 元旦
            new DateTime(2025, 4, 5), // 元旦
            new DateTime(2025, 4, 6), // 元旦
            new DateTime(2025, 5, 1), // 春节
            new DateTime(2025, 5, 2), // 春节
            new DateTime(2025, 5, 3), // 春节
            new DateTime(2025, 5, 4), // 春节
            new DateTime(2025, 5, 5), // 春节
            // ... 添加其他节假日
        };
 
        private  bool IsHoliday()
        {
            DateTime now = DateTime.Now;
            return holidays.Contains(now);
        }

        /// <summary>
        /// 判断是否为工作日
        /// </summary>
        /// <returns>返回True，表示可以显示广告</returns>
        public  bool NonTimeLock()
        {

            if (IsHoliday()) return true;
            
            if (CheckToDay())
            {
                if (CheckTime(startTime, endTime))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            
        }

        #endregion

        #region 时间屏蔽（使用黑包时间）


        public void GetSwitchState_Time()
        {
            bool isExpiredTime = IsExpired(ADManager.Instance.blockTime_Black);

            _switchState.SetState(true,isExpiredTime,isExpiredTime);
            ADManager.Instance.switchState = _switchState;
            callBack?.Invoke();
            callBack = null;
            Debug.Log($"审核时间屏蔽 {ADManager.Instance.blockTime_Black.ToFormattedString()}：白包:{_switchState.White},黑包:{_switchState.Black},宝箱:{_switchState.Box}".FH6_Aqua());
        }
        /// <summary>
        /// 只判断时间
        /// </summary>
        /// <param name="blockTime">指定屏蔽时间</param>
        /// <returns></returns>
        public  bool IsExpired(BlockTime blockTime)
        {
            return DateTime.Now > new DateTime(blockTime.year, blockTime.month, blockTime.day, blockTime.hour, blockTime.minute, blockTime.second);
        }
       
        #endregion

        #region 路总

        /// <summary>
        /// 深圳路总
        /// </summary>
        public void GetSwitchState_LuZong()
        {
            _switchState.SetState(true, true, false);
          ADManager.Instance.switchState = _switchState;
            
        }

        #endregion
}
#region json反序列化数据__地区屏蔽

[Serializable]
public class ResponseRootData
{
    public int code;
    public ResponseData data;
}

[Serializable]
public class ResponseData
{
    public string country;
    public string province;
    public string city;
    public string ip;
    public string server_time;
}

#endregion

#region 广告数据__玩伴

[System.Serializable]
class SwitchJsonData
{
    public string insert_interval; //广告间隔
    public string check_rate; //自点击率
    public string insert_show; //广告展示
    public string insert_limit; //广告上限
    public string group;
    public string ifopen; //开关开启  1为开启  2为关闭
    public string time_status;
    public string channel_count; //
    public string ad_insert_weights; //插屏权重
    public string ad_excitation_weights; //激励权重
    public string ad_full; //全屏状态
}

#endregion
