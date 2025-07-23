using System;
using UnityEngine;


namespace QGMiniGame
{
    [Serializable]
    public class QGBaseResponse
    {
        public string callbackId;
        public string errMsg;
        public string errCode;
    }

    [Serializable]
    public class QGCommonResponse<H> : QGBaseResponse
    {
        [SerializeField] public H data;
    }

    [Serializable]
    public class QGLoginBean
    {
        public string uid; // 用户唯一标识uid&openId一致
        public string openId; // 用户唯一标识uid&openId一致
        public string avatar; // 用户头像
        public string nickName; // 用户昵称
        public string token; //调用接口获取登录凭证（token）。通过凭证进而换取用户登录态信息，包括用户的唯一标识（openid）
    }

    public class PayParam
    {
        public int orderAmount; //单位为分，如商品价格为6元则要传“600”，传“6”或者“600.0”则会报错
        public string productName; //商品名称 (OPPO vivo 必填)
        public string productId; //商品 ID （荣耀必填）
        public string externalInfo; //cp 透传信息，非空 （小米必填）
    }

    public class vivoPayParam
    {
        public string appId; //由开发者平台申请得到--在游戏的支付服务页面里
        public string cpOrderNumber; //由商户自定义，每笔订单必须唯一。
        public string productName; //商品名称
        public string productDesc; //商品描述
        public int orderAmount; //单位为分，如商品价格为6元则要传“600”，传“6”或者“600.0”则会报错
        public string notifyUrl; //商户指定的回调url，支付成功后vivo会向此url通知支付结果。建议传，以保证支付结果准确。
        public string expireTime; //由商户自定义，格式为yyyyMMddHHmmss
        public string extInfo; //扩展参数（长度限制为64位）
        public string vivoSignature; //参与签名的字段为以上所有参数，计算方法参见签名计算说明
    }

    [Serializable]
    public class QGPayBean
    {
        public string result; //商户订单号
        public string code; //错误码
        public string message; //消息内容
    }

    [Serializable]
    public class QGFileBean
    {
        public bool hasShortcutInstalled; // 是否创建桌面图标
    }

    [Serializable]
    public class QGShortcutBean
    {
        public bool hasShortcutInstalled; // 是否创建桌面图标
    }

    [Serializable]
    public class QGFileResponse : QGBaseResponse
    {
        public string textStr;  // 读取的文本
        public byte[] textData; // 读取的二进制数据
        public string encoding;
        public int byteLength;
    }

    [Serializable]
    public class QGKeyboardInputResponse : QGBaseResponse
    {
        public string value;
    }

    public class QGFileParam
    {
        public string uri; //需要读取的本地文件uri，不能是tmp类型的uri
        public string encoding = "utf8"; //编码格式，encoding 的合法值: utf8，binary。默认 utf8
        public int position = 0; //读取二进制数据的起始位置，默认值为文件的起始位置
        public int length = int.MaxValue; //读取二进制的长度，不填写则读取到文件结尾

        public string textStr;  // 写入的文本
        public byte[] textData; // 写入的二进制数据
    }

    // 获取网络状态 
    [Serializable]
    public class QGGetNetworkType
    {
        public string networkType; // 网络类型
                                   // public string isConnected; // 当前是否有网络链接
    }

    [Serializable]
    public class QGOnNetworkStatus : QGBaseResponse
    {
        public string isConnected;
        public string networkType;
    }

    // 获取系统信息
    [Serializable]
    public class QGSystemInfo
    {
        public string manufacturer;//设备生产商
        public string brand; // 手机品牌
        public string model; // 手机型号
        public string product; // 设备代号
        public float pixelRatio; // 设备像素比
        public float benchmarkLevel; // 设备性能等级（仅 Android）。取值为：（性能未知）-1，0<（低端机）<44，44<=（中端机）<58，>=58（高端机）
        public string deviceOrientation; // 设备方向
        public string language; // 系统语言
        public string region;
        public string theme; // 系统当前主题
        public float battery; 
        public int wifiSignal; 
        public string osType;
        public string osVersionName;
        public int osVersionCode;  
        public string system;
        public string platformVersionName; // 客户端平台
        public float platformVersionCode; // 平台版本号
        public int allianceVersion; // 联盟接口版本号 
        public string COREVersion; 

