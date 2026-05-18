mergeInto(LibraryManager.library, {

    ShowBannerAdInVivo: function (posIdStr) {

        console.log("banner id: " + UTF8ToString(posIdStr));
        if (window.bannerAd != null) {
            window.bannerAd.hide();
        }

        window.bannerAd = qg.createBannerAd({
            posId: UTF8ToString(posIdStr),
            style: {}
        });

        window.bannerAd.onError(function (err) {
            console.log(err.errCode + "--" + err.errMsg);
        });

        window.bannerAd.show().then(function () {
            console.log("banner广告展示成功");
        }).catch(function (err) {
            console.log("banner广告展示失败", err.toString());
        });

    },
    HideBannerAdInVivo: function () {
        if (window.bannerAd != null) {
            window.bannerAd.hide();
        }
    },

    ShowCustomAdInVivo: function (posIdStr, leftPosIdStr, orientation) {

        // 横竖屏
        var sysInfo = qg.getSystemInfoSync();
        var _top;
        var _left;
        if (orientation == 0) {
            _top = 100;
            _left = (sysInfo.screenWidth - 1080) / 2;
            // _left = 530;
            console.log(sysInfo.screenWidth);
            console.log("_left ====== ", _left);
        } else {
            // _top = (sysInfo.screenHeight - 720) * 0.65;
            // _left = (sysInfo.screenWidth - 1080) * 0.5;
            _top = sysInfo.screenHeight * 0.5;
            _left = sysInfo.screenWidth * 0.1;
            console.log(sysInfo.screenWidth);
            console.log(_left);
        }

        if (qg.createCustomAd) {
            window.customAd = qg.createCustomAd({
                posId: UTF8ToString(posIdStr),
                style: {left: _left, top: _top}
            });

            window.customAd.onError(function (err) {
                console.log("模板原生加载失败", err.toString());

                window.customAd = qg.createCustomAd({
                    posId: UTF8ToString(leftPosIdStr),
                    style: {left: _left, top: _top}
                });

                window.customAd.show().then(function () {
                    console.log("原生左图展示成功");
                }).catch(function (err) {
                    console.log("原生左图展示失败", err.toString());
                });

            });

            window.customAd.show().then(function () {
                console.log("模板原生展示成功");
            }).catch(function (err) {
                console.log("模板原生展示失败", err.toString());
            });
        }
    },
    HideCustomAdInVivo: function () {
        if (window.customAd != null) {
            window.customAd.hide();
        }
    },

    ShowInterstitialAdInVivo: function (posIdStr, orientation) {

        if (qg.createInterstitialAd) {
            window.interstitialAd = qg.createInterstitialAd({
                posId: UTF8ToString(posIdStr)
            });

            window.interstitialAd.show().then(function () {
                console.log("插屏展示成功");
            }).catch(function (err) {
                console.log("插屏展示失败", err.toString());
            });
        }
    },
    HideInterstitialAdInVivo: function () {
        if (window.interstitialAd != null) {
            window.interstitialAd.hide();
        }
    },

    CreateShortcutButtonInVivo: function () {
        qg.hasShortcutInstalled({
            success: function (status) {
                if (status) {
                    //console.log("已创建");
                    qg.showToast({
                        message: "桌面图标已创建~",
                        duration: 2,
                    });
                } else {
                    qg.installShortcut({
                        success: function () {
                            qg.showToast({
                                message: "桌面图标创建成功~",
                                duration: 2,
                            });
                        },
                        fail: function () {
                            //console.log("创建桌面图标失败！");
                            qg.showToast({
                                message: "创建桌面图标失败！",
                                duration: 2,
                            });
                        },
                        complete: function () {
                            //console.log("create_ShortcutButton ------------> complete");
                        },
                    });
                }
            },
            fail: function () {
                console.log("获取桌面图标是否创建失败~");
            },
        });

    },

    InitRewardInVivo: function (posIdStr) {
        if (window.rewardedAd != null) {
            return;
        }
        window.rewardedAd = qg.createRewardedVideoAd({
            posId: UTF8ToString(posIdStr),
        });
        window.rewardedAd.load();
        window.rewardedAd.onError(function (err) {
            console.log("激励视频广告加载失败", err.toString());

            window.rewardedAd.load();
        });

        window.rewardedAd.onLoad(function (res) {
            console.log("激励视频广告加载完成-onload触发", res.toString());
        });
    },

    ShowRewardAdInVivo: function (posIdStr, receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);

        window.rewardedAd.show().then(function () {
            console.log("激励视频广告展示完成");
        }).catch(function (err) {
            console.log("激励视频广告展示失败", err.toString());
            qg.showToast({
                message: "暂无激励视频",
                duration: 1
            })
        });

        window.rewardedAd.onClose(function (res) {
            console.log('视频广告关闭回调')
            var result = {key: _receiverKey};
            if (res && res.isEnded) {
                console.log("正常播放结束，可以下发游戏奖励");
                result.data = "Success";
            } else {
                console.log("播放中途退出，不下发游戏奖励");
                result.data = "Failure";
            }
            unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
            window.rewardedAd.offClose();
        });

        window.rewardedAd.onError(function (err) {
            console.log("激励视频广告加载失败", err.toString());
            qg.showToast({
                message: '暂无激励视频',
                duration: 1
            })
        });

    },

    Init: function (adUnitIdStr) {
        if (window.rewardedAd != null) {
            return;
        }

        window.rewardedAd = qg.createRewardedVideoAd({
            posId: UTF8ToString(adUnitIdStr),
        });

        window.rewardedAd.load();

        window.rewardedAd.onError(function (err) {
            console.log("激励视频广告加载失败", err.toString());
            //qg.showToast({
            // message: "暂无广告，请稍后再试",
            //  duration: 1,
            //});
        });

        window.rewardedAd.onLoad(function (res) {
            console.log("激励视频广告加载完成-onload触发", res.toString());
        });

    },

    ExitApplicationInVivo: function () {
        qg.exitApplication();
    },

    // 设置窗口的大小  
    resizeWindow: function () {
        var sysInfo = qg.getSystemInfoSync();
        window.resizeTo(sysInfo.screenWidth, sysInfo.screenHeight);
        console.log("窗口宽度", sysInfo.screenWidth);
        console.log("窗口高度 ", sysInfo.screenHeight);
    },

    InitWindowResizeInVivo: function () {
        // 监听窗口大小变化事件  
        window.onresize = function () {
            console.log("触发windowResize");
            resizeWindow();
        };
    },
    CheckCanShowBox: function (posIdStr, receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);
        if (qg.createNewNativeAd) {
            var native_Ad = qg.createNewNativeAd({
                posId: UTF8ToString(posIdStr)
            });

            native_Ad.onLoad(function (res) {
                console.log("原生自渲染2.0广告加载完成-onload触发", JSON.stringify(res));

                if (native_Ad) {
                    //取消监听原生自渲染2.0广告加载结束事件
                    native_Ad.offLoad();
                    //console.log("原生自渲染2.0广告 移除广告加载事件");
                }
                var result = {
                    key: _receiverKey,
                    data: "1"
                };
                unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
            });

            //监听原生自渲染2.0广告加载失败事件
            native_Ad.onError(function (errMsg) {
                console.log("原生自渲染2.0广告加载失败 onError ---------> errMsg: " + JSON.stringify(errMsg));
                var err_Msg = errMsg;
                var err_Code = err_Msg["errCode"];

                console.log("err_Code : ", err_Code);

                var result = {
                    key: _receiverKey,
                    data: "0"
                };
                unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
                if (native_Ad) native_Ad.offError();
            });
        }

    },
    CheckMaiLiangInVivo: function (receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);
        var result = {
            key: _receiverKey,
            data: "0"
        };
        if (qg.getEnterOptionsSync) {
            const enterOption = qg.getEnterOptionsSync()
            if (enterOption && enterOption.query && enterOption.query.type === 'ad') {
                result.data = "1"

                // vivo广告联盟启动来源
                // enterOption.path中包含了投放链接的完整参数，enterOption.query中包含全部的启动信息，可按需解析，注意判断非空
            }
        }
        unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));

    }
});