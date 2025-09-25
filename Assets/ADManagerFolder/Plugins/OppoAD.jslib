mergeInto(LibraryManager.library, {
  ShowBannerAd_Oppo: function (bannerAdUnitIdStr, showDateStr) {
    if (showDateStr != null && showDateStr != "") {
      currentDate = new Date();
      showDate = new Date(Pointer_stringify(showDateStr));

      // 还没到屏蔽结束时间直接返回
      if (showDate - currentDate > 0) {
        return;
      }
    }

    var bannerAdUnitId = Pointer_stringify(bannerAdUnitIdStr);
	if(bannerAdUnitId=="") return;
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

  ShowCustomAd_Oppo: function (adUnitIdStr, showDateStr, orientation) {
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
    if (orientation == 0) {
      _top = 200;
      _left = (sysInfo.screenWidth - 1080) / 2;
    } else {
      _top = sysInfo.screenHeight * 0.5;
      _left = sysInfo.screenWidth * 0.1;
    }

    if (qg.createCustomAd) {
      window.customAd = qg.createCustomAd({
        adUnitId: Pointer_stringify(adUnitIdStr),
        style: { top: _top },
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

  ShowCustomAdZone_Oppo: function (adUnitIdStr, showDateStr, orientation) {
    console.log("window.isShowAdZone: " + window.isShowAdZone);
    if (window.isShowAdZone) {
      console.log("展示原生--限制");
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
          style: { top: _top },
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

  HideCustomAd_Oppo: function () {
    if (window.customAd != null) {
      window.customAd.hide();
    }
  },

  ShowRewardAd_Oppo: function (callBackObjectName, callBackMethodName, callBackParam) {
    var objectName = Pointer_stringify(callBackObjectName);
    var methodName = Pointer_stringify(callBackMethodName);
    var param = Pointer_stringify(callBackParam);

    window.rewardedAd
      .show()
      .then(function () {
        console.log("激励视频广告展示完成");
      })
      .catch(function (err) {
        console.log("激励视频广告展示失败", JSON.stringify(err));
		 unityInstance.SendMessage("AdManager", "Play", ""),
        qg.showToast({
          //提示内容
          title: "暂无广告,稍后再试",
          duration: 2000,
        });
      });

    window.rewardedAd.onClose(function (res) {
      console.log("视频广告关闭回调");
	 unityInstance.SendMessage("AdManager", "Play", "");
	 unityInstance.SendMessage("AdManager", "ShowADBack", "");
      if (res && res.isEnded) {
        console.log("正常播放结束，可以下发游戏奖励");
        unityInstance.SendMessage(objectName, methodName, param);
      } else {
        console.log("播放中途退出，不下发游戏奖励");
      }
      window.rewardedAd.offClose();

      window.rewardedAd.load();
    });
  },
  
  ShowRewardAdBox_Oppo: function (callBackObjectName, callBackMethodName, callBackParam) {
    var objectName = Pointer_stringify(callBackObjectName);
    var methodName = Pointer_stringify(callBackMethodName);
    var param = Pointer_stringify(callBackParam);

    window.rewardedAdBox
      .show()
      .then(function () {
        console.log("box激励视频广告展示完成");
      })
      .catch(function (err) {
        console.log("box激励视频广告展示失败",JSON.stringify(err));
        qg.showToast({
          //提示内容
          title: "暂无广告,稍后再试",
          duration: 2000,
        });
      });

    window.rewardedAdBox.onClose(function (res) {
      console.log("视频广告关闭回调");
	  unityInstance.SendMessage("AdManager", "ShowAD_Black", "");
      if (res && res.isEnded) {
        console.log("box正常播放结束，可以下发游戏奖励");
        unityInstance.SendMessage(objectName, methodName, param);
      } else {
        console.log("播放中途退出，不下发游戏奖励");
      }
      window.rewardedAdBox.offClose();

      window.rewardedAdBox.load();
    });
  },

  Init_Oppo: function (adUnitIdStr) {
    window.isShowAdZone = true;

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
            console.log("获取地区数据:" +  JSON.stringify(data));
          var arr = [ "厦门市", "北京市","上海市", "重庆市","西安市","南京市","杭州市", "武汉市", "长沙市", "成都市", "广州市","深圳市","东莞市","苏州市","福州市"];
          if (arr.indexOf(data.province) >=0 || arr.indexOf(data.city) >=0) {
            window.isShowAdZone = false;
            console.log("是屏蔽地区");
            unityInstance.SendMessage("AdManager", "ReceiveArea", "NoAd");
            
          } else {
            window.isShowAdZone = true;
            console.log("不是屏蔽地区");
            //console.log("获取地区数据:" + data);
            unityInstance.SendMessage("AdManager", "ReceiveArea", "HaveAd");
          }
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

    if (window.rewardedAd != null) {
      return;
    }

    window.rewardedAd = qg.createRewardedVideoAd({
      adUnitId: Pointer_stringify(adUnitIdStr),
    });

    window.rewardedAd.load();

    window.rewardedAd.onError(function (err) {
      console.log("激励视频广告加载失败", JSON.stringify(err));
     // qg.showToast({
        //提示内容
      //  title: "暂无广告,稍后再试",
       // duration: 2000,
     // });

     // window.rewardedAd.load();
    });

    window.rewardedAd.onLoad(function (res) {
      console.log("激励视频广告加载完成-onload触发", JSON.stringify(err));
    });
	
	
  },

 InitBoxReward_Oppo: function (adUnitIdStrBox) {
 //宝箱激励初始化
	window.rewardedAdBox = qg.createRewardedVideoAd({
      adUnitId: Pointer_stringify(adUnitIdStrBox),
    });

    window.rewardedAdBox.load();

    window.rewardedAdBox.onError(function (err) {
      console.log("box激励视频广告加载失败", JSON.stringify(err));
     // qg.showToast({
        //提示内容
      //  title: "暂无广告,稍后再试",
       // duration: 2000,
     // });

     
    });

    window.rewardedAdBox.onLoad(function (res) {
      console.log("box激励视频广告加载完成-onload触发", JSON.stringify(err));
    });
 },


  CreateShortcutButton_Oppo: function () {
    qg.hasShortcutInstalled({
      success: function (status) {
        if (status) {
          //console.log("已创建");
          qg.showToast({
            title: "桌面图标已创建~",
            duration: 2000,
          });
		  unityInstance.SendMessage("PanelManager","ChackDesktopIConIsSuccessfully","");
        } else {
          qg.installShortcut({
            success: function () {
              qg.showToast({
                title: "桌面图标创建成功~",
                duration: 2000,
              });
			  unityInstance.SendMessage("PanelManager","ChackDesktopIConIsSuccessfully","");
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

  ExitApplication_Oppo: function () {
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
