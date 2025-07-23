var QgGameBridge = {

    $CONSTANT: {
        ACTION_CALL_BACK_CLASS_NAME_DEFAULT: "QGMiniGameManager",
        ACTION_CALL_BACK_METHORD_NAME_DEFAULT: "DefaultResponseCallback",
        ACTION_CALL_BACK_METHORD_NAME_FILESYSTEM: "FileSystemResponseCallback",
        ACTION_CALL_BACK_METHORD_NAME_AD_ERROR: "AdOnErrorCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_LOAD: "AdOnLoadCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_SHOW: "AdOnShowCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE: "AdOnCloseCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_HIDE: "AdOnHideCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED: "RewardedVideoAdOnCloseCallBack",
        ACTION_CALL_BACK_METHORD_NAME_AD_LOAD_NATIVE: "NativeAdOnLoadCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_PLAY: "pdOnPlayCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_CANPLAY: "pdOnCanPlayCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_PAUSE: "pdOnPauseCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_STOP: "pdOnStopCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_ENDED: "pdOnEndedCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_TIMEUPDATE: "pdOnTimeUpdateCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_ERROR: "pdOnErrorCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_WAITING: "pdOnWaitingCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_SEEKING: "pdOnSeekingCallBack",
        ACTION_CALL_BACK_METHORD_NAME_PD_SEEKED: "pdOnSeekedCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_START: "rdOnStartCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_RESUME: "rdOnResumeCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_PAUSE: "rdOnPauseCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_STOP: "rdOnStopCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_FRAMERECORDED: "rdOnFrameRecordedCallBack",
        ACTION_CALL_BACK_METHORD_NAME_RD_ERROR: "rdOnErrorCallBack",
    },

    $mAdMap: {},

    $mFileData: {},

    $property: {
        cameraObj: null,
        cameraImageCallback: null,
        cameraData: null,
        cameraArPoseCallback: null,
        cameraArPose: null,
        deviceMotionChangeData: null,
        deviceMotionChangeCallback: null,
        cameraObjYuv: null,
        cameraImageYuvCallback: null,
        cameraYuvData: null,
        cameraArPlaneCallback: null,
        cameraArPlane: null,
        readFileData: null,
        onShowCallBack: null,
        onHideCallBack: null,
        onAuthDialogShowCallBack : null,
        onAuthDialogCloseCallBack : null,
      },

    QGCollectIndex: function (index) {},

    QGGetNetworkType: function (success, fail) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        qg.getNetworkType({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res,
                });
                console.log(json);
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetNetworkTypeCallback",
                    json
                );
                console.log("qg.getNetworkType success " + res.networkType);
            },
            fail: function (err) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetNetworkTypeCallback",
                    json
                );
                console.log("qg.getNetworkType fail " + err);
            },
        });
    },

    QGOnNetworkStatusChange: function (callback) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var callbackID = Pointer_stringify(callback);
        var func = function (data) {
            var json = JSON.stringify({
                callbackId: callbackID,
                isConnected: data.isConnected,
                networkType: data.networkType,
            });
            console.log(data);
            console.log(json);
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "OnNetworkStatusChangeResponseCallback",
                json
            );
        };
        qg.onNetworkStatusChange(func);
    },

    QGSetClipboardData: function (p, success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        qg.setClipboardData({
            data: UTF8ToString(p),
            success: function () {
                var json = JSON.stringify({
                    callbackId: successID,
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "SetClipboardDataCallback", json);
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                    errCode: res.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "SetClipboardDataCallback", json);
            }
        })
    },

    QGGetClipboardData: function (success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        qg.getClipboardData({
            success: function (data) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: data.data
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "GetClipboardDataCallback", json);
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                    errCode: res.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "GetClipboardDataCallback", json);
            }
        })
    },

    QGVibrateShort: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        qg.vibrateShort({
            type: "light", // heavy?medium?light
            success: function (res) {
                console.log("qg.vibrateShort-light");
            },
            fail: function (res) {},
        });
    },

    QGVibrateLong: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        qg.vibrateLong({
            success: function (res) {
                console.log("qg.vibrateLong");
            },
            fail: function (res) {},
        });
    },

    QGGetSystemInfo: function (success, fail) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);

        qg.getSystemInfo({
            success: function (res) {
                try {
                    if (res.safeArea && typeof res.safeArea === 'string') {
                        res.safeArea = parseStringToObject(res.safeArea)
                    }
                    if (res.allianceVersion && typeof res.allianceVersion === 'string') {
                        res.allianceVersion = parseInt(res.allianceVersion)
                    }
                } catch (err) {
                    console.error(err)
                }
                
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res,
                });
                console.log(json);
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "SystemInfoCallback",
                    json
                );
                console.log("getSystemInfo:" + JSON.stringify(res));
            },
            fail: function (err) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "SystemInfoCallback",
                    json
                );
                console.log("getSystemInfo failed");
            },
        });
    },

    QGGetSystemInfoSync: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var res = qg.getSystemInfoSync();
        try {
            if (res.safeArea && typeof res.safeArea === 'string') {
                res.safeArea = parseStringToObject(res.safeArea)
            }
            if (res.allianceVersion && typeof res.allianceVersion === 'string') {
                res.allianceVersion = parseInt(res.allianceVersion)
            }
        } catch (err) {
            console.error(err)
        }
        
        var returnStr = JSON.stringify(res);
        console.log(returnStr);
        if (!returnStr) {
            returnStr = "{}";
        }

        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    QGGetWindowInfo: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var res;
        try {
            res = qg.getWindowInfo();
        } catch (err) {
            console.error("qg.getWindowInfo error: ", err);

            var systemInfo = qg.getSystemInfoSync();
            res = {
                pixelRatio: systemInfo.pixelRatio || 1, 
                windowWidth: systemInfo.windowWidth,
                windowHeight: systemInfo.windowHeight,
                statusBarHeight: systemInfo.statusBarHeight,
                screenTop: systemInfo.screenTop || 0
            }
            if (systemInfo.safeArea){
                if (typeof systemInfo.safeArea === 'string') {
                    res.safeArea = parseStringToObject(systemInfo.safeArea)
                } else {
                    res.safeArea = systemInfo.safeArea;
                }
            }
        }
      
        var returnStr = JSON.stringify(res);
        console.log(returnStr);
        if (!returnStr) {
            returnStr = "{}";
        }

        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    QGGetMenuButtonBoundingClientRect: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var res = qg.getMenuButtonBoundingClientRect();
        var returnStr = JSON.stringify(res);
        console.log(returnStr);
        if (!returnStr) {
            returnStr = "{}";
        }

        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    QGShowModal: function (param, success, fail, complete) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.showModal(Object.assign({}, paramData, {
            success: function (res) {
                console.log("showModal success: " + JSON.stringify(res));
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "ShowModalCallback",
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    data: res,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "ShowModalCallback",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: res,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "ShowModalCallback",
                        json
                    );
                }
            },
        }));
    },

    QGShowKeyboard: function (param, success, cancel, complete) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        var successID = UTF8ToString(success);
        var cancelID = UTF8ToString(cancel);
        var completeID = UTF8ToString(complete);

        qg.showKeyboard({
            defaultValue: paramData.defaultValue,
            maxLength: paramData.maxLength,
            multiple: paramData.multiple,
            confirmHold: paramData.confirmHold,
            confirmType: paramData.confirmType,
            success: function (ret) {
                var json = JSON.stringify({
                    callbackId: successID,
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            },
            cancel: function (ret) {
                var json = JSON.stringify({
                    callbackId: cancelID,
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            },
            complete: function () {
                var json = JSON.stringify({
                    callbackId: completeID
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            }
        })
    },

    QGOnKeyboardInput: function (callback) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var callbackID = UTF8ToString(callback);

        var func = function (data) {
            var json = JSON.stringify({
                callbackId: callbackID,
                value: data.value
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
        };
        qg.onKeyboardInput(func);
    },

    QGOnKeyboardConfirm: function (callback) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var callbackID = UTF8ToString(callback);

        var func = function (data) {
            var json = JSON.stringify({
                callbackId: callbackID,
                value: data.value
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
        };
        qg.onKeyboardConfirm(func);
    },

    QGOnKeyboardComplete: function (callback) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var callbackID = UTF8ToString(callback);

        var func = function (data) {
            var json = JSON.stringify({
                callbackId: callbackID,
                value: data.value
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "OnKeyboardInputResponseCallback", json);
        };
        qg.onKeyboardComplete(func);
    },

    QGOffKeyboardInput: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.offKeyboardInput();
    },

    QGOffKeyboardConfirm: function () {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.offKeyboardConfirm();
    },

    QGOffKeyboardComplete: function (callback) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.offKeyboardComplete();
    },

    QGHideKeyboard: function () {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.hideKeyboard();
    },

    QGDownLoadFile: function (param, success, fail) {
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        try {
            if (paramData.header && paramData.header !== "") {
                paramData.header = JSON.parse(paramData.header);
            }
        } catch (err) {
            console.error(err)
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        qg.downloadFile(Object.assign({}, paramData, {
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res.statusCode ? res : res.data,
                });
                console.log("success: " + JSON.stringify(res));
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "DownloadFileCallback",
                    json
                );
            },
            fail: function (err) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                });
                console.log("fail: " + JSON.stringify(err));
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "DownloadFileCallback",
                    json
                );
            },
        }));
    },

    QGUploadFile: function (param, success, fail) {
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        try {
            if (paramData.header && paramData.header !== "") {
                paramData.header = JSON.parse(paramData.header);
            }
            if (paramData.formData && paramData.formData !== "") {
                paramData.formData = JSON.parse(paramData.formData);
            }
        } catch (err) {
            console.error(err)
        }
        qg.uploadFile(Object.assign({}, paramData, {
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res.statusCode ? res : res.data,
                });
                console.log("success: " + JSON.stringify(res));
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "UploadFileCallback",
                    json
                );
            },
            fail: function (err) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: err.errMsg,
                    errCode: err.errCode,
                });
                console.log("fail: " + JSON.stringify(err));
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "UploadFileCallback",
                    json
                );
            },
        }));
    },

    QGCreateVideo: function (pdId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var pdIdStr = UTF8ToString(pdId);
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        if (!paramData.src) {
            console.log("The video resource address cannot be empty");
            return;
        }
        var video = qg.createVideo(Object.assign({}, paramData, {}));
        console.log("qg.createVideo json " + JSON.stringify(paramData));
        // video.play();
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }

        mAdMap.set(pdIdStr, video);
        QGUniversalCallback(video, pdId);
    },

    QGPlayAudio: function (playerId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var pdIdStr = UTF8ToString(playerId);
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        var innerAudioContext = qg.createInnerAudioContext();
        innerAudioContext.startTime = paramData.startTime ? paramData.startTime : 0;
        innerAudioContext.autoplay = paramData.autoplay ? paramData.autoplay : false;
        innerAudioContext.loop = paramData.loop ? paramData.loop : false;
        innerAudioContext.volume = paramData.volume;
        innerAudioContext.playbackRate = paramData.playbackRate;
        innerAudioContext.src = paramData.src;
        innerAudioContext.play();
        console.log("QGPlayAudio -js url: " + paramData.src);
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        mAdMap.set(pdIdStr, innerAudioContext);
        QGUniversalCallback(innerAudioContext, playerId);
    },

    $QGUniversalCallback: function (pd, pdId) {
        var pdIdStr = UTF8ToString(pdId);
        if (pd.onCanplay) {
            pd.onCanplay(function () {
                console.log("onCanplay success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_CANPLAY,
                    json
                );
            });
        }

        if (pd.onPlay) {
            pd.onPlay(function () {
                console.log("onPlay success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_PLAY,
                    json
                );
            });
        }

        if (pd.onPause) {
            pd.onPause(function () {
                console.log("onPause success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_PAUSE,
                    json
                );
            });
        }

        if (pd.onStop) {
            pd.onStop(function () {
                console.log("onStop success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_STOP,
                    json
                );
            });
        }

        if (pd.onEnded) {
            pd.onEnded(function () {
                console.log("onEndedfunction success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_ENDED,
                    json
                );
            });
        }

        if (pd.onTimeUpdate) {
            pd.onTimeUpdate(function () {
                console.log("onTimeUpdate success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_TIMEUPDATE,
                    json
                );
            });
        }

        if (pd.onError) {
            pd.onError(function () {
                console.log("onError success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_ERROR,
                    json
                );
            });
        }

        if (pd.onWaiting) {
            pd.onWaiting(function () {
                console.log("onWaiting success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_WAITING,
                    json
                );
            });
        }

        if (pd.onSeeking) {
            pd.onSeeking(function () {
                console.log("onSeeking success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_SEEKING,
                    json
                );
            });
        }

        if (pd.onSeeked) {
            pd.onSeeked(function () {
                console.log("onSeeked success");
                var json = JSON.stringify({
                    callbackId: pdIdStr,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_PD_SEEKED,
                    json
                );
            });
        }
    },

    QGPlayMedia: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        console.log("playerId: ", playerId);
        var pdIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(pdIdStr);
        if (!pd || pd == null || pd == "undefined") {
            console.log("innerAudioContext is null");
            return;
        }
        if (pd && pd.play) {
            pd.play();
        }
    },

    QGPauseMedia: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var pdIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(pdIdStr);
        if (pd && pd.pause) {
            pd.pause();
        }
    },

    QGStopMedia: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var pdIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(pdIdStr);
        if (pd && pd.stop) {
            pd.stop();
        }
    },

    QGDestroyMedia: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var pdIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(pdIdStr);
        if (pd && pd.destroy) {
            pd.destroy();
        }
    },

    QGSeekMedia: function (playerId, time) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var pdIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(pdIdStr);
        if (pd && pd.seek) {
            pd.seek(time);
        }
    },

    QGAudioPlayerVolume: function (playerId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            console.log("QGAudioPlayerVolume", param);
            pd.volume = param;
        }
    },

    QGAudioPlayerLoop: function (playerId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            pd.loop = param;
        }
    },

    QGVideoPlayerRequestFullScreen: function (playerId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd && pd.requestFullScreen) {
            pd.requestFullScreen(param);
        }
    },

    QGVideoPlayerExitFullScreen: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd && pd.exitFullScreen) {
            pd.exitFullScreen();
        }
    },

    QGAudioPlayerGetDuration: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return 0;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            return pd.duration;
        } else {
            return 0;
        }
    },

    QGAudioPlayerGetCurrentTime: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return 0;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            return pd.currentTime;
        } else {
            return 0;
        }
    },

    QGAudioPlayerGetPaused: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return true;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            return pd.paused;
        } else {
            return true;
        }
    },

    QGAudioPlayerGetBuffered: function (playerId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return 0;
        }
        var playerIdStr = UTF8ToString(playerId);
        var pd = mAdMap.get(playerIdStr);
        if (pd) {
            return pd.buffered;
        } else {
            return 0;
        }
    },

    QGOnAudioInterruptionBegin: function () {
        qg.onAudioInterruptionBegin(function () {
            console.log(
                "onAudioInterruptionBegin success: " + new Date().toLocaleString()
            );
        });
        console.log("qg.onAudioInterruptionBegin");
    },

    QGOffAudioInterruptionBegin: function () {
        qg.offAudioInterruptionBegin();
        console.log("qg.onAudioInterruptionBegin");
    },

    QGOnAudioInterruptionEnd: function () {
        qg.onAudioInterruptionEnd(function () {
            console.log(
                "onAudioInterruptionEnd success: " + new Date().toLocaleString()
            );
        });
        console.log("qg.onAudioInterruptionEnd");
    },

    QGOffAudioInterruptionEnd: function () {
        qg.offAudioInterruptionEnd();
        console.log("qg.onAudioInterruptionEnd");
    },

    QGOnError: function () {
        qg.onError(function (res) {
            console.log("onError success: " + res.message.slice(0, 149));
        });
    },

    QGOffError: function () {
        qg.offError();
    },

    QGShowToast: function (param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        qg.showToast(Object.assign({}, paramData, {}));
    },

    QGShowLoading: function (param, mask) {
        var paramStr = UTF8ToString(param);
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.showLoading({
            title: paramStr,
            mask: Boolean(mask)
        });
    },

    QGHideLoading: function (success) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        qg.hideLoading({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
        });
    },

    QGSetTimeout: function (times, callback) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var callbackID = UTF8ToString(callback);
        var func = function () {
            var json = JSON.stringify({
                callbackId: callbackID,
            });
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                json
            );
        };
        setTimeout(func, times);
    },

    QGExitApplication: function () {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.exitApplication();
    },

    QGGetManifestInfo: function (success, fail) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        qg.getManifestInfo({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    errCode: res.errCode,
                    errMsg: res.errMsg,
                    data: JSON.parse(res.manifest),
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "ManifestInfoCallback",
                    json
                );
                console.log("QGGetManifestInfo -js " + json);
            },
            fail: function (err) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errCode: res.errCode,
                    errMsg: res.errMsg,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "ManifestInfoCallback",
                    json
                );
                console.log("getManifestInfo err", JSON.stringify(err));
            },
        });
    },

    QGGetProvider: function () {
        if (typeof qg == "undefined") {
            return;
        }
        var provider = qg.getProvider();
        console.log("qg.getProvider: ", provider);
        if (provider) {
            var bufferSize = lengthBytesUTF8(provider) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(provider, buffer, bufferSize);
            return buffer;
        } else {
            console.log("qg.getProvider fail");
            return;
        }
    },

    QGSetPreferredFramesPerSecond: function (fps) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (typeof fps == "number") {
            qg.setPreferredFramesPerSecond(fps);
            console.log("Change the render frame rate to ", fps);
        }
    },

    QGSetUserCloudStorage: function (param, success, fail, complete) {
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        var tempKey = paramData.key;
        var tempValue = paramData.value;
        var kvDataItem = {};
        kvDataItem[tempKey] = tempValue;
        qg.setUserCloudStorage({
            KVDataList: [kvDataItem],
            success: function (res) {
                console.log("QGSetUserCloudStorage success -js", res);

                var json = JSON.stringify({
                    callbackId: successID,
                    errMsg: res.errMsg,
                    code: res.code,
                    errCode: res.errCode,
                    data: res.data,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "SetUserCloudStorageCallBack",
                    json
                );
            },
            fail: function (res) {
                console.log("QGSetUserCloudStorage fail -js", res);
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                    code: res.code,
                    errCode: res.errCode,
                    data: res.data,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "SetUserCloudStorageCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "SetUserCloudStorageCallBack",
                        json
                    );
                }
  
            },
        });
    },

    QGGetUserCloudStorage: function (param, success, fail, complete) {
        var paramStr = UTF8ToString(param);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.getUserCloudStorage({
            keyList: [paramStr],
            success: function (res) {
                if (typeof res.KVDataList === 'string') {
                    var tempKVDataList = JSON.parse(res.KVDataList);
                }
                var cloudStorageValue = tempKVDataList[0][paramStr];
                if (cloudStorageValue) {
                    var keyValue = {
                        key: paramStr,
                        value: cloudStorageValue,
                    };
                    var json = JSON.stringify({
                        callbackId: successID,
                        data: keyValue,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetUserCloudStorageCallBack",
                        json
                    );
                } else {
                    console.log("There is no such value");
                }
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    data: null,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetUserCloudStorageCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetUserCloudStorageCallBack",
                        json
                    );
                }
            },
        });
    },

    QGRemoveUserCloudStorage: function (param) {
        var paramStr = UTF8ToString(param);
        qg.removeUserCloudStorage({
            keyList: [paramStr],
            success: function (res) {},
            fail: function (res) {},
            complete: function (res) {},
        });
    },

    QGGetBatteryInfo: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.getBatteryInfo({
            success: function (res) {
                var BatteryInfo = {
                    level: res.level,
                    isCharging: res.isCharging,
                };
                var json = JSON.stringify({
                    callbackId: successID,
                    data: BatteryInfo,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetBatteryInfoCallBack",
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    data: null,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetBatteryInfoCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetBatteryInfoCallBack",
                        json
                    );
                }
            },
        });
    },

    QGGetBatteryInfoSync: function () {
        var batteryInfo = qg.getBatteryInfoSync();
        var json = JSON.stringify({
            level: batteryInfo.level,
            isCharging: batteryInfo.isCharging,
        });

        if (!json) {
            json = "{}";
        }

        var bufferSize = lengthBytesUTF8(json) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(json, buffer, bufferSize);
        return buffer;
    },

    QGGetDeviceId: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.getDeviceId({
            success: function (res) {
                var DeviceId = {
                    deviceId: res.deviceId,
                    oaid: res.oaid ? res.oaid : ""
                };
                var json = JSON.stringify({
                    callbackId: successID,
                    data: DeviceId,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetDeviceIdCallBack",
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    data: null,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetDeviceIdCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetDeviceIdCallBack",
                        json
                    );
                }
            },
        });
    },

    QGGetScreenBrightness: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.getScreenBrightness({
            success: function (res) {
                if (qg.getProvider() === "vivo") {
                    res.value = res.value / 255;
                }
                var value = {
                    value: res.value,
                };
                var json = JSON.stringify({
                    callbackId: successID,
                    data: value,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetScreenBrightnessCallBack",
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetScreenBrightnessCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetScreenBrightnessCallBack",
                        json
                    );
                }
            },
        });
    },

    QGSetScreenBrightness: function (param, success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        if (qg.getProvider() === "vivo") {
            param = param * 255;
        }
        qg.setScreenBrightness({
            value: param,
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                        json
                    );
                }
            },
        });
    },

    QGSetKeepScreenOn: function (param, success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.setKeepScreenOn({
            keepScreenOn: Boolean(param),
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                        json
                    );
                }
            },
        });
    },

    QGGetLocation: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.getLocation({
            success: function (res) {
                var data = {
                    latitude: res["latitude"],
                    longitude: res["longitude"],
                    speed: res["speed"],
                    accuracy: res["accuracy"],
                    altitude: res["altitude"],
                    verticalAccuracy: res["verticalAccuracy"],
                    horizontalAccuracy: res["horizontalAccuracy"],
                };
                var json = JSON.stringify({
                    callbackId: successID,
                    data: data,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetLocationCallBack",
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "GetLocationCallBack",
                    json
                );
            },
            complete: function (res) {
                if (completeID) {
                    var json = JSON.stringify({
                        callbackId: completeID,
                        data: null,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "GetLocationCallBack",
                        json
                    );
                }
            },
        });
    },

    QGOnAccelerometerChange: function (success) {
        var successID = UTF8ToString(success);
        qg.onAccelerometerChange(function (x, y, z) {
            var param = {
                x: x,
                y: y,
                z: z,
            };
            var json = JSON.stringify({
                callbackId: successID,
                data: param,
            });
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "OnAccelerometerChangeCallBack",
                json
            );
            console.log("QGOnAccelerometerChange -js:", json);
        });
    },

    QGStartAccelerometer: function (param, success, fail, complete) {
        var paramStr = UTF8ToString(param);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.startAccelerometer({
            interval: paramStr,
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                var json = JSON.stringify({
                    callbackId: completeID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
        });
    },

    QGStopAccelerometer: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.stopAccelerometer({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                var json = JSON.stringify({
                    callbackId: completeID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
        });
    },

    QGStartCompass: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.startCompass({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                var json = JSON.stringify({
                    callbackId: completeID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
        });
    },

    QGStopCompass: function (success, fail, complete) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var completeID = UTF8ToString(complete);
        qg.stopCompass({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
            complete: function (res) {
                var json = JSON.stringify({
                    callbackId: completeID,
                });
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                    json
                );
            },
        });
    },

    QGOnCompassChange: function (success) {
        var successID = UTF8ToString(success);
        qg.onCompassChange(function (res) {
            var json = JSON.stringify({
                callbackId: successID,
                data: res.direction,
            });
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "OnCompassChangeCallBack",
                json
            );
            console.log("QGOnCompassChange -js:", json);
        });
    },

    QGStartDeviceMotionListening: function (param) {
        var paramStr = UTF8ToString(param);
        qg.startDeviceMotionListening({
            interval: paramStr,
            success: function (res) {
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "StartDeviceMotionListeningSuccess",
                    "Success"
                );
                console.log("startDeviceMotionListening success ", JSON.stringify(res));
            },
            fail: function (err) {
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "StartDeviceMotionListeningFail",
                    "Fail"
                );
                console.log("startDeviceMotionListening fail ", JSON.stringify(err));
            },
            complete: function () {
                console.log("startDeviceMotionListening complete ");
            },
        });
    },

    QGStopDeviceMotionListening: function () {
        qg.stopDeviceMotionListening({
            success: function (res) {
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "StopDeviceMotionListeningSuccess",
                    "Success"
                );
                console.log("stopDeviceMotionListening success ", JSON.stringify(res));
            },
            fail: function (err) {
                unityInstance.SendMessage(
                    CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                    "StopDeviceMotionListeningFail",
                    "Fail"
                );
                console.log("stopDeviceMotionListening fail ", JSON.stringify(err));
            },
            complete: function () {
                console.log("stopDeviceMotionListening complete ");
            },
        });
    },

    QGOnDeviceMotionChange: function (deviceMotionChangeCallback) {
        qg.onDeviceMotionChange(function (res) {
            // console.log("onDeviceMotionChange", JSON.stringify(res));
            property.deviceMotionChangeData = res;
        });
        property.deviceMotionChangeCallback = deviceMotionChangeCallback;
    },

    QGGetDeviceMotionChange: function () {
        if (!property.deviceMotionChangeCallback) {
            return;
        }
        var alphaStr = property.deviceMotionChangeData.alpha.toString();
        var betaStr = property.deviceMotionChangeData.beta.toString();
        var gammaStr = property.deviceMotionChangeData.gamma.toString();
        var alphaString = allocateUTF8(alphaStr);
        var betaString = allocateUTF8(betaStr);
        var gammaString = allocateUTF8(gammaStr);
        dynCall("viii", property.deviceMotionChangeCallback, [
            alphaString,
            betaString,
            gammaString,
        ]);
    },

    QGOffDeviceMotionChange: function () {
        qg.offDeviceMotionChange();
    },

    QGIsStartupByShortcut: function (success, fail) {
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        if (qg.isStartupByShortcut) {
            qg.isStartupByShortcut({
                success: function (res) {
                    var json = JSON.stringify({
                        callbackId: successID,
                        isStartupByShortcut: res,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        "IsStartupByShortcutCallBack",
                        json
                    );
                },
                fail: function (err) {
                    var json = JSON.stringify({
                        callbackId: failID,
                        errMsg: err,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT,
                        json
                    );
                },
            });
        }
    },

    QGGetRecorderManager: function (recordId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        try {
            var record = qg.getRecorderManager();
            var recordIdStr = UTF8ToString(recordId);

            if (record.onStart) {
                record.onStart(function () {
                    console.log("onStart success");
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_START,
                        json
                    );
                });
            }

            if (record.onResume) {
                record.onResume(function () {
                    console.log("onResume success");
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_RESUME,
                        json
                    );
                });
            }

            if (record.onPause) {
                record.onPause(function () {
                    console.log("onPause success");
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_PAUSE,
                        json
                    );
                });
            }

            if (record.onStop) {
                record.onStop(function (res) {
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                        errMsg: res.tempFilePath,
                    });
                    console.log("onStop success,", json);
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_STOP,
                        json
                    );
                });
            }

            if (record.onFrameRecorded) {
                record.onFrameRecorded(function () {
                    console.log("onFrameRecorded success");
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_FRAMERECORDED,
                        json
                    );
                });
            }

            if (record.onError) {
                record.onError(function () {
                    console.log("onError success");
                    var json = JSON.stringify({
                        callbackId: recordIdStr,
                    });
                    unityInstance.SendMessage(
                        CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                        CONSTANT.ACTION_CALL_BACK_METHORD_NAME_RD_ERROR,
                        json
                    );
                });
            }
            if (!(mAdMap instanceof Map)) {
                mAdMap = new Map();
            }
            mAdMap.set(recordIdStr, record);
        } catch (error) {
            console.log("qg.getRecorderManager fail: " + error);
        }
    },

    QGRecorderStart: function (recordId, param) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var recordIdStr = UTF8ToString(recordId);
        var paramStr = UTF8ToString(param);
        var paramData = JSON.parse(paramStr);
        const options = {
            duration: paramData.duration,
            sampleRate: paramData.sampleRate,
            numberOfChannels: paramData.numberOfChannels,
            encodeBitRate: paramData.encodeBitRate,
            format: paramData.format,
            frameSize: paramData.frameSize,
            audioSource: paramData.audioSource,
        };
        console.log("record options:", JSON.stringify(options));
        var record = mAdMap.get(recordIdStr);
        if (record) {
            try {
                record.start(options);
            } catch (error) {
                console.log("record.start fail: " + error);
            }
        }
    },

    QGRecorderPause: function (recordId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var recordIdStr = UTF8ToString(recordId);
        var record = mAdMap.get(recordIdStr);
        if (record && record.pause) {
            record.pause();
        }
    },

    QGRecorderResume: function (recordId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var recordIdStr = UTF8ToString(recordId);
        var record = mAdMap.get(recordIdStr);
        if (record && record.resume) {
            record.resume();
        }
    },

    QGRecorderStop: function (recordId) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map();
        }
        var recordIdStr = UTF8ToString(recordId);
        var record = mAdMap.get(recordIdStr);
        if (record && record.stop) {
            record.stop();
        }
    },

    QGOnShow: function (cb) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var cbIdStr = UTF8ToString(cb);
        property.onShowCallBack = function (res) {
            var json = JSON.stringify({
                callbackId: cbIdStr,
                type: res.type,
                query: res.query,
                referrerInfo: res.referrerInfo,
            });
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "QGOnShowCallBack",
                json
            );
            console.log("jslib qg.onShow: ", JSON.stringify(res));
        };
        qg.onShow(property.onShowCallBack);
    },

    QGOffShow: function (cb) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (property.onShowCallBack === null) {
            console.log("onShowCallBack is null");
            return;
        }
        qg.offShow(property.onShowCallBack);
        property.onShowCallBack = null;
    },

    QGOnHide: function (cb) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var cbIdStr = UTF8ToString(cb);
        property.onHideCallBack = function (res) {
            var json = JSON.stringify({
                callbackId: cbIdStr,
            });
            unityInstance.SendMessage(
                CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
                "QGOnHideCallBack",
                json
            );
            console.log("jslib qg.onHide: ", JSON.stringify(res));
        };
        qg.onHide(property.onHideCallBack);
    },

    QGOffHide: function (cb) {
        if (typeof qg == "undefined") {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        if (property.onHideCallBack === null) {
            console.log("onHideCallBack is null");
            return;
        }
        qg.offHide(property.onHideCallBack);
        property.onHideCallBack = null;
    },

    // Storage
    QGStorageSetItem: function (keyName, keyValue) {
        var keyNameStr = UTF8ToString(keyName);
        var keyValueStr = UTF8ToString(keyValue);
        localStorage.setItem(keyNameStr, keyValueStr);
        console.log("QGStorageSetItem success");
    },
    QGStorageGetItem: function (keyName) {
        var keyNameStr = UTF8ToString(keyName);
        var returnStr = localStorage.getItem(keyNameStr);
        console.log("QGStorageGetItem", typeof returnStr);
        if (!returnStr) {
            returnStr = "";
        }
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        console.log("QGStorageGetItem -js", buffer);
        return buffer;
    },
    QGStorageRemoveItem: function (keyName) {
        var keyNameStr = UTF8ToString(keyName);
        localStorage.removeItem(keyNameStr);
        console.log("QGStorageRemoveItem: " + keyNameStr);
    },

    QGStorageClear: function () {
        localStorage.clear();
        console.log("QGStorageClear");
    },

    QGLogin: function (success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);

        if (qg.unionLogin) {
            console.log("调用统一sdk登录 qg.unionLogin...")
            qg.unionLogin({
                success: function (res) {
                    if (!res.openId && res.uid) {
                        res.openId = res.uid
                    }
                    var json = JSON.stringify({
                        callbackId: successID,
                        data: res
                    })
                    console.log("统一sdk登录成功 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
                },
                fail: function (res) {
                    var json = JSON.stringify({
                        callbackId: failID,
                        errMsg: res.message,
                        errCode: res.code
                    })
                    console.log("统一sdk登录失败 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
                }
            })
        } else {
            qg.login({
                success: function (res) {
                    var json = JSON.stringify({
                        callbackId: successID,
                        data: res.data
                    })
                    console.log("统一sdk登录成功 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
                },
                fail: function (res) {
                    var json = JSON.stringify({
                        callbackId: failID,
                        errMsg: res.errMsg,
                        errCode: res.errCode
                    })
                    console.log("统一sdk登录失败 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "LoginResponseCallback", json);
                }
            })
        }
    },

    QGPay: function (param, success, fail, cancel, complete) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var paramStr = UTF8ToString(param);
        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var cancelID = UTF8ToString(cancel);
        var completeID = UTF8ToString(complete);

        // 解析出orderInfo
        var orderInfo;
        try {
            orderInfo = JSON.parse(paramStr);
        } catch (error) {
            console.warn("orderInfo解析错误...");
        }

        if (qg.unionPay) {
            console.log("调用统一sdk支付 qg.unionPay...")
            qg.unionPay({
                orderInfo: orderInfo,
                success: function (ret) {
                    var json = JSON.stringify({
                        callbackId: successID,
                        data: ret
                    })
                    console.log("调用统一sdk支付成功 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                },
                fail: function (err) {
                    var json = JSON.stringify({
                        callbackId: failID,
                        errMsg: err.message,
                        errCode: err.code
                    })
                    console.log("调用统一sdk支付失败 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                },
                complete: function () {
                    var json = JSON.stringify({
                        callbackId: completeID
                    })
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                }
            })
        } else {
            qg.pay({
                orderInfo: orderInfo,
                success: function (ret) {
                    var json = JSON.stringify({
                        callbackId: successID,
                        data: ret.data
                    })
                    console.log("调用统一sdk支付成功 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                },
                fail: function (err) {
                    var json = JSON.stringify({
                        callbackId: failID,
                        errMsg: res.errMsg,
                        errCode: res.errCode
                    })
                    console.log("调用统一sdk支付失败 " + json)
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                },
                cancel: function (ret) {
                    console.log("调用统一sdk支付取消 " + ret)
                    // 错误回调
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", JSON.stringify({
                        callbackId: failID,
                        data: ret.data
                    }));

                    // 取消回调
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", JSON.stringify({
                        callbackId: cancelID,
                        data: ret.data
                    }));
                },
                complete: function () {
                    var json = JSON.stringify({
                        callbackId: completeID
                    })
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "PayResponseCallback", json);
                }
            })
        }
    },

    QGHasShortcutInstalled: function (success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);

        qg.hasShortcutInstalled({
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: {
                        hasShortcutInstalled: res
                    }
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ShortcutResponseCallback", json);
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res.errMsg,
                    errCode: res.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, "ShortcutResponseCallback", json);
            }
        })
    },

    QGInstallShortcut: function (message, success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var messageStr = UTF8ToString(message);

        qg.installShortcut({
            message: messageStr,
            success: function (res) {
                var json = JSON.stringify({
                    callbackId: successID,
                    data: res
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            },
            fail: function (res) {
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: res
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            }
        })
    },

    QGGetAdInfo: function () {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var result = qg.adInfo ? JSON.stringify(qg.adInfo) : "";
        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);

        return buffer;
    },

    QGCreateBannerAd: function (adId, posId, style, adIntervals) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var posIdStr = UTF8ToString(posId);
        var styleStr = UTF8ToString(style);
        var adIdStr = UTF8ToString(adId);

        var bannerAd;
        if (styleStr) {
            bannerAd = qg.createBannerAd({
                adUnitId: posIdStr,
                style: JSON.parse(styleStr),
                adIntervals: adIntervals
            });
        } else {
            bannerAd = qg.createBannerAd({
                adUnitId: posIdStr,
                adIntervals: adIntervals
            });
        }
        if (bannerAd) {
            mAdMap.set(adIdStr, bannerAd)
            bannerAd.onLoad(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
            })
            bannerAd.onClose(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE, json);
            })
            bannerAd.onError(function (err) {
                var json = JSON.stringify({
                    callbackId: adIdStr,
                    errMsg: err.errMsg,
                    errCode: err.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
            })
        }
    },

    QGCreateInterstitialAd: function (adId, posId) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var posIdStr = UTF8ToString(posId);
        var adIdStr = UTF8ToString(adId);

        var interstitialAd = qg.createInterstitialAd({
            adUnitId: posIdStr
        });
        if (interstitialAd) {
            mAdMap.set(adIdStr, interstitialAd)
            interstitialAd.onLoad(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
            })
            interstitialAd.onClose(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE, json);
            })
            interstitialAd.onError(function (err) {
                var json = JSON.stringify({
                    callbackId: adIdStr,
                    errMsg: err.errMsg,
                    errCode: err.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
            })
        }
    },

    QGCreateRewardedVideoAd: function (adId, posId) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var posIdStr = UTF8ToString(posId);
        var adIdStr = UTF8ToString(adId);

        var rewardedVideoAd = qg.createRewardedVideoAd({
            adUnitId: posIdStr
        });
        if (rewardedVideoAd) {
            mAdMap.set(adIdStr, rewardedVideoAd)
            rewardedVideoAd.onLoad(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
            })

            rewardedVideoAd.onClose(function (rec) {
                var json = JSON.stringify({
                    callbackId: adIdStr,
                    isEnded: rec.isEnded
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE_REWARDED, json);
            })
            rewardedVideoAd.onError(function (err) {
                console.error(" rewardedVideoAd.onError = " + JSON.stringify(err))
                var json = JSON.stringify({
                    callbackId: adIdStr,
                    errMsg: err.errMsg,
                    errCode: err.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
            })
        }
    },

    QGCreateCustomAd: function (adId, posId, style) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var posIdStr = UTF8ToString(posId);
        var adIdStr = UTF8ToString(adId);
        var styleStr = UTF8ToString(style);

        var customAd = qg.createCustomAd({
            adUnitId: posIdStr,
            style: styleStr ? JSON.parse(styleStr) : {}
        });
        if (customAd) {
            mAdMap.set(adIdStr, customAd)
            customAd.onLoad(function (rec) {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_LOAD, json);
            })
            customAd.onClose(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_CLOSE, json);
            })
            customAd.onHide(function () {
                var json = JSON.stringify({
                    callbackId: adIdStr
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_HIDE, json);
            })
            customAd.onError(function (err) {
                var json = JSON.stringify({
                    callbackId: adIdStr,
                    errMsg: err.errMsg,
                    errCode: err.errCode
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_AD_ERROR, json);
            })
        }
    },

    QGShowAd: function (adId, success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var adIdStr = UTF8ToString(adId);

        var ad = mAdMap.get(adIdStr)

        if (ad) {
            ad.show().then(function () {
                var json = JSON.stringify({
                    callbackId: successID
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            }).catch(function (err) {
                var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg
                var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: errMsgStr,
                    errCode: errCodeValue
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            })
        } else {
            var json = JSON.stringify({
                callbackId: failID,
                errMsg: "ad is undefined",
                errCode: 404
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        }
    },

    QGHideAd: function (adId, success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var adIdStr = UTF8ToString(adId);

        var ad = mAdMap.get(adIdStr)

        if (ad) {
            ad.hide().then(function () {
                var json = JSON.stringify({
                    callbackId: successID
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            }).catch(function (err) {
                var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg
                var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: errMsgStr,
                    errCode: errCodeValue
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            })
        } else {
            var json = JSON.stringify({
                callbackId: failID,
                errMsg: "ad is undefined",
                errCode: 404
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        }
    },

    QGLoadAd: function (adId, success, fail) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var successID = UTF8ToString(success);
        var failID = UTF8ToString(fail);
        var adIdStr = UTF8ToString(adId);

        var ad = mAdMap.get(adIdStr)

        if (ad) {
            ad.load().then(function () {
                var json = JSON.stringify({
                    callbackId: successID
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            }).catch(function (err) {
                var errMsgStr = !err ? "" : err.data ? err.data.errMsg : err.errMsg
                var errCodeValue = !err ? "" : err.data ? err.data.errCode : err.errCode
                var json = JSON.stringify({
                    callbackId: failID,
                    errMsg: errMsgStr,
                    errCode: errCodeValue
                })
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
            })
        } else {
            var json = JSON.stringify({
                callbackId: failID,
                errMsg: "ad is undefined",
                errCode: 404
            })
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, json);
        }
    },

    QGDestroyAd: function (adId) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var adIdStr = UTF8ToString(adId);

        var ad = mAdMap.get(adIdStr)

        if (ad) {
            ad.destroy()
            mAdMap.delete(adIdStr);
        }
    },

    QGIsShow: function (adId) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        if (!(mAdMap instanceof Map)) {
            mAdMap = new Map()
        }

        var adIdStr = UTF8ToString(adId);

        var ad = mAdMap.get(adIdStr)

        if (ad) {
            return ad.isShow();
        }
    },

    QGGetEnvUSER_DATA_PATH: function () {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var result = qg.env && qg.env.USER_DATA_PATH ? qg.env.USER_DATA_PATH : "";
        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);

        return buffer;
    },

    QGStorageSetIntSync: function (key, value) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var valueStr = value + "";

        qg.setStorageSync(keyStr, valueStr)
    },

    QGStorageGetIntSync: function (key, defaultValue) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var defaultValueStr = defaultValue + "";

        var result = qg.getStorageSync(keyStr)
        if (!result) {
            result = defaultValueStr
        }
        return parseInt(result);
    },

    QGStorageSetStringSync: function (key, value) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var valueStr = UTF8ToString(value);

        qg.setStorageSync(keyStr, valueStr)
    },

    QGStorageGetStringSync: function (key, defaultValue) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var defaultValueStr = UTF8ToString(defaultValue);

        var result = qg.getStorageSync(keyStr)
        if (!result) {
            result = defaultValueStr
        }

        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);

        return buffer;
    },

    QGStorageSetFloatSync: function (key, value) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var valueStr = value + "";

        qg.setStorageSync(keyStr, valueStr)
    },

    QGStorageGetFloatSync: function (key, defaultValue) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);
        var defaultValueStr = defaultValue + "";

        var result = qg.getStorageSync(keyStr)
        if (!result) {
            result = defaultValueStr
        }
        return parseFloat(result);
    },

    QGStorageDeleteAllSync: function () {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        qg.clearStorageSync()
    },

    QGStorageDeleteKeySync: function (key) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);

        qg.removeStorageSync(keyStr)
    },

    QGStorageHasKeySync: function (key) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }
        var keyStr = UTF8ToString(key);

        var result = qg.getStorageSync(keyStr)
        return (result ? true : false)
    },



    QGGetFileBuffer: function (buffer, callBackId) {
        if (typeof (qg) == 'undefined') {
            console.log("qg.minigame.jslib  qg is undefined");
            return;
        }

        var callBackIdStr = UTF8ToString(callBackId);
        HEAPU8.set(new Uint8Array(mFileData[callBackIdStr]), buffer);
        delete mFileData[callBackIdStr];
    },

    QG_FS_SaveFile: function (option, callbackId) {
        QG_runFSMethod('saveFile', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_SaveFileSync: function (tempFilePath, filePath) {
        var fs = qg.getFileSystemManager();
        var res
        try {
            res = fs.saveFileSync(QG_stringify(tempFilePath), QG_stringify(filePath));
            if (res.savedFilePath && typeof res !== 'string') {
                res = res.savedFilePath
            }
        } catch (e) {
            console.error(e);
            if (e.message) {
                res = e.message;
            } else {
                res = 'fail';
            }
        }

        var bufferSize = lengthBytesUTF8(res) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(res, buffer, bufferSize);
        return buffer;
    },
    QG_FS_Mkdir: function (dirPath, recursive, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.mkdir(Object.assign({}, {
            dirPath: QG_stringify(dirPath),
            recursive: Boolean(recursive)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_MkdirSync: function (dirPath, recursive) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            fs.mkdirSync(QG_stringify(dirPath), Boolean(recursive));
            returnStr = 'mkdir:ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_ReadFile: function (option, callbackId) {
        QG_runFSMethod('readFile', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_ReadFileSync: function (option) {
        var returnStr = ''
        var fs = qg.getFileSystemManager();
        var config = QG_formatJsonStr(QG_stringify(option));
        try {
            var filePath = config.filePath;
            var res = fs.readFileSync(config.filePath, config.encoding, config.position, config.length);
            if (!config.encoding && typeof res !== 'string') {
                mFileData[filePath] = res;
                returnStr = "" + res.byteLength;
            } else {
                returnStr = res;
            }
        } catch (e) {
            console.error(e);
            if (!config.encoding || config.encoding === 'binary') {
                returnStr = '0';
            } else {
                if (e.message) {
                    returnStr = e.message;
                } else {
                    returnStr = 'fail';
                }
            }

        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_Readdir: function (option, callbackId) {
        QG_runFSMethod('readdir', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_ReaddirSync: function (dirPath) {
        var res = '';
        var fs = qg.getFileSystemManager();
        try {
            var result = fs.readdirSync(QG_stringify(dirPath));
            result = result.files || result || [];
            res = JSON.stringify(result);
        } catch (e) {
            console.error(e);
            res = '[]';
        }
        var bufferSize = lengthBytesUTF8(res) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(res, buffer, bufferSize);
        return buffer;
    },
    QG_FS_ReadZipEntry: function (option, callbackId) {
        QG_runFSMethod('readZipEntry', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_ReadZipEntryString: function (option, callbackId) {
        QG_runFSMethod('readZipEntry', QG_stringify(option), QG_stringify(callbackId), true);
    },
    QG_FS_ReadCompressedFile: function (option, callbackId) {
        QG_runFSMethod('readCompressedFile', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_ReadCompressedFileSync: function (option, callbackId) {
        var fs = qg.getFileSystemManager();
        var ress = fs.readCompressedFileSync(QG_formatJsonStr(QG_stringify(option)));
        if (typeof ress !== 'string') {
            mFileData[QG_stringify(callbackId)] = ress;
            return ress.byteLength;
        } else {
            console.error('readCompressedFileSync error: ' + ress)
            return 0;
        }
    },
    QG_FS_CopyFile: function (srcPath, destPath, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.copyFile(Object.assign({}, {
            srcPath: QG_stringify(srcPath),
            destPath: QG_stringify(destPath)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_CopyFileSync: function (srcPath, destPath) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            fs.copyFileSync(QG_stringify(srcPath), QG_stringify(destPath));
            returnStr = 'copyFile:ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_GetFileInfo: function (option, callbackId) {
        QG_runFSMethod('getFileInfo', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_GetSavedFileList: function (option, callbackId) {
        QG_runFSMethod('getSavedFileList', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_Unzip: function (option, callbackId) {
        QG_runFSMethod('unzip', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_AccessFile: function (path, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.access(Object.assign({}, {
            path: QG_stringify(path)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_AccessFileSync: function (path) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            fs.accessSync(QG_stringify(path));
            returnStr = 'access:ok';
        } catch (e) {
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_RemoveSavedFile: function (option, callbackId) {
        QG_runFSMethod('removeSavedFile', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_Rmdir: function (dirPath, recursive, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.rmdir(Object.assign({}, {
            dirPath: QG_stringify(dirPath),
            recursive: Boolean(recursive)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_RmdirSync: function (dirPath, recursive) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            fs.rmdirSync(QG_stringify(dirPath), Boolean(recursive));
            returnStr = 'rmdirSync:ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_Unlink: function (filePath, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.unlink(Object.assign({}, {
            filePath: QG_stringify(filePath)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_UnlinkSync: function (filePath) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            fs.unlinkSync(QG_stringify(filePath));
            returnStr = 'unlink:ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }
        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_WriteFile: function (filePath, data, dataLength, encoding, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.writeFile(Object.assign({}, {
            filePath: QG_stringify(filePath),
            data: HEAPU8.slice(data, dataLength + data).buffer,
            encoding: QG_stringify(encoding)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_WriteStringFile: function (filePath, data, encoding, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.writeFile(Object.assign({}, {
            filePath: QG_stringify(filePath),
            data: QG_stringify(data),
            encoding: QG_stringify(encoding)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_WriteFileSync: function (filePath, data, encoding) {
        var returnStr = '';
        try {
            var fs = qg.getFileSystemManager();
            // @ts-ignore
            fs.writeFileSync(QG_stringify(filePath), QG_stringify(data), QG_stringify(encoding));
            returnStr = 'ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }

        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_WriteBinFileSync: function (filePath, data, dataLength, encoding) {
        var returnStr = '';
        var fs = qg.getFileSystemManager();
        try {
            fs.writeFileSync(QG_stringify(filePath), HEAPU8.slice(data, dataLength + data).buffer, QG_stringify(encoding));
            returnStr = 'ok';
        } catch (e) {
            console.error(e);
            if (e.message) {
                returnStr = e.message;
            } else {
                returnStr = 'fail';
            }
        }

        var bufferSize = lengthBytesUTF8(returnStr || '') + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    QG_FS_AppendFile: function (filePath, data, dataLength, encoding, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.appendFile(Object.assign({}, {
            filePath: QG_stringify(filePath),
            data: HEAPU8.slice(data, dataLength + data).buffer,
            encoding: QG_stringify(encoding) || 'binary'
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_AppendStringFile: function (filePath, data, encoding, s, f, c) {
        var fs = qg.getFileSystemManager();
        fs.appendFile(Object.assign({}, {
            filePath: QG_stringify(filePath),
            data: QG_stringify(data),
            encoding: QG_stringify(encoding)
        }, QG_handleText(QG_stringify(s), QG_stringify(f), QG_stringify(c))));
    },
    QG_FS_AppendFileSync: function (filePath, data, dataLength, encoding) {
        var fs = qg.getFileSystemManager();
        try {
            fs.appendFileSync(QG_stringify(filePath), HEAPU8.slice(data, dataLength + data).buffer, QG_stringify(encoding) || 'binary');
        } catch (e) {
            console.error(e);
        }

    },
    QG_FS_AppendFileStringSync: function (filePath, data, encoding) {
        var fs = qg.getFileSystemManager();
        try {
            fs.appendFileSync(QG_stringify(filePath), QG_stringify(data), QG_stringify(encoding));
        } catch (e) {
            console.error(e);
        }
    },
    QG_FS_Rename: function (option, callbackId) {
        QG_runFSMethod('rename', QG_stringify(option), QG_stringify(callbackId));
    },
    QG_FS_RenameSync: function (oldPath, newPath) {
        var fs = qg.getFileSystemManager();
        try {
            fs.renameSync(QG_stringify(oldPath), QG_stringify(newPath));
        } catch (e) {
            console.error(e);
        }
    },
    QG_FS_Stat: function (conf, callbackId) {
        var config = QG_formatJsonStr(QG_stringify(conf));
        var callbackID = QG_stringify(callbackId);
        qg.getFileSystemManager().stat(Object.assign({}, config, {
            success: function (res) {
                const returnRes = {
                    stats: res.stats
                }
                if (!Array.isArray(res.stats)) {
                    // @ts-ignore C#中特殊处理
                    returnRes.one_stat = res.stats;
                    // @ts-ignore C#中特殊处理
                    returnRes.stats = null;
                }
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, 'FileSystemStatResponseCallback', JSON.stringify({
                    callbackId: callbackID,
                    type: 'success',
                    res: JSON.stringify(returnRes),
                }));
            },
            fail: function (res) {
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, 'FileSystemStatResponseCallback', JSON.stringify({
                    callbackId: callbackID,
                    type: 'fail',
                    res: JSON.stringify(res),
                }));
            },
            complete: function (res) {
                unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, 'FileSystemStatResponseCallback', JSON.stringify({
                    callbackId: callbackID,
                    type: 'complete',
                    res: JSON.stringify({
                        errMsg: '',
                        errCode: 200,
                    }),
                }));
            },
        }));
    },
    QG_FS_StatSync: function (path, recursive) {
        var fs = qg.getFileSystemManager();
        var resArray;
        var res;
        try {
            var ress = fs.statSync(QG_stringify(path), Boolean(recursive));

            if (Array.isArray(ress)) {
                resArray = ress;
            } else {
                var result = {
                    path: QG_stringify(path),
                    stats: ress.stats ? ress.stats : ress
                }
                resArray = [result];
            }
            res = JSON.stringify(resArray);
        } catch (e) {
            console.error(e);
            res = "{}"
        }

        var bufferSize = lengthBytesUTF8(res) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(res, buffer, bufferSize);
        return buffer;
    },
    $QG_stringify: function (str) {
        if (typeof UTF8ToString !== "undefined") {
            return UTF8ToString(str)
        }
        return Pointer_stringify(str)
    },
    $QG_handleText: function (s, f, c) {
        function textFormat(id, res) {
            if (!id) {
                return false;
            }
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_DEFAULT, JSON.stringify({
                callbackId: id,
                errMsg: res.errMsg,
                errCode: res.errCode,
            }));
        }
        return {
            success: function (res) {
                textFormat(s, res);
            },
            fail: function (res) {
                textFormat(f, res);
            },
            complete: function (res) {
                textFormat(c, res);
            },
        };
    },
    $QG_runFSMethod: function (method, option, callbackId, isString) {
        try {
            if (!isString) {
                isString = false;
            }
            var fs = qg.getFileSystemManager();
            var config;
            if (typeof option === 'string') {
                config = QG_formatJsonStr(option);
            } else {
                config = option;
            }
            if (method === 'readZipEntry' && !config.encoding) {
                config.encoding = 'utf-8';
                console.error('fs.readZipEntry不支持读取ArrayBuffer，已改为utf-8');
            }

            fs[method](Object.assign({}, config, {
                success: function (res) {
                    var returnRes = '';
                    if (method === 'readCompressedFile') {
                        mFileData[callbackId] = res.data;
                        var arrayBufferLength = 0;
                        if (res.data && res.data.byteLength !== undefined && res.data.byteLength !== null) {
                            arrayBufferLength = res.data.byteLength;
                        }
                        returnRes = JSON.stringify({
                            arrayBufferLength: arrayBufferLength,
                        });
                    } else if (method === 'readFile') {
                        if (config.encoding && config.encoding !== 'binary') {
                            returnRes = JSON.stringify({
                                stringData: res.data || '',
                            });
                        } else {
                            mFileData[callbackId] = res.data;
                            var arrayBufferLength = 0;
                            if (res.data && res.data.byteLength !== undefined && res.data.byteLength !== null) {
                                arrayBufferLength = res.data.byteLength;
                            }
                            returnRes = JSON.stringify({
                                arrayBufferLength: arrayBufferLength,
                            });
                        }
                    } else {
                        if (
                            (method !== 'removeSavedFile' &&
                                method !== 'rename' &&
                                method !== 'unzip') ||
                            (res.errCode && res.errMsg)
                        ) {
                            returnRes = JSON.stringify(res);
                        } else {
                            returnRes = JSON.stringify({
                                errCode: 0,
                                errMsg: res ? res + '' : 'ok',
                            });
                        }
                    }
                    // console.log(`fs.${method} success:`, res);
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_FILESYSTEM, JSON.stringify({
                        callbackId: callbackId,
                        type: 'success',
                        res: returnRes,
                        method: isString ? (method + '_string') : method,
                    }));
                },
                fail: function (res) {

                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_FILESYSTEM, JSON.stringify({
                        callbackId: callbackId,
                        type: 'fail',
                        res: JSON.stringify({
                            errMsg: res.errMsg ? res.errMsg : res + '',
                            errCode: res.errCode ? res.errCode : 200,
                        }),
                        method: isString ? (method + '_string') : method,
                    }));
                },
                complete: function (res) {
                    unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_FILESYSTEM, JSON.stringify({
                        callbackId: callbackId,
                        type: 'complete',
                        res: JSON.stringify({
                            errMsg: '',
                            errCode: 200,
                        }),
                        method: isString ? (method + '_string') : method,
                    }));
                },
            }));
        } catch (e) {
            unityInstance.SendMessage(CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT, CONSTANT.ACTION_CALL_BACK_METHORD_NAME_FILESYSTEM, JSON.stringify({
                callbackId: callbackId,
                type: 'complete',
                res: JSON.stringify({
                    errMsg: '',
                    errCode: 200,
                }),
                method: isString ? (method + '_string') : method,
            }));
        }
    },
    $QG_formatJsonStr: function (str, type) {
        function isBase64(str) {
            var base64Pattern = /^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$/;
            return base64Pattern.test(str);
        }

        function base64ToArrayBuffer(base64) {
            var binaryString = atob(base64);
            var len = binaryString.length;
            var bytes = new Uint8Array(len);
            for (var i = 0; i < len; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            return bytes.buffer;
        }

        function convertBase64ToData(input) {
            if (isBase64(input)) {
                return base64ToArrayBuffer(input);
            }
            return input;
        }

        if (!str) {
            return {};
        }
        if (type === 'string|arrayBuffer') {
            return convertBase64ToData(str);
        }
        try {
            var data = JSON.parse(str);
            Object.keys(data).forEach(function (v) {
                if (data[v] === null) {
                    delete data[v];
                }
            });
            if (type) {
                var conf = ResType[type];
                if (!conf) {
                    return data;
                }
                Object.keys(conf).forEach(function (key) {
                    if (data[key]) {
                        if (conf[key] === 'arrayBuffer') {
                            data[key] = new Uint8Array(data[key]).buffer;
                        } else if (conf[key] === 'string|arrayBuffer') {
                            data[key] = convertBase64ToData(data[key]);
                        }
                    }
                });
            }
            return data;
        } catch (e) {
            return str;
        }
    },
    $parseStringToObject: function(input) {
        var obj = {};
    
        // Remove the curly braces
        var cleanedInput = input.replace(/{|}$/g, '');
        // Split the string by ", " to get key-value pairs
        var pairs = cleanedInput.split(', ');
    
        pairs.forEach(function(pair) {
            // Split each pair by "=" to separate key and value
            var keyValue = pair.split('=');
            var key = keyValue[0];
            var value = keyValue[1];
            // Convert value to a number and add to the object
            obj[key] = Number(value);
        });
    
        return obj;
    }
};

autoAddDeps(QgGameBridge, '$mAdMap');
autoAddDeps(QgGameBridge, '$CONSTANT');
autoAddDeps(QgGameBridge, '$mFileData');
autoAddDeps(QgGameBridge, "$property");
autoAddDeps(QgGameBridge, '$QG_stringify');
autoAddDeps(QgGameBridge, '$QG_handleText');
autoAddDeps(QgGameBridge, '$QG_runFSMethod');
autoAddDeps(QgGameBridge, '$QG_formatJsonStr');
autoAddDeps(QgGameBridge, '$QGUniversalCallback');
autoAddDeps(QgGameBridge, '$parseStringToObject');

mergeInto(LibraryManager.library, QgGameBridge);
