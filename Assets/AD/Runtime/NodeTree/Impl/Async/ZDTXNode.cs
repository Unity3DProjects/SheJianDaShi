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
    public sealed class ZDTXNode : OnceGlobalInitNode<ZDTXNode>
    {
        private static int ggType;

        private static int GGType
        {
            get => ggType;
            set
            {
                switch (value)
                {
                    case 1:
                        ggType = value;
                        break;
                    case 2:
                        ggType = value;
                        break;
                    case 3:
                        ggType = value;
                        break;
                    default:
                        ggType = 1;
                        Debug.LogError($"Error GGType {value}");
                        break;
                }
            }
        }


        private INode gg1, gg2, gg3;

        public ZDTXNode(INode gg1, INode gg2, INode gg3)
        {
            this.gg1 = gg1;
            this.gg2 = gg2;
            this.gg3 = gg3;
        }

        private string URL =>
            $"http://datacenter.zywxgames.com:15855/api/index/params?pkm={zdtxValue.包名}&canshu={zdtxValue.渠道号}&url=new&yys=yd";

        private ZDTXValue zdtxValue = new ZDTXValue();


        public override void InitTree(Tree tree)
        {
            tree.Data.Add(new Tree.Key("指点天下屏蔽"), zdtxValue);
            using (tree.Data.CreateGroup(nameof(gg1))) gg1.InitTree(tree);
            using (tree.Data.CreateGroup(nameof(gg2))) gg2.InitTree(tree);
            using (tree.Data.CreateGroup(nameof(gg3))) gg3.InitTree(tree);
        }


        protected override void OnceGlobalInit()
        {
            switch (ADManager.Instance.Platform)
            {
                case EPlatform.OPPO:
#if UNITY_EDITOR
                    StartRequest();
#else
                ADManager.Instance.BindReceiver("OppoZDTXShield", HandleResult);
                OppoAdAdapter.ZDTX(zdtxValue.包名,zdtxValue.渠道号, "OppoZDTXShield");
#endif
                    break;
                default:
                    StartRequest();
                    break;
            }

            OnlyOnceInit = false;
        }

        protected override void OnceInit()
        {
            NodeExtensions.Init(gg1, gg2, gg3);
        }

        private Queue<AdSourceType> queue = new Queue<AdSourceType>();

        protected override void OnCall(AdSourceType type)
        {
            if (ggType == 0)
            {
                queue.Enqueue(type);
                return;
            }

            bool result;
            switch (GGType)
            {
                case 1:
                    if (gg1.TryCall(type, out result))
                        Complete(result);
                    break;
                case 2:
                    if (gg2.TryCall(type, out result))
                        Complete(result);
                    break;
                case 3:
                    if (gg3.TryCall(type, out result))
                        Complete(result);
                    break;
            }
        }

        private void StartRequest()
        {
            var request = new UnityWebRequest();
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            request.downloadHandler = downloadHandler;
            request.url = URL;
            this.Log(URL);
            request.method = UnityWebRequest.kHttpVerbGET;
            request.SendWebRequest().completed += op =>
            {
#if UNITY_2020_3_OR_NEWER
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    HandleResult(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(request.error);
                    RequestFallback();
                    break;
                case UnityWebRequest.Result.InProgress:
                    Debug.LogError("请求中");
                    RequestFallback();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("请求成功但是连接协议错误");
                    RequestFallback();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("数据错误");
                    RequestFallback();
                    break;
                default:
                    Debug.Log("请求不到");
                    RequestFallback();
                    break;
            }
#else
                if (!string.IsNullOrWhiteSpace(request.error))
                {
                    Debug.LogError(request.error);
                    RequestFallback();
                }
                else
                {
                    HandleResult(request.downloadHandler.text);
                }

#endif
                request.Dispose();
            };
        }

        private void HandleResult(string msg)
        {
            string[] split = msg.Split('&');
            if (split.Length > 1)
            {
                string result = split[1];
                Debug.LogError($"接收到 {result}");
                switch (result)
                {
                    case "0":
                        GGType = 1;
                        break;
                    case "1":
                        GGType = 2;
                        break;
                    case "2":
                        GGType = 3;
                        break;
                    default:
                        GGType = 1;
                        break;
                }
            }
            else GGType = 1;

            this.Log($"指点天下开关:{GGType}");
            foreach (var type in queue) Call(type);
            queue.Clear();
        }

        private void RequestFallback()
        {
            GGType = 1;
            foreach (var type in queue) Call(type);
            queue.Clear();
        }

        private static string GetChannel()
        {
#if VIVO
        return "VIVO1";
#elif OPPO
        return "OPPO1";
#elif XiaoMi
        return "XIAOMI1";
#elif RongYao
        return "RY1";
#else
            return "";
#endif
        }

        [Serializable]
        public class ZDTXValue : Tree.Value
        {
            public string 包名;
            public string 渠道号 = GetChannel();

            public override void Load(Tree.Value other)
            {
                if (other is ZDTXValue v)
                {
                    包名 = v.包名;
                    渠道号 = v.渠道号;
                }
            }

            public override bool Resettable => true;

            public override void Reset()
            {
                渠道号 = GetChannel();
            }
        }
    }
}