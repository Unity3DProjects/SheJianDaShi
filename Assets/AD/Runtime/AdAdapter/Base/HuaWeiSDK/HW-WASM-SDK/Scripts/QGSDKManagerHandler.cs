using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace HWWASM
{
    public class QGSDKManagerHandler : MonoBehaviour
    {
        #region Instance

        private static QGSDKManagerHandler _instance = null;

        public static QGSDKManagerHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(
                        "QGSDKManagerHandler"
                    ).AddComponent<QGSDKManagerHandler>();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        protected void OnDestroy()
        {
            _instance = null;
        }

        #endregion

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void QG_GameLogin(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_IsEnvReady(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_IsSandboxActivated(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_ObtainOwnedPurchases(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_ObtainProductInfo(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_CreatePurchaseIntent(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_ConsumeOwnedPurchase(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_ObtainOwnedPurchaseRecord(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_StartIapActivity(string conf, string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_HideKeyboard(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_ShowKeyboard(string conf,string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_UpdateKeyboard(string value,string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_OnKeyboardInput();

    [DllImport("__Internal")]
    private static extern void QG_OffKeyboardInput();

    [DllImport("__Internal")]
    private static extern void QG_OnKeyboardConfirm();

    [DllImport("__Internal")]
    private static extern void QG_OffKeyboardConfirm();

    [DllImport("__Internal")]
    private static extern void QG_OnKeyboardComplete();

    [DllImport("__Internal")]
    private static extern void QG_OffKeyboardComplete();

    [DllImport("__Internal")]
    private static extern void QG_LocalStorageClear();

    [DllImport("__Internal")]
    private static extern void QG_LocalStorageRemoveItem(string key);

    [DllImport("__Internal")]
    private static extern void QG_LocalStorageSetItem(string key, string value);

    [DllImport("__Internal")]
    private static extern string QG_LocalStorageGetItem(string key);

    [DllImport("__Internal")]
    private static extern string QG_LocalStorageKey(int index);

    [DllImport("__Internal")]
    private static extern int QG_LocalStorageLength();

    [DllImport("__Internal")]
    private static extern void QG_SetPreferredFramesPerSecond(int fps);

    [DllImport("__Internal")]
    private static extern void QG_GetClipboardData(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_SetClipboardData(string conf,string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_OnTouchStart();

    [DllImport("__Internal")]
    private static extern void QG_OffTouchStart();

    [DllImport("__Internal")]
    private static extern void QG_OnTouchMove();

    [DllImport("__Internal")]
    private static extern void QG_OffTouchMove();

    [DllImport("__Internal")]
    private static extern void QG_OnTouchEnd();

    [DllImport("__Internal")]
    private static extern void QG_OffTouchEnd();

    [DllImport("__Internal")]
    private static extern void QG_OnTouchCancel();

    [DllImport("__Internal")]
    private static extern void QG_OffTouchCancel();

    [DllImport("__Internal")]
    private static extern void QG_GetSystemInfo(string callbackId);

    [DllImport("__Internal")]
    private static extern string QG_GetSystemInfoSync();

    [DllImport("__Internal")]
    private static extern void QG_OnError();

    [DllImport("__Internal")]
    private static extern void QG_OffError();

    [DllImport("__Internal")]
    private static extern void QG_ExitApplication(string callbackId);

    [DllImport("__Internal")]
    private static extern string QG_GetLaunchOptionsSync();

    [DllImport("__Internal")]
    private static extern void QG_OnHide();

    [DllImport("__Internal")]
    private static extern void QG_OffHide();

    [DllImport("__Internal")]
    private static extern void QG_OnShow();

    [DllImport("__Internal")]
    private static extern void QG_OffShow();

    [DllImport("__Internal")]
    private static extern void QG_NavigateToQuickApp(string conf,string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_OpenDeeplink(string conf);

    [DllImport("__Internal")]
    private static extern void QG_HasShortcutInstalled(string callbackId);

    [DllImport("__Internal")]
    private static extern void QG_InstallShortcut(string conf,string callbackId);

    [DllImport("__Internal")]
    private static extern string QG_ENV_USER_DATA_PATH();

    [DllImport("__Internal")]
    private static extern void QG_PreDownloadAudios(string paths, string callbackId);
#else
        private static void QG_GameLogin(string conf, string callbackId)
        {
        }

        private static void QG_IsEnvReady(string conf, string callbackId)
        {
        }

        private static void QG_IsSandboxActivated(string conf, string callbackId)
        {
        }

        private static void QG_ObtainOwnedPurchases(string conf, string callbackId)
        {
        }

        private static void QG_ObtainProductInfo(string conf, string callbackId)
        {
        }

        private static void QG_CreatePurchaseIntent(string conf, string callbackId)
        {
        }

        private static void QG_ConsumeOwnedPurchase(string conf, string callbackId)
        {
        }

        private static void QG_ObtainOwnedPurchaseRecord(string conf, string callbackId)
        {
        }

        private static void QG_StartIapActivity(string conf, string callbackId)
        {
        }

        private static void QG_HideKeyboard(string callbackId)
        {
        }

        private static void QG_ShowKeyboard(string conf, string callbackId)
        {
        }

        private static void QG_UpdateKeyboard(string value, string callbackId)
        {
        }

        private static void QG_OnKeyboardInput()
        {
        }

        private static void QG_OffKeyboardInput()
        {
        }

        private static void QG_OnKeyboardConfirm()
        {
        }

        private static void QG_OffKeyboardConfirm()
        {
        }

        private static void QG_OnKeyboardComplete()
        {
        }

        private static void QG_OffKeyboardComplete()
        {
        }

        private static void QG_LocalStorageClear()
        {
        }

        private static void QG_LocalStorageRemoveItem(string key)
        {
        }

        private static void QG_LocalStorageSetItem(string key, string value)
        {
        }

        private static string QG_LocalStorageGetItem(string key)
        {
            return null;
        }

        private static string QG_LocalStorageKey(int index)
        {
            return null;
        }

        private static int QG_LocalStorageLength()
        {
            return 0;
        }

        private static void QG_SetPreferredFramesPerSecond(int fps)
        {
        }

        private static void QG_GetClipboardData(string callbackId)
        {
        }

        private static void QG_SetClipboardData(string conf, string callbackId)
        {
        }

        private static void QG_OnTouchStart()
        {
        }

        private static void QG_OffTouchStart()
        {
        }

        private static void QG_OnTouchMove()
        {
        }

        private static void QG_OffTouchMove()
        {
        }

        private static void QG_OnTouchEnd()
        {
        }

        private static void QG_OffTouchEnd()
        {
        }

        private static void QG_OnTouchCancel()
        {
        }

        private static void QG_OffTouchCancel()
        {
        }

        private static void QG_GetSystemInfo(string callbackId)
        {
        }

        private static string QG_GetSystemInfoSync()
        {
            return null;
        }

        private static void QG_OnError()
        {
        }

        private static void QG_OffError()
        {
        }

        private static void QG_ExitApplication(string callbackId)
        {
        }

        private static string QG_GetLaunchOptionsSync()
        {
            return null;
        }

        private static void QG_OnHide()
        {
        }

        private static void QG_OffHide()
        {
        }

        private static void QG_OnShow()
        {
        }

        private static void QG_OffShow()
        {
        }

        private static void QG_NavigateToQuickApp(string conf, string callbackId)
        {
        }

        private static void QG_OpenDeeplink(string conf)
        {
        }

        private static void QG_HasShortcutInstalled(string callbackId)
        {
        }

        private static void QG_InstallShortcut(string conf, string callbackId)
        {
        }

        private static void QG_DownloadFile(string conf, string callbackId)
        {
        }

        private static string QG_ENV_USER_DATA_PATH()
        {
            return null;
        }
        private static void QG_PreDownloadAudios(string paths, string callbackId)
        {
        }
#endif

        public void GameLogin(GameLoginOption gameLoginOption)
        {
            string callbackId = RTCallback<LoginSuccessResult, LoginFailResult>.AddCallback(
                gameLoginOption.success,
                gameLoginOption.fail,
                gameLoginOption.complete
            );
            string conf = JsonUtility.ToJson(gameLoginOption);
            QG_GameLogin(conf, callbackId);
        }

        public RewardedVideoAd CreateRewardedVideoAd(CreateRewardedVideoAdOption createRewardedVideoAdOption)
        {
            return RewardedVideoAdFactory.Instance.CreateRewardedVideoAd(createRewardedVideoAdOption);
        }

        public void IsEnvReady(IsEnvReadyOption isEnvReadyOption)
        {
            string callbackId = RTCallback<IsEnvReadySuccessResult, IsEnvReadyFailResult>.AddCallback(
                isEnvReadyOption.success,
                isEnvReadyOption.fail,
                isEnvReadyOption.complete
            );
            string conf = JsonUtility.ToJson(isEnvReadyOption);
            QG_IsEnvReady(conf, callbackId);
        }

        public void IsSandboxActivated(IsSandboxActivatedOption isSandboxActivatedOption)
        {
            string callbackId = RTCallback<IsSandboxActivatedSuccessResult, IsSandboxActivatedFailResult>.AddCallback(
                isSandboxActivatedOption.success,
                isSandboxActivatedOption.fail,
                isSandboxActivatedOption.complete
            );
            string conf = JsonUtility.ToJson(isSandboxActivatedOption);
            QG_IsSandboxActivated(conf, callbackId);
        }

        public void ObtainOwnedPurchases(ObtainOwnedPurchasesOption obtainOwnedPurchasesOption)
        {
            string callbackId =
                RTCallback<ObtainOwnedPurchasesSuccessResult, ObtainOwnedPurchasesFailResult>.AddCallback(
                    obtainOwnedPurchasesOption.success,
                    obtainOwnedPurchasesOption.fail,
                    obtainOwnedPurchasesOption.complete
                );
            string conf = JsonUtility.ToJson(obtainOwnedPurchasesOption);
            QG_ObtainOwnedPurchases(conf, callbackId);
        }

        public void ObtainProductInfo(ObtainProductInfoOption obtainProductInfoOption)
        {
            string callbackId = RTCallback<ObtainProductInfoSuccessResult, ObtainProductInfoFailResult>.AddCallback(
                obtainProductInfoOption.success,
                obtainProductInfoOption.fail,
                obtainProductInfoOption.complete
            );
            string conf = JsonUtility.ToJson(obtainProductInfoOption);
            QG_ObtainProductInfo(conf, callbackId);
        }

        public void CreatePurchaseIntent(CreatePurchaseIntentOption createPurchaseIntentOption)
        {
            string callbackId =
                RTCallback<CreatePurchaseIntentSuccessResult, CreatePurchaseIntentFailResult>.AddCallback(
                    createPurchaseIntentOption.success,
                    createPurchaseIntentOption.fail,
                    createPurchaseIntentOption.complete
                );
            string conf = JsonUtility.ToJson(createPurchaseIntentOption);
            QG_CreatePurchaseIntent(conf, callbackId);
        }

        public void ConsumeOwnedPurchase(ConsumeOwnedPurchaseOption consumeOwnedPurchaseOption)
        {
            string callbackId =
                RTCallback<ConsumeOwnedPurchaseSuccessResult, ConsumeOwnedPurchaseFailResult>.AddCallback(
                    consumeOwnedPurchaseOption.success,
                    consumeOwnedPurchaseOption.fail,
                    consumeOwnedPurchaseOption.complete
                );
            string conf = JsonUtility.ToJson(consumeOwnedPurchaseOption);
            QG_ConsumeOwnedPurchase(conf, callbackId);
        }

        public void ObtainOwnedPurchaseRecord(ObtainOwnedPurchaseRecordOption obtainOwnedPurchaseRecordOption)
        {
            string callbackId = RTCallback<ObtainOwnedPurchaseRecordSuccessResult, ObtainOwnedPurchaseRecordFailResult>
                .AddCallback(
                    obtainOwnedPurchaseRecordOption.success,
                    obtainOwnedPurchaseRecordOption.fail,
                    obtainOwnedPurchaseRecordOption.complete
                );
            string conf = JsonUtility.ToJson(obtainOwnedPurchaseRecordOption);
            QG_ObtainOwnedPurchaseRecord(conf, callbackId);
        }

        public void StartIapActivity(StartIapActivityOption startIapActivityOption)
        {
            string callbackId = RTCallbackFail<StartIapActivityFailResult>.AddCallback(
                startIapActivityOption.success,
                startIapActivityOption.fail,
                startIapActivityOption.complete
            );
            string conf = JsonUtility.ToJson(startIapActivityOption);
            QG_StartIapActivity(conf, callbackId);
        }

        private Action<string> _onKeyboardInputAction = null;

        private Action<string> _onKeyboardConfirmAction = null;

        private Action<string> _onKeyboardCompleteAction = null;

        public void HideKeyboard(HideKeyboardOption hideKeyboardOption)
        {
            string callbackId = RTCallback.AddCallback(
                hideKeyboardOption.success,
                hideKeyboardOption.fail,
                hideKeyboardOption.complete
            );
            QG_HideKeyboard(callbackId);
        }

        private static readonly string[] ConfirmType = { "done", "next", "search", "go", "send" };

        public void ShowKeyboard(ShowKeyboardOption showKeyboardOption)
        {
            string callbackId = RTCallback.AddCallback(
                showKeyboardOption.success,
                showKeyboardOption.fail,
                showKeyboardOption.complete
            );
            if (showKeyboardOption.maxLength == 0)
            {
                showKeyboardOption.maxLength = 100;
            }

            if (!string.IsNullOrEmpty(showKeyboardOption.confirmType))
            {
                bool success = false;
                foreach (string type in ConfirmType)
                {
                    if (type == showKeyboardOption.confirmType)
                    {
                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    showKeyboardOption.confirmType = "";
                }
            }

            string conf = JsonUtility.ToJson(showKeyboardOption);
            QG_ShowKeyboard(conf, callbackId);
        }

        public void UpdateKeyboard(UpdateKeyboardOption updateKeyboardOption)
        {
            string callbackId = RTCallback<UpdateKeyboardSuccessResult, UpdateKeyboardFailResult>.AddCallback(
                updateKeyboardOption.success,
                updateKeyboardOption.fail,
                updateKeyboardOption.complete
            );
            string conf = JsonUtility.ToJson(updateKeyboardOption);
            QG_UpdateKeyboard(conf, callbackId);
        }

        public void OnKeyboardInput(Action<string> action)
        {
            if (action == null)
            {
                return;
            }

            _onKeyboardInputAction += action;
            if (_onKeyboardInputAction.GetInvocationList().Length == 1)
            {
                QG_OnKeyboardInput();
            }
        }

        public void OffKeyboardInput(Action<string> action)
        {
            if (_onKeyboardInputAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onKeyboardInputAction = null;
                QG_OffKeyboardInput();
            }
            else
            {
                _onKeyboardInputAction -= action;
                if (_onKeyboardInputAction == null)
                {
                    QG_OffKeyboardInput();
                }
            }
        }

        public void OnKeyboardConfirm(Action<string> action)
        {
            if (action == null)
            {
                return;
            }

            _onKeyboardConfirmAction += action;
            if (_onKeyboardConfirmAction.GetInvocationList().Length == 1)
            {
                QG_OnKeyboardConfirm();
            }
        }

        public void OffKeyboardConfirm(Action<string> action)
        {
            if (_onKeyboardConfirmAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onKeyboardConfirmAction = null;
                QG_OffKeyboardConfirm();
            }
            else
            {
                _onKeyboardConfirmAction -= action;
                if (_onKeyboardConfirmAction == null)
                {
                    QG_OffKeyboardConfirm();
                }
            }
        }

        public void OnKeyboardComplete(Action<string> action)
        {
            if (action == null)
            {
                return;
            }

            _onKeyboardCompleteAction += action;
            if (_onKeyboardCompleteAction.GetInvocationList().Length == 1)
            {
                QG_OnKeyboardComplete();
            }
        }

        public void OffKeyboardComplete(Action<string> action)
        {
            if (_onKeyboardCompleteAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onKeyboardCompleteAction = null;
                QG_OffKeyboardComplete();
            }
            else
            {
                _onKeyboardCompleteAction -= action;
                if (_onKeyboardCompleteAction == null)
                {
                    QG_OffKeyboardComplete();
                }
            }
        }

        private class StorageJson
        {
            public int errCode = 0;
            public string data = null;
        }

        public int LocalStorageLength()
        {
            return QG_LocalStorageLength();
        }

        public void LocalStorageClear()
        {
            QG_LocalStorageClear();
        }

        public void LocalStorageRemoveItem(string key)
        {
            QG_LocalStorageRemoveItem(key);
        }

        public void LocalStorageSetItem(string key, string value)
        {
            QG_LocalStorageSetItem(key, value);
        }

        public string LocalStorageGetItem(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            string res = QG_LocalStorageGetItem(key);
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }

            StorageJson storage = JsonUtility.FromJson<StorageJson>(res);
            if (storage.errCode == 0)
            {
                return storage.data;
            }

            return null;
        }

        public string LocalStorageKey(int index)
        {
            string res = QG_LocalStorageKey(index);
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }

            StorageJson storage = JsonUtility.FromJson<StorageJson>(res);
            if (storage.errCode == 0)
            {
                return storage.data;
            }

            return null;
        }

        public void SetPreferredFramesPerSecond(int fps)
        {
            QG_SetPreferredFramesPerSecond(fps);
        }

        public InnerAudioContext CreateInnerAudioContext(InnerAudioContextOption param = null)
        {
            if (param == null)
            {
                param = new InnerAudioContextOption();
            }
            return InnerAudioContextFactory.Instance.CreateInnerAudioContext(param);
        }

        private static Dictionary<string, Action<int>> PreDownloadAudiosDict = new Dictionary<string, Action<int>>();
        public void PreDownloadAudios(string[] pathList, Action<int> action)
        {
            string callbackId = Guid.NewGuid().ToString();
            PreDownloadAudiosDict.Add(callbackId, action);
            QG_PreDownloadAudios(string.Join(",", pathList), callbackId);
        }

        private CacheManager _cacheManager;

        public CacheManager GetCacheManager()
        {
            return _cacheManager ?? (_cacheManager = new CacheManager());
        }

        public void GetClipboardData(GetClipboardDataOption getClipboardDataOption)
        {
            string callbackId = RTCallbackSuc<GetClipboardDataSuccessResult>.AddCallback(
                getClipboardDataOption.success,
                getClipboardDataOption.fail,
                getClipboardDataOption.complete
            );
            QG_GetClipboardData(callbackId);
        }

        public void SetClipboardData(SetClipboardDataOption setClipboardDataOption)
        {
            string callbackId = RTCallback.AddCallback(
                setClipboardDataOption.success,
                setClipboardDataOption.fail,
                setClipboardDataOption.complete
            );
            string conf = JsonUtility.ToJson(setClipboardDataOption);
            QG_SetClipboardData(conf, callbackId);
        }

        private Action<OnTouchStartCallbackResult> _onTouchStartAction = null;
        private Action<OnTouchStartCallbackResult> _onTouchMoveAction = null;
        private Action<OnTouchStartCallbackResult> _onTouchEndAction = null;
        private Action<OnTouchStartCallbackResult> _onTouchCancelAction = null;

        public void OnTouchStart(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchStartAction == null)
            {
                QG_OnTouchStart();
            }

            _onTouchStartAction += result;
        }

        public void OffTouchStart(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchStartAction == null)
            {
                return;
            }

            if (result == null)
            {
                _onTouchStartAction = null;
                QG_OffTouchStart();
            }
            else
            {
                _onTouchStartAction -= result;
                if (_onTouchStartAction == null)
                {
                    QG_OffTouchStart();
                }
            }
        }

        public void OnTouchMove(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchMoveAction == null)
            {
                QG_OnTouchMove();
            }

            _onTouchMoveAction += result;
        }

        public void OffTouchMove(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchMoveAction == null)
            {
                return;
            }

            if (result == null)
            {
                _onTouchMoveAction = null;
                QG_OffTouchMove();
            }
            else
            {
                _onTouchMoveAction -= result;
                if (_onTouchMoveAction == null)
                {
                    QG_OffTouchMove();
                }
            }
        }

        public void OnTouchEnd(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchEndAction == null)
            {
                QG_OnTouchEnd();
            }

            _onTouchEndAction += result;
        }

        public void OffTouchEnd(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchEndAction == null)
            {
                return;
            }

            if (result == null)
            {
                _onTouchEndAction = null;
                QG_OffTouchEnd();
            }
            else
            {
                _onTouchEndAction -= result;
                if (_onTouchEndAction == null)
                {
                    QG_OffTouchEnd();
                }
            }
        }

        public void OnTouchCancel(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchCancelAction == null)
            {
                QG_OnTouchCancel();
            }

            _onTouchCancelAction += result;
        }

        public void OffTouchCancel(Action<OnTouchStartCallbackResult> result)
        {
            if (_onTouchCancelAction == null)
            {
                return;
            }

            if (result == null)
            {
                _onTouchCancelAction = null;
                QG_OffTouchCancel();
            }
            else
            {
                _onTouchCancelAction -= result;
                if (_onTouchCancelAction == null)
                {
                    QG_OffTouchCancel();
                }
            }
        }

        public void GetSystemInfo(GetSystemInfoOption getSystemInfoOption)
        {
            string callbackId = RTCallbackSuc<GetSystemInfoSuccessResult>.AddCallback(
                getSystemInfoOption.success,
                getSystemInfoOption.fail,
                getSystemInfoOption.complete
            );
            QG_GetSystemInfo(callbackId);
        }

        public GetSystemInfoSuccessResult GetSystemInfoSync()
        {
            string res = QG_GetSystemInfoSync();
            return JsonUtility.FromJson<GetSystemInfoSuccessResult>(res);
        }

        private Action<OnErrorResult> _onErrorAction = null;

        public void OnError(Action<OnErrorResult> action)
        {
            if (action == null)
            {
                return;
            }

            _onErrorAction += action;
            if (_onErrorAction.GetInvocationList().Length == 1)
            {
                QG_OnError();
            }
        }

        public void OffError(Action<OnErrorResult> action)
        {
            if (_onErrorAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onErrorAction = null;
                QG_OffError();
            }
            else
            {
                _onErrorAction -= action;
                if (_onErrorAction == null)
                {
                    QG_OffError();
                }
            }
        }

        public void ExitApplication(ExitApplicationOption exitApplicationOption)
        {
            string callbackId = RTCallback.AddCallback(
                exitApplicationOption.success,
                exitApplicationOption.fail,
                exitApplicationOption.complete
            );
            QG_ExitApplication(callbackId);
        }

        public GetLaunchOptionsSyncResult GetLaunchOptionsSync()
        {
            string res = QG_GetLaunchOptionsSync();
            return JsonUtility.FromJson<GetLaunchOptionsSyncResult>(res);
        }

        private Action _onHideAction = null;
        private Action<string> _onShowAction = null;

        public void OnHide(Action action)
        {
            if (action == null)
            {
                return;
            }

            _onHideAction += action;
            if (_onHideAction.GetInvocationList().Length == 1)
            {
                QG_OnHide();
            }
        }

        public void OffHide(Action action)
        {
            if (_onHideAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onHideAction = null;
                QG_OffHide();
            }
            else
            {
                _onHideAction -= action;
                if (_onHideAction == null)
                {
                    QG_OffHide();
                }
            }
        }

        public void OnShow(Action<string> action)
        {
            if (action == null)
            {
                return;
            }

            _onShowAction += action;
            if (_onShowAction.GetInvocationList().Length == 1)
            {
                QG_OnShow();
            }
        }

        public void OffShow(Action<string> action)
        {
            if (_onShowAction == null)
            {
                return;
            }

            if (action == null)
            {
                _onShowAction = null;
                QG_OffShow();
            }
            else
            {
                _onShowAction -= action;
                if (_onShowAction == null)
                {
                    QG_OffShow();
                }
            }
        }

        public void NavigateToQuickApp<T>(NavigateToQuickAppOption<T> navigateToQuickAppOption)
        {
            string callbackId = RTCallback.AddCallback(
                navigateToQuickAppOption.success,
                navigateToQuickAppOption.fail,
                navigateToQuickAppOption.complete
            );
            string conf = JsonUtility.ToJson(navigateToQuickAppOption);
            QG_NavigateToQuickApp(conf, callbackId);
        }

        public void OpenDeeplink(OpenDeeplinkOption openDeeplinkOption)
        {
            string conf = JsonUtility.ToJson(openDeeplinkOption);
            QG_OpenDeeplink(conf);
        }

        public void HasShortcutInstalled(HasShortcutInstalledOption hasShortcutInstalledOption)
        {
            string callbackId =
                RTCallback<HasShortcutInstalledSuccessResult, HasShortcutInstalledFailResult>.AddCallback(
                    hasShortcutInstalledOption.success,
                    hasShortcutInstalledOption.fail,
                    hasShortcutInstalledOption.complete
                );
            QG_HasShortcutInstalled(callbackId);
        }

        public void InstallShortcut(InstallShortcutOption installShortcutOption)
        {
            string callbackId = RTCallback<InstallShortcutSuccessResult, InstallShortcutFailResult>.AddCallback(
                installShortcutOption.success,
                installShortcutOption.fail,
                installShortcutOption.complete
            );
            string conf = JsonUtility.ToJson(installShortcutOption);
            QG_InstallShortcut(conf, callbackId);
        }

        public DownloadTask DownloadFile(DownloadFileOption downloadFileOption)
        {
            return DownloadTaskFactory.Instance.DownloadFile(downloadFileOption);
        }

        public string USER_DATA_PATH()
        {
            return QG_ENV_USER_DATA_PATH();
        }

        public FileSystemManager GetFileSystemManager()
        {
            return FileSystemManager.Instance;
        }

        # region JS Call

        public void _OnGameLoginCallback(string res)
        {
            RTCallback<LoginSuccessResult, LoginFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 激励视频广告创建结果回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnCreateRewardedVideoAdCallback(string res)
        {
            RTCallback<CreateRewardedVideoAdSuccessResult, CreateRewardedVideoAdFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 激励视频广告onLoad回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _RewardedVideoAdOnLoad(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            RewardedVideoAd rewardedVideoAd =
                RewardedVideoAdFactory.Instance._GetRewardedVideoAd((callbackBase._callbackId));
            rewardedVideoAd?._GetLoadCallback()?.Invoke();
        }

        /// <summary>
        /// 激励视频广告onClose回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _RewardedVideoAdOnClose(string res)
        {
            RewardedVideoAdCloseResult rewardedVideoAdCloseResult =
                JsonUtility.FromJson<RewardedVideoAdCloseResult>(res);
            RewardedVideoAd rewardedVideoAd =
                RewardedVideoAdFactory.Instance._GetRewardedVideoAd(rewardedVideoAdCloseResult._callbackId);
            rewardedVideoAd?._GetCloseCallback()?.Invoke(rewardedVideoAdCloseResult);
        }

        /// <summary>
        /// 激励视频广告onError回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _RewardedVideoAdOnError(string res)
        {
            AdErrorResult adErrorResult = JsonUtility.FromJson<AdErrorResult>(res);
            RewardedVideoAd rewardedVideoAd =
                RewardedVideoAdFactory.Instance._GetRewardedVideoAd(adErrorResult._callbackId);
            rewardedVideoAd?._GetErrorCallback()?.Invoke(adErrorResult);
        }

        /// <summary>
        /// 判断当前华为帐号所属国家或地区是否支持华为IAP支付回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnIsEnvReadyCallback(string res)
        {
            RTCallback<IsEnvReadySuccessResult, IsEnvReadyFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 判断华为帐号和快游戏RPK版本是否满足沙盒条件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnIsSandboxActivatedCallback(string res)
        {
            RTCallback<IsSandboxActivatedSuccessResult, IsSandboxActivatedFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 查询用户的已购买数据，包括消耗型商品、非消耗型商品和订阅类商品回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnObtainOwnedPurchasesCallback(string res)
        {
            RTCallback<ObtainOwnedPurchasesSuccessResult, ObtainOwnedPurchasesFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 查询在AGC控制台配置的商品详情回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnObtainProductInfoCallback(string res)
        {
            RTCallback<ObtainProductInfoSuccessResult, ObtainProductInfoFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 创建在AGC控制台配置的商品购买订单，支持购买消耗型商品、非消耗型商品、订阅类商品回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnCreatePurchaseIntentCallback(string res)
        {
            RTCallback<CreatePurchaseIntentSuccessResult, CreatePurchaseIntentFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 消耗已支付成功的消耗型商品回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnConsumeOwnedPurchaseCallback(string res)
        {
            RTCallback<ConsumeOwnedPurchaseSuccessResult, ConsumeOwnedPurchaseFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 查询消耗型商品的历史消耗商品信息或订阅类商品的订阅收据回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnObtainOwnedPurchaseRecordCallback(string res)
        {
            RTCallback<ObtainOwnedPurchaseRecordSuccessResult, ObtainOwnedPurchaseRecordFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 打开华为应用内支付相关页面回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnStartIapActivityCallback(string res)
        {
            RTCallbackFail<StartIapActivityFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 隐藏软键盘回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnHideKeyboardCallback(string res)
        {
            RTCallback.OnCallback(res);
        }

        /// <summary>
        /// 显示软键盘回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnShowKeyboardCallback(string res)
        {
            RTCallback.OnCallback(res);
        }

        /// <summary>
        /// 更新键盘输入框显示的默认值的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnUpdateKeyboardCallback(string res)
        {
            RTCallback<UpdateKeyboardSuccessResult, UpdateKeyboardFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 监听键盘输入事件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnKeyboardInputCallback(string res)
        {
            if (_onKeyboardInputAction != null)
            {
                OnKeyboardInputResult obj = JsonUtility.FromJson<OnKeyboardInputResult>(res);
                _onKeyboardInputAction.Invoke(obj.value);
            }
        }

        /// <summary>
        /// 监听用户点击键盘 confirm 按钮时的事件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnKeyboardConfirmCallback(string res)
        {
            if (_onKeyboardConfirmAction != null)
            {
                OnKeyboardConfirmResult obj = JsonUtility.FromJson<OnKeyboardConfirmResult>(res);
                _onKeyboardConfirmAction.Invoke(obj.value);
            }
        }

        /// <summary>
        /// 监听键盘收起的事件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnKeyboardCompleteCallback(string res)
        {
            if (_onKeyboardCompleteAction != null)
            {
                OnKeyboardCompleteResult obj = JsonUtility.FromJson<OnKeyboardCompleteResult>(res);
                _onKeyboardCompleteAction.Invoke(obj.value);
            }
        }

        /// <summary>
        /// 监听音频自然播放至结束的事件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnEnded(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onEnded", null);
        }

        /// <summary>
        /// 监听音频播放事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnPlay(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onPlay", null);
        }

        /// <summary>
        /// 监听音频暂停事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnPause(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onPause", null);
        }

        /// <summary>
        /// 监听音频停止事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnStop(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onStop", null);
        }

        /// <summary>
        /// 监听音频播放错误事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnError(string res)
        {
            InnerAudioContextErrorResult innerAudioContextErrorResult =
                JsonUtility.FromJson<InnerAudioContextErrorResult>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((innerAudioContextErrorResult._callbackId));
            innerAudioContext?._HandleCallBack("onError", innerAudioContextErrorResult);
        }

        /// <summary>
        /// 监听音频进入可以播放状态的事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnCanPlay(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onCanPlay", null);
        }

        /// <summary>
        /// 监听音频加载中事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnWaiting(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onWaiting", null);
        }

        /// <summary>
        /// 监听音频进行跳转操作的事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnSeeking(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onSeeking", null);
        }

        /// <summary>
        /// 监听音频完成跳转操作的事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _InnerAudioContextOnSeeked(string res)
        {
            CallbackBase callbackBase = JsonUtility.FromJson<CallbackBase>(res);
            InnerAudioContext innerAudioContext =
                InnerAudioContextFactory.Instance._GetInnerAudioContext((callbackBase._callbackId));
            innerAudioContext?._HandleCallBack("onSeeked", null);
        }

        /// <summary>
        /// 监听音频预下载的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnPreDownloadAudiosCallback(string res)
        {
            InnerAudioContextErrorResult result = JsonUtility.FromJson<InnerAudioContextErrorResult>(res);
            if (PreDownloadAudiosDict.ContainsKey(result._callbackId)) {
                var action = PreDownloadAudiosDict[result._callbackId];
                action.Invoke(result.errCode);
            }
        }

        /// <summary>
        /// 获取系统剪贴板的内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnGetClipboardDataCallback(string res)
        {
            RTCallbackSuc<GetClipboardDataSuccessResult>.OnCallback(res);
        }

        /// <summary>
        /// 设置系统剪贴板的内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnSetClipboardDataCallback(string res)
        {
            RTCallback.OnCallback(res);
        }

        /// <summary>
        /// 监听开始触摸事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnTouchStartCallback(string res)
        {
            if (_onTouchStartAction != null)
            {
                OnTouchStartCallbackResult obj = JsonUtility.FromJson<OnTouchStartCallbackResult>(res);
                _onTouchStartAction.Invoke(obj);
            }
        }

        /// <summary>
        /// 监听触点移动事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnTouchMoveCallback(string res)
        {
            if (_onTouchMoveAction != null)
            {
                OnTouchStartCallbackResult obj = JsonUtility.FromJson<OnTouchStartCallbackResult>(res);
                _onTouchMoveAction.Invoke(obj);
            }
        }

        /// <summary>
        /// 监听触摸结束事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnTouchEndCallback(string res)
        {
            if (_onTouchEndAction != null)
            {
                OnTouchStartCallbackResult obj = JsonUtility.FromJson<OnTouchStartCallbackResult>(res);
                _onTouchEndAction.Invoke(obj);
            }
        }

        /// <summary>
        /// 监听触点失效事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnTouchCancelCallback(string res)
        {
            if (_onTouchCancelAction != null)
            {
                OnTouchStartCallbackResult obj = JsonUtility.FromJson<OnTouchStartCallbackResult>(res);
                _onTouchCancelAction.Invoke(obj);
            }
        }

        /// <summary>
        /// 获取系统信息的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnGetSystemInfoCallback(string res)
        {
            RTCallbackSuc<GetSystemInfoSuccessResult>.OnCallback(res);
        }

        /// <summary>
        /// 监听全局错误事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnErrorCallback(string res)
        {
            if (_onErrorAction != null)
            {
                OnErrorResult obj = JsonUtility.FromJson<OnErrorResult>(res);
                _onErrorAction.Invoke(obj);
            }
        }

        /// <summary>
        /// 退出当前快游戏的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnExitApplicationCallback(string res)
        {
            RTCallback.OnCallback(res);
        }

        /// <summary>
        /// 监听隐藏到后台事件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnHideCallback(string res)
        {
            if (_onHideAction != null)
            {
                _onHideAction.Invoke();
            }
        }

        /// <summary>
        /// 监听快游戏回到前台的事件回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnShowCallback(string res)
        {
            if (_onShowAction != null)
            {
                _onShowAction.Invoke(res);
            }
        }

        /// <summary>
        /// 打开另一个快游戏的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnNavigateToQuickAppCallback(string res)
        {
            RTCallback.OnCallback(res);
        }

        /// <summary>
        /// 获取桌面图标是否创建的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnHasShortcutInstalledCallback(string res)
        {
            RTCallback<HasShortcutInstalledSuccessResult, HasShortcutInstalledFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 创建桌面图标的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnInstallShortcutCallback(string res)
        {
            RTCallback<InstallShortcutSuccessResult, InstallShortcutFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 下载文件资源到本地的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _OnDownloadFileCallback(string res)
        {
            RTCallback<DownloadFileSuccessResult, DownloadFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 监听下载进度变化事件的回调。
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _DownloadTaskOnProgressUpdateCallback(string res)
        {
            DownloadTaskOnProgressUpdateResult downloadTaskOnProgressUpdateResult =
                JsonUtility.FromJson<DownloadTaskOnProgressUpdateResult>(res);
            DownloadTask downloadTask =
                DownloadTaskFactory.Instance._GetDownloadTask((downloadTaskOnProgressUpdateResult._callbackId));
            downloadTask?._GetOnProgressUpdateCallback()?.Invoke(downloadTaskOnProgressUpdateResult);
        }

        /// <summary>
        /// 判断文件/目录是否存在的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerAccessCallback(string res)
        {
            RTCallbackFail<AccessFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 复制文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerCopyFileCallback(string res)
        {
            RTCallbackFail<CopyFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 创建目录的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerMkdirCallback(string res)
        {
            RTCallbackFail<MkdirFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 删除目录的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerRmdirCallback(string res)
        {
            RTCallbackFail<RmdirFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 读取目录内文件列表的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerReaddirCallback(string res)
        {
            RTCallback<ReaddirSuccessResult, ReaddirFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 读取本地文件内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerReadFileStringCallback(string res)
        {
            RTCallback<ReadFileStringSuccessResult,ReadFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 读取本地文件内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerReadFileBinaryCallback(string res)
        {
            FSReadFileCallback.OnCallback(res);
        }

        /// <summary>
        /// 重命名文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerRenameCallback(string res)
        {
            RTCallbackFail<RenameFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 获取文件stats对象的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerStatFileStatsCallback(string res)
        {
            RTCallback<StatSuccessFileStatsResult, StatFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 获取文件stats对象的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerStatStatsCallback(string res)
        {
            RTCallback<StatSuccessStatsResult, StatFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 删除文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerUnlinkCallback(string res)
        {
            RTCallbackFail<UnlinkFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 解压缩文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerUnzipCallback(string res)
        {
            RTCallbackFail<UnzipFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 写文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerWriteFileStringCallback(string res)
        {
            RTCallbackFail<WriteFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 写文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerWriteFileBinaryCallback(string res)
        {
            RTCallbackFail<WriteFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 保存临时文件到本地的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerSaveFileCallback(string res)
        {
            RTCallback<SaveFileSuccessResult, SaveFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 在文件结尾追加内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerAppendFileStringCallback(string res)
        {
            RTCallbackFail<AppendFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 在文件结尾追加内容的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerAppendFileBinaryCallback(string res)
        {
            RTCallbackFail<AppendFileFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 获取本地临时文件或本地用户文件的文件信息的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerGetFileInfoCallback(string res)
        {
            RTCallback<GetFileInfoSuccessResult, GetFileInfoFailResult>.OnCallback(res);
        }

        /// <summary>
        /// 删除该快游戏下已保存的本地缓存文件的回调
        /// </summary>
        /// <param name="res">回调结果</param>
        public void _FileSystemManagerRemoveSavedFileCallback(string res)
        {
            RTCallbackFail<RemoveSavedFileFailResult>.OnCallback(res);
        }

        #endregion
    }
}