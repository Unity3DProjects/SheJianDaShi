mergeInto(LibraryManager.library, {
    ShowBannerAdInOPPO: function (bannerAdUnitIdStr) {

        var bannerAdUnitId = Pointer_stringify(bannerAdUnitIdStr);
        if (window.bannerAd != null) {
            window.bannerAd.hide();
        }

        window.bannerAd = qg.createBannerAd({
            adUnitId: bannerAdUnitId,
        });

        window.bannerAd.onError(function (err) {
            console.log(err.errCode + "--" + err.errMsg);
        });

        window.bannerAd
            .show()
            .then(function () {
                console.log("banner广告展示成功");
            })
            .catch(function (err) {
                console.log("banner广告展示失败", err.toString());
            });
    },

    ShowCustomAdInOPPO: function (adUnitIdStr, orientation) {

        // 横竖屏
        var sysInfo = qg.getSystemInfoSync();
        var _top;
        var _left;
        if (orientation == 0) {
            _top = 100;
            _left = (sysInfo.screenWidth - 1080) / 2;
        } else {
            _top = sysInfo.screenHeight * 0.5;
            _left = sysInfo.screenWidth * 0.1;
        }

        if (qg.createCustomAd) {
            window.customAd = qg.createCustomAd({
                adUnitId: Pointer_stringify(adUnitIdStr),
                style: {top: _top},
            });

            window.customAd.onLoad(function () {
                console.log("customAd广告加载成功");
            });

            window.customAd.onError(function (err) {
                console.log("模板原生加载失败", err.toString());
            });

            window.customAd
                .show()
                .then(function () {
                    console.log("模板原生展示成功");
                })
                .catch(function (err) {
                    console.log("模板原生展示失败", err.toString());
                });
        }
    },

    ShowCustomAdZoneInOPPO: function (adUnitIdStr, orientation) {
        console.log("window.isShowAdZone: " + window.isShowAdZone);
        if (window.isShowAdZone) {
            console.log("展示原生--限制");

            // 横竖屏
            var sysInfo = qg.getSystemInfoSync();
            var _top;
            var _left;
            if (orientation == 0) {
                _top = 200;
                _left = (sysInfo.screenWidth - 1080) / 2;
            } else {
                _top = sysInfo.screenHeight * 0.55;
                _left = sysInfo.screenWidth * 0.1;
            }

            if (qg.createCustomAd) {
                window.customAdZone = qg.createCustomAd({
                    adUnitId: Pointer_stringify(adUnitIdStr),
                    style: {top: _top},
                });

                window.customAdZone.onLoad(function () {
                    console.log("customAd广告加载成功");
                });

                window.customAdZone.onError(function (err) {
                    console.log("模板原生加载失败", err.toString());
                });

                window.customAdZone
                    .show()
                    .then(function () {
                        console.log("模板原生展示成功");
                    })
                    .catch(function (err) {
                        console.log("模板原生展示失败", err.toString());
                    });
            }
        }
    },

    HideCustomAd: function () {
        if (window.customAd != null) {
            window.customAd.hide();
        }
    },

    ShowRewardAdInOPPO: function (posIdStr, receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);
        if (window.rewardAdLoaded) {
            window.rewardedAd
                .show()
                .then(function () {
                    console.log("激励视频广告展示完成");
                })
                .catch(function (err) {
                    console.log("激励视频广告展示失败", err.toString());
                });

            window.rewardedAd.onClose(function (res) {
                console.log("视频广告关闭回调");
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

                window.rewardedAd.load();
            });
        } else {
            var result = {key: _receiverKey};
            result.data = "Failure";
            unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
            console.error("激励广告加载失败，无法展示");
        }
    },
    IsArea: function (receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);
        var xhr = new XMLHttpRequest();
        xhr.open("get", "https://yxapi.tomatojoy.cn/getIp", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send();
        xhr.onreadystatechange = function () {
            console.log("请求状态" + xhr.readyState);
            if (xhr.readyState == 4) {
                if (xhr.status >= 200 && xhr.status < 400) {
                    var json = JSON.parse(xhr.responseText);
                    var data = json.data;
                    //console.log("获取地区数据:" + data.city);
                    console.log("获取地区数据:" + JSON.stringify(data));
                    var arr = ["厦门市", "北京市", "上海市", "重庆市", "西安市", "南京市", "杭州市", "武汉市", "长沙市", "成都市", "广州市", "深圳市", "东莞市", "苏州市", "福州市"];
                    var result = {key: _receiverKey};
                    if (arr.indexOf(data.province) >= 0 || arr.indexOf(data.city) >= 0) {
                        result.data = "NoAd";
                        console.log("是屏蔽地区");
                    } else {
                        result.data = "HaveAd";
                        console.log("不是屏蔽地区");
                        //console.log("获取地区数据:" + data);
                    }
                    unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
                } else {
                    //xhr = new XMLHttpRequest();
                    //xhr.open("get", "https://yxapi.tomatojoy.cn/getIp", true);
                    //xhr.setRequestHeader("Content-Type", "application/json");
                    //xhr.send();
                    console.log("请求失败.非200-400状态");
                }
            } else {
                console.log("请求失败  readyState!=4 ");
            }
        };
        console.log("初始化地区结束");
    },

    InitRewardAdInOPPO: function (adUnitIdStr) {

        if (window.rewardedAd != null) {
            return;
        }

        window.rewardedAd = qg.createRewardedVideoAd({
            adUnitId: Pointer_stringify(adUnitIdStr),
        });

        window.rewardedAd.load();

        window.rewardedAd.onError(function (err) {
            console.log("激励视频广告加载失败", err.toString());
            window.rewardAdLoaded = false;

            //window.rewardedAd.destroy();//一定要销毁广告
            setTimeout(function () {
                window.rewardedAd.load();//加上延迟2s请求
            }, 2500);
        });

        window.rewardedAd.onLoad(function (res) {
            console.log("激励视频广告加载完成-onload触发", res.toString());
            window.rewardAdLoaded = true;
        });
    },

    ZDTX: function (BaoMing, QuDaoHao, receiverKey) {
        var _receiverKey = UTF8ToString(receiverKey);
        var baoming = Pointer_stringify(BaoMing);
        var quDaoHao = Pointer_stringify(QuDaoHao);
        var request = new XMLHttpRequest();
        var url = "http://datacenter.zywxgames.com:15855/api/index/params?pkm=" + baoming + "&canshu=" + quDaoHao + "&url=new&yys=yd";
        request.open("get", url, true);
        request.setRequestHeader("Content-Type", "application/json");
        request.send();
        request.onreadystatechange = function () {
            if (request.readyState == 4) {
                if (request.status >= 200 && request.status < 400) {
                    var data = request.responseText;
                    console.log("ZDTX请求成功: " + data)

                    var result = {key: _receiverKey};
                    result.data = data;


                    unityInstance.SendMessage("ADManager", "Receiver", JSON.stringify(result));
                } else {
                    console.log("请求失败，非200-400状态")
                    unityInstance.SendMessage("ADManager", "Receiver", "0");
                }
            } else {
                console.log("不等于4");
            }

        }
    },

    CreateShortcutButton: function () {
        qg.hasShortcutInstalled({
            success: function (status) {
                if (status) {
                    //console.log("已创建");
                    qg.showToast({
                        title: "桌面图标已创建~",
                        duration: 2000,
                    });
                } else {
                    qg.installShortcut({
                        success: function () {
                            qg.showToast({
                                title: "桌面图标创建成功~",
                                duration: 2000,
                            });
                        },
                        fail: function () {
                            //console.log("创建桌面图标失败！");
                            qg.showToast({
                                title: "创建桌面图标失败！",
                                duration: 2000,
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

    ExitApplicationInOPPO: function () {
        console.log("enter app exit");
        qg.exitApplication({
            success: function () {
                console.log("exitApplication success");
            },
            fail: function () {
                console.log("exitApplication fail");
            },
            complete: function () {
                console.log("exitApplication complete");
            },
        });
    },
});
