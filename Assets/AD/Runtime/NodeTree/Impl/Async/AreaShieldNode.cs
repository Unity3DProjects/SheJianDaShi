using System;
using System.Collections.Generic;
using AD.NodeTree;
using AD.Tools;
using UnityEngine;
using UnityEngine.Networking;
using Tree = AD.NodeTree.Tree;

#if !UNITY_EDITOR
using AD.AdAdapter;

#endif
namespace AD.NodeTree
{
    public sealed class AreaShieldNode : OnceGlobalInitNode<AreaShieldNode>
    {
        public static string[] DefaultArea => new string[]
        {
            "广州", "厦门", "北京", "西深圳", "长沙", "南京", "东莞", "杭州", "重庆", "成都", "武汉", "上海", /*"青岛"*/
        };

        private const string AreaKey = nameof(AreaKey);
        private const string Key = "OppoAreaShield";
        private const string Url = "https://api.live.bilibili.com/client/v1/Ip/getInfoNew";

        private AreaValue areaValue = new AreaValue();

        private INode @in, @out;

        public AreaShieldNode(INode @out, INode @in)
        {
            this.@in = @in;
            this.@out = @out;
        }

        private int state = 0;

        private bool InArea
        {
            get => state == 2;
            set
            {
                switch (value)
                {
                    case true:
                        state = 2;
                        break;
                    case false:
                        state = 1;
                        break;
                }
            }
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key(AreaKey, "屏蔽地区"), areaValue);
            using (tree.Data.CreateGroup("屏蔽地区内")) @in.InitTree(tree);
            using (tree.Data.CreateGroup("屏蔽地区外")) @out.InitTree(tree);
        }


        protected override void OnceGlobalInit()
        {
            switch (ADManager.Instance.Platform)
            {
                case EPlatform.OPPO:
#if UNITY_EDITOR
                    StartRequest();
#else
                ADManager.Instance.BindReceiver(Key, GetArea);
                OppoAdAdapter.IsArea(Key);
#endif
                    break;
                default:
                    StartRequest();
                    break;
            }
        }

        protected override void OnceInit()
        {
            NodeExtensions.Init(@in, @out);
        }

        private Queue<AdSourceType> queue = new Queue<AdSourceType>();

        protected override void OnCall(AdSourceType type)
        {
            if (state == 0)
            {
                queue.Enqueue(type);
                return;
            }

            bool result;
            switch (InArea)
            {
                case true:
                    if (@in.TryCall(type, out result))
                        Complete(result);
                    break;
                case false:
                    if (@out.TryCall(type, out result))
                        Complete(result);
                    break;
            }
        }
#if !UNITY_EDITOR
    public void GetArea(string msg)
    {
        InArea = msg == "NoAd";
        foreach (var type in queue) Call(type);
        queue.Clear();
    }
#endif


        #region new GetInfo

        /// <summary>
        /// 获取地区信息网络请求
        /// </summary>
        /// <returns></returns>
        private void StartRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(Url);
            request.SendWebRequest().completed += op =>
            {
                if (request.isHttpError || request.isNetworkError)
                {
                    RequestFallback();
                }
                else
                {
                    HandleResult(request.downloadHandler.text);
                }

                request.Dispose();
            };
        }

        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="requestData"></param>
        private void HandleResult(string requestData)
        {
            var data = JsonUtility.FromJson<ResponseRootData>(requestData);
            InArea = CheckCity(data);
            foreach (var type in queue) Call(type);
            queue.Clear();
        }

        private void RequestFallback()
        {
            InArea = false;
            foreach (var type in queue) Call(type);
            queue.Clear();
        }

        /// <summary>
        /// 检查是否在屏蔽地区
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckCity(ResponseRootData data)
        {
            this.Log($"使用地区屏蔽中,所在地：{data.data.city}");
            var provinceName = T_Replace(data.data.province, "省");
            var cityName = T_Replace(data.data.city, "市");
            foreach (var area in areaValue.area)
            {
                if (area.Contains(provinceName) || area.Contains(cityName))
                {
                    return true;
                }
            }

            return false;
        }

        public static string T_Replace(string origin, string clear)
        {
            if (origin.Contains(clear))
            {
                return origin.Replace(clear, "");
            }

            return origin;
        }

        #endregion

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

        [Serializable]
        public sealed class AreaValue : Tree.Value
        {
            public string[] area = DefaultArea;

            public override bool Resettable => true;

            public override void Reset()
            {
                area = DefaultArea;
            }

            public override void Load(Tree.Value other)
            {
                if (other is AreaValue t)
                    area = t.area;
            }
        }
    }
}