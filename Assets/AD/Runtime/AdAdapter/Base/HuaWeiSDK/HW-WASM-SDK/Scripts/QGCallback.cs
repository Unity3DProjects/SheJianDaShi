using System;
using System.Collections.Generic;
using UnityEngine;

namespace HWWASM
{
    public class CallbackBase
    {
        public string _callbackId = null;
        public RTCallbackType _callbackType = RTCallbackType.SUCCESS;
    }

    public class ActionParamBase<TS, TF>
    {
        /// <summary>
        /// 接口调用成功的回调函数。
        /// </summary>
        public Action<TS> success;

        /// <summary>
        /// 接口调用失败的回调函数。
        /// </summary>
        public Action<TF> fail;

        /// <summary>
        /// 接口调用结束的回调函数（调用成功、失败都会回调）。
        /// </summary>
        public Action complete;
    }
    
    public class ActionParamBaseSuc<TS>
    {
        /// <summary>
        /// 接口调用成功的回调函数。
        /// </summary>
        public Action<TS> success;

        /// <summary>
        /// 接口调用失败的回调函数。
        /// </summary>
        public Action fail;

        /// <summary>
        /// 接口调用结束的回调函数（调用成功、失败都会回调）。
        /// </summary>
        public Action complete;
    }

    public class ActionParamBaseFail<TF>
    {
        /// <summary>
        /// 接口调用成功的回调函数。
        /// </summary>
        public Action success;

        /// <summary>
        /// 接口调用失败的回调函数。
        /// </summary>
        public Action<TF> fail;

        /// <summary>
        /// 接口调用结束的回调函数（调用成功、失败都会回调）。
        /// </summary>
        public Action complete;
    }

    public class ActionParamBase
    {
        /// <summary>
        /// 接口调用成功的回调函数。
        /// </summary>
        public Action success;

        /// <summary>
        /// 接口调用失败的回调函数。
        /// </summary>
        public Action fail;

        /// <summary>
        /// 接口调用结束的回调函数（调用成功、失败都会回调）。
        /// </summary>
        public Action complete;
    }

    public enum RTCallbackType
    {
        SUCCESS = 0,
        FAIL = 1,
        COMPLETE = 2
    }

    /// <summary>
    /// 回调存储类
    /// </summary>
    /// <typeparam name="TFail">fail回调结果对象的类型</typeparam>
    class RTCallbackFail<TFail>
    {
        private Action _successCallback;
        private Action<TFail> _failCallback;
        private Action _completeCallback;

        private static readonly Dictionary<string, RTCallbackFail<TFail>> CallbackDict =
            new Dictionary<string, RTCallbackFail<TFail>>();

        public static string AddCallback(
            Action successCallback,
            Action<TFail> failCallback,
            Action completeCallback
        )
        {
            RTCallbackFail<TFail> callback = new RTCallbackFail<TFail>();
            if (successCallback != null)
            {
                callback._successCallback += successCallback;
            }

            if (failCallback != null)
            {
                callback._failCallback += failCallback;
            }

            if (completeCallback != null)
            {
                callback._completeCallback += completeCallback;
            }

            string callbackId = Guid.NewGuid().ToString();
            CallbackDict.Add(callbackId, callback);
            return callbackId;
        }

        public static void OnCallback(string res)
        {
            CallbackBase obj = JsonUtility.FromJson<CallbackBase>(res);
            RTCallbackFail<TFail> callback = CallbackDict[obj._callbackId];
            if (callback == null)
            {
                return;
            }

            switch (obj._callbackType)
            {
                case RTCallbackType.SUCCESS:
                    if (callback._successCallback != null)
                    {
                        callback._successCallback.Invoke();
                        callback._successCallback = null;
                    }

                    break;
                case RTCallbackType.FAIL:
                    if (callback._failCallback != null)
                    {
                        TFail failResp = JsonUtility.FromJson<TFail>(res);
                        callback._failCallback.Invoke(failResp);
                        callback._failCallback = null;
                    }

                    break;
                case RTCallbackType.COMPLETE:
                    if (callback._completeCallback != null)
                    {
                        callback._completeCallback.Invoke();
                        callback._completeCallback = null;
                    }

                    CallbackDict.Remove(obj._callbackId);
                    break;
            }
        }
    }

