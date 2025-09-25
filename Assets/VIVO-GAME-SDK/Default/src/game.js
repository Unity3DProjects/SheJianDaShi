/* eslint-disable  quotes */
require("./qgame-adapter.js")
require("./store.js")
require("./unityAdapter.js")
const md5Utils = require('./md5.js');

//使用该功能时，需判断引擎是否支持
//创建CustomizeLoading组件
var loading;
if (qg.createCustomizeLoading) {
  var bgImageSrc = UnityLoader.EnvConfig.getConfig("bgImageSrc")
  var background = 'https://wwwstatic.vivo.com.cn/vivoportal/files/resource//funtouch/1612511921756/images/originos-night-img1-lg.jpg';
  if (bgImageSrc !== undefined && bgImageSrc !== null && bgImageSrc !== '') {
    console.log('background update', bgImageSrc);
    background = bgImageSrc;
  }
  loading = qg.createCustomizeLoading({
    background: background,
    text: '请耐心等待加载中...',
    textColor: '#ffffff',
    loadingColorTop: '#ffffff',
    loadingColorBottom: '#ffffff',
    loadingProgress: 0
  });
}

function updateLoading(progress) {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  //根据实际场景进行更新进度、背景、文字以及文字颜色
  //更新CustomizeLoading样式
  loading.update({
    loadingProgress: progress * 100
  });
}

function updateLoading(progress,textStr){
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }
  loading.update({
    loadingProgress: progress * 100,
    text: textStr
  });
}

function updateLoadingError() {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  loading.update({
    text: '加载失败，请重启游戏'
  });
}

function removeLoading() {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  //移除CustomizeLoading组件
  loading.remove();
}

function compileRateSimulate() {
  if (!qg.createCustomizeLoading || !loading) {
    clearInterval(loadingTask)
    return;
  }
  loadCurRate += 0.02
  if (loadCurRate >= 0.99) {
    clearInterval(loadingTask)
    return
  }
  updateLoading(loadCurRate,"编译初始化中...");
}

var loadingTask = null
const down_take_rate = 0.5 // 下载所占比例
var loadCurRate = 0

var wasmUrl = UnityLoader.EnvConfig.getConfig("wasmUrl")
var subUnityPkg = UnityLoader.EnvConfig.getConfig("subUnityPkg")
UnityLoader.UnityInfo.unityStartTime = qg.getPerformance().now()

function execUnity() {
  /* eslint-disable  quotes */
  preloadAssets();
  loadCurRate = down_take_rate
  loadingTask = setInterval(compileRateSimulate, 1000)
  window['unityInstance'] = window.UnityLoader.instantiate("/buildUnity/webgl.json", {
    onProgress: function (_, i) {
      // 更新启动loading组件进度
      // unity自身进度逻辑：文件准备完成90%，编译完成99%，完成100%
      // 由于编译时间较久，此处仅使用100%的逻辑，其他使用模拟数据
      if (i === 1) {
        clearInterval(loadingTask)
        updateLoading(i,"开始游戏");

        window.UnityLoader.printLog('加载完成')

        UnityLoader.UnityInfo.unityWasmLoadTime = qg.getPerformance().now() - UnityLoader.UnityInfo.unityWasmDownTime

        setTimeout(function (){
          //移除启动loading组件
          removeLoading();
        },300);

        //上报埋点
        if (qg.reportGameInfo) {
          reportLanchGameSingle()
        }
      }
    }
  })

}

function reportLanchGameSingle() {
  var unityPackage = ""
  if (qg.getSystemInfoSync().miniGame) {
    unityPackage = qg.getSystemInfoSync().miniGame.package;
  }
  var unityVersion = UnityLoader.UnityInfo.unityVersion.string;
  var unityPluginVersion = UnityLoader.UnityInfo.unityPluginVersion.toString();
  var unityWasmDownTime = UnityLoader.UnityInfo.unityWasmDownTime.toString();
  var unityWasmLoadTime = UnityLoader.UnityInfo.unityWasmLoadTime.toString();
  var unityCodeSize = UnityLoader.UnityInfo.unityCodeSize.toString();
  var unityDataSize = UnityLoader.UnityInfo.unityDataSize.toString();
  qg.reportGameInfo({
    unityPackage: unityPackage,
    unityVersion: unityVersion,
    unityPluginVersion: unityPluginVersion,
    unityWasmDownTime: unityWasmDownTime,
    unityWasmLoadTime: unityWasmLoadTime,
    unityCodeSize: unityCodeSize,
    unityDataSize: unityDataSize
  })

  //包体检测 提示
  var dataSize = UnityLoader.UnityInfo.unityDataSize / (1024 * 1024)
  if( dataSize > 15 ) {
    window.UnityLoader.printError('data wasm 首包资源文件过大，建议优化缩减到15M以内')
  }

  var codeSize = UnityLoader.UnityInfo.unityCodeSize / (1024 * 1024)
  if( codeSize > 30 ) {
    window.UnityLoader.printError('code wasm 代码逻辑文件过大，建议优化缩减到30M以内')
  }
}

