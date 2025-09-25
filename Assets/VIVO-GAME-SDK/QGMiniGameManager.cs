using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace QGMiniGame
{
    public class QGMiniGameManager : MonoBehaviour
    {
        #region Instance

        private static QGMiniGameManager instance = null;


        public static QGMiniGameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(QGMiniGameManager).Name).AddComponent<QGMiniGameManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        #endregion

        #region 登录

        public void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGLogin(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 用户信息

        public void GetUserInfo(Action<QGCommonResponse<QGUserInfoBean>> successCallback = null, Action<QGCommonResponse<QGUserInfoBean>> failCallback = null)
        {
            QGGetUserInfo(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 获取桌面图标是否创建

        public void HasShortcutInstalled(Action<QGCommonResponse<QGShortcutBean>> successCallback = null, Action<QGCommonResponse<QGShortcutBean>> failCallback = null)
        {
            QGHasShortcutInstalled(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 创建桌面图标

        public void InstallShortcut(string message, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGInstallShortcut(message, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 创建Banner广告

        public QGBannerAd CreateBannerAd(QGCreateBannerAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGBannerAd ad = new QGBannerAd(adId);
            QGCreateBannerAd(adId, param.posId, JsonUtility.ToJson(param.style), param.adIntervals);
            return ad;
        }

        #endregion

        #region 创建插屏广告

        public QGInterstitialAd CreateInterstitialAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGInterstitialAd ad = new QGInterstitialAd(adId);
            QGCreateInterstitialAd(adId, param.posId);
            return ad;
        }

        #endregion

        #region 创建激励视频广告

        public QGRewardedVideoAd CreateRewardedVideoAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGRewardedVideoAd ad = new QGRewardedVideoAd(adId);
            QGCreateRewardedVideoAd(adId, param.posId);
            return ad;
        }

        #endregion

        #region 创建原生广告

        public QGNativeAd CreateNativeAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGNativeAd ad = new QGNativeAd(adId);
            QGCreateNativeAd(adId, param.posId);
            return ad;
        }

        #endregion

        #region 创建模板广告

        public QGCustomAd CreateCustomAd(QGCreateCustomAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGCustomAd ad = new QGCustomAd(adId);
            QGCreateCustomAd(adId, param.posId, JsonUtility.ToJson(param.style));
            return ad;
        }

        public bool IsShow(string adId)
        {
            return QGIsShow(adId);
        }

        #endregion

        #region 创建横幅广告

        public QGBoxBannerAd CreateBoxBannerAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGBoxBannerAd ad = new QGBoxBannerAd(adId);
            QGCreateBoxBannerAd(adId, param.posId);
            return ad;
        }

        #endregion

        #region 创建九宫格广告

        public QGBoxPortalAd CreateBoxPortalAd(QGCreateBoxPortalAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGBoxPortalAd ad = new QGBoxPortalAd(adId);
            QGCreateBoxPortalAd(adId, param.posId, param.image, param.marginTop);
            return ad;
        }

        #endregion

        #region 原生广告曝光

        public void ReportAdShow(string adId, QGNativeReportParam param)
        {
            QGReportAdShow(adId, param.adId);
        }

        public void ReportAdClick(string adId, QGNativeReportParam param)
        {
            QGReportAdClick(adId, param.adId);
        }

        #endregion

        #region 广告通用逻辑
        public void ShowAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGShowAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void HideAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGHideAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void LoadAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGLoadAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void DestroyAd(string adId)
        {
            QGDestroyAd(adId);
        }

        #endregion

        #region unity的PlayerPrefs

        public void StorageSetIntSync(string key, int value)
        {
            QGStorageSetIntSync(key, value);
        }

        public int StorageGetIntSync(string key, int defaultValue)
        {
            return QGStorageGetIntSync(key, defaultValue);
        }

        public void StorageSetStringSync(string key, string value)
        {
            QGStorageSetStringSync(key, value);
        }

        public string StorageGetStringSync(string key, string defaultValue)
        {
            return QGStorageGetStringSync(key, defaultValue);
        }

        public void StorageSetFloatSync(string key, float value)
        {
            QGStorageSetFloatSync(key, value);
        }

        public float StorageGetFloatSync(string key, float defaultValue)
        {
            return QGStorageGetFloatSync(key, defaultValue);
        }

        public void StorageDeleteAllSync()
        {
            QGStorageDeleteAllSync();
        }

        public void StorageDeleteKeySync(string key)
        {
            QGStorageDeleteKeySync(key);
        }

        public bool StorageHasKeySync(string key)
        {
            return QGStorageHasKeySync(key);
        }

        #endregion

        #region 支付
        public void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null, Action<QGCommonResponse<QGPayBean>> cancelCallback = null, Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGPay(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(cancelCallback), QGCallBackManager.Add(completeCallback));
        }
        #endregion

        #region 判断文件是否存在
        public string AccessFile(string uri)
        {
            return QGAccessFile(uri);
        }
        #endregion

        #region 读取文件
        public void ReadFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGReadFile(param.uri, param.encoding, param.position, param.length, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        #endregion

        #region 同步读取文件
        public QGFileInfo ReadFileSync(QGFileParam param)
        {
            QGFileInfo fileInfo = new QGFileInfo();
            if (param.encoding.Equals("utf8"))
            {
                fileInfo.textStr = QGReadFileSyncStr(param.uri, param.encoding, param.position, param.length);
            }
            else 
            {
                string textDataStr = QGReadFileSyncData(param.uri, param.encoding, param.position, param.length);
                String[] dataStrs = textDataStr.Split('-');
                int length = int.Parse(dataStrs[0]);
                IntPtr byteArrayPtr = new IntPtr(int.Parse(dataStrs[1]));
                byte[] byteArray = new byte[length];
                Marshal.Copy(byteArrayPtr, byteArray, 0, length);

                fileInfo.textData = byteArray;
            }
            return fileInfo;
        }
        #endregion

        #region 写入文件
        public void WriteFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGWriteFile(param.uri, param.encoding, param.position, param.textStr, param.textData, param.textData == null ? 0 : param.textData.Length, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        #endregion
        
        #region 写入文件同步方法
        public string WriteFileSync(QGFileParam param)
        {
            return QGWriteFileSync(param.uri, param.encoding, param.position, param.textStr, param.textData, param.textData == null ? 0 : param.textData.Length);
        }
        #endregion

        #region 键盘
        public void ShowKeyboard(KeyboardParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> cancelCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGShowKeyboard(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(cancelCallback), QGCallBackManager.Add(completeCallback));
        }

        public string OnKeyboardInput(Action<QGKeyboardInputResponse> successCallback = null)
        {
            string inputId = QGCallBackManager.Add(successCallback);
            QGOnKeyboardInput(inputId);
            return inputId;
        }

        public string OnKeyboardConfirm(Action<QGKeyboardInputResponse> successCallback = null)
        {
            string confirmId = QGCallBackManager.Add(successCallback);
            QGOnKeyboardConfirm(confirmId);
            return confirmId;
        }

        public string OnKeyboardComplete(Action<QGKeyboardInputResponse> successCallback = null)
        {
            string completeId = QGCallBackManager.Add(successCallback);
            QGOnKeyboardComplete(completeId);
            return completeId;
        }

        public void OffKeyboardInput(string inputId, Action callback = null)
        {
            QGCallBackManager.responseCallBacks.Remove(inputId);
            callback();
        }

        public void OffKeyboardConfirm(string confirmId, Action callback)
        {
            QGCallBackManager.responseCallBacks.Remove(confirmId);
            callback();
        }

        public void OffKeyboardComplete(string completeId, Action callback)
        {
            QGCallBackManager.responseCallBacks.Remove(completeId);
            callback();
        }

        public void HideKeyboard()
        {
            QGHideKeyboard();
        }
        #endregion

        #region 退出游戏
        public void ExitApplication()
        {
            QGExitApplication();
        }
        #endregion

        #region V订阅
        public void Subscribe(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGSubscribe(param.templateIds, param.clientId, param.userId, param.scene, param.type, param.subDesc, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void GetStatus(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGGetStatus(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void UnSubscribe(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGUnSubscribe(param.templateIds, param.clientId, param.userId, param.scene, param.type, param.subDesc, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void IsRelationExist(SubscribeParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGIsRelationExist(param.templateIds, param.clientId, param.userId, param.scene, param.type, param.subDesc, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 判断当前是否是vivo环境

        public bool IsVivoRuntime()
        {
            return QGIsVivoRuntime();
        }
        #endregion

        #region JS回调
        public void LoginResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGLoginBean>>(msg);
        }

        public void GetUserInfoResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGUserInfoBean>>(msg);
        }

        public void ShortcutResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGShortcutBean>>(msg);
        }

        public void DefaultResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGBaseResponse>(msg);
        }

        public void PayResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGPayBean>>(msg);
        }

        public void ReadFileResponseCallback(string msg)
        {
            if (msg.Contains("utf8"))
            {
                QGCallBackManager.InvokeResponseCallback<QGFileResponse>(msg);
            }
            else
            {
                QGFileResponse response = JsonUtility.FromJson<QGFileResponse>(msg);
                var fileBuffer = new byte[response.byteLength];
                QGGetFileBuffer(fileBuffer, response.callbackId);
                response.textData = fileBuffer;
                var callback = (Action<QGFileResponse>)QGCallBackManager.responseCallBacks[response.callbackId];
                callback(response);
                QGCallBackManager.responseCallBacks.Remove(response.callbackId);
            }

        }

        public void WriteFileResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGFileResponse>(msg);
        }

        public void OnKeyboardInputResponseCallback(string msg)
        {
            var res = JsonUtility.FromJson<QGKeyboardInputResponse>(msg);
            var callBack = (Action<QGKeyboardInputResponse>)QGCallBackManager.responseCallBacks[res.callbackId];
            if (callBack != null)
            {
                callBack(res);
            }
        }

        // 广告通用回调 
        public void AdOnErrorCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onErrorAction?.Invoke(res);
            }
        }

        public void AdOnLoadCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onLoadAction?.Invoke();
            }
        }

        public void AdOnCloseCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onCloseAction?.Invoke();
            }
        }

        public void AdOnHideCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onHideAction?.Invoke();
            }
        }

        public void AdOnShowCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null && ad is QGBoxPortalAd)
            {
                ((QGBoxPortalAd)ad).onShowAction?.Invoke();
            }
        }

        public void NativeAdOnLoadCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGNativeResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null && ad is QGNativeAd)
            {
                ((QGNativeAd)ad).onLoadNativeAction?.Invoke(res);
            }
        }

        public void RewardedVideoAdOnCloseCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGRewardedVideoResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null && ad is QGRewardedVideoAd)
            {
                ((QGRewardedVideoAd)ad).onCloseRewardedVideoAction?.Invoke(res);
            }
        }

        #endregion



        [DllImport("__Internal")]
        private static extern void QGLogin(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGGetUserInfo(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGHasShortcutInstalled(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGInstallShortcut(string m, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGCreateBannerAd(string a, string p, string s, int i);

        [DllImport("__Internal")]
        private static extern void QGCreateInterstitialAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateRewardedVideoAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateNativeAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateCustomAd(string a, string p, string s);

        [DllImport("__Internal")]
        private static extern void QGCreateBoxBannerAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateBoxPortalAd(string a, string p, string i, int m);

        [DllImport("__Internal")]
        private static extern void QGShowAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGHideAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGLoadAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGDestroyAd(string a);

        [DllImport("__Internal")]
        private static extern bool QGIsShow(string a);

        [DllImport("__Internal")]
        private static extern void QGReportAdShow(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGReportAdClick(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGStorageSetIntSync(string k, int v);

        [DllImport("__Internal")]
        private static extern int QGStorageGetIntSync(string k, int d);

        [DllImport("__Internal")]
        private static extern void QGStorageSetStringSync(string k, string v);

        [DllImport("__Internal")]
        private static extern string QGStorageGetStringSync(string k, string d);

        [DllImport("__Internal")]
        private static extern void QGStorageSetFloatSync(string k, float v);

        [DllImport("__Internal")]
        private static extern float QGStorageGetFloatSync(string k, float d);

        [DllImport("__Internal")]
        private static extern void QGStorageDeleteAllSync();

        [DllImport("__Internal")]
        private static extern void QGStorageDeleteKeySync(string k);

        [DllImport("__Internal")]
        private static extern bool QGStorageHasKeySync(string k);

        [DllImport("__Internal")]
        private static extern void QGPay(string p, string s, string f, string c, string o);

        [DllImport("__Internal")]
        private static extern string QGAccessFile(string u);

        [DllImport("__Internal")]
        private static extern void QGReadFile(string u, string e, int p, int l, string s, string f);

        [DllImport("__Internal")]
        private static extern string QGReadFileSyncStr(string u, string e, int p, int l);

        [DllImport("__Internal")]
        private static extern string QGReadFileSyncData(string u, string e, int p, int l);

        [DllImport("__Internal")]
        private static extern void QGGetFileBuffer(byte[] d, string c);

        [DllImport("__Internal")]
        private static extern void QGWriteFile(string u, string e, int p, string t, byte[] d, int l, string c, string f);
        
        [DllImport("__Internal")]
        private static extern string QGWriteFileSync(string u, string e, int p, string t, byte[] d, int l);

        [DllImport("__Internal")]
        private static extern void QGShowKeyboard(string p, string s, string c, string o);

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardInput(string p);

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardConfirm(string p);

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardComplete(string p);

        [DllImport("__Internal")]
        private static extern void QGHideKeyboard();

        [DllImport("__Internal")]
        private static extern void QGExitApplication();

        [DllImport("__Internal")]
        private static extern void QGSubscribe(string t, string c, string u, string s, int h, string l, string success, string fail);

        [DllImport("__Internal")]
        private static extern void QGGetStatus(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGUnSubscribe(string t, string c, string u, string s, int h, string l, string success, string fail);

        [DllImport("__Internal")]
        private static extern void QGIsRelationExist(string t, string c, string u, string s, int h, string l, string success, string fail);

        [DllImport("__Internal")]
        private static extern bool QGIsVivoRuntime();
    }
}