    /// <summary>
    /// 回调存储类
    /// </summary>
    /// <typeparam name="TSuc">success回调结果对象的类型</typeparam>
    /// <typeparam name="TFail">fail回调结果对象的类型</typeparam>
    class RTCallback<TSuc, TFail>
        where TSuc : CallbackBase where TFail : CallbackBase
    {
        private Action<TSuc> _successCallback;
        private Action<TFail> _failCallback;
        private Action _completeCallback;

        private static readonly Dictionary<string, RTCallback<TSuc, TFail>> CallbackDict =
            new Dictionary<string, RTCallback<TSuc, TFail>>();

        public static string AddCallback(
            Action<TSuc> successCallback,
            Action<TFail> failCallback,
            Action completeCallback
        )
        {
            RTCallback<TSuc, TFail> callback = new RTCallback<TSuc, TFail>();
            if (successCallback != null)
            {
                callback._successCallback += successCallback;
            }

            if (failCallback != null)
            {
                callback._failCallback += failCallback;
            }

            if (completeCallback != null)
            {
                callback._completeCallback += completeCallback;
            }

            string callbackId = Guid.NewGuid().ToString();
            CallbackDict.Add(callbackId, callback);
            return callbackId;
        }

        public static void OnCallback(string res)
        {
            CallbackBase obj = JsonUtility.FromJson<CallbackBase>(res);
            RTCallback<TSuc, TFail> callback = CallbackDict[obj._callbackId];
            if (callback == null)
            {
                return;
            }

            switch (obj._callbackType)
            {
                case RTCallbackType.SUCCESS:
                    if (callback._successCallback != null)
                    {
                        TSuc successResp = JsonUtility.FromJson<TSuc>(res);
                        callback._successCallback.Invoke(successResp);
                        callback._successCallback = null;
                    }

                    break;
                case RTCallbackType.FAIL:
                    if (callback._failCallback != null)
                    {
                        TFail failResp = JsonUtility.FromJson<TFail>(res);
                        callback._failCallback.Invoke(failResp);
                        callback._failCallback = null;
                    }

                    break;
                case RTCallbackType.COMPLETE:
                    if (callback._completeCallback != null)
                    {
                        callback._completeCallback.Invoke();
                        callback._completeCallback = null;
                    }

                    CallbackDict.Remove(obj._callbackId);
                    break;
            }
        }
    }
    
    /// <summary>
    /// 回调存储类
    /// </summary>
    /// <typeparam name="TSuc">success回调结果对象的类型</typeparam>
    class RTCallbackSuc<TSuc>
    {
        private Action<TSuc> _successCallback;
        private Action _failCallback;
        private Action _completeCallback;

        private static readonly Dictionary<string, RTCallbackSuc<TSuc>> CallbackDict =
            new Dictionary<string, RTCallbackSuc<TSuc>>();

        public static string AddCallback(
            Action<TSuc> successCallback,
            Action failCallback,
            Action completeCallback
        )
        {
            RTCallbackSuc<TSuc> callback = new RTCallbackSuc<TSuc>();
            if (successCallback != null)
            {
                callback._successCallback += successCallback;
            }

            if (failCallback != null)
            {
                callback._failCallback += failCallback;
            }

            if (completeCallback != null)
            {
                callback._completeCallback += completeCallback;
            }

            string callbackId = Guid.NewGuid().ToString();
            CallbackDict.Add(callbackId, callback);
            return callbackId;
        }

        public static void OnCallback(string res)
        {
            CallbackBase obj = JsonUtility.FromJson<CallbackBase>(res);
            RTCallbackSuc<TSuc> callback = CallbackDict[obj._callbackId];
            if (callback == null)
            {
                return;
            }

            switch (obj._callbackType)
            {
                case RTCallbackType.SUCCESS:
                    if (callback._successCallback != null)
                    {
                        TSuc successResp = JsonUtility.FromJson<TSuc>(res);
                        callback._successCallback.Invoke(successResp);
                        callback._successCallback = null;
                    }

                    break;
                case RTCallbackType.FAIL:
                    if (callback._failCallback != null)
                    {
                        callback._failCallback.Invoke();
                        callback._failCallback = null;
                    }

                    break;
                case RTCallbackType.COMPLETE:
                    if (callback._completeCallback != null)
                    {
                        callback._completeCallback.Invoke();
                        callback._completeCallback = null;
                    }

                    CallbackDict.Remove(obj._callbackId);
                    break;
            }
        }
    }