        public MiniGame miniGame;

        public float screenHeight; // 屏幕高度
        public float screenWidth; // 屏幕宽度
        public float windowHeight; // 可使用窗口高度
        public float windowWidth; // 可使用窗口宽度
        public float statusBarHeight; // 状态栏/异形缺口高度
        public bool isNotch; // 状态栏/异形缺口高度
        public bool isHole; // 状态栏/异形缺口高度

        public int hole_x; 
        public int hole_y;
        public int hole_radius;

        public SafeArea safeArea;
    }

    [Serializable]
    public class SafeArea
    {
        public int bottom;
        public int left;
        public int right;
        public int top;
        public int width;
        public int height;
    }

    [Serializable]
    public class MiniGame
    {
        public string package;
        public int version;
        public string buildType;
    }

    // 获取系统信息
    [Serializable]
    public class QGWindowInfo
    {
        public float pixelRatio; // 设备像素比
        public float windowHeight; // 可使用窗口高度
        public float windowWidth; // 可使用窗口宽度
        public float statusBarHeight; // 状态栏/异形缺口高度
        public float screenTop; // 窗口上边缘的 y 值
        public SafeArea safeArea;
    }

    [Serializable]
    public class MenuButtonBoundingClientRect
    {
        public int bottom;
        public int left;
        public int right;
        public int top;
        public int width;
        public int height;
    }

    [Serializable]
    public class ShowModalParam
    {
        public string title;
        public string content;
        public bool showCancel = true;
        public string cancelText = "取消";
        public string cancelColor = "#FFCB1B";
        public string confirmText = "确定";
        public string confirmColor = "#FFCB1B";
        public bool editable = false;
        public string placeholderText;
    }

    [Serializable]
    public class ShowModalResponse
    {
        public string content;
        public bool confirm; //确定
        public bool cancel; //取消
    }

    [Serializable]
    public class DownLoadFileParam
    {
        public string filePath;
        public string url;
        public string header;
        public int timeout;
    }

    [Serializable]
    public class DownLoadFileResponse
    {
        public int dataLength;
        public int statusCode; 
        public string tempFilePath;
        public string filePath;
    }

    [Serializable]
    public class UpLoadFileParam
    {
        public string filePath; 
        public string url;  
        public string name;
        public string header;
        public string formData;
        public int timeout;
    }

    [Serializable]
    public class UpLoadFileResponse
    {
        public int statusCode;
        public string data;
    }

    [Serializable]
    public class VideoParam
    {
        //必要参数 具体参数参考: http://test.gamedoc.so-quick.cn/api2.0/%E5%AA%92%E4%BD%93/%E8%A7%86%E9%A2%91.html?h=CreateVideo
        public string src;      //必要参数 视频的资源地址
        //选填参数
        public int x;      //视频的左上角横坐标 默认值0
        public int y;      //视频的左上角纵坐标 默认值0
        public int width;  //视频的宽度 默认值300
        public int height; //视频的高度 默认值150
        public string poster;   //视频的封面
        public float initialTime; //视频的初始播放位置，单位为 s 秒  默认值0
        public float playbackRate;//视频的播放速率，有效值有 0.5、0.8、1.0、1.25、1.5 默认值1.0
        public string objectFit; //视频的缩放模式 fill:填充  contain:包含，保持原有长宽比例  cover: 覆盖，保持原有长宽比例 默认值'contain'
        public bool controls;      //视频是否显示控件 true
        public bool showProgress;   //是否显示视频底部进度条
        public bool showProgressInControlMode;   //是否显示控制栏的进度条
        public string backgroundColor;   //视频背景颜色
        public bool autoplay;   //视频是否自动播放 默认值false
        public bool loop;       //视频是否循环播放 默认值false
        public bool muted;      //视频是否静音 默认值false
        public bool enableProgressGesture;   //是否启用手势控制播放进度 默认值false
        public bool enablePlayGesture;   //是否开启双击播放的手势
        public bool showCenterPlayBtn; //是否显示视频中央的播放按钮 默认值false
        public bool underGameView; //视频是否显示在游戏画布之下（配合 Canvas.getContext('webgl', {alpha: true}) 使主屏 canvas 实现透明效果）
        public bool autoPauseIfNavigate; //	视频跳转后自动暂停播放
        public bool autoPauseIfOpenNative; //视频跳转原生页后自动暂停播放

    }

