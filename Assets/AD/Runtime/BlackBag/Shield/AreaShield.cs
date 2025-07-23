using System;
using System.Collections;
using AD.Runtime.AdAdapter;
using AD.Runtime.BlackBag.Shield.Interface;
using AD.Runtime.Tools.Tools;
using UnityEngine;
using UnityEngine.Networking;

namespace AD.Runtime.BlackBag.Shield
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1000)]
    public class AreaShield : MonoBehaviour, IShield
    {
        public EPlatform Platform => ADManager.Instance.Platform;
        // [SerializeField] private EPlatform mPlatform = EPlatform.None;

        private const string url = "https://api.live.bilibili.com/client/v1/Ip/getInfoNew";
        //*/ "https://yxapi.tomatojoy.cn/getIp";

        private string[] Are =
        {
            "5bm/5bee", "5Y6m6Zeo", "5YyX5Lqs", "6KW/5rex5Zyz", "6ZW/5rKZ", "5Y2X5Lqs", "5Lic6I6e", "5p2t5bee",
            "6YeN5bqG", "5oiQ6YO9", "5q2m5rGJ",
            "5LiK5rW3", /*"6Z2S5bKb"*/
        };

        /// <summary>
        /// 地区屏蔽bool
        /// </summary>
        public static bool CanShowAd = true;


        private void Start()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return;
            }

            switch (Platform)
            {
                case EPlatform.None:
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
                    break;
                case EPlatform.VIVO:
                    $"使用时间屏蔽中：{TimeCheck.TimeReach}".Log();
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
                    break;
                case EPlatform.OPPO:
#if UNITY_EDITOR
                    $"使用时间屏蔽中：{TimeCheck.BlackTimeReach}".Log();
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
#endif
                    OppoAdAdapter.IsArea(gameObject.name, nameof(GetArea));
                    break;
                case EPlatform.XiaoMi:
                    $"使用时间屏蔽中：{TimeCheck.TimeReach}".Log();
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
                    break;
                case EPlatform.HuaWei:
                    $"使用时间屏蔽中：{TimeCheck.TimeReach}".Log();
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
                    break;
                default:
                    $"使用时间屏蔽中：{TimeCheck.TimeReach}".Log();
                    if (TimeCheck.TimeReach)
                        StartCoroutine(GetSeverDate());
                    break;
            }
        }

        public void GetArea(string msg)
        {
            if (msg == "HaveAd")
            {
                CanShowAd = true;
            }
            else if (msg == "NoAd")
            {
                CanShowAd = false;
            }
        }

        #region new GetInfo

        /// <summary>
        /// 获取地区信息网络请求
        /// </summary>
        /// <returns></returns>
        private IEnumerator GetSeverDate()
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError("错误" + request.error);
            }

            while (!request.isDone)
            {
                CanShowAd = false;
            }

            GetDate(request.downloadHandler.text);
        }

        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="requestData"></param>
        private void GetDate(string requestData)
        {
            var data = JsonUtility.FromJson<ResponseRootData>(requestData);
            CanShowAd = CheckCity(data);
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
        private bool CheckCity(ResponseRootData data)
        {
            this.Log($"使用地区屏蔽中,所在地：{data.data.city}");
            var City = T_Replace(data.data.city, "市");
            var _city = NewBaseCode(City);
            foreach (var city in Are)
            {
                if (city.Contains(_city))
                {
                    return false;
                }
            }

            return true;
        }

        public static string T_Replace(string origin, string clear)
        {
            if (origin.Contains(clear))
            {
                return origin.Replace(clear, "");
            }

            return origin;
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

        public void DestroySelf()
        {
            DestroyImmediate(this,false);
        }
    }

    #region json反序列化数据

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
}