using System.Collections.Generic;
using AD.NodeTree;
using AD.Tools;
using UnityEngine;
using UnityEngine.Networking;
using Tree = AD.NodeTree.Tree;

namespace AD.NodeTree
{
    public class WanBanNode : OnceGlobalInitNode<WanBanNode>
    {
        private string postUrl = "http://mf777.top/api/mf0202";
        private const string productId = "product_id";
        private const string channelId = "channel_id";
        private const string belonging_province = "belonging_province";

        /// <summary>
        /// 0,1,2 ->未设置value,false,true
        /// </summary>
        private int isOn;

        private bool IsOn
        {
            get => isOn == 2;
            set
            {
                switch (value)
                {
                    case true:
                        isOn = 2;
                        break;
                    case false:
                        isOn = 1;
                        break;
                }
            }
        }

        private INode on, off;

        public WanBanNode(INode @on, INode off)
        {
            this.@on = @on;
            this.off = off;
        }

        public override void InitTree(Tree tree)
        {
            using (tree.Data.CreateGroup("玩伴开关On")) @on.InitTree(tree);
            using (tree.Data.CreateGroup("玩伴开关Off")) off.InitTree(tree);
        }

        private Queue<AdSourceType> queue = new Queue<AdSourceType>();

        protected override void OnCall(AdSourceType type)
        {
            if (isOn == 0)
            {
                queue.Enqueue(type);
                return;
            }

            bool result;
            switch (IsOn)
            {
                case true:
                    if (@on.TryCall(type, out result))
                        Complete(result);
                    break;
                case false:
                    if (off.TryCall(type, out result))
                        Complete(result);
                    break;
            }
        }

        protected override void OnceGlobalInit()
        {
            StartRequest();
        }

        protected override void OnceInit()
        {
            NodeExtensions.Init(@on, off);
        }


        private void StartRequest()
        {
            WWWForm form = new WWWForm();
            form.AddField(productId, "10578417"); //产品Id
            form.AddField(channelId, "22"); //渠道
            form.AddField(belonging_province, ""); //手机卡地址，没用
            var request = UnityWebRequest.Post(postUrl, form);
            request.SendWebRequest().completed += op =>
            {
                if (request.isHttpError || request.isNetworkError)
                {
                    Debug.LogError(request.error);
                    RequestFallback();
                }
                else
                {
                    HandleResult(request.downloadHandler.text);
                    this.Log($"玩伴开关：{IsOn}");
                }
            };
        }

        private void HandleResult(string text)
        {
            //Debug.Log(GetjsonText);
            try
            {
                SwitchJsonData jsdata = JsonUtility.FromJson<SwitchJsonData>(text);
                if (jsdata.ifopen == "1")
                {
                    IsOn = true;
                }
                else
                {
                    IsOn = false;
                }
            }
            catch
            {
                IsOn = false;
            }

            foreach (var type in queue) Call(type);
            queue.Clear();
        }

        private void RequestFallback()
        {
            IsOn = false;
            foreach (var type in queue) Call(type);
            queue.Clear();
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
}