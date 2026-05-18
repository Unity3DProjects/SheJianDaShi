var qg_wasm_sdk_api = {
    QG_GameLogin: function (conf, callbackId) {
        window.ral.QG_GameLogin(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_CreateRewardedVideoAd: function (conf, callbackId) {
        window.ral.QG_CreateRewardedVideoAd(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_RewardedVideoAdLoad: function (callbackId) {
        window.ral.QG_RewardedVideoAdLoad(Pointer_stringify(callbackId));
    },
    QG_RewardedVideoAdShow: function (callbackId) {
        window.ral.QG_RewardedVideoAdShow(Pointer_stringify(callbackId));
    },
    QG_RewardedVideoAdDestroy: function (callbackId) {
        window.ral.QG_RewardedVideoAdDestroy(Pointer_stringify(callbackId));
    },
    QG_SetTagForChildProtection: function (conf, callbackId) {
        window.ral.QG_SetTagForChildProtection(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_SetTagForUnderAgeOfPromise: function (conf, callbackId) {
        window.ral.QG_SetTagForUnderAgeOfPromise(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_SetAdContentClassification: function (conf, callbackId) {
        window.ral.QG_SetAdContentClassification(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_SetNonPersonalizedAd: function (conf, callbackId) {
        window.ral.QG_SetNonPersonalizedAd(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_IsEnvReady: function (conf, callbackId) {
        window.ral.QG_IsEnvReady(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_IsSandboxActivated: function (conf, callbackId) {
        window.ral.QG_IsSandboxActivated(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_ObtainOwnedPurchases: function (conf, callbackId) {
        window.ral.QG_ObtainOwnedPurchases(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_ObtainProductInfo: function (conf, callbackId) {
        window.ral.QG_ObtainProductInfo(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_CreatePurchaseIntent: function (conf, callbackId) {
        window.ral.QG_CreatePurchaseIntent(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_ConsumeOwnedPurchase: function (conf, callbackId) {
        window.ral.QG_ConsumeOwnedPurchase(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_ObtainOwnedPurchaseRecord: function (conf, callbackId) {
        window.ral.QG_ObtainOwnedPurchaseRecord(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_StartIapActivity: function (conf, callbackId) {
        window.ral.QG_StartIapActivity(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_HideKeyboard: function (callbackId) {
        window.ral.QG_HideKeyboard(Pointer_stringify(callbackId));
    },
    QG_ShowKeyboard: function (conf, callbackId) {
        window.ral.QG_ShowKeyboard(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_UpdateKeyboard: function (value, callbackId) {
        window.ral.QG_UpdateKeyboard(Pointer_stringify(value), Pointer_stringify(callbackId));
    },
    QG_OnKeyboardInput: function () {
        window.ral.QG_OnKeyboardInput();
    },
    QG_OffKeyboardInput: function () {
        window.ral.QG_OffKeyboardInput();
    },
    QG_OnKeyboardConfirm: function () {
        window.ral.QG_OnKeyboardConfirm();
    },
    QG_OffKeyboardConfirm: function () {
        window.ral.QG_OffKeyboardConfirm();
    },
    QG_OnKeyboardComplete: function () {
        window.ral.QG_OnKeyboardComplete();
    },
    QG_OffKeyboardComplete: function () {
        window.ral.QG_OffKeyboardComplete();
    },
    QG_LocalStorageClear: function () {
        window.ral.QG_LocalStorageClear();
    },
    QG_LocalStorageRemoveItem: function (key) {
        window.ral.QG_LocalStorageRemoveItem(Pointer_stringify(key));
    },
    QG_LocalStorageSetItem: function (key, value) {
        window.ral.QG_LocalStorageSetItem(Pointer_stringify(key), Pointer_stringify(value));
    },
    QG_LocalStorageGetItem: function (key) {
        var returnStr = window.ral.QG_LocalStorageGetItem(Pointer_stringify(key));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_LocalStorageKey: function (index) {
        var returnStr = window.ral.QG_LocalStorageKey(index);
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_LocalStorageLength: function () {
        return window.ral.QG_LocalStorageLength();
    },
    QG_SetPreferredFramesPerSecond: function (fps) {
        window.ral.QG_SetPreferredFramesPerSecond(fps);
    },
    QG_CreateInnerAudioContext: function (callbackId) {
        window.ral.QG_CreateInnerAudioContext(Pointer_stringify(callbackId));
    },
    QG_CreateInnerAudioContextWithParam: function (callbackId, src, loop, startTime, autoplay, volume, obeyMuteSwitch, needDownload) {
        window.ral.QG_CreateInnerAudioContextWithParam(Pointer_stringify(callbackId), Pointer_stringify(src), loop, startTime, autoplay, volume, obeyMuteSwitch, needDownload);
    },
    QG_PreDownloadAudios: function (paths, callbackId) {
        window.ral.QG_PreDownloadAudios(Pointer_stringify(paths), Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextPlay: function (callbackId) {
        window.ral.QG_InnerAudioContextPlay(Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextPause: function (callbackId) {
        window.ral.QG_InnerAudioContextPause(Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextStop: function (callbackId) {
        window.ral.QG_InnerAudioContextStop(Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextSeek: function (position, callbackId) {
        window.ral.QG_InnerAudioContextSeek(position, Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextDestroy: function (callbackId) {
        window.ral.QG_InnerAudioContextDestroy(Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextSetBool: function (key, value, callbackId) {
        window.ral.QG_InnerAudioContextSetBool(Pointer_stringify(key), value, Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextSetString: function (key, value, callbackId) {
        window.ral.QG_InnerAudioContextSetString(Pointer_stringify(key), Pointer_stringify(value), Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextSetFloat: function (key, value, callbackId) {
        window.ral.QG_InnerAudioContextSetFloat(Pointer_stringify(key), value, Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextGetBool: function (key, callbackId) {
        return window.ral.QG_InnerAudioContextGetBool(Pointer_stringify(key), Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextGetString: function (key, callbackId) {
        var returnStr = window.ral.QG_InnerAudioContextGetString(Pointer_stringify(key), Pointer_stringify(callbackId));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_InnerAudioContextGetFloat: function (key, callbackId) {
        return window.ral.QG_InnerAudioContextGetFloat(Pointer_stringify(key), Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextAddListener: function (key, callbackId) {
        return window.ral.QG_InnerAudioContextAddListener(Pointer_stringify(key), Pointer_stringify(callbackId));
    },
    QG_InnerAudioContextRemoveListener: function (key, callbackId) {
        return window.ral.QG_InnerAudioContextRemoveListener(Pointer_stringify(key), Pointer_stringify(callbackId));
    },
    QG_CacheManagerAddCacheIdentifiers: function (conf) {
        window.ral.QG_CacheManagerAddCacheIdentifiers(Pointer_stringify(conf));
    },
    QG_CacheManagerAddExcludeCacheIdentifiers: function (conf) {
        window.ral.QG_CacheManagerAddExcludeCacheIdentifiers(Pointer_stringify(conf));
    },
    QG_CacheManagerHasCache: function (url) {
        return window.ral.QG_CacheManagerHasCache(Pointer_stringify(url));
    },
    QG_CacheManagerDeleteCache: function (url) {
        window.ral.QG_CacheManagerDeleteCache(Pointer_stringify(url));
    },
    QG_CacheManagerClearLru: function () {
        window.ral.QG_CacheManagerClearLru();
    },
    QG_GetClipboardData: function (callbackId) {
        window.ral.QG_GetClipboardData(Pointer_stringify(callbackId));
    },
    QG_SetClipboardData: function (conf, callbackId) {
        window.ral.QG_SetClipboardData(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_OnTouchStart: function () {
        window.ral.QG_OnTouchStart();
    },
    QG_OffTouchStart: function () {
        window.ral.QG_OffTouchStart();
    },
    QG_OnTouchMove: function () {
        window.ral.QG_OnTouchMove();
    },
    QG_OffTouchMove: function () {
        window.ral.QG_OffTouchMove();
    },
    QG_OnTouchEnd: function () {
        window.ral.QG_OnTouchEnd();
    },
    QG_OffTouchEnd: function () {
        window.ral.QG_OffTouchEnd();
    },
    QG_OnTouchCancel: function () {
        window.ral.QG_OnTouchCancel();
    },
    QG_OffTouchCancel: function () {
        window.ral.QG_OffTouchCancel();
    },
    QG_GetSystemInfo: function (callbackId) {
        window.ral.QG_GetSystemInfo(Pointer_stringify(callbackId));
    },
    QG_GetSystemInfoSync: function () {
        var returnStr = window.ral.QG_GetSystemInfoSync();
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_OnError: function () {
        window.ral.QG_OnError();
    },
    QG_OffError: function () {
        window.ral.QG_OffError();
    },
    QG_ExitApplication: function (callbackId) {
        window.ral.QG_ExitApplication(Pointer_stringify(callbackId));
    },
    QG_GetLaunchOptionsSync: function () {
        var returnStr = window.ral.QG_GetLaunchOptionsSync();
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_OnHide: function () {
        window.ral.QG_OnHide();
    },
    QG_OffHide: function () {
        window.ral.QG_OffHide();
    },
    QG_OnShow: function () {
        window.ral.QG_OnShow();
    },
    QG_OffShow: function () {
        window.ral.QG_OffShow();
    },
    QG_NavigateToQuickApp: function (conf, callbackId) {
        window.ral.QG_NavigateToQuickApp(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_OpenDeeplink: function (conf) {
        window.ral.QG_OpenDeeplink(Pointer_stringify(conf));
    },
    QG_HasShortcutInstalled: function (callbackId) {
        window.ral.QG_HasShortcutInstalled(Pointer_stringify(callbackId));
    },
    QG_InstallShortcut: function (conf, callbackId) {
        window.ral.QG_InstallShortcut(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_DownloadFile: function (conf, callbackId) {
        window.ral.QG_DownloadFile(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_DownloadTaskAbort: function (callbackId) {
        window.ral.QG_DownloadTaskAbort(Pointer_stringify(callbackId));
    },
    QG_ENV_USER_DATA_PATH: function () {
        var returnStr = window.ral.QG_ENV_USER_DATA_PATH();
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_CopyDataFromJs: function (callbackId, offset) {
        var fileContentCache = window.ral.QG_CopyDataFromJs(Pointer_stringify(callbackId));
        HEAPU8.set(new Uint8Array(fileContentCache), offset);
    },
    QG_GetFileSystemManager: function () {
        window.ral.QG_GetFileSystemManager();
    },
    QG_FileSystemManagerAccess: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerAccess(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerAccessSync: function (path) {
        var returnStr = window.ral.QG_FileSystemManagerAccessSync(Pointer_stringify(path));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerCopyFile: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerCopyFile(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerCopyFileSync: function (srcPath, destPath) {
        var returnStr = window.ral.QG_FileSystemManagerCopyFileSync(Pointer_stringify(srcPath), Pointer_stringify(destPath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerMkdir: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerMkdir(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerMkdirSync: function (dirPath, recursive) {
        var returnStr = window.ral.QG_FileSystemManagerMkdirSync(Pointer_stringify(dirPath), recursive);
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerRmdir: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerRmdir(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerRmdirSync: function (dirPath, recursive) {
        var returnStr = window.ral.QG_FileSystemManagerRmdirSync(Pointer_stringify(dirPath), recursive);
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerReaddir: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerReaddir(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerReaddirSync: function (dirPath) {
        var returnStr = window.ral.QG_FileSystemManagerReaddirSync(Pointer_stringify(dirPath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerReadFileString: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerReadFileString(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerReadFileBinary: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerReadFileBinary(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerReadFileStringSync: function (filePath) {
        var returnStr = window.ral.QG_FileSystemManagerReadFileStringSync(Pointer_stringify(filePath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerReadFileBinarySync: function (callbackId, filePath) {
        var returnStr = window.ral.QG_FileSystemManagerReadFileBinarySync(Pointer_stringify(callbackId), Pointer_stringify(filePath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerRename: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerRename(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerRenameSync: function (oldPath, newPath) {
        var returnStr = window.ral.QG_FileSystemManagerRenameSync(Pointer_stringify(oldPath), Pointer_stringify(newPath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerStat: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerStat(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerStatSync: function (path, recursive) {
        var returnStr = window.ral.QG_FileSystemManagerStatSync(Pointer_stringify(path), recursive);
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerUnlink: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerUnlink(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerUnlinkSync: function (path) {
        var returnStr = window.ral.QG_FileSystemManagerUnlinkSync(Pointer_stringify(path));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerUnzip: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerUnzip(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerWriteFileString: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerWriteFileString(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerWriteFileBinary: function (filePath, data, dataLength, callbackId) {
        window.ral.QG_FileSystemManagerWriteFileBinary(Pointer_stringify(filePath), HEAPU8.slice(data, dataLength + data).buffer, Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerWriteFileStringSync: function (filePath, data) {
        var returnStr = window.ral.QG_FileSystemManagerWriteFileStringSync(Pointer_stringify(filePath), Pointer_stringify(data));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerWriteFileBinarySync: function (filePath, data, dataLength) {
        var returnStr = window.ral.QG_FileSystemManagerWriteFileBinarySync(Pointer_stringify(filePath), HEAPU8.slice(data, dataLength + data));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerSaveFile: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerSaveFile(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerSaveFileSync: function (tempFilePath, filePath) {
        var returnStr = window.ral.QG_FileSystemManagerSaveFileSync(Pointer_stringify(tempFilePath), Pointer_stringify(filePath));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerAppendFileString: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerAppendFileString(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerAppendFileBinary: function (filePath, data, dataLength, callbackId) {
        window.ral.QG_FileSystemManagerAppendFileBinary(Pointer_stringify(filePath), HEAPU8.slice(data, dataLength + data).buffer, Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerAppendFileStringSync: function (filePath, data) {
        var returnStr = window.ral.QG_FileSystemManagerAppendFileStringSync(Pointer_stringify(filePath), Pointer_stringify(data));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerAppendFileBinarySync: function (filePath, data, dataLength) {
        var returnStr = window.ral.QG_FileSystemManagerAppendFileBinarySync(Pointer_stringify(filePath), HEAPU8.slice(data, dataLength + data));
        return QGHelper.stringToBuffer(returnStr);
    },
    QG_FileSystemManagerGetFileInfo: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerGetFileInfo(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    QG_FileSystemManagerRemoveSavedFile: function (conf, callbackId) {
        window.ral.QG_FileSystemManagerRemoveSavedFile(Pointer_stringify(conf), Pointer_stringify(callbackId));
    },
    $QGHelper: {
        stringToBuffer: function (valueStr) {
            var bufferSize = lengthBytesUTF8(valueStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(valueStr, buffer, bufferSize);
            return buffer;
        }
    }
};
autoAddDeps(qg_wasm_sdk_api, "$QGHelper");
mergeInto(LibraryManager.library, qg_wasm_sdk_api)