    /// <summary>
    /// 回调存储类
    /// </summary>
    class RTCallback
    {
        private Action _successCallback;
        private Action _failCallback;
        private Action _completeCallback;

        private static readonly Dictionary<string, RTCallback> CallbackDict =
            new Dictionary<string, RTCallback>();

        public static string AddCallback(
            Action successCallback,
            Action failCallback,
            Action completeCallback
        )
        {
            RTCallback callback = new RTCallback();
            if (successCallback != null)
            {
                callback._successCallback += successCallback;
            }

            if (failCallback != null)
            {
                callback._failCallback += failCallback;
            }

            if (completeCallback != null)
            {
                callback._completeCallback += completeCallback;
            }

            string callbackId = Guid.NewGuid().ToString();
            CallbackDict.Add(callbackId, callback);
            return callbackId;
        }

        public static void OnCallback(string res)
        {
            CallbackBase obj = JsonUtility.FromJson<CallbackBase>(res);
            RTCallback callback = CallbackDict[obj._callbackId];
            if (callback == null)
            {
                return;
            }

            switch (obj._callbackType)
            {
                case RTCallbackType.SUCCESS:
                    if (callback._successCallback != null)
                    {
                        callback._successCallback.Invoke();
                        callback._successCallback = null;
                    }

                    break;
                case RTCallbackType.FAIL:
                    if (callback._failCallback != null)
                    {
                        callback._failCallback.Invoke();
                        callback._failCallback = null;
                    }

                    break;
                case RTCallbackType.COMPLETE:
                    if (callback._completeCallback != null)
                    {
                        callback._completeCallback.Invoke();
                        callback._completeCallback = null;
                    }

                    CallbackDict.Remove(obj._callbackId);
                    break;
            }
        }
    }

    class FSReadFileCallback
    {
        private static Dictionary<string, _ReadFileCallback> _readFileCallbackDict = new Dictionary<string, _ReadFileCallback>();

        private class _ReadFileCallback
        {
            public Action<ReadFileBinarySuccessResult> successCallback;
            public Action<ReadFileFailResult> failCallback;
            public Action completeCallback;
        }

        public static string AddCallback(
            Action<ReadFileBinarySuccessResult> successCallback,
            Action<ReadFileFailResult> failCallback,
            Action completeCallback
        )
        {
            _ReadFileCallback callback = new _ReadFileCallback();
            if (successCallback != null)
            {
                callback.successCallback += successCallback;
            }

            if (failCallback != null)
            {
                callback.failCallback += failCallback;
            }

            if (completeCallback != null)
            {
                callback.completeCallback += completeCallback;
            }

            string callbackId = Guid.NewGuid().ToString();
            _readFileCallbackDict.Add(callbackId, callback);
            return callbackId;
        }

        public static void OnCallback(string res)
        {
            _ReadFileJSON json = JsonUtility.FromJson<_ReadFileJSON>(res);
            string callbackId = json._callbackId;
            _ReadFileCallback callback = _readFileCallbackDict[callbackId];
            switch (json._callbackType)
            {
                case RTCallbackType.SUCCESS:
                    if (callback.successCallback != null)
                    {
                        byte[] buffer = new byte[json.data];
                        if (json.data > 0)
                        {
                            FileSystemManager.QG_CopyDataFromJs(callbackId, buffer);
                        }

                        ReadFileBinarySuccessResult obj = new ReadFileBinarySuccessResult
                        {
                            _callbackId = callbackId,
                            _callbackType = json._callbackType,
                            data = buffer
                        };
                        callback.successCallback.Invoke(obj);
                        callback.successCallback = null;
                    }
                    break;
                case RTCallbackType.FAIL:
                    if (callback.failCallback != null)
                    {
                        ReadFileFailResult obj = new ReadFileFailResult
                        {
                            _callbackId = callbackId,
                            _callbackType = json._callbackType,
                            errCode = json.errCode,
                            errMsg = json.errMsg,
                        };
                        callback.failCallback.Invoke(obj);
                        callback.failCallback = null;
                    }
                    break;
                case RTCallbackType.COMPLETE:
                    if (callback.completeCallback != null)
                    {
                        callback.completeCallback.Invoke();
                        callback.completeCallback = null;
                    }
                    _readFileCallbackDict.Remove(callbackId);
                    break;
            }
        }

        private class _ReadFileJSON : CallbackBase
        {
            public int errCode = 0;
            public string errMsg = null;
            public int data = 0;
        }
    }
}