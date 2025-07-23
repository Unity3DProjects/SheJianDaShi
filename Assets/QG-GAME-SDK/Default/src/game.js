/* eslint-disable  quotes */
require("./qgame-adapter.js")
require("./store.js")
require("./unityAdapter.js")
// 引入统一SDK功能
require('./unionSdkGame.js')
require("./asset-cache.js")
const md5Utils = require('./md5.js');

//适配OPPO的wasm编译，OPPO未开放wasm，需要调用该方法才行
if (qg.setWasmTaskCompile && qg.getProvider() === "OPPO") {
  qg.setWasmTaskCompile(true)
}

let isHonor = () => {
  const result = window.qg.getProvider().toLowerCase() === 'honor'
  isHonor = () => result
  return isHonor()
}

//使用该功能时，需判断引擎是否支持
//创建CustomizeLoading组件
var loading;
if (qg.createCustomizeLoading) {
  var bgImageSrc = UnityLoader.EnvConfig.getConfig("bgImageSrc");
  var background =
    "https://wwwstatic.vivo.com.cn/vivoportal/files/resource//funtouch/1612511921756/images/originos-night-img1-lg.jpg";
  if (bgImageSrc !== undefined && bgImageSrc !== null && bgImageSrc !== "") {
    console.log("background update", bgImageSrc);
    background = bgImageSrc;
  }
  loading = qg.createCustomizeLoading({
    background: background,
    text: "请耐心等待加载中...",
    textColor: "#ffffff",
    loadingColorTop: "#ffffff",
    loadingColorBottom: "#ffffff",
    loadingProgress: 0,
  });
}

function updateLoading(progress, textStr = null){
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }
  if (textStr !== null) {
    loading.update({
      loadingProgress: progress * 100,
      text: textStr
    });
  } else {
    //根据实际场景进行更新进度、背景、文字以及文字颜色
    //更新CustomizeLoading样式
    loading.update({
      loadingProgress: progress * 100
    });
  }
}

function updateLoadingError() {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }
  reportUnityWasmInfo();
  loading.update({
    text: "加载失败，请重启游戏",
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
    clearInterval(loadingTask);
    return;
  }
  loadCurRate += 0.02;
  if (loadCurRate >= 0.99) {
    clearInterval(loadingTask);
    return;
  }
  updateLoading(loadCurRate, "编译初始化中...");
}

var loadingTask = null;
const down_take_rate = 0.5; // 下载所占比例
var loadCurRate = 0;

var wasmUrl = UnityLoader.EnvConfig.getConfig("wasmUrl");
var wasmDataUrl = UnityLoader.EnvConfig.getConfig("wasmDataUrl");
var subUnityPkg = UnityLoader.EnvConfig.getConfig("subUnityPkg");

function execUnity() {
  /* eslint-disable  quotes */
  preloadAssets();
  loadCurRate = down_take_rate;
  loadingTask = setInterval(compileRateSimulate, 1000);
  window["unityInstance"] = window.UnityLoader.instantiate(
    "/buildUnity/webgl.json",
    {
      onProgress: function (_, i) {
        // 更新启动loading组件进度
        // unity自身进度逻辑：文件准备完成90%，编译完成99%，完成100%
        // 由于编译时间较久，此处仅使用100%的逻辑，其他使用模拟数据
        if (i === 1) {
          clearInterval(loadingTask);
          updateLoading(i, "开始游戏");
          window.UnityLoader.printLog("加载完成");
          UnityLoader.UnityInfo.unityWasmLoadTime = qg.getPerformance().now();

          setTimeout(function () {
            //移除启动loading组件
            removeLoading();
          }, 300);

          reportUnityWasmInfo();
        }
      },
    }
  );
}

function reportUnityWasmInfo() {
  const data = UnityLoader.UnityInfo;
  console.log("reportUnityWasmInfo start");
  if (qg.reportGameInfo) {
    const info = {
      unityVersion: data.unityVersion.string,
      unityPluginVersion: data.unityPluginVersion,
      unityStartTime: data.unityStartTime,
      unityWasmDownTime: data.unityWasmDownTime,
      unityWasmLoadTime: data.unityWasmLoadTime,
      unityCodeSize: data.unityCodeSize,
      unityDataSize: data.unityDataSize,
      unityWasmType: data.unityWasmType,
      unityWasmResult: data.unityWasmResult,
    };
    console.log("reportUnityWasmInfo real", info);
    qg.reportGameInfo(info);
  }
  //包体检测 提示
  var dataSize = data.unityDataSize / (1024 * 1024);
  if (dataSize > 15) {
    window.UnityLoader.printError(
      "data wasm 首包资源文件过大，建议优化缩减到15M以内"
    );
  }
  var codeSize = data.unityCodeSize / (1024 * 1024);
  if (codeSize > 30) {
    window.UnityLoader.printError(
      "code wasm 代码逻辑文件过大，建议优化缩减到30M以内"
    );
  }
}

