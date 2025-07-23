using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QG
    {

        public static string Provider_Vivo = "vivo";
        public static string Provider_OPPO = "OPPO";
        public static string Provider_Xiaomi = "xiaomi";
        public static string Provider_Redmi = "Redmi";
        public static string Provider_HONOR = "HONOR";

        // 平台能力
        #region 统一SDK Login  登录
        // http://test.gamedoc.so-quick.cn/api2.0/tutorial/%E7%BB%9F%E4%B8%80SDK.html#13-%E5%AE%A2%E6%88%B7%E7%AB%AF-sdk-api-%E4%BD%BF%E7%94%A8
        //QG.Login(
        //(msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
        ///);
        public static void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.Login(successCallback, failCallback);
        }
        #endregion

        #region 统一SDK Pay 支付
        //  http://test.gamedoc.so-quick.cn/api2.0/tutorial/%E7%BB%9F%E4%B8%80SDK.html#13-%E5%AE%A2%E6%88%B7%E7%AB%AF-sdk-api-%E4%BD%BF%E7%94%A8
        /*PayParam param = new PayParam()
        {
            orderAmount = 1,
            productName = "xxxx",
            productDesc = "dddd",
            productId = "xxxx"，
            externalInfo = "xxxx"
        };
        QG.Pay(
            param,
           (msg) => { Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay cancel = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay complete = " + JsonUtility.ToJson(msg)); }
            );
         */
        public static void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null, Action<QGCommonResponse<QGPayBean>> cancelCallback = null, Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGMiniGameManager.Instance.Pay(param, successCallback, failCallback, cancelCallback, completeCallback);
        }
        #endregion

        #region vivo Pay 支付
        //  https://minigame.vivo.com.cn/documents/#/api/service/newpay
        /* vivoPayParam param = new vivoPayParam()
        {
            appId = "123",
            cpOrderNumber = "234",
            productName = "xxxx",
            productDesc = "dddd",
            orderAmount = 1,
            notifyUrl = "mmnmn",
            expireTime = "2565",
            extInfo = "8955",
            vivoSignature = "98898"
        };
        QG.Pay(
            param,
           (msg) => { Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay cancel = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.Pay complete = " + JsonUtility.ToJson(msg)); }
            );
         */
        public static void Pay(vivoPayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null, Action<QGCommonResponse<QGPayBean>> cancelCallback = null, Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGMiniGameManager.Instance.Pay(param, successCallback, failCallback, cancelCallback, completeCallback);
        }
        #endregion

        #region 统一SDK广告对象

        /// <summary>
        /// 获取 qg.env.USER_DATA_PATH 文件本地缓存路径
        public static JsonData GetAdInfo()
        {
            return QGMiniGameManager.Instance.GetAdInfo();
        }
        #endregion

        #region CreateBannerAd  创建Banner广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/banner-ad
        //var bannerAd = QG.CreateBannerAd(new QGCreateBannerAdParam()
        //{
        //posId = "fb6051b85cf046a7b3410deef10910ac"
        //});
        //bannerAd.OnLoad(() => {
        //bannerAd.Show(
        //(msg) => { Debug.Log("QG.bannerAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.bannerAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //bannerAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.bannerAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        public static QGBannerAd CreateBannerAd(QGCreateBannerAdParam param)
        {
            return QGMiniGameManager.Instance.CreateBannerAd(param);
        }
        #endregion

        #region CreateInterstitialAd  创建插屏广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/interstitial-ad
        //interstitialAd = QG.CreateInterstitialAd(new QGCommonAdParam()
        //{
        //posId = "a11c3c4d0637485abf41024e6191c9cc"
        //});
        //interstitialAd.OnLoad(() => {
        //interstitialAd.Show(
        //(msg) => { Debug.Log("QG.interstitialAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.interstitialAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //interstitialAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.interstitialAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        public static QGInterstitialAd CreateInterstitialAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateInterstitialAd(param);
        }
        #endregion

        #region CreateRewardedVideoAd  创建激励视频广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/incentive-video-ad
        //var rewardedVideoAd = QG.CreateRewardedVideoAd(new QGCommonAdParam()
        //{
        //posId = "cf6d0f43846341828291afe78b816551"
        //});
        //rewardedVideoAd.OnLoad(() => {
        //rewardedVideoAd.Show(
        //(msg) => { Debug.Log("QG.rewardedVideoAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.rewardedVideoAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //rewardedVideoAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.rewardedVideoAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        //rewardedVideoAd.OnClose((QGRewardedVideoResponse msg) =>
        //{
        //if (msg.isEnded) {
        //Debug.Log("QG.rewardedVideoAd.OnClose success = " +  " 播放成功");
        //}
        //});
        public static QGRewardedVideoAd CreateRewardedVideoAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateRewardedVideoAd(param);
        }
        #endregion

        #region CreateCustomAd  创建模板广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/custom-ad
        //var customAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
        //{
        //posId = "25a289ebd99b4ccd99c49524931f97a0"
        //});
        //customAd.OnLoad(() => {
        //Debug.Log("QG.customAd.OnLoad success = ");
        //customAd.Show(
        //(msg) => { Debug.Log("QG.customAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.customAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //customAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.customAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        //customAd.OnHide(() =>
        //{
        //Debug.Log("QG.customAd.OnHide success ");
        //});
        public static QGCustomAd CreateCustomAd(QGCreateCustomAdParam param)
        {
            return QGMiniGameManager.Instance.CreateCustomAd(param);
        }
        #endregion


        // 系统信息
        #region GetSystemInfo 异步获取系统信息

        public static void GetSystemInfo(Action<QGCommonResponse<QGSystemInfo>> successCallback = null, Action<QGCommonResponse<QGSystemInfo>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetSystemInfo(successCallback, failCallback);
        }

        #endregion

        #region GetSystemInfoSync 同步获取系统信息

        public static QGSystemInfo GetSystemInfoSync()
        {
            return QGMiniGameManager.Instance.GetSystemInfoSync();
        }

        #endregion

        #region getWindowInfo 获取窗口信息

        public static QGWindowInfo GetWindowInfo()
        {
            return QGMiniGameManager.Instance.GetWindowInfo();
        }

        public static MenuButtonBoundingClientRect GetMenuButtonBoundingClientRect()
        {
            return QGMiniGameManager.Instance.GetMenuButtonBoundingClientRect();
        }

        #endregion

        #region 系统信息获取 GetManifestInfo、GetProvider、GetBatteryInfo、GetDeviceId
        public static void GetManifestInfo(Action<QGCommonResponse<QGManifestInfoRponse>> successCallback = null, Action<QGCommonResponse<QGManifestInfoRponse>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetManifestInfo(successCallback, failCallback);
        }

        public static string GetProvider()
        {
            return QGMiniGameManager.Instance.GetProvider();
        }

        // 判断当前运行环境是否是vivo
        public static bool isVivo()
        {
            return Provider_Vivo.Equals(QGMiniGameManager.Instance.GetProvider());
        }

        // 判断当前运行环境是否是OPPO
        public static bool isOPPO()
        {
            return Provider_OPPO.Equals(QGMiniGameManager.Instance.GetProvider());
        }

        // 判断当前运行环境是否是xiaomi
        public static bool isMi()
        {
            string provider = QGMiniGameManager.Instance.GetProvider();
            return Provider_Xiaomi.Equals(provider) || Provider_Redmi.Equals(provider) || provider.Contains("mi");
        }

        // 判断当前运行环境是否是荣耀
        public static bool isHonor()
        {
            return Provider_HONOR.Equals(QGMiniGameManager.Instance.GetProvider());
        }

        public static void GetBatteryInfo(Action<QGCommonResponse<BatteryInfoParam>> successCallback = null, Action<QGCommonResponse<BatteryInfoParam>> failCallback = null, Action<QGCommonResponse<BatteryInfoParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetBatteryInfo(successCallback, failCallback, completeCallback);
        }

        public static BatteryInfoParam GetBatteryInfoSync()
        {
            string batteryJsonStr = QGMiniGameManager.Instance.GetBatteryInfoSync();
            BatteryInfoParam batteryInfoParam = JsonUtility.FromJson<BatteryInfoParam>(batteryJsonStr);
            return batteryInfoParam;
        }

        public static void GetDeviceId(Action<QGCommonResponse<DeviceIdParam>> successCallback = null, Action<QGCommonResponse<DeviceIdParam>> failCallback = null, Action<QGCommonResponse<DeviceIdParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetDeviceId(successCallback, failCallback, completeCallback);
        }

        #endregion

        #region 获取网络类型

        public static void GetNetworkType(Action<QGCommonResponse<QGGetNetworkType>> successCallback = null, Action<QGCommonResponse<QGGetNetworkType>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetNetworkType(successCallback, failCallback);
        }

        #endregion

        #region 剪贴板
        // https://minigame.vivo.com.cn/documents/#/api/device/clipboard?id=qgsetclipboarddataobject-object
        public static void SetClipboardData(string data, Action<QGBaseResponse> succCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.SetClipboardData(data, succCallback, failCallback);
        }
        public static void GetClipboardData(Action<QGCommonResponse<string>> succCallback = null, Action<QGCommonResponse<string>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetClipboardData(succCallback, failCallback);
        }
        #endregion

        #region 帧率设置
        public static void SetPreferredFramesPerSecond(int fps)
        {
            QGMiniGameManager.Instance.SetPreferredFramesPerSecond(fps);
        }
        #endregion

        #region SetTimeout
        public static void SetTimeout(int times, Action<QGBaseResponse> action = null)
        {
            QGMiniGameManager.Instance.SetTimeout(times, action);
        }
        #endregion

        #region 判断桌面启动
        public static void IsStartupByShortcut(Action<QGIsStartupByShortcutParam> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.IsStartupByShortcut(successCallback, failCallback);
        }
        #endregion

        #region 生命周期
        public static void OnShow(Action<QGOnshowResponse> onShowCallback = null)
        {
            QGMiniGameManager.Instance.OnShow(onShowCallback);
        }

        public static void OffShow(Action<QGBaseResponse> onShowCallback = null)
        {
            QGMiniGameManager.Instance.OffShow(onShowCallback);
        }

        public static void OnHide(Action<QGBaseResponse> onHideCallback = null)
        {
            QGMiniGameManager.Instance.OnHide(onHideCallback);
        }

        public static void OffHide(Action<QGBaseResponse> onHideCallback = null)
        {
            QGMiniGameManager.Instance.OffHide(onHideCallback);
        }

        #endregion


        // 系统监听

        #region OnNetworkStatusChange 监听网络状态变化事件

        public static void OnNetworkStatusChange(Action<QGOnNetworkStatus> callback)
        {
            QGMiniGameManager.Instance.OnNetworkStatusChange(callback);
        }

        #endregion

        #region OnAudioInterruptionBegin 监听qg.onAudioInterruptionBegin

        public static void OnAudioInterruptionBegin()
        {
            QGMiniGameManager.Instance.OnAudioInterruptionBegin();
        }

        #endregion 

        #region OffAudioInterruptionBegin 取消监听qg.onAudioInterruptionBegin

        public static void OffAudioInterruptionBegin()
        {
            QGMiniGameManager.Instance.OffAudioInterruptionBegin();
        }

        #endregion 

        #region OnAudioInterruptionEnd 监听qg.onAudioInterruptionEnd

        public static void OnAudioInterruptionEnd()
        {
            QGMiniGameManager.Instance.OnAudioInterruptionEnd();
        }

        #endregion 

        #region OffAudioInterruptionEnd 取消监听qg.onAudioInterruptionEnd

        public static void OffAudioInterruptionEnd()
        {
            QGMiniGameManager.Instance.OffAudioInterruptionEnd();
        }

        #endregion

        #region OnError 监听全局错误事件

        public static void OnError()
        {
            QGMiniGameManager.Instance.OnError();
        }

        #endregion 

        #region OffError 取消监听全局错误事件

        public static void OffError()
        {
            QGMiniGameManager.Instance.OffError();
        }

        #endregion

        #region 屏幕亮度，传感器

        public static void GetScreenBrightness(Action<QGCommonResponse<ScreenBrightnessParam>> successCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> failCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetScreenBrightness(successCallback, failCallback, completeCallback);
        }

        public static void SetScreenBrightness(float param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SetScreenBrightness(param, successCallback, failCallback, completeCallback);
        }

        public static void SetKeepScreenOn(bool param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SetKeepScreenOn(param, successCallback, failCallback, completeCallback);
        }

        public static void GetLocation(Action<QGCommonResponse<GetLocationParam>> successCallback = null, Action<QGCommonResponse<GetLocationParam>> failCallback = null, Action<QGCommonResponse<GetLocationParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetLocation(successCallback, failCallback, completeCallback);
        }

        public static void OnAccelerometerChange(Action<QGCommonResponse<onAccelerometerChangeParam>> successCallback = null)
        {
            QGMiniGameManager.Instance.OnAccelerometerChange(successCallback);
        }

        public static void StartAccelerometer(string param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StartAccelerometer(param, successCallback, failCallback, completeCallback);
        }

        public static void StopAccelerometer(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StopAccelerometer(successCallback, failCallback, completeCallback);
        }

        public static void OnCompassChange(Action<QGCommonResponse<float>> successCallback = null)
        {
            QGMiniGameManager.Instance.OnCompassChange(successCallback);
        }

        public static void StartCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StartCompass(successCallback, failCallback, completeCallback);
        }

        public static void StopCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StopCompass(successCallback, failCallback, completeCallback);
        }
        #endregion


        //交互

        #region 退出游戏
        public static void ExitApplication()
        {
            QGMiniGameManager.Instance.ExitApplication();
        }

        #endregion

        #region HasShortcutInstalled  获取桌面图标是否创建
        //QG.HasShortcutInstalled(
        //(msg) => { Debug.Log("QG.HasShortcutInstalled success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg); }
        //);
        public static void HasShortcutInstalled(Action<QGCommonResponse<QGShortcutBean>> succCallback = null, Action<QGCommonResponse<QGShortcutBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.HasShortcutInstalled(succCallback, failCallback);
        }
        #endregion

        #region InstallShortcut  创建桌面图标
        //QG.InstallShortcut(
        //"我来自Unity",
        //(msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
        //);
        public static void InstallShortcut(string message, Action<QGBaseResponse> succCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.InstallShortcut(message, succCallback, failCallback);
        }
        #endregion

        #region VibrateShort 短振动

        public static void VibrateShort()
        {
            QGMiniGameManager.Instance.VibrateShort();
        }

        #endregion

        #region VibrateLong 长振动

        public static void VibrateLong()
        {
            QGMiniGameManager.Instance.VibrateLong();
        }

        #endregion

        #region ShowModal 显示对话框

        public static void ShowModal(ShowModalParam showModalParam, Action<QGCommonResponse<ShowModalResponse>> successCallback = null, Action<QGCommonResponse<ShowModalResponse>> failCallback = null, Action<QGCommonResponse<ShowModalResponse>> completeCallback = null)
        {
            QGMiniGameManager.Instance.ShowModal(showModalParam, successCallback, failCallback, completeCallback);
        }

        #endregion

        #region 软键盘
        public static void ShowKeyboard(KeyboardParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> cancelCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.ShowKeyboard(param, successCallback, cancelCallback, completeCallback);
        }

        public static string OnKeyboardInput(Action<QGKeyboardInputResponse> callback)
        {
            return QGMiniGameManager.Instance.OnKeyboardInput(callback);
        }

        public static string OnKeyboardConfirm(Action<QGKeyboardInputResponse> callback)
        {
            return QGMiniGameManager.Instance.OnKeyboardConfirm(callback);
        }

        public static string OnKeyboardComplete(Action<QGKeyboardInputResponse> callback)
        {
            return QGMiniGameManager.Instance.OnKeyboardComplete(callback);
        }

        public static void OffKeyboardInput(string inputId, Action callback)
        {
            QGMiniGameManager.Instance.OffKeyboardInput(inputId, callback);
        }

        public static void OffKeyboardConfirm(string confirmId, Action callback)
        {
            QGMiniGameManager.Instance.OffKeyboardConfirm(confirmId, callback);
        }

        public static void OffKeyboardComplete(string completeId, Action callback)
        {
            QGMiniGameManager.Instance.OffKeyboardComplete(completeId, callback);
        }

        public static void OffKeyboardInput()
        {
            QGMiniGameManager.Instance.OffKeyboardInput();
        }

        public static void OffKeyboardConfirm()
        {
            QGMiniGameManager.Instance.OffKeyboardConfirm();
        }

        public static void OffKeyboardComplete()
        {
            QGMiniGameManager.Instance.OffKeyboardComplete();
        }

        public static void HideKeyboard()
        {
            QGMiniGameManager.Instance.HideKeyboard();
        }

        #endregion

        #region ShowToast 提示框

        public static void ShowToast(ShowToastParam param)
        {
            QGMiniGameManager.Instance.ShowToast(param);
        }

        #endregion

        #region ShowLoading 进度条

        public static void ShowLoading(string title, bool mask)
        {
            QGMiniGameManager.Instance.ShowLoading(title, mask);
        }

        public static void HideLoading(Action<QGBaseResponse> success = null)
        {
            QGMiniGameManager.Instance.HideLoading(success);
        }
        #endregion


        // 多媒体
        #region PlayVideo 播放远程视频

        public static QGVideoPlayer CreateVideo(VideoParam param)
        {
            return QGMiniGameManager.Instance.CreateVideo(param);
        }

        #endregion

        #region PlayAudio 播放远程音频

        public static QGAudioPlayer PlayAudio(AudioParam param)
        {
            return QGMiniGameManager.Instance.PlayAudio(param);
        }

        #endregion

        #region 录音
        public static QGRecordManager GetRecorderManager()
        {
            return QGMiniGameManager.Instance.GetRecorderManager();
        }
        #endregion


        // 网络
        #region DownLoad

        public static void DownLoadFile(DownLoadFileParam param, Action<QGCommonResponse<DownLoadFileResponse>> successCallback = null, Action<QGCommonResponse<DownLoadFileResponse>> failCallback = null)
        {
            QGMiniGameManager.Instance.DownLoadFile(param, successCallback, failCallback);
        }

        #endregion

        #region UploadFile 

        public static void UploadFile(UpLoadFileParam param, Action<QGCommonResponse<UpLoadFileResponse>> successCallback = null, Action<QGCommonResponse<UpLoadFileResponse>> failCallback = null)
        {
            QGMiniGameManager.Instance.UploadFile(param, successCallback, failCallback);
        }

        #endregion

        #region UserCloudStorage 云存储

        public static UserCloudStorageParam userCloudStorageParam = new UserCloudStorageParam();
        public static void SetUserCloudStorage(string key, string value, Action<QGCommonResponse<string>> successCallback = null, Action<QGCommonResponse<string>> failCallback = null, Action<QGCommonResponse<string>> completeCallback = null)
        {
            userCloudStorageParam.key = key;
            userCloudStorageParam.value = value;
            QGMiniGameManager.Instance.SetUserCloudStorage(userCloudStorageParam, successCallback, failCallback, completeCallback);
        }

        public static void GetUserCloudStorage(string key, Action<QGCommonResponse<UserCloudStorageParam>> successCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> failCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetUserCloudStorage(key, successCallback, failCallback, completeCallback);
        }

        public static void RemoveUserCloudStorage(string key)
        {
            QGMiniGameManager.Instance.RemoveUserCloudStorage(key);
        }

        #endregion


        // 文件&数据缓存
        #region 文件

        /// <summary>
        /// 获取 qg.env.USER_DATA_PATH 文件本地缓存路径
        public static string GetQGEnvUSER_DATA_PATH()
        {
            return QGMiniGameManager.Instance.GetQGEnvUSER_DATA_PATH();
        }
        #endregion


        #region 文件

        public static QGFileSystemManager GetFileSystemManager()
        {
            return QGMiniGameManager.Instance.GetFileSystemManager();
        }
        #endregion

        #region storage 
        // 数据存储
        public static void StorageSetItem(string keyName, string keyValue)
        {
            QGMiniGameManager.StorageSetItem(keyName, keyValue);
        }
        // 数据读取  
        public static string StorageGetItem(string keyName)
        {
            return QGMiniGameManager.Instance.StorageGetItem(keyName);
        }
        // 清除数据 
        public static void StorageRemoveItem(string keyName)
        {
            QGMiniGameManager.StorageRemoveItem(keyName);
        }
        // 调用该方法会清空存储中的所有键名 
        public static void StorageClear()
        {
            QGMiniGameManager.StorageClear();
        }
        #endregion

        #region 覆盖unity的PlayerPrefs

        public static void StorageSetIntSync(string key, int value)
        {
            QGMiniGameManager.Instance.StorageSetIntSync(key, value);
        }

        public static int StorageGetIntSync(string key, int defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetIntSync(key, defaultValue);
        }

        public static void StorageSetStringSync(string key, string value)
        {
            QGMiniGameManager.Instance.StorageSetStringSync(key, value);
        }

        public static string StorageGetStringSync(string key, string defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetStringSync(key, defaultValue);
        }

        public static void StorageSetFloatSync(string key, float value)
        {
            QGMiniGameManager.Instance.StorageSetFloatSync(key, value);
        }

        public static float StorageGetFloatSync(string key, float defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetFloatSync(key, defaultValue);
        }

        public static void StorageDeleteAllSync()
        {
            QGMiniGameManager.Instance.StorageDeleteAllSync();
        }

        public static void StorageDeleteKeySync(string key)
        {
            QGMiniGameManager.Instance.StorageDeleteKeySync(key);
        }

        public static bool StorageHasKeySync(string key)
        {
            return QGMiniGameManager.Instance.StorageHasKeySync(key);
        }

        #endregion

    }
}
