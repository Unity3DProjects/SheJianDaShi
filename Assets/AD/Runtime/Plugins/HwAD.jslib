mergeInto(LibraryManager.library, {

    LoadNativeAdInfo: function ()
    {
        var timeScape = Date.now() - window.lastShowNativeTimestamp;
        console.log("距离上次显示原生时间间隔为: " + timeScape / 1000);
        if (window.nativeAd != null && timeScape > 3 * 1000)
        {
            window.nativeAd.load();
            window.lastShowNativeTimestamp = Date.now();
        }
    },

    OnCloseNativeAd: function ()
    {
        window.isShowNativeAd = false;
        window.currentNativeAdId = null;
    },



    ReportNativeAdShow: function ()
    {
        if (window.currentNativeAdId)
        {
            console.log("上报原生展示");
            window.isShowNativeAd = true;
            window.nativeAd.reportAdShow({ adId: window.currentNativeAdId });
        }
    },

    ReportNativeAdClick: function ()
    {
        if (window.currentNativeAdId)
        {
            console.log("上报原生点击");
            window.isShowNativeAd = false;
            window.nativeAd.reportAdClick({ adId: window.currentNativeAdId });
        }
    },

    ShowBannerAd: function ()
    {
        var timeScape = Date.now() - window.lastShowBannerTimestamp;
        console.log("距离上次显示banner时间间隔为: " + timeScape / 1000);
        if (window.bannerAd != null)
        {
            window.bannerAd.show();
            window.lastShowBannerTimestamp = Date.now();
        }
    },

    HideBannerAd: function ()
    {
        if (window.bannerAd != null)
        {
            window.bannerAd.hide();
        }
    },

    ShowRewardAd: function (callBackObjectName, callBackMethodName, callBackParam)
    {
        var objectName = Pointer_stringify(callBackObjectName);
        var methodName = Pointer_stringify(callBackMethodName);
        var param = Pointer_stringify(callBackParam);

        window.rewardedAd.show();
       window.qg.unityInstance.Module.SendMessage("ADAnalyticsManager", "OnVideoClick");

        window.rewardedAd.onClose(function (res)
        {
            console.log('视频广告关闭回调')
            if (res && res.isEnded)
            {
                console.log("正常播放结束，可以下发游戏奖励");
                window.qg.unityInstance.Module.SendMessage(objectName, methodName, param);
            } else
            {
                console.log("播放中途退出，不下发游戏奖励");
            }
            window.rewardedAd.offClose();

            window.rewardedAd.load();
        });
    },

    GetOAID: function () {
        if ("function" == typeof window["qg"].getOAID) {
            console.info("function == typeof qg.getOAID");
            window["qg"].getOAID({
              success: function (ret) {
                console.info("qg.getOAID success :" + JSON.stringify(ret.oaid));
                window.qg.unityInstance.Module.SendMessage("ADAnalyticsManager", "GetOAID", ret.oaid);
              },
              fail: function (erromsg, errocode) {
                console.info("qg.getOAID fail " + errocode + ": " + erromsg);
              },
              complete: function () {
                console.info("qg.getOAID complete");
              },
            });
          } else {
            console.info("function != typeof qg.getOAID");
          }
        },
    
 

    InitAd: function (nativeAdIDStr, bannerAdIDStr, rewardedAdIDStr, showAdDateStr)
    {
        var showDateStr = Pointer_stringify(showAdDateStr);
        if (showDateStr != null && showDateStr != "")
        {
            currentDate = new Date();
            showDate = new Date(showDateStr);

            window.canShowAd = currentDate - showDate > 0;
        }

        // 原生初始化
        window.lastShowNativeTimestamp = 0;
        window.isShowNativeAd = false;
        var nativeAdID = Pointer_stringify(nativeAdIDStr);
        window.nativeAd = qg.createNativeAd({
            adUnitId: nativeAdID
        })

        window.nativeAd.onLoad(function (res)
        {
            if (res && res.adList && window.canShowAd)
            {
                var nativeInfo = {
                    title: res.adList[0].title,
                    source: res.adList[0].source,
                    imgUrl: res.adList[0].imgUrlList[0],
                    clickBtnText: res.adList[0].clickBtnTxt,
					
					appName:res.adList[0].appName,
					developerName:res.adList[0].developerName,
					versionName:res.adList[0].versionName,
					appDetailUrl:res.adList[0].appDetailUrl,
					permissionUrl:res.adList[0].permissionUrl,
					privacyUrl:res.adList[0].privacyUrl
					
                };

                console.log("原生加载成功," + JSON.stringify(nativeInfo));
                window.qg.unityInstance.Module.SendMessage("AD", "OnLoadNativeAdInfoSuccess", JSON.stringify(nativeInfo));
                window.currentNativeAdId = res.adList[0].adId;
            }
        });

        window.nativeAd.onError(function (e)
        {
            console.error('加载原生广告失败: ' + JSON.stringify(e));
            const errCode = e.errCode;
            const errMsg = e.errMsg;
        });

if(window.canShowAd){
        // banner初始化
        window.lastShowBannerTimestamp = 0;
        var sysInfo = qg.getSystemInfoSync();
        var bannerAdID = Pointer_stringify(bannerAdIDStr);
        window.bannerAd = qg.createBannerAd({
            adUnitId: bannerAdID,
            style: {
                top: sysInfo.safeArea.height- 57,
                left: 0,
                height: 57,
                width: 360,
            },
            adIntervals: 60
        })
		
		    //监听banner广告错误事件
        window.bannerAd.onError(function(errMsg, errCode) {
        console.info("Banner onError ---------> errMsg: " + JSON.stringify(errMsg) + "  errCode:" + errCode);
      });
	}
		

        // 激励视频初始化
        var rewardedAdID = Pointer_stringify(rewardedAdIDStr);
        window.rewardedAd = qg.createRewardedVideoAd({
            adUnitId: rewardedAdID,
        });

        window.rewardedAd.onLoad(function (res)
        {
            console.log('激励视频广告加载完成-onload触发', res.toString());
        });

        window.rewardedAd.onError(function (err)
        {
            console.log("激励视频广告加载失败", JSON.stringify(err));
           // qg.showToast({
                //提示内容
               // message: "暂无激励,稍后再试",
               // duration: 1000
           // });
            setTimeout(window.rewardedAd.load(), 3000);
        });

        window.rewardedAd.load();

        // 隐藏至后台处理
        qg.onHide(function ()
        {
            console.log("游戏切至后台");
            window.currentNativeAdId = null;
            window.qg.unityInstance.Module.SendMessage("AD", "OnGameSwitchToBackground");
        });

        // 从后台切换回游戏的处理
        qg.onShow(function ()
        {
            console.log("后台切回游戏");
            if (window.isShowNativeAd)
            {
                window.qg.unityInstance.Module.SendMessage("AD", "ShowNativeAd");
            }
        });

        console.log("初始化完成");
    },

    Login_JS: function (appidStr)
    {
        window.AppID = Pointer_stringify(appidStr);

        qg.gameLoginWithReal({
            appid: window.AppID,
            forceLogin: 1,
            success: function (data)
            {
                console.log("save player info with real success:" + data);
                window.qg.unityInstance.Module.SendMessage('AD', 'OnLoginSuccess');
            },
            fail: function (data, code)
            { 
                window.qg.unityInstance.Module.SendMessage('AD', 'OnLoginFail');
                console.log("game login with real fail:" + data + ", code:" + code);
               
            },
            complete: function ()
            {
                console.log("登陆方法执行完毕");
            }
        });
    },

    Logout_JS: function ()
    {
        qg.gameLoginWithReal({
            appid: window.AppID,
            forceLogin: -1,
            fail:function(data,code)
            {
                localStorage.clear();
            }
        });
    },

    ExitApplication: function ()
    {
        console.log("enter app exit");
        qg.exitApplication({
            success: function ()
            {
                console.log("exitApplication success");
            },
            fail: function ()
            {
                console.log("exitApplication fail");
            },
            complete: function ()
            {
                console.log("exitApplication complete");
            }
        });
    },

    OpenUrl: function(url)
    {
        var uriStr = Pointer_stringify(url);
        qg.openDeeplink({uri: uriStr});
    },
	
	
	
	//L_测试------------------------------
	 OnClick_JieShao: function(url)
    {
        var uriStrJieShao = Pointer_stringify(url);
        qg.openDeeplink({uri: uriStrJieShao});
    }, 
	OnClick_YinSi: function(url)
    {
        var uriStrYinSi = Pointer_stringify(url);
        qg.openDeeplink({uri: uriStrYinSi});
    }, 
	OnClick_QuanXian: function(url)
    {
        var uriStrQuanXian = Pointer_stringify(url);
        qg.openDeeplink({uri: uriStrQuanXian});
    },
	
	
	
	
	
	
	
	
	

    SaveStrData: function (key, data)
    {
        var keyStr = Pointer_stringify(key);
        var dataStr = Pointer_stringify(data);
        localStorage.setItem(keyStr, dataStr);
    },

    LoadStrData: function (key, defaultValue)
    {
        var keyStr = Pointer_stringify(key);
        var defaultStr = Pointer_stringify(defaultValue);

        var value = localStorage.getItem(keyStr);
        if (value == null || value == undefined)
        {
            value = defaultStr;
        }
        var bufferSize = lengthBytesUTF8(value) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(value, buffer, bufferSize);
        return buffer;
    },

    ClearData: function ()
    {
        localStorage.clear();
    }

});