function downloadSource(sourceUrl, cb) {
  const fs = window.qg.getFileSystemManager()

    var key = md5Utils.hex_md5(sourceUrl)
    var cache_key = window.qg.getStorageSync('mini_wasm_cache_url_md5')
    if (!cache_key) {
      cache_key = 'default'
    }
    try {
      if (cache_key === key) {
        fs.accessSync(window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.data.unityweb")
        fs.accessSync(window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.wasm.code.unityweb")
        updateLoading(down_take_rate);
        execUnity();
        return
      }
    } catch (err) {
      console.error(err)
    }
    
    if (cache_key !== 'default') {
      try {
        fs.rmdirSync(window.qg.env.USER_DATA_PATH + "/" + cache_key, true)
      } catch (err) {
        console.error(err)
      } 
    }
    qg.setStorage({
      key: 'mini_wasm_cache_url_md5',
      data: key,
      success: function (data) {
        window.UnityLoader.printLog('mini_wasm_cache_url_md5 cache success')
      },
      fail: function (data) {
        window.UnityLoader.printError(`mini_wasm_cache_url_md5 cache fail, code = ${data}`)
      }
    })
    var downPath = window.qg.env.USER_DATA_PATH + "/" + key + "/wasm_zipsource"
    if (isHonor()) {
      try {
        fs.accessSync(downPath);
      } catch (err) {
        fs.mkdirSync(downPath, true);
      }
      downPath = `${downPath}/wasm.zip`
    }

    var downloadTask = window.qg.downloadFile({
      url: sourceUrl,
      filePath: downPath,
      success: function () {
        fs.unzip({
          zipFilePath: downPath,
          targetPath: window.qg.env.USER_DATA_PATH + "/" + key + "/",
          success: function (uri) {
            updateLoading(down_take_rate)
            execUnity()
            UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now() - UnityLoader.UnityInfo.unityStartTime
          },
          fail: function (data, code) {
            window.UnityLoader.printError(`wasm unzip handling fail, code = ${data}`)
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
  var preloadUrl = UnityLoader.EnvConfig.getConfig("preloadUrl");
  var preloadUrlList = (typeof preloadUrl === "string" && preloadUrl.trim() !== "") ? preloadUrl.split(";") : [];
  if (preloadUrlList && preloadUrlList.length) {
    /**
     * 网络请求方法
     *
     * @param {XMLHttpRequest} xhr
     */
    function webRequest(xhr) {
      xhr.send();
    }
    preloadUrlList.forEach((url, index) => {
      if (url && url.trim().length != 0) {
        // 创建网络请求
        const xhr = new XMLHttpRequest();
        xhr.open("GET", url, true);
        xhr.responseType = "arraybuffer";
        xhr.onload = function () {
          if (xhr.readyState !== 4) {
            return;
          }
          if (xhr.status >= 200 && xhr.status < 400) {
            window.UnityLoader.printLog(
              "preloadAssets request success " + " url = " + url
            );
          }
          else {
            window.UnityLoader.printError(
              "preloadAssets request fail " +
                " url = " +
                url +
                " status = " +
                xhr.status +
                " statusText = " +
                xhr.statusText
            );
          }
        }
        // 通过缓存系统进行请求
        window.cacheXMLHttpRequest ? window.cacheXMLHttpRequest(xhr, webRequest.bind(null, xhr)) : webRequest(xhr);
      }
    });
  }
}

function downloadWebglData(sourceUrl, cb) {
  const fs = window.qg.getFileSystemManager()
  var key = md5Utils.hex_md5(sourceUrl);
  var cache_key = window.qg.getStorageSync("mini_wasm_cache_url_md5");
  if (cache_key === key) {
    try {
      cb(true);
      UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now();
      UnityLoader.UnityInfo.unityWasmResult = 4;
      console.log("downloadWebglData success by local");
      return;
    }catch(err){
      console.error("downloadWebglData access fail", err);
    }
  }
  if (cache_key) {
    try {
      fs.rmdirSync(window.qg.env.USER_DATA_PATH + "/" + cache_key, true);
    }
    catch (err) { 
      console.error("downloadWebglData rmdirSync fail", err);
    }
  }
  qg.setStorage({
    key: "mini_wasm_cache_url_md5",
    data: key,
    success: function (data) {
      window.UnityLoader.printLog("downloadWebglData cache success");
    },
    fail: function (data, code) {
      window.UnityLoader.printError(
        `downloadWebglData cache fail, code = ${code}`
      );
    },
  });
  var downPath = window.qg.env.USER_DATA_PATH + "/" + key + "/wasm_zipsource";
  if (isHonor()) {
    try {
      fs.accessSync(downPath);
    } catch (err) {
      fs.mkdirSync(downPath, true);
    }
    downPath = `${downPath}/wasm.zip`
  }

  var downloadTask = window.qg.downloadFile({
    url: sourceUrl,
    filePath: downPath,
    success: function () {
      fs.unzip({
        zipFilePath: downPath,
        targetPath: window.qg.env.USER_DATA_PATH + "/" + key + "/",
        success: function (uri) {
          cb(true);
          UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now();
          UnityLoader.UnityInfo.unityWasmResult = 0; // 下载成功，解压成功
          console.log("downloadWebglData prepare success");
        },
        fail: function (data, code) {
          cb(false);
          window.UnityLoader.printError(
            `downloadWebglData unzip handling fail, code = ${code}`
          );
          UnityLoader.UnityInfo.unityWasmResult = -2; // 下载成功，解压失败
        },
      });
    },
    fail: function (e) {
      cb(false);
      UnityLoader.UnityInfo.unityWasmResult = -1; // 下载失败
      window.UnityLoader.printError(
        "downloadWebglData file fail " + JSON.stringify(e)
      );
    },
  });
  downloadTask.onProgressUpdate(function (msg) {
    var progress = msg["progress"];
    updateLoading((progress / 100) * down_take_rate);
  });
}

function loadUnityWasmZip() {
  downloadSource(wasmUrl, (data) => {
    if (data) {
      updateLoading(down_take_rate);
      execUnity();
    } else {
      updateLoadingError();
    }
  });
}

var loadSubUnityStep2 = false;
var loadSubUnityStep1 = false;
function loadSubUnity() {
  loadSubUnityWasmData(); // wasm.data部分存放到自有cdn，一般是因为这部分数据>30MB以上需要这样做
  console.log("loadSubUnityStep2 start");
  qg.loadSubpackage({
    name: "unitySubPkg",
    success: function (info) {
      console.log("loadSubUnityStep2 success", info);
      loadSubUnityStep2 = true;
      execUnity2();
    },
    fail: function (e) {
      console.log("loadSubUnityStep2 failed", e);
      UnityLoader.UnityInfo.unityWasmResult = -1; // 下载失败
      updateLoadingError();
    },
    complete: function () {},
  });
}

function loadSubUnityWasmData() {
  if (wasmDataUrl && wasmDataUrl.trim().length != 0) {
    console.log("loadSubUnityStep1 start", wasmDataUrl);
    downloadWebglData(wasmDataUrl, function (data) {
      if (data) {
        console.log("loadSubUnityStep1 end", data);
        updateLoading(down_take_rate);
        loadSubUnityStep1 = true;
        execUnity2();
      } else {
        console.log("loadSubUnityStep1 failed", data);
        updateLoadingError();
      }
    });
  } else {
    window.wasmDataLoadStatus = true;
    loadSubUnityStep1 = true;
  }
}

function execUnity2() {
  console.log("downloadWebglData execUnity2 loadSubUnityStep1", loadSubUnityStep1, "loadSubUnityStep2", loadSubUnityStep2);
  if (loadSubUnityStep1 && loadSubUnityStep2) {
    UnityLoader.UnityInfo.unityWasmResult = 0;
    UnityLoader.UnityInfo.unityWasmDownTime = qg.getPerformance().now();
    execUnity();
  }
}

UnityLoader.UnityInfo.unityStartTime = qg.getPerformance().now();
if (wasmUrl && wasmUrl.trim().length != 0) {
  //自定义loading 网络下载入口
  UnityLoader.UnityInfo.unityWasmType = 1;
  loadUnityWasmZip()
} else if (subUnityPkg) {
  //分包加载自定义loading
  UnityLoader.UnityInfo.unityWasmType = 2;
  loadSubUnity();
} else {
  UnityLoader.UnityInfo.unityWasmType = 3;
  //自定义loading 本地加载入口
  execUnity();
}