function downloadSource(sourceUrl) {

  var key = md5Utils.hex_md5(sourceUrl)
  var cache_key = window.qg.getStorageSync({
    key: 'mini_wasm_cache_url_md5',
    default: 'default'
  })
  if (cache_key === key &&
    'true' === window.qg.accessFile({
      uri: window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.data.unityweb"
    }) &&
    'true' === window.qg.accessFile({
      uri: window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.wasm.code.unityweb"
    })) {
    updateLoading(down_take_rate);
    execUnity();
    return
  }
  if (cache_key !== 'default') {
    window.qg.rmdir({
      uri: window.qg.env.USER_DATA_PATH + "/" + cache_key
    })
  }
  qg.setStorage({
    key: 'mini_wasm_cache_url_md5',
    value: key,
    success: function (data) {
      window.UnityLoader.printLog('mini_wasm_cache_url_md5 cache success')
    },
    fail: function (data, code) {
      window.UnityLoader.printError(`mini_wasm_cache_url_md5 cache fail, code = ${code}`)
    }
  })
  var downPath = window.qg.env.USER_DATA_PATH + "/" + key + "/wasm_zipsource"
  var downloadTask = window.qg.downloadFile({
    url: sourceUrl,
    filePath: downPath,
    success: function () {
      window.qg.unzipFile({
        srcUri: downPath,
        dstUri: window.qg.env.USER_DATA_PATH + "/" + key + "/",
        success: function (uri) {
          updateLoading(down_take_rate)
          execUnity()
          UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now() - UnityLoader.UnityInfo.unityStartTime
        },
        fail: function (data, code) {
          window.UnityLoader.printError(`wasm unzip handling fail, code = ${code}`)
          updateLoadingError()
        }
      })
    },
    fail: function (e) {
      window.UnityLoader.printError("wasm download file fail " + JSON.stringify(e))
      updateLoadingError()
    }
  });
  downloadTask.onProgressUpdate(function (msg) {
    var progress = msg["progress"];
    updateLoading(progress / 100 * down_take_rate)
  });
}

function preloadAssets() {
  var preloadUrl = UnityLoader.EnvConfig.getConfig("preloadUrl")
  var preloadUrlList = preloadUrl.split(';')
  if (preloadUrlList) {
    preloadUrlList.forEach((url, index) => {
      if (url && url.value.trim().length != 0) {
        // 创建网络请求
        qg.request({
          url: url,
          dataType: 'arraybuffer',
          success: function (ret) {
            window.UnityLoader.printLog("preloadAssets request success " + " url = " + url)
          },
          fail: function (error, code) {
            window.UnityLoader.printError("preloadAssets request fail " + " url = " + url + " code = " + code)
          }
        });
      }
    });
  }

}

function loadSubUnity() {
  qg.loadSubpackage({
    name: 'unitySubPkg',
    success: function (info) {
      window.UnityLoader.printLog("load sub unity pkg success " + JSON.stringify(info))
      execUnity()
      UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now() - UnityLoader.UnityInfo.unityStartTime
    },
    fail: function (info) {
      window.UnityLoader.printLog("load sub unity pkg fail " + JSON.stringify(info))
      updateLoadingError()
    },
    complete: function () {

    }
  })
}

if (wasmUrl && wasmUrl.trim().length != 0) {
  //自定义loading 网络下载入口
  downloadSource(wasmUrl)
} else if (subUnityPkg) {
  //分包加载自定义loading
  loadSubUnity()
} else {
  //自定义loading 本地加载入口
  execUnity()
}