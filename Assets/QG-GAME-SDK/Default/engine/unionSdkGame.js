// 解析本地sdk配置信息，并初始化sdk
try {
  var useSdk = UnityLoader.EnvConfig.getConfig("useSdk");
  if (useSdk) {
    const sdk = require('./unionSdk.js');
    const fs = qg.getFileSystemManager();
    const configStr = fs.readFileSync('QGSdkConfig.json', 'utf8');
    const config = JSON.parse(configStr)
  
    // 初始化sdk,会把⽅法挂载在qg上
    if (sdk && sdk.init) {
      sdk.init({config: config, adInfo: config.sdkAdInfoConfig})
    } else if (window.init){
      init({config: config, adInfo: config.sdkAdInfoConfig})
    }
  }
} catch (e) {
  console.error(e);
}
