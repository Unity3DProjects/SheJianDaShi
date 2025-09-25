mergeInto(LibraryManager.library, {

    ShowBannerAd_Vivo: function (posIdStr, showDateStr) {
        if (showDateStr != null && showDateStr != "")
        {
            currentDate = new Date();
            showDate = new Date(Pointer_stringify(showDateStr));

            // 还没到屏蔽结束时间直接返回
            if (showDate - currentDate > 0)
            {
                return;
            }
        }

        console.log("banner id: " + Pointer_stringify(posIdStr));
        if (window.bannerAd != null) {
            window.bannerAd.hide();
        }

        window.bannerAd = qg.createBannerAd({
            posId: Pointer_stringify(posIdStr),
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

    HideBannerAd: function () {
        if (window.bannerAd != null) {
            window.bannerAd.hide();
        }
    },

    ShowCustomAd_Vivo: function (posIdStr, leftPosIdStr, showDateStr, orientation) {
        if (showDateStr != null && showDateStr != "") {
            currentDate = new Date();
            showDate = new Date(Pointer_stringify(showDateStr));

            // 还没到屏蔽结束时间直接返回
            if (showDate - currentDate > 0) {
                return;
            }
        }

        // 横竖屏
        var sysInfo = qg.getSystemInfoSync();
        var _top;
        var _left;
        if (orientation = 0) {
            _top = 100;
            _left = (sysInfo.screenWidth - 1080) / 2;
        }
        else {
            _top = (sysInfo.screenHeight - 720) * 0.65;
            _left = (sysInfo.screenWidth - 1080) * 0.5;
        }

        if (qg.createCustomAd) {
            window.customAd = qg.createCustomAd({
                posId: Pointer_stringify(posIdStr),
                style: { left: _left, top: _top }
            });

            window.customAd.onError(function (err) {
                console.log("模板原生加载失败", err.toString());

                window.customAd = qg.createCustomAd({
                    posId: Pointer_stringify(leftPosIdStr),
                    style: { left: _left, top: _top }
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

    HideCustomAd: function () {
        if (window.customAd != null) {
            window.customAd.hide();
        }
    },

 Init_Vivo: function (adUnitIdStr,PosIdStr_BaoXiang) {
       if (window.rewardedAd != null) {
          return;
        }
	
	    window.rewardedAd = qg.createRewardedVideoAd({
          posId: Pointer_stringify(adUnitIdStr),
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

if(PosIdStr_BaoXiang=="") return;
    //判断路总的自渲染宝箱是否开启
    if (window["qg"].createNewNativeAd) {
      var nativeAd = window["qg"].createNewNativeAd({
        posId: Pointer_stringify(PosIdStr_BaoXiang),
      });

      nativeAd.onLoad(function (res) {
        console.info("原生自渲染2.0广告加载完成-onload触发", JSON.stringify(res));

        if (nativeAd) {
          //取消监听原生自渲染2.0广告加载结束事件
          nativeAd.offLoad();
          //console.log("原生自渲染2.0广告 移除广告加载事件");
        }

        unityInstance.SendMessage("AdManager", "ChackLuZongBox", "Open");
      });

      //监听原生自渲染2.0广告加载失败事件
      nativeAd.onError(function (errMsg) {
        console.log("原生自渲染2.0广告加载失败 onError ---------> errMsg: " + JSON.stringify(errMsg));
        var err_Msg = errMsg;
        var err_Code = err_Msg["errCode"];

        console.info("err_Code : ", err_Code);
        unityInstance.SendMessage("Box1111", "ChackBox", err_Code + "");

        if (nativeAd) nativeAd.offError();
      });
    }

    },

ShowRewardAd_Vivo: function (callBackObjectName, callBackMethodName, callBackParam, posIdStr) {
    //window.rewardedAd = qg.createRewardedVideoAd({
     // posId: Pointer_stringify(posIdStr),
   // });

    var objectName = Pointer_stringify(callBackObjectName);
    var methodName = Pointer_stringify(callBackMethodName);
    var param = Pointer_stringify(callBackParam);

    //window.rewardedAd.onLoad(function (res) {
    //  console.log("激励视频广告加载完成-onload触发", res.toString());
      window.rewardedAd
        .show()
        .then(function () {
		
          console.log("激励视频广告展示完成");
        })
        .catch(function (err) {
          console.log("激励视频广告展示失败", err.toString());
          qg.showToast({
            message: "暂无广告，请稍后再试",
            duration: 1,
          });
		  unityInstance.SendMessage("AdManager", "Play", "");
        });
   // });

    window.rewardedAd.onClose(function (res) {
      console.log("视频广告关闭回调");
	    unityInstance.SendMessage("AdManager", "ShowADBack", "");
      if (res && res.isEnded) {
        console.log("正常播放结束，可以下发游戏奖励");
        unityInstance.SendMessage(objectName, methodName, param);
       
      } else {
        console.log("播放中途退出，不下发游戏奖励");
		unityInstance.SendMessage("AdManager", "Play", "");
      }
	  window.rewardedAd.load();
	  
    });

   // window.rewardedAd.onError(function (err) {
   //   console.log("激励视频广告加载失败", err.toString());
   //   qg.showToast({
   //     message: "暂无广告，请稍后再试",
    //    duration: 1,
   //   });
   // });
  },

CreateShortcutButton_Vivo: function () {
    qg.hasShortcutInstalled({
      success: function (status) {
        if (status) {
          //console.log("已创建");
          qg.showToast({
            message: "桌面图标已创建~",
            duration: 2,
          });
		  unityInstance.SendMessage("PanelManager","ChackDesktopIConIsSuccessfully","");
        } else {
          qg.installShortcut({
            success: function () {
              qg.showToast({
                message: "桌面图标创建成功~",
                duration: 2,
              });
			   unityInstance.SendMessage("PanelManager","ChackDesktopIConIsSuccessfully","");
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
  
  
ExitApplication_Vivo: function () {
        qg.exitApplication();
    }

});