using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace HWWASM
{
    /// <summary>
    /// 激励视频广告。
    /// </summary>
    public class RewardedVideoAd
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void QG_CreateRewardedVideoAd(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_RewardedVideoAdLoad(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_RewardedVideoAdShow(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_RewardedVideoAdDestroy(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_SetTagForChildProtection(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_SetTagForUnderAgeOfPromise(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_SetAdContentClassification(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_SetNonPersonalizedAd(string conf, string callbackId);
#else
        private static void QG_CreateRewardedVideoAd(string conf, string callbackId)
        {
        }

        private static void QG_RewardedVideoAdLoad(string callbackId)
        {
        }

        private static void QG_RewardedVideoAdShow(string callbackId)
        {
        }

        private static void QG_RewardedVideoAdDestroy(string callbackId)
        {
        }

        private static void QG_SetTagForChildProtection(string conf, string callbackId)
        {
        }

        private static void QG_SetTagForUnderAgeOfPromise(string conf, string callbackId)
        {
        }

        private static void QG_SetAdContentClassification(string conf, string callbackId)
        {
        }

        private static void QG_SetNonPersonalizedAd(string conf, string callbackId)
        {
        }
#endif

        private readonly string _callbackId;

        private Action _loadCallback;

        private Action<RewardedVideoAdCloseResult> _closeCallback;

        private Action<AdErrorResult> _errorCallback;

        public RewardedVideoAd(CreateRewardedVideoAdOption option, string callbackId)
        {
            _callbackId = callbackId;
            QG_CreateRewardedVideoAd(JsonUtility.ToJson(option), _callbackId);
        }

        /// <summary>
        /// 手动拉取广告，用于刷新广告。成功回调OnLoad设置的回调，失败回调OnError设置的回调。
        /// </summary>
        public void Load()
        {
            QG_RewardedVideoAdLoad(_callbackId);
        }

        /// <summary>
        /// 激励视频广告组件默认是隐藏的，调用 show 方法展示广告。失败回调OnError设置的回调。
        /// </summary>
        public void Show()
        {
            QG_RewardedVideoAdShow(_callbackId);
        }

        /// <summary>
        /// 设置广告加载成功回调。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnLoad(Action callback)
        {
            _loadCallback = callback;
        }

        /// <summary>
        /// 移除激励视频广告加载成功监听。
        /// </summary>
        public void OffLoad()
        {
            _loadCallback = null;
        }

        /// <summary>
        /// 监听激励视频广告关闭事件。只有在用户主动关闭激励视频广告时，广告才会关闭。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnClose(Action<RewardedVideoAdCloseResult> callback)
        {
            _closeCallback = callback;
        }

        /// <summary>
        /// 移除激励视频广告关闭监听。
        /// </summary>
        public void OffClose()
        {
            _closeCallback = null;
        }

        /// <summary>
        /// 监听激励视频广告加载错误事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnError(Action<AdErrorResult> callback)
        {
            _errorCallback = callback;
        }

        /// <summary>
        /// 移除激励视频广告加载错误监听。
        /// </summary>
        public void OffError()
        {
            _errorCallback = null;
        }

        /// <summary>
        /// 设置儿童保护标签。
        /// <para>-1：您不希望表明您的广告内容是否需要符合COPPA的规定。</para>
        /// <para>0：表明您的广告内容不需要符合COPPA的规定。</para>
        /// <para>1：表明您的广告内容需要符合COPPA的规定（该广告请求无法获取到任何广告）。</para>
        /// </summary>
        /// <param name="childProtection">儿童保护标签参数</param>
        public void SetTagForChildProtection(int childProtection)
        {
            SetTagForChildProtectionOption option = new SetTagForChildProtectionOption
                { childProtection = childProtection };
            QG_SetTagForChildProtection(JsonUtility.ToJson(option), _callbackId);
        }

        /// <summary>
        /// 设置面向未达到法定承诺年龄用户标签。
        /// <para>-1: 表明您尚未指定广告请求是否要符合未达到法定承诺年龄用户的广告标准。</para>
        /// <para>0: 表明您不希望广告请求符合未达到法定承诺年龄用户的广告标准。</para>
        /// <para>1: 表明您希望广告请求符合未达到法定承诺年龄用户的广告标准。</para>
        /// </summary>
        /// <param name="underAgeOfPromiseStr">未达到法定承诺年龄用户的设置参数</param>
        public void SetTagForUnderAgeOfPromise(int underAgeOfPromiseStr)
        {
            SetTagForUnderAgeOfPromiseOption option = new SetTagForUnderAgeOfPromiseOption
                { underAgeOfPromiseStr = underAgeOfPromiseStr };
            QG_SetTagForUnderAgeOfPromise(JsonUtility.ToJson(option), _callbackId);
        }

        /// <summary>
        /// 设置广告内容分级上限。
        /// <para>W：适合幼儿及以上年龄段观众的内容。</para>
        /// <para>PI：适合少儿及以上年龄段观众的内容。</para>
        /// <para>J：适合青少年及以上年龄段观众的内容。</para>
        /// <para>A：仅适合成人观众的内容。</para>
        /// </summary>
        /// <param name="adContentClassification">广告内容类型参数</param>
        public void SetAdContentClassification(string adContentClassification)
        {
            SetAdContentClassificationOption option = new SetAdContentClassificationOption
                { adContentClassification = adContentClassification };
            QG_SetAdContentClassification(JsonUtility.ToJson(option), _callbackId);
        }

        /// <summary>
        /// 设置是否请求非个性化广告。
        /// <para>0：请求个性化广告与非个性化广告。</para>
        /// <para>1：请求非个性化广告。</para>
        /// </summary>
        /// <param name="personalizedAd">是否请求非个性化广告参数</param>
        public void SetNonPersonalizedAd(int personalizedAd)
        {
            SetNonPersonalizedAdOption option = new SetNonPersonalizedAdOption { personalizedAd = personalizedAd };
            QG_SetNonPersonalizedAd(JsonUtility.ToJson(option), _callbackId);
        }

        /// <summary>
        /// 销毁激励视频广告。
        /// </summary>
        public void Destroy()
        {
            QG_RewardedVideoAdDestroy(_callbackId);
            RewardedVideoAdFactory.Instance._RemoveRewardedVideoAd(_callbackId);
        }

        public Action _GetLoadCallback()
        {
            return _loadCallback;
        }

        public Action<RewardedVideoAdCloseResult> _GetCloseCallback()
        {
            return _closeCallback;
        }

        public Action<AdErrorResult> _GetErrorCallback()
        {
            return _errorCallback;
        }

        private class SetTagForChildProtectionOption
        {
            public int childProtection;
        }

        private class SetTagForUnderAgeOfPromiseOption
        {
            public int underAgeOfPromiseStr;
        }

        private class SetAdContentClassificationOption
        {
            public string adContentClassification;
        }

        private class SetNonPersonalizedAdOption
        {
            public int personalizedAd;
        }
    }
}