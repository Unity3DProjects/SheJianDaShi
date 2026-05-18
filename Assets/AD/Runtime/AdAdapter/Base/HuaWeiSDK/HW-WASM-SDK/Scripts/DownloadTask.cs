using System;
using UnityEngine;
using System.Runtime.InteropServices;
namespace HWWASM
{
    public class DownloadTask
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void QG_DownloadFile(string conf, string callbackId);
        
        [DllImport("__Internal")]
        private static extern void QG_DownloadTaskAbort(string callbackId);
#else
        private static void QG_DownloadFile(string conf, string callbackId)
        {
        }
        
        private static void QG_DownloadTaskAbort(string callbackId)
        {
        }
        
#endif
        private readonly string _callbackId;
        
        private Action<DownloadTaskOnProgressUpdateResult> _downloadTaskOnProgressUpdateCallback;
        
        public DownloadTask(DownloadFileOption downloadFileOption, string callbackId)
        {
            _callbackId = callbackId;
            string conf = JsonUtility.ToJson(downloadFileOption);
            QG_DownloadFile(conf, callbackId);
        }
        
        /// <summary>
        /// 中断下载任务。
        /// [DownloadTask.abort()](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-download-0000001083746136#section7213192712255)
        /// </summary>
        public void Abort()
        {
            QG_DownloadTaskAbort(_callbackId);
        }
        
        /// <summary>
        /// 监听下载进度变化事件。
        /// [DownloadTask.onProgressUpdate(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-download-0000001083746136#section14594135102614)
        /// </summary>
        /// <param name="callback">回调方法。</param>
        public void OnProgressUpdate(Action<DownloadTaskOnProgressUpdateResult> callback)
        {
            _downloadTaskOnProgressUpdateCallback = callback;
        }
        
        public Action<DownloadTaskOnProgressUpdateResult> _GetOnProgressUpdateCallback()
        {
            return _downloadTaskOnProgressUpdateCallback;
        }
    }
}