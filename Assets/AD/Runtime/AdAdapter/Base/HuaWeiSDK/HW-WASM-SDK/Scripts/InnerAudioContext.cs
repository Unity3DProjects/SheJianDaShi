using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

namespace HWWASM
{
    /// <summary>
    /// 音频。
    /// </summary>
    public class InnerAudioContext
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void QG_CreateInnerAudioContext(string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_CreateInnerAudioContextWithParam(string callbackId, string src, bool loop, float startTime, bool autoplay, float volume, bool obeyMuteSwitch, bool needDownload);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextPlay(string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextPause(string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextStop(string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextSeek(float position, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextDestroy(string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextSetBool(string key, bool value, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextSetString(string key, string value, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextSetFloat(string key, float value, string callbackId);

        [DllImport("__Internal")]
        private static extern bool QG_InnerAudioContextGetBool(string key, string callbackId);

        [DllImport("__Internal")]
        private static extern float QG_InnerAudioContextGetFloat(string key, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextAddListener(string key, string callbackId);

        [DllImport("__Internal")]
        private static extern void QG_InnerAudioContextRemoveListener(string key, string callbackId);
#else
        private static void QG_CreateInnerAudioContext(string callbackId)
        {
        }
        private static void QG_CreateInnerAudioContextWithParam(string callbackId, string src, bool loop, float startTime, bool autoplay, float volume, bool obeyMuteSwitch, bool needDownload)
        {
        }

        private static void QG_InnerAudioContextPlay(string callbackId)
        {
        }

        private static void QG_InnerAudioContextPause(string callbackId)
        {
        }

        private static void QG_InnerAudioContextStop(string callbackId)
        {
        }

        private static void QG_InnerAudioContextSeek(float position, string callbackId)
        {
        }

        private static void QG_InnerAudioContextDestroy(string callbackId)
        {
        }

        private static void QG_InnerAudioContextSetBool(string key, bool value, string callbackId)
        {
        }

        private static void QG_InnerAudioContextSetString(string key, string value, string callbackId)
        {
        }

        private static void QG_InnerAudioContextSetFloat(string key, float value, string callbackId)
        {
        }

        private static bool QG_InnerAudioContextGetBool(string key, string callbackId)
        {
            return false;
        }
        private static float QG_InnerAudioContextGetFloat(string key, string callbackId)
        {
            return 0;
        }
        private static void QG_InnerAudioContextAddListener(string key, string callbackId)
        {
        }
        private static void QG_InnerAudioContextRemoveListener(string key, string callbackId)
        {
        }

#endif

        private readonly string _instanceId;

        private readonly string SRC = "src";

        private readonly string START_TIME = "startTime";

        private readonly string AUTO_PLAY = "autoplay";

        private readonly string LOOP = "loop";

        private readonly string OBEY_MUTE_SWITCH = "obeyMuteSwitch";

        private readonly string VOLUME = "volume";

        private readonly string DURATION = "duration";

        private readonly string CURRENT_TIME = "currentTime";

        private readonly string PAUSED = "paused";

        private readonly string BUFFERED = "buffered";

        private readonly string NEED_DOWNLOAD = "_needDownload";

        private bool _autoplay = false;

        private bool _loop = false;

        private string _src = "";

        private float _startTime = 0;

        private float _volume = 1;

        private bool _obeyMuteSwitch = true;

        private bool _needDownload = true;

        private bool _canPlay = false;

        private Action _onEndedCallback;

        private Action _onPlayCallback;

        private Action _onPauseCallback;

        private Action _onStopCallback;

        private Action<InnerAudioContextErrorResult> _onErrorCallback;

        private Action _onCanPlayCallback;

        private Action _onWaitingCallback;

        private Action _onSeekingCallback;

        private Action _onSeekedCallback;

        public InnerAudioContext(string callbackId, InnerAudioContextOption param)
        {
            _instanceId = callbackId;
            _src = param.src;
            _loop = param.loop;
            _startTime = param.startTime;
            _autoplay = param.autoplay;
            _volume = param.volume;
            _obeyMuteSwitch = param.obeyMuteSwitch;
            _needDownload = param.needDownload;
            QG_CreateInnerAudioContextWithParam(_instanceId, _src, _loop, _startTime, _autoplay, _volume, _obeyMuteSwitch, _needDownload);
        }

        /// <summary>
        /// 音频资源的地址，用于直接播放。
        /// </summary>
        public string Src
        {
            get => _src;
            set
            {
                QG_InnerAudioContextSetString(SRC, value, _instanceId);
                _src = value;
            }
        }

        /// <summary>
        /// 开始播放的位置（单位：s），默认为 0。
        /// </summary>
        public float StartTime
        {
            get => _startTime;
            set
            {
                QG_InnerAudioContextSetFloat(START_TIME, value, _instanceId);
                _startTime = value;
            }
        }

        /// <summary>
        /// 是否自动开始播放，默认为 false。
        /// </summary>
        public bool Autoplay
        {
            get => _autoplay;
            set
            {
                QG_InnerAudioContextSetBool(AUTO_PLAY, value, _instanceId);
                _autoplay = value;
            }
        }

        /// <summary>
        /// 是否循环播放，默认为 false。
        /// </summary>
        public bool Loop
        {
            get => _loop;
            set
            {
                QG_InnerAudioContextSetBool(LOOP, value, _instanceId);
                _loop = value;
            }
        }

        /// <summary>
        /// 是否遵循系统静音开关，默认为true。
        /// </summary>
        public bool ObeyMuteSwitch
        {
            get => _obeyMuteSwitch;
            set
            {
                QG_InnerAudioContextSetBool(OBEY_MUTE_SWITCH, value, _instanceId);
                _obeyMuteSwitch = value;
            }
        }

        /// <summary>
        /// 音量。范围 0 ~ 1。默认为 1。
        /// </summary>
        public float Volume
        {
            get => _volume;
            set
            {
                QG_InnerAudioContextSetFloat(VOLUME, value, _instanceId);
                _volume = value;
            }
        }

        /// <summary>
        /// 是否需要先下载后播放
        /// </summary>
        public bool NeedDownload
        {
            get => _needDownload;
            set
            {
                QG_InnerAudioContextSetBool(NEED_DOWNLOAD, value, _instanceId);
                _needDownload = value;
            }
        }

        /// <summary>
        /// 当前音频的长度（单位 s）。只有在当前有合法的src时返回（只读）。
        /// </summary>
        public float Duration => QG_InnerAudioContextGetFloat(DURATION, _instanceId);

        /// <summary>
        /// 当前音频的播放位置（单位 s）。只有在当前有合法的src时返回，时间保留小数点后6位（只读）。
        /// </summary>
        public float CurrentTime => QG_InnerAudioContextGetFloat(CURRENT_TIME, _instanceId);

        /// <summary>
        /// 当前是否为暂停或停止状态（只读）。
        /// </summary>
        public bool Paused => QG_InnerAudioContextGetBool(PAUSED, _instanceId);

        /// <summary>
        /// 音频缓冲的时间点，仅保证当前播放时间点到此时间点内容已缓冲（只读）。
        /// </summary>
        public float Buffered => QG_InnerAudioContextGetFloat(BUFFERED, _instanceId);

        /// <summary>
        /// 播放。
        /// </summary>
        public void Play()
        {
            QG_InnerAudioContextPlay(_instanceId);
        }

        /// <summary>
        /// 暂停。暂停后的音频再播放会从暂停处开始播放。
        /// </summary>
        public void Pause()
        {
            QG_InnerAudioContextPause(_instanceId);
        }

        /// <summary>
        /// 停止。停止后的音频再播放会从头开始播放。
        /// </summary>
        public void Stop()
        {
            QG_InnerAudioContextStop(_instanceId);
        }

        /// <summary>
        /// 跳转到指定位置。
        /// </summary>
        /// <param name="position">跳转的时间，单位 s。</param>
        public void Seek(float position)
        {
            QG_InnerAudioContextSeek(position, _instanceId);
        }

        /// <summary>
        /// 销毁当前实例。
        /// </summary>
        public void Destroy()
        {
            QG_InnerAudioContextDestroy(_instanceId);
            InnerAudioContextFactory.Instance._RemoveInnerAudioContext(_instanceId);
        }

        /// <summary>
        /// 监听音频自然播放至结束的事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnEnded(Action callback)
        {
            if (_onEndedCallback == null)
            {
                QG_InnerAudioContextAddListener("onEnded", _instanceId);
            }
            _onEndedCallback += callback;
        }

        /// <summary>
        /// 取消监听音频自然播放至结束的事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffEnded(Action callback = null)
        {
            if (callback == null)
            {
                _onEndedCallback = null;
            }
            else
            {
                _onEndedCallback -= callback;
            }
            if (_onEndedCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offEnded", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频播放事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnPlay(Action callback)
        {
            if (_onPlayCallback == null)
            {
                QG_InnerAudioContextAddListener("onPlay", _instanceId);
            }
            _onPlayCallback += callback;
        }

        /// <summary>
        /// 取消监听音频播放事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffPlay(Action callback = null)
        {
            if (callback == null)
            {
                _onPlayCallback = null;
            }
            else
            {
                _onPlayCallback -= callback;
            }
            if (_onPlayCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offPlay", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频暂停事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnPause(Action callback)
        {
            if (_onPauseCallback == null)
            {
                QG_InnerAudioContextAddListener("onPause", _instanceId);
            }
            _onPauseCallback += callback;
        }

        /// <summary>
        /// 取消监听音频暂停事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffPause(Action callback = null)
        {
            if (callback == null)
            {
                _onPauseCallback = null;
            }
            else
            {
                _onPauseCallback -= callback;
            }
            if (_onPauseCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offPause", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频停止事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnStop(Action callback)
        {
            if (_onStopCallback == null)
            {
                QG_InnerAudioContextAddListener("onStop", _instanceId);
            }
            _onStopCallback += callback;
        }

        /// <summary>
        /// 取消监听音频停止事件。
        /// <param name="callback">回调方法</param>
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffStop(Action callback = null)
        {
            if (callback == null)
            {
                _onStopCallback = null;
            }
            else
            {
                _onStopCallback -= callback;
            }
            if (_onStopCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offStop", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频播放错误事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnError(Action<InnerAudioContextErrorResult> callback)
        {
            if (_onErrorCallback == null)
            {
                QG_InnerAudioContextAddListener("onError", _instanceId);
            }
            _onErrorCallback += callback;
        }

        /// <summary>
        /// 取消监听音频播放错误事件。
        /// <param name="callback">回调方法</param>
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffError(Action<InnerAudioContextErrorResult> callback = null)
        {
            if (callback == null)
            {
                _onErrorCallback = null;
            }
            else
            {
                _onErrorCallback -= callback;
            }
            if (_onErrorCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offError", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频进入可以播放状态的事件，但不保证后面可以流畅播放。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnCanPlay(Action callback)
        {
            _onCanPlayCallback += callback;
             if (_canPlay) {
                _onCanPlayCallback?.Invoke();
             }
        }

        /// <summary>
        /// 取消监听音频进入可以播放状态的事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffCanPlay(Action callback = null)
        {
            if (callback == null)
            {
                _onCanPlayCallback = null;
            }
            else
            {
                _onCanPlayCallback -= callback;
            }
            if (_onCanPlayCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offCanplay", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频加载中事件。当音频因为数据不足，需要停下来加载时会触发。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnWaiting(Action callback)
        {
            if (_onWaitingCallback == null)
            {
                QG_InnerAudioContextAddListener("onWaiting", _instanceId);
            }
            _onWaitingCallback += callback;
        }

        /// <summary>
        /// 取消监听音频加载中事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffWaiting(Action callback = null)
        {
            if (callback == null)
            {
                _onWaitingCallback = null;
            }
            else
            {
                _onWaitingCallback -= callback;
            }
            if (_onWaitingCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offWaiting", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频进行跳转操作的事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnSeeking(Action callback)
        {
            if (_onSeekingCallback == null)
            {
                QG_InnerAudioContextAddListener("onSeeking", _instanceId);
            }
            _onSeekingCallback += callback;
        }

        /// <summary>
        /// 取消监听音频进行跳转操作的事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffSeeking(Action callback = null)
        {
            if (callback == null)
            {
                _onSeekingCallback = null;
            }
            else
            {
                _onSeekingCallback -= callback;
            }
            if (_onSeekingCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offSeeking", _instanceId);
            }
        }

        /// <summary>
        /// 监听音频完成跳转操作的事件。
        /// </summary>
        /// <param name="callback">回调方法</param>
        public void OnSeeked(Action callback)
        {
            if (_onSeekedCallback == null)
            {
                QG_InnerAudioContextAddListener("onSeeked", _instanceId);
            }
            _onSeekedCallback += callback;
        }

        /// <summary>
        /// 取消监听音频完成跳转操作的事件。
        /// </summary>
        /// <param name="callback">需要取消监听的回调方法</param>
        public void OffSeeked(Action callback = null)
        {
            if (callback == null)
            {
                _onSeekedCallback = null;
            }
            else
            {
                _onSeekedCallback -= callback;
            }
            if (_onSeekedCallback == null)
            {
                QG_InnerAudioContextRemoveListener("offSeeked", _instanceId);
            }
        }

        /// <summary>
        /// 内部函数,请不要调用
        /// </summary>
        /// <param name="callbackFunc">回调方法名</param>
        /// <param name="res">回调参数</param>
        public void _HandleCallBack(string callbackFunc, InnerAudioContextErrorResult res)
        {
            switch (callbackFunc){
                case "onCanplay":
                    _onCanPlayCallback?.Invoke();
                    _canPlay = true;
                    break;
                case "onPlay":
                    _onPlayCallback?.Invoke();
                    break;
                case "onPause":
                    _onPauseCallback?.Invoke();
                    break;
                case "onStop":
                    _onStopCallback?.Invoke();
                    break;
                case "onEnded":
                    _onEndedCallback?.Invoke();
                    break;
                case "onError":
                    _onErrorCallback?.Invoke(res);
                    break;
                case "onWaiting":
                    _onWaitingCallback?.Invoke();
                    break;
                case "onSeeking":
                    _onSeekingCallback?.Invoke();
                    break;
                case "onSeeked":
                    _onSeekedCallback?.Invoke();
                    break;
            }
        }
    }
}