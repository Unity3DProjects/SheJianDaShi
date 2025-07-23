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
    public class ZDTXShield : MonoBehaviour, IShield
    {
        public EPlatform Platform => ADManager.Instance.Platform;

        /// <summary>
        /// 1为白包广告，2开启黑包广告，3开启宝箱
        /// </summary>
        public static int getGGType = 1;

        [SerializeField] private string 包名;
        [SerializeField] private string 渠道号 = "VIVO1";
        private UnityWebRequest request;

        private string URL =>
            $"http://datacenter.zywxgames.com:15855/api/index/params?pkm={包名}&canshu={渠道号}&url=new&yys=yd";

        private void Awake()
        {
            if (!TimeCheck.TimeReach)
                return;
            switch (Platform)
            {
                case EPlatform.OPPO:
#if UNITY_EDITOR
                    StartCoroutine(GetADType());

#endif
                    OppoAdAdapter.ZDTX(包名, 渠道号);
                    break;
                default:
                    StartCoroutine(GetADType());
                    break;
            }
        }

        #region jsLib网络请求

        public void GetTypeValue(string value)
        {
            switch (value)
            {
                case "0":
                    Debug.Log("接收到0");
                    break;
                case "1":
                    getGGType = 2;
                    Debug.Log("接收到1");
                    break;
                case "2":
                    Debug.Log("接收到2");
                    getGGType = 3;
                    break;
            }
        }

        #endregion

        private IEnumerator GetADType()
        {
            request = new UnityWebRequest();
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            request.downloadHandler = downloadHandler;
            request.url = URL;
            this.Log(URL);
            request.method = UnityWebRequest.kHttpVerbGET;
            yield return request.SendWebRequest();
#if UNITY_2020_3_OR_NEWER
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    DealMessage(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(request.error);
                    break;
                case UnityWebRequest.Result.InProgress:
                    Debug.LogError("请求中");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("请求成功但是连接协议错误");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("数据错误");
                    break;
                default:
                    Debug.Log("请求不到");
                    break;
            }
#else
        if (!string.IsNullOrWhiteSpace(request.error))
        {
            Debug.LogError(request.error);
            yield break;
        }

        DealMessage(request.downloadHandler.text);
#endif
        }

        private void DealMessage(string handel)
        {
            string[] strings1 = handel.Split('&');
            if (strings1.Length > 1)
            {
                string result = strings1[1];
                result.Log();
                switch (result)
                {
                    case "1":
                        getGGType = 2;
                        break;
                    case "2":
                        getGGType = 3;
                        break;
                }
            }

            this.Log($"指点天下开关:{getGGType}");
        }

        private void OnDestroy()
        {
            if (TimeCheck.TimeReach)
                request.Dispose();
        }

        public void DestroySelf()
        {
            DestroyImmediate(this, false);
        }
    }
}