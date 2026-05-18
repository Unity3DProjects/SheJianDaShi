using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QG
    {

        #region Login  登录
        // http://minigame.vivo.com.cn/documents/#/api/service/newaccount?id=login
        //QG.Login(
        //(msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
        ///);
        public static void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.Login(successCallback, failCallback);
        }
        #endregion

        #region GetUserInfo 获取用户信息
        // http://minigame.vivo.com.cn/documents/#/api/service/newaccount
        //QG.GetUserInfo(
        //(msg) => { Debug.Log("QG.GetUserInfo success = " + JsonUtility.ToJson(msg));},
        //(msg) => { Debug.Log("QG.GetUserInfo fail = " + msg.errMsg); }
        //);
        public static void GetUserInfo(Action<QGCommonResponse<QGUserInfoBean>> succCallback = null, Action<QGCommonResponse<QGUserInfoBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetUserInfo(succCallback, failCallback);
        }
        #endregion

        #region GetSystemInfo 获取系统信息
        // https://minigame.vivo.com.cn/documents/#/api/system/system-info?id=qggetsysteminfoobject-object
        public static void GetSystemInfo(Action<QGCommonResponse<string>> succCallback = null, Action<QGCommonResponse<string>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetSystemInfo(succCallback, failCallback);
        }
        public static string GetSystemInfoSync()
        {
           return QGMiniGameManager.Instance.GetSystemInfoSync();
        }
        #endregion

        #region HasShortcutInstalled  获取桌面图标是否创建
        //QG.HasShortcutInstalled(
        //(msg) => { Debug.Log("QG.HasShortcutInstalled success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg); }
        //);
        // http://minigame.vivo.com.cn/documents/#/api/system/shortcut
        public static void HasShortcutInstalled(Action<QGCommonResponse<QGShortcutBean>> succCallback = null, Action<QGCommonResponse<QGShortcutBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.HasShortcutInstalled(succCallback, failCallback);
        }
        #endregion

        #region InstallShortcut  创建桌面图标
        // http://minigame.vivo.com.cn/documents/#/api/system/shortcut
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

        #region CreateNativeAd  创建原生广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/native-ad
        //var nativeAd = QG.CreateNativeAd(new QGCommonAdParam()
        //{
        //posId = "0f36aa6ce7fd44a3a7c391c2a524308c"
        //});
        //nativeAd.OnLoad((QGNativeResponse rec) => {
        //Debug.Log("QG.nativeAd.OnLoad success = " + JsonUtility.ToJson(rec));
        //if (rec != null && rec.adList != null)
        //{
        //var nativeCurrentAd = rec.adList[0];
        //if (nativeCurrentAd != null)
        //{
        //nativeAd.ReportAdShow(new QGNativeReportParam()
        //{
        //adId = nativeCurrentAd.adId
        //});
        //nativeAd.ReportAdClick(new QGNativeReportParam()
        //{
        //adId = nativeCurrentAd.adId
        //});
        //}
        //}
        //});
        //nativeAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.nativeAd.OnError success = " + JsonUtility.ToJson(msg));
        //});

        public static QGNativeAd CreateNativeAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateNativeAd(param);
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

        #region CreateBoxBannerAd  创建横幅广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/box-ad
        //boxBannerAd = QG.CreateBoxBannerAd(new QGCommonAdParam()
        //{
        //posId = "xxxxxx1"
        //});
        //boxBannerAd.OnLoad(() => {
        //Debug.Log("QG.boxBannerAd.OnLoad success = ");
        //boxBannerAd.Show(
        //(msg) => { Debug.Log("QG.boxBannerAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.boxBannerAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //boxBannerAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.boxBannerAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        //boxBannerAd.OnClose(() =>
        //{
        //Debug.Log("QG.boxBannerAd.OnClose success ");
        //});
        public static QGBoxBannerAd CreateBoxBannerAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateBoxBannerAd(param);
        }
        #endregion

        #region CreateBoxBannerAd  创建九宫格广告
        // http://minigame.vivo.com.cn/documents/#/api/ad/box-ad
        //var boxPortalAd = QG.CreateBoxPortalAd(new QGCreateBoxPortalAdParam()
        //{
        //posId = "xxxxxx2",
        //image = "",
        //marginTop = 200
        //});
        //boxPortalAd.OnLoad(() => {
        //boxPortalAd.Show(
        //(msg) => { Debug.Log("QG.boxPortalAd.Show success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.boxPortalAd.Show fail = " + msg.errMsg); }
        //);
        //});
        //boxPortalAd.OnError((QGBaseResponse msg) =>
        //{
        //Debug.Log("QG.boxPortalAd.OnError success = " + JsonUtility.ToJson(msg));
        //});
        //boxPortalAd.OnShow(() =>
        //{
        //Debug.Log("QG.boxPortalAd.OnShow success = ");
        //});
        public static QGBoxPortalAd CreateBoxPortalAd(QGCreateBoxPortalAdParam param)
        {
            return QGMiniGameManager.Instance.CreateBoxPortalAd(param);
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

        #region Pay 支付
        // http://minigame.vivo.com.cn/documents/#/api/service/newpay
        /*PayParam param = new PayParam()
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
        public static void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null, Action<QGCommonResponse<QGPayBean>> cancelCallback = null, Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGMiniGameManager.Instance.Pay(param, successCallback, failCallback, cancelCallback, completeCallback);
        }
        #endregion

        #region AccessFile 判断文件是否存在
        //http://minigame.vivo.com.cn/documents/#/api/data/file?id=qgaccessfileobject-object
        // 注意uri格式，请参考 http://minigame.vivo.com.cn/documents/#/api/data/file-system
        public static string AccessFile(string uri)
        {
            return QGMiniGameManager.Instance.AccessFile(uri);
        }
        #endregion

        #region ReadFile 读取文件
        //http://minigame.vivo.com.cn/documents/#/api/data/file?id=qgreadfileobject-object
        // 注意uri格式，请参考 http://minigame.vivo.com.cn/documents/#/api/data/file-system
        /*QGFileParam param = new QGFileParam()
        {
            uri = "internal://files/hehe.txt",
            encoding = "utf8"
        };
        QG.ReadFile(
            param,
           (msg) => { Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.ReadFile fail = " + JsonUtility.ToJson(msg)); }
        );*/
        public static void ReadFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.ReadFile(param, successCallback, failCallback);
        }
        #endregion

        #region ReadFileSync 同步读取文件
        //https://minigame.vivo.com.cn/documents/#/api/data/file?id=qgreadfilesyncobject-object-1031
        // 注意uri格式，请参考 http://minigame.vivo.com.cn/documents/#/api/data/file-system
        public static QGFileInfo ReadFileSync(QGFileParam param)
        {
            return QGMiniGameManager.Instance.ReadFileSync(param);
        }
        #endregion

        #region WriteFile 写入文件
        //http://minigame.vivo.com.cn/documents/#/api/data/file?id=qgwritefileobject-object
        // 注意uri格式，请参考 http://minigame.vivo.com.cn/documents/#/api/data/file-system
        /*QGFileParam param = new QGFileParam()
        {
            uri = "internal://files/hehe.txt",
            encoding = "utf8",
            textStr = "你是谁啊 "
        };
        QG.WriteFile(
            param,
           (msg) => { Debug.Log("QG.WriteFile success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.WriteFile fail = " + JsonUtility.ToJson(msg)); }
        );*/
        public static void WriteFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.WriteFile(param, successCallback, failCallback);
        }
        #endregion

        #region WriteFileSync 写入文件 同步方法
        //http://minigame.vivo.com.cn/documents/#/api/data/file?id=qgwritefileobject-object
        // 注意uri格式，请参考 http://minigame.vivo.com.cn/documents/#/api/data/file-system
        /*QGFileParam param = new QGFileParam()
        {
            uri = "internal://files/hehe.txt",
            encoding = "utf8",
            textStr = "你是谁啊 "
        };
        QG.WriteFileSync(
            param
        );*/
        public static string WriteFileSync(QGFileParam param)
        {
            return QGMiniGameManager.Instance.WriteFileSync(param);
        }
        #endregion

        #region 软键盘
        //http://minigame.vivo.com.cn/documents/#/api/interface/keyboard
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

        public static void OffKeyboardInput(string inputId ,Action callback) 
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

        public static void HideKeyboard()
        {
            QGMiniGameManager.Instance.HideKeyboard();
        }

        #endregion

        #region 退出游戏
        public static void ExitApplication()
        {
            QGMiniGameManager.Instance.ExitApplication();
        }

        #endregion

        #region V订阅
        public static void Subscribe(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.Subscribe(param, successCallback, failCallback);
        }

        public static void GetStatus(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.GetStatus(successCallback, failCallback);
        }

        public static void UnSubscribe(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.UnSubscribe(param, successCallback, failCallback);
        }

        public static void IsRelationExist(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.IsRelationExist(param, successCallback, failCallback);
        }

        #endregion

        #region IsVivoRuntime 判断是否是vivo运行环境
        // https://minigame.vivo.com.cn/documents/#/api/service/provider
        //bool isVivoRuntime = QG.IsVivoRuntime();
        public static bool IsVivoRuntime()
        {
            return QGMiniGameManager.Instance.IsVivoRuntime();
        }
        #endregion

        #region Get/SetSystemInfo 剪贴板
        // https://minigame.vivo.com.cn/documents/#/api/device/clipboard?id=qgsetclipboarddataobject-object
        public static void SetClipboardData(string text, Action<QGBaseResponse> succCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.SetClipboardData(text, succCallback, failCallback);
        }
        public static void GetClipboardData(Action<QGCommonResponse<string>> succCallback = null, Action<QGCommonResponse<string>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetClipboardData(succCallback, failCallback);
        }
        #endregion
    }
}

