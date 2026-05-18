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
        public string token; //调用接口获取登录凭证（token）。通过凭证进而换取用户登录态信息，包括用户的唯一标识（openid）
    }

    [Serializable]
    public class QGPayBean
    {
        public string result; //商户订单号
        public string code; //错误码
    }

    [Serializable]
    public class QGFileBean
    {
        public bool hasShortcutInstalled; // 是否创建桌面图标
    }

    [Serializable]
    public class QGUserInfoBean
    {
        public string nickName; // 用户昵称
        public string smallAvatar; // 用户社区小头像
        public string biggerAvatar; // 用户社区大头像
        public int gender; //性别：0，保密；1，男；2，女
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
    public class QGNativeResponse : QGBaseResponse
    {
        [SerializeField] public NativeItemBean[] adList; // 原生广告数据列表
    }

    [Serializable]
    public class QGRewardedVideoResponse : QGBaseResponse
    {
        [SerializeField] public bool isEnded; // 视频是否是在用户完整观看的情况下被关闭的，true 表示用户是在视频播放完以后关闭的视频，false 表示用户在视频播放过程中关闭了视频
    }

    [Serializable]
    public class NativeItemBean
    {
        public string adId; // 广告标识，用来上报曝光与点击
        public string title; // 广告标题
        public string desc; // 广告描述
        public string icon; // 推广应用的Icon图标
        public string[] imgUrlList; // 广告图片，建议使用该图片资源
        public string logoUrl; // 广告标签图片
        public string clickBtnTxt; // 点击按钮文本描述
        public int creativeType; // 获取广告类型，取值说明：0：混合
        public int interactionType; // 获取广告点击之后的交互类型，取值说明： 1：网址类 2：应用下载类 8：快应用生态应用
    }

    public class QGNativeReportParam
    {
        public string adId; //广告位id
    }

    [Serializable]
    public class QGKeyboardInputResponse : QGBaseResponse
    {
        public string value;
    }

    public class QGCommonAdParam
    {
        public string posId; //广告位id（必填 非常重要）
    }

    public class QGCreateCustomAdParam : QGCommonAdParam
    {
        public Style style; // 广告位置
    }

    public class QGCreateBannerAdParam : QGCommonAdParam
    {
        public int adIntervals; // 刷新时间
        public Style style; // 广告位置
    }

    public class QGCreateBoxPortalAdParam : QGCommonAdParam
    {
        public string image; //替换悬浮icon的默认图标，支持本地和网络图片 注：图片宽度不能超过300px，否则会以宽度300进行缩放
        public int marginTop; //盒子九宫格广告悬浮Icon相对顶部的距离，单位：px，不同分辨率自行调整
    }

    public class Style
    {
        public int left;
        public int top;
    }

    public class PayParam
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

    public class QGFileParam
    {
        public string uri; //需要读取的本地文件uri，不能是tmp类型的uri
        public string encoding = "utf8"; //编码格式，encoding 的合法值: utf8，binary。默认 utf8
        public int position = 0; //读取二进制数据的起始位置，默认值为文件的起始位置
        public int length = int.MaxValue; //读取二进制的长度，不填写则读取到文件结尾

        public string textStr;  // 写入的文本
        public byte[] textData; // 写入的二进制数据
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

    public class SubscribeParam
    {
        public string templateIds;
        public string clientId;
        public string userId;
        public string scene;
        public int type;
        public string subDesc;
    }

}
