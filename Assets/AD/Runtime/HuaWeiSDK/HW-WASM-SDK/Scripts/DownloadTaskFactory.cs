using System.Collections.Generic;
using UnityEngine;

namespace HWWASM
{
    public class DownloadTaskFactory
    {
        private readonly Dictionary<string, DownloadTask> _dictionary = new Dictionary<string, DownloadTask>();
        
        public static DownloadTaskFactory Instance { get; } = new DownloadTaskFactory();
        
        public DownloadTask DownloadFile(DownloadFileOption downloadFileOption)
        {
            string callbackId = RTCallback<DownloadFileSuccessResult, DownloadFileFailResult>.AddCallback(
                downloadFileOption.success,
                downloadFileOption.fail,
                downloadFileOption.complete
            );
            DownloadTask downloadTask = new DownloadTask(downloadFileOption, callbackId);
            _dictionary.Add(callbackId, downloadTask);
            return downloadTask;
        }
        
        public DownloadTask _GetDownloadTask(string callbackId)
        {
            return _dictionary.ContainsKey(callbackId) ? _dictionary[callbackId] : null;
        }
    }
}