    [Serializable]
    public class AudioParam
    {
        public string src;
        public float startTime;
        public bool autoplay;
        public bool loop;
        public float volume = 1f;
        public float playbackRate = 1f;
    }

    [Serializable]
    public class ShowToastParam
    {
        public string title;
        public string icon;
        public string image;
        public int duration;
        public bool mask;
    }

    [Serializable]
    public class QGManifestInfoRponse
    {
        public string package;  //游戏包名
        public string name;     //游戏名
        public string versionName; //游戏版本名
        public string versionCode; //游戏版本号
        public string minPlatformVersion; //最小平台版本号
        public string icon; //桌面图标
        public string orientation; //设备方向
        public string type; //不填或者默认值为 app，取值为 app 或 game
        public object config; //logLevel 取值
        public object subpackages; //分包功能，有分包时才需要，可选字段
    }

    [Serializable]
    public class UserCloudStorageParam
    {
        public string key;  //云储存 key
        public string value;//云储存 value
    }

    [Serializable]
    public class BatteryInfoParam
    {
        public float level; //设备电量，范围 1 - 100
        public bool isCharging; //是否正在充电中
    }

    [Serializable]
    public class DeviceIdParam
    {
        public string deviceId; //设备唯一标识
        public string oaid; //设备唯一标识oaid
    }

    [Serializable]
    public class ScreenBrightnessParam
    {
        public float value; //屏幕亮度
    }

    [Serializable]
    public class GetLocationParam
    {
        public float latitude;  //纬度，范围为 -90~90，负数表示南纬
        public float longitude; //经度，范围为 -180~180，负数表示西经
        public float speed;     //速度，单位 m/s
        public float accuracy;  //位置的精确度
        public float altitude;  //高度，单位 m
        public float verticalAccuracy;  //垂直精度，单位 m（Android 无法获取，返回 0）
        public float horizontalAccuracy;//水平精度，单位 m
    }

    [Serializable]
    public class onAccelerometerChangeParam
    {
        public float x;  //x 轴
        public float y;  //y 轴
        public float z;  //z 轴

    }

    [Serializable]
    public class DeviceMotionChangeParam
    {
        public float alpha;
        public float beta;
        public float gamma;
    }

    [Serializable]
    public class QGIsStartupByShortcutParam : QGBaseResponse
    {
        public bool isStartupByShortcut;
    }

    [Serializable]
    public class RecordParam
    {
        //选填 具体参数参考:https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/media/record?id=recordermanagerstartobject
        public int duration = 60000;  //录音的时长，单位 ms，最大值 600000（10 分钟） 默认值 60000
        public int sampleRate = 8000; //采样率 默认值 8000
        public int numberOfChannels = 2;     //录音通道数  默认值 2
        public int encodeBitRate = 48000;  //编码码率 默认值 48000
        public string format = "aac";     //音频格式  默认值 aac
        public int frameSize = 1000;  ///指定帧大小，单位 KB
        public string audioSource = "auto";  //指定录音的音频输入源 默认值 auto
    }

    [Serializable]
    public class QGOnshowResponse : QGBaseResponse
    {
        public string type; // 1：冷启动（首次启动） 2：热启动（非首次启动）
        public Query query; //启动小游戏的 query 参数
        public ReferrerInfo referrerInfo; //其他信息,包含跳转到该小游戏的应用带来的额外数据
    }

    [Serializable]
    public class Query
    {
        public string userEntry;
    }

    [Serializable]
    public class ReferrerInfo
    {
        public string package;
        public string type;
        public string extraData;
    }

    public class QGFileInfo 
    {
        public string textStr;
        public byte[] textData;
    }

    public class KeyboardParam
    {
        public string defaultValue; //键盘输入框显示的默认值
        public int maxLength; //键盘中文本的最大长度
        public bool multiple; //是否为多行输入
        public bool confirmHold; //当点击完成时键盘是否收起
        public string confirmType; //键盘右下角confirm按钮类型，只影响按钮的文本内容
    }

}
