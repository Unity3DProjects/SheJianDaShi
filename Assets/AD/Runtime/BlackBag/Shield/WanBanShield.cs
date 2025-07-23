using System.Collections;
using AD.Runtime.BlackBag.Shield.Interface;
using AD.Runtime.Tools.Tools;
using UnityEngine;
using UnityEngine.Networking;

namespace AD.Runtime.BlackBag.Shield
{
    [DisallowMultipleComponent]
    public class WanBanShield : MonoBehaviour, IShield
    {
        private string postUrl = "http://mf777.top/api/mf0202";
        private const string productId = "product_id";
        private const string channelId = "channel_id";
        private const string belonging_province = "belonging_province";
        public static bool IsOn = false;

        private void Awake()
        {
            if (TimeCheck.TimeReach)
            {
                StartCoroutine(GetSwitchText());
            }
        }

        IEnumerator GetSwitchText()
        {
            WWWForm form = new WWWForm();
            form.AddField(productId, "10578417"); //产品Id
            form.AddField(channelId, "22"); //渠道
            form.AddField(belonging_province, ""); //手机卡地址，没用
            UnityWebRequest request = UnityWebRequest.Post(postUrl, form);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
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

                this.Log($"玩伴开关：{IsOn}");
            }
        }

        public void DestroySelf()
        {
            DestroyImmediate(this,false);
        }
    }

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
}