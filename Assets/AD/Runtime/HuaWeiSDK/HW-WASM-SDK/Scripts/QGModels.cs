using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace HWWASM
{
    [Preserve]
    public class LoginSuccessResult : CallbackBase
    {
        /// <summary>
        /// 帐号ID，如果游戏不需要华为帐号的登录结果进行鉴权，那么当返回playgerId的时候就可以使用该值进入游戏。
        /// </summary>
        public string playerId;

        /// <summary>
        /// 用户的昵称。
        /// </summary>
        public string displayName;

        /// <summary>
        /// 玩家等级。
        /// </summary>
        public int playerLevel;

        /// <summary>
        /// 当isAuth为1的时候，应用需要校验返回的参数鉴权签名。
        /// </summary>
        public int isAuth;

        /// <summary>
        /// 时间戳，用于鉴权签名校验。
        /// </summary>
        public string ts;

        /// <summary>
        /// 鉴权签名。
        /// </summary>
        public string gameAuthSign;

        /// <summary>
        /// 高清头像链接，假如没有设置则为空字符串。
        /// </summary>
        public string hiResImageUri;

        /// <summary>
        /// 头像链接，假如没有设置则为空字符串。
        /// </summary>
        public string imageUri;
    }

    [Preserve]
    public class LoginFailResult : CallbackBase
    {
        /// <summary>
        /// 异常时返回状态码。
        /// </summary>
        public int code;

        /// <summary>
        /// 异常时返回的消息内容。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class GameLoginOption : ActionParamBase<LoginSuccessResult, LoginFailResult>
    {
        /// <summary>
        /// 玩家未登录华为帐号或鉴权失败时，是否拉起登录场景。
        /// <para>0：表示如果玩家未登录华为帐号或鉴权失败，不会主动拉起帐号登录场景，适用于单机游戏的登录场景。</para>
        /// <para>1：表示如果玩家未登录华为帐号或鉴权失败，会主动拉起帐号登录场景，适用于网游的登录场景和单机游戏支付前强制登录场景。</para>
        /// </summary>
        public int forceLogin;

        /// <summary>
        /// 游戏appid，在华为开发者联盟上创建快游戏后分配的唯一标识。
        /// </summary>
        public string appid;
    }

    /// <summary>
    /// 创建激励视频广告请求参数。
    /// </summary>
    [Preserve]
    public class
        CreateRewardedVideoAdOption : ActionParamBase<CreateRewardedVideoAdSuccessResult,
            CreateRewardedVideoAdFailResult>
    {
        /// <summary>
        /// 激励视频广告位标识。
        /// </summary>
        public string adUnitId;
    }

    /// <summary>
    /// 创建激励视频广告回调结果。
    /// </summary>
    [Preserve]
    public class CreateRewardedVideoAdSuccessResult : CallbackBase
    {
        /// <summary>
        /// 返回码，0表示成功。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class CreateRewardedVideoAdFailResult : CallbackBase
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code;

        /// <summary>
        /// 异常时返回的消息内容。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class AdErrorResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class RewardedVideoAdCloseResult : CallbackBase
    {
        /// <summary>
        /// 视频是否是在用户完整观看的情况下被关闭的。
        /// <para>true 表示用户是在视频播放完以后关闭的。</para>
        /// <para>false 表示用户在视频播放过程中关闭的。</para>
        /// </summary>
        public bool isEnded;
    }

    [Preserve]
    public class IsEnvReadyOption : ActionParamBase<IsEnvReadySuccessResult, IsEnvReadyFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的游戏唯一标识。
        /// </summary>
        public string applicationID;
    }

    [Preserve]
    public class IsEnvReadySuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。若当前用户的华为帐号支持IAP支付，则返回值0。
        /// </summary>
        public int returnCode;
    }

    [Preserve]
    public class IsEnvReadyFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class
        IsSandboxActivatedOption : ActionParamBase<IsSandboxActivatedSuccessResult, IsSandboxActivatedFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的游戏唯一标识。
        /// </summary>
        public string applicationID;
    }

    [Preserve]
    public class IsSandboxActivatedSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 是否是沙盒帐号。
        /// </summary>
        public bool isSandboxUser;

        /// <summary>
        /// 快游戏相关客户端的APK版本，例如“快应用中心”、“花瓣轻游”等是否满足沙盒条件，固定返回true。
        /// </summary>
        public bool isSandboxApk;

        /// <summary>
        /// 快游戏相关客户端的版本信息。
        /// </summary>
        public string versionInApk;

        /// <summary>
        /// 快游戏在华为应用市场最新的版本信息。
        /// </summary>
        public string versionFrMarket;
    }

    [Preserve]
    public class IsSandboxActivatedFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class
        ObtainOwnedPurchasesOption : ActionParamBase<ObtainOwnedPurchasesSuccessResult, ObtainOwnedPurchasesFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的游戏唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 商品类型：
        /// 0：消耗型商品。
        /// 1：非消耗型商品。
        /// 2：订阅类商品。
        /// </summary>
        public int priceType;

        /// <summary>
        /// 开通应用内支付服务时的公钥。
        /// </summary>
        public string publicKey;

        /// <summary>
        /// 数据定位标志。首次请求时无需传入。
        /// </summary>
        public string continuationToken;
    }

    [Preserve]
    public class ObtainOwnedPurchasesSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 商品ID列表。
        /// </summary>
        public List<string> itemList;

        /// <summary>
        /// 商品信息详情,可参见InAppPurchaseData。
        /// </summary>
        public List<string> inAppPurchaseDataList;

        /// <summary>
        /// 与“inAppPurchaseDataList”中每条商品信息一一对应的签名字符串：
        /// <para>使用IAP私钥签名算法为“SHA256WITHRSA”。</para>
        /// <para>应用需对每条商品信息进行验签，若验签失败需重新获取商品信息。</para>
        /// </summary>
        public List<string> inAppSignature;

        /// <summary>
        /// 数据定位标志。用户拥有非常大的商品数量时，若返回continuationToken时，应用必须继续调用当前方法，并传入当前continuationToken值，直到调用后不再返回continuationToken。
        /// </summary>
        public string continuationToken;

        /// <summary>
        /// 订阅类商品切换时的订阅关系信息，可参见InAppPurchaseData。
        /// </summary>
        public List<string> placedInappPurchaseDataList;

        /// <summary>
        /// 与“placedInappPurchaseDataList”中每个订阅关系一一对应的签名字符串：
        /// <para>使用IAP私钥签名算法为“SHA256WITHRSA”。</para>
        /// <para>应用需要对每个订阅关系进行验签，若验签失败需重新获取订阅关系。</para>
        /// <para>仅在查询订阅类商品且用户存在订阅类商品时返回。</para>
        /// </summary>
        public List<string> placedInappSignatureList;
    }


    [Preserve]
    public class ObtainOwnedPurchasesFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    [Serializable]
    public class InAppPurchaseData
    {
        /// <summary>
        /// 应用ID。
        /// </summary>
        [Preserve] public long applicationId;

        /// <summary>
        /// 是否自动续订：
        /// <para>非订阅类商品返回false。</para>
        /// 订阅类商品：
        /// <para>true：处于订阅状态，且在下一个续费周期自动续订。</para>
        /// <para>false：订阅类商品已取消订阅。用户可在下个续费周期之前访问历史订阅内容。</para>
        /// </summary>
        [Preserve] public bool autoRenewing;

        /// <summary>
        /// 华为支付的订单ID，在成功支付后生成。
        /// </summary>
        [Preserve] public string orderId;

        /// <summary>
        /// 商品类型：
        /// 0 : 消耗型商品。
        /// 1 : 非消耗型商品。
        /// 2 : 订阅类商品。
        /// </summary>
        [Preserve] public int kind;

        /// <summary>
        /// 快游戏包名。
        /// </summary>
        [Preserve] public string packageName;

        /// <summary>
        /// 商品ID。
        /// </summary>
        [Preserve] public string productId;

        /// <summary>
        /// 商品名称。
        /// </summary>
        [Preserve] public string productName;

        /// <summary>
        /// 商品购买的时间戳。
        /// </summary>
        [Preserve] public long purchaseTime;

        /// <summary>
        /// 订单交易状态：
        /// -1：初始化。
        /// 0：已购买。
        /// 1：已取消。
        /// 2：已退款。
        /// </summary>
        [Preserve] public int purchaseState;

        /// <summary>
        /// 商户侧保留信息，调用接口时传入。
        /// </summary>
        [Preserve] public string developerPayload;

        /// <summary>
        /// 应用请求消耗商品时自定义的挑战字，唯一标识本次消耗请求。
        /// </summary>
        [Preserve] public string developerChallenge;

        /// <summary>
        /// 消费状态：
        /// 0：未消费。
        /// 1：已消费。
        /// </summary>
        [Preserve] public int consumptionState;

        /// <summary>
        /// 标识商品和用户对应关系的购买令牌。
        /// </summary>
        [Preserve] public string purchaseToken;

        /// <summary>
        /// 购买类型：
        /// 0：沙盒环境。
        /// 1：促销（暂不支持）。
        /// </summary>
        [Preserve] public int purchaseType;

        /// <summary>
        /// 支付商品的币种。
        /// </summary>
        [Preserve] public string currency;

        /// <summary>
        /// 商品展示价格。
        /// </summary>
        [Preserve] public long price;

        /// <summary>
        /// 国家/地区码。
        /// </summary>
        [Preserve] public string country;

        /// <summary>
        /// 支付方式：0：花币。3：信用卡。4：支付宝。6：话费。13：PayPal。16：借记卡。17：微信。19：礼品卡。20：零钱。21：花币卡。24：WP。31：华为Pay。32：花呗。200：MPESA。
        /// </summary>
        [Preserve] public string payType;

        #region 以下参数只有查询订阅类商品时才返回

        /// <summary>
        /// 续期订阅类商品的订单ID，由支付服务器在续期扣费时生成。
        /// </summary>
        [Preserve] public string lastOrderId;

        /// <summary>
        /// 订阅类商品所属订阅组ID。
        /// </summary>
        [Preserve] public string productGroup;

        /// <summary>
        /// 首次购买时间戳。
        /// </summary>
        [Preserve] public long oriPurchaseTime;

        /// <summary>
        /// 订阅ID。
        /// </summary>
        [Preserve] public string subscriptionId;

        /// <summary>
        /// 原订阅ID。返回该参数信息时，表示当前订阅从其他商品切换而来，该参数可关联商品切换前的订阅信息。
        /// </summary>
        [Preserve] public string oriSubscriptionId;

        /// <summary>
        /// 订阅类商品购买数量。
        /// </summary>
        [Preserve] public int quantity;

        /// <summary>
        /// 已付费订阅的天数，免费试用期限和优惠续费周期除外。
        /// </summary>
        [Preserve] public long daysLasted;

        /// <summary>
        /// 自动续费周期数，优惠续费周期除外。若值为0或不存在，表示还没有成功续期。
        /// </summary>
        [Preserve] public long numOfPeriods;

        /// <summary>
        /// 成功续费周期数。
        /// </summary>
        [Preserve] public long numOfDiscount;

        /// <summary>
        /// 订阅类商品过期的时间戳：
        /// <para>若是一个将来时间，表示是一个已成功续费订单的续期日期或超期日期。</para>
        /// <para>若是一个过去时间，表示订阅类商品已过期。</para>
        /// </summary>
        [Preserve] public long expirationDate;

        /// <summary>
        /// 订阅类商品已过期的原因：a:用户取消。b:商品不可用。c:用户签约信息异常。d:Billing错误。e:用户未同意涨价。f:未知错误。
        /// </summary>
        [Preserve] public int expirationIntent;

        /// <summary>
        /// 系统是否仍在尝试自动完成续期订阅类商品：
        /// 0：终止尝试。
        /// 1：仍在尝试。
        /// </summary>
        [Preserve] public int retryFlag;

        /// <summary>
        /// 订阅类商品是否在优惠续费周期内过期：
        /// 0：否。
        /// 1：是。
        /// </summary>
        [Preserve] public int introductoryFlag;

        /// <summary>
        /// 订阅类商品是否在免费试用期限内过期：
        /// 0：否。
        /// 1：是。
        /// </summary>
        [Preserve] public int trialFlag;

        /// <summary>
        /// 订阅取消时间。在用户通过客服取消订阅，或用户升级、跨级到同组其它商品且立即生效时，需取消原有订阅的上次收据。
        /// </summary>
        [Preserve] public long cancelTime;

        /// <summary>
        /// 用户取消订阅的原因：
        /// 2：用户升级、跨级等。
        /// 1：用户应用内遇到问题而取消订阅。
        /// 0：其它原因取消，例如用户不小心订阅了商品。
        /// </summary>
        [Preserve] public int cancelReason;

        /// <summary>
        /// 订阅类商品的续期状态：
        /// 1：当前周期到期后自动续期。
        /// 0：用户主动停止续期。应用可以给用户提供其它订阅选项，例如推荐同组更低级别的商品。
        /// </summary>
        [Preserve] public int renewStatus;

        /// <summary>
        /// 订阅类商品提价时的用户意见：
        /// 1：用户已经同意提价。
        /// 0：用户未采取动作，超期后订阅失效。
        /// </summary>
        [Preserve] public int priceConsentStatus;

        /// <summary>
        /// 下次续期价格。若有priceConsentStatus，提示用户新的续期价格。
        /// </summary>
        [Preserve] public long renewPrice;

        /// <summary>
        /// 订阅类商品提价后的状态：
        /// <para>true：已收费且未过期，也未退款，您可以为用户提供对应的服务。</para>
        /// <para>false：未购买，已过期，或购买后已退款。</para>
        /// </summary>
        [Preserve] public bool subIsvalid;

        /// <summary>
        /// 用户取消订阅后订阅关系保留的天数，不表示当前订阅类商品已取消。
        /// </summary>
        [Preserve] public int cancelledSubKeepDays;

        /// <summary>
        /// 重新订阅商品的恢复时间。
        /// </summary>
        [Preserve] public long resumeTime;

        #endregion
    }

    [Preserve]
    public class ObtainProductInfoOption : ActionParamBase<ObtainProductInfoSuccessResult, ObtainProductInfoFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 商品类型：
        /// 0：消耗型商品。
        /// 1：非消耗型商品。
        /// 2：订阅类商品。
        /// </summary>
        public int priceType;

        /// <summary>
        /// 商品ID列表。
        /// </summary>
        public List<string> productIds;
    }

    [Preserve]
    public class ObtainProductInfoSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 商品详细信息。
        /// </summary>
        public List<ProductInfo> productInfoList;
    }

    [Preserve]
    public class ObtainProductInfoFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    [Serializable]
    public class ProductInfo
    {
        /// <summary>
        /// 商品ID。
        /// </summary>
        [Preserve] public string productId;

        /// <summary>
        /// 商品类型：
        /// 0 : 消耗型商品。
        /// 1 : 非消耗型商品。
        /// 2 : 订阅类商品。
        /// </summary>
        [Preserve] public int priceType;

        /// <summary>
        /// 商品展示价格（含税）。格式为“币种 商品价格”，例如“EUR 0.15”；部分国家/地区格式为“货币符号 商品价格”，例如中国大陆“￥0.15”。
        /// </summary>
        [Preserve] public string price;

        /// <summary>
        /// 商品微单位价格。商品实际价格乘以1,000,000后的微单位价格。
        /// </summary>
        [Preserve] public long microsPrice;

        /// <summary>
        /// 支付商品的币种。
        /// </summary>
        [Preserve] public string currency;

        /// <summary>
        /// 商品名称。
        /// </summary>
        [Preserve] public string productName;

        /// <summary>
        /// 商品简介。
        /// </summary>
        [Preserve] public string productDesc;

        /// <summary>
        /// 订阅类商品的自动续费周期单位。
        /// </summary>
        [Preserve] public string subPeriod;

        /// <summary>
        /// 订阅类商品的优惠价格（不含税）。格式为“币种 商品价格”，例如“EUR 0.15”；部分国家/地区格式为“货币符号 商品价格”，例如中国大陆“￥0.15”。
        /// </summary>
        [Preserve] public string subSpecialPrice;

        /// <summary>
        /// 订阅类商品的优惠微单位价格。
        /// </summary>
        [Preserve] public long subSpecialPriceMicros;

        /// <summary>
        /// 订阅类商品的优惠周期单位。
        /// </summary>
        [Preserve] public string subSpecialPeriod;

        /// <summary>
        /// 订阅类商品的优惠续费周期。
        /// </summary>
        [Preserve] public string subSpecialPeriodCycles;

        /// <summary>
        /// 订阅类商品的免费试用期限。
        /// </summary>
        [Preserve] public string subsFreeTrialPeriod;

        /// <summary>
        /// 订阅类商品所属订阅组ID。
        /// </summary>
        [Preserve] public string subsGroupId;

        /// <summary>
        /// 订阅类商品所属订阅组名称。
        /// </summary>
        [Preserve] public string subsGroupTitle;

        /// <summary>
        /// 订阅类商品在订阅组的级别。
        /// </summary>
        [Preserve] public string subsProductLevel;

        /// <summary>
        /// 商品原价的微单位价格。
        /// </summary>
        [Preserve] public string originalMicroPrice;

        /// <summary>
        /// 商品原价（含税）。格式为“币种 商品价格”，例如“EUR 0.15”；部分国家/地区格式为“货币符号 商品价格”，例如中国大陆“￥0.15”。
        /// </summary>
        [Preserve] public string originalLocalPrice;

        /// <summary>
        /// 订阅类商品状态：
        /// 0：商品有效。
        /// 1：商品无法订阅，且无法续订。
        /// 3：商品下线，无法订阅，但支持老用户续订。
        /// </summary>
        [Preserve] public int status;
    }

    [Preserve]
    public class
        CreatePurchaseIntentOption : ActionParamBase<CreatePurchaseIntentSuccessResult, CreatePurchaseIntentFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 开通应用内支付服务时的公钥。
        /// </summary>
        public string publicKey;

        /// <summary>
        /// 待支付商品ID。
        /// </summary>
        public string productId;

        /// <summary>
        /// 商品类型：
        /// 0：消耗型商品。
        /// 1：非消耗型商品。
        /// 2：订阅类商品。
        /// </summary>
        public int priceType;

        /// <summary>
        /// 商户侧保留信息，长度限制为[0, 256]。若传入值，在成功支付后的回调结果中原样返回给应用。
        /// </summary>
        public string developerPayload;
    }

    [Preserve]
    public class CreatePurchaseIntentSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 使用私钥签名购买的签名字符串。
        /// </summary>
        public string inAppDataSignature;

        /// <summary>
        /// 购买订单详情信息，可参见InAppPurchaseData。
        /// </summary>
        public string inAppPurchaseData;
    }

    [Preserve]
    public class CreatePurchaseIntentFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    [Serializable]
    public class ConsumePurchaseData
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        [Preserve] public long applicationId;

        /// <summary>
        /// 是否自动续订：
        /// <para>非订阅类商品返回false。</para>
        /// 订阅类商品：
        /// <para>true：处于订阅状态，且在下一个续费周期自动续订。</para>
        /// <para>false：订阅类商品已取消订阅。用户可在下个续费周期之前访问历史订阅内容。</para>
        /// </summary>
        [Preserve] public bool autoRenewing;

        /// <summary>
        /// 华为支付的订单ID，在成功支付后生成。
        /// </summary>
        [Preserve] public string orderId;

        /// <summary>
        /// 快游戏包名。
        /// </summary>
        [Preserve] public string packageName;

        /// <summary>
        /// 商品ID。
        /// </summary>
        [Preserve] public string productId;

        /// <summary>
        /// 商品购买的时间戳。从1970年1月1日0时起到商品购买时间的毫秒数。
        /// </summary>
        [Preserve] public long purchaseTime;

        /// <summary>
        /// 订单交易状态：
        /// -1：初始化。
        /// 0：已购买。
        /// 1：已取消。
        /// 2：已退款。
        /// </summary>
        [Preserve] public int purchaseState;

        /// <summary>
        /// 商户侧保留信息，调用接口时传入。
        /// </summary>
        [Preserve] public string developerPayload;

        /// <summary>
        /// 用于唯一标识商品和用户对应关系的购买令牌，在支付完成时由华为支付服务器生成。
        /// </summary>
        [Preserve] public string purchaseToken;

        /// <summary>
        /// 应用请求消耗商品时自定义的挑战字，唯一标识本次消耗请求。
        ///<para>消耗成功后此挑战字会记录在购买信息中并返回。如果挑战字与已有挑战字重复，表示重复消耗，同样会原样返回该挑战字。</para>
        /// </summary>
        [Preserve] public string developerChallenge;

        /// <summary>
        /// 消费状态：0：未消费。1：已消费。
        /// </summary>
        [Preserve] public int consumptionState;

        /// <summary>
        /// 支付商品的币种。
        /// </summary>
        [Preserve] public string currency;

        /// <summary>
        /// 商品展示价格（含税）。格式为“币种 商品价格”，例如“EUR 0.15”；部分国家/地区格式为“货币符号 商品价格”，例如中国大陆“￥0.15”。
        /// </summary>
        [Preserve] public long price;

        /// <summary>
        /// 国家/地区码。中国大陆为CN，其他国家/地区请参见华为IAP范围覆盖。
        /// </summary>
        [Preserve] public string country;

        /// <summary>
        /// 返回码，成功返回为0。
        /// </summary>
        [Preserve] public string responseCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        [Preserve] public string responseMessage;
    }

    [Preserve]
    public class
        ConsumeOwnedPurchaseOption : ActionParamBase<ConsumeOwnedPurchaseSuccessResult, ConsumeOwnedPurchaseFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 应用请求消耗商品时自定义的挑战字。
        /// </summary>
        public string developerChallenge;

        /// <summary>
        /// 用户购买商品的标识。
        /// </summary>
        public string purchaseToken;

        /// <summary>
        /// 开通应用内支付服务时的公钥。
        /// </summary>
        public string publicKey;
    }

    [Preserve]
    public class ConsumeOwnedPurchaseSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 消耗结果信息，可参见ConsumePurchaseData。
        /// </summary>
        public string consumePurchaseData;

        /// <summary>
        /// 使用IAP私钥生成的签名字符串。
        /// </summary>
        public string dataSignature;
    }

    [Preserve]
    public class ConsumeOwnedPurchaseFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class ObtainOwnedPurchaseRecordOption : ActionParamBase<ObtainOwnedPurchaseRecordSuccessResult,
        ObtainOwnedPurchaseRecordFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 商品类型：
        /// 0：消耗型商品。
        /// 1：非消耗型商品。
        /// 2：订阅类商品。
        /// </summary>
        public int priceType;

        /// <summary>
        /// 开通应用内支付服务时的公钥。
        /// </summary>
        public string publicKey;

        /// <summary>
        /// 数据定位标志。首次请求时无需传入。
        /// </summary>
        public string continuationToken;
    }

    [Preserve]
    public class ObtainOwnedPurchaseRecordSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功返回码。
        /// </summary>
        public int returnCode;

        /// <summary>
        /// 返回码描述信息。
        /// </summary>
        public string errMsg;

        /// <summary>
        /// 已购买的商品ID列表。
        /// </summary>
        public List<string> itemList;

        /// <summary>
        /// 已购买的商品详情信息，可参见InAppPurchaseData。
        /// </summary>
        public List<string> inAppPurchaseDataList;

        /// <summary>
        /// 与“inAppPurchaseDataList”中每条商品信息一一对应的签名字符串：
        /// <para>使用IAP私钥签名算法为“SHA256WITHRSA”。</para>
        /// <para>应用需对每条商品信息进行验签，若验签失败需重新获取商品信息。</para>
        /// </summary>
        public List<string> inAppSignature;

        /// <summary>
        /// 数据定位标志。若返回continuationToken时，应用必须继续调用当前方法，并传入当前continuationToken值，直到调用后不再返回continuationToken。
        /// </summary>
        public string continuationToken;

        /// <summary>
        /// 商品切换后的商品详情信息，可参见InAppPurchaseData。
        /// </summary>
        public List<string> placedInappPurchaseDataList;

        /// <summary>
        /// 与“placedInappPurchaseDataList”中每个订阅关系一一对应的签名字符串：
        /// <para>仅在查询订阅类商品且用户存在订阅类商品时返回。</para>
        /// <para>使用IAP私钥签名算法为“SHA256WITHRSA”。</para>
        /// <para>应用需要对每个订阅关系进行验签，若验签失败需重新获取订阅关系。</para>
        /// </summary>
        public List<string> placedInappSignatureList;
    }

    [Preserve]
    public class ObtainOwnedPurchaseRecordFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class StartIapActivityOption : ActionParamBaseFail<StartIapActivityFailResult>
    {
        /// <summary>
        /// 创建快游戏后分配的唯一标识。
        /// </summary>
        public string applicationID;

        /// <summary>
        /// 跳转的页面类型：
        /// 2：华为管理订阅页。
        /// 3：华为编辑订阅页。
        /// </summary>
        public int type;

        /// <summary>
        /// 已订阅的商品ID。仅当跳转至华为编辑订阅页时传入参数值。
        /// </summary>
        public string subscribeProductId;
    }

    [Preserve]
    public class StartIapActivityFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string data;

        /// <summary>
        /// 错误码。
        /// </summary>
        public int code;
    }

    [Preserve]
    public class HideKeyboardOption : ActionParamBase
    {
    }

    [Preserve]
    public class ShowKeyboardOption : ActionParamBase
    {
        /// <summary>
        /// 键盘输入框显示的默认值。
        /// </summary>
        public string defaultValue;

        /// <summary>
        /// 键盘中文本的最大长度。
        /// </summary>
        public int maxLength;

        /// <summary>
        /// 是否为多行输入。
        /// </summary>
        public bool multiple;

        /// <summary>
        /// 当点击完成时键盘是否保持。
        /// </summary>
        public bool confirmHold;

        /// <summary>
        /// 键盘右下角confirm按钮的类型，只影响按钮的文本内容。
        /// </summary>
        public string confirmType;
    }

    [Preserve]
    public class UpdateKeyboardOption : ActionParamBase<UpdateKeyboardSuccessResult, UpdateKeyboardFailResult>
    {
        /// <summary>
        /// 键盘输入的当前值。
        /// </summary>
        public string value;
    }

    [Preserve]
    public class UpdateKeyboardSuccessResult : CallbackBase
    {
        /// <summary>
        /// 返回成功信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class UpdateKeyboardFailResult : CallbackBase
    {
        /// <summary>
        /// 返回失败信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class OnKeyboardInputResult : CallbackBase
    {
        /// <summary>
        /// 键盘输入的当前值。
        /// </summary>
        public string value;
    }

    [Preserve]
    public class OnKeyboardConfirmResult : CallbackBase
    {
        /// <summary>
        /// 键盘输入的当前值。
        /// </summary>
        public string value;
    }

    [Preserve]
    public class InnerAudioContextOption
    {
        /// <summary>
        /// 音频资源的地址，用于直接播放。可以设置为网络地址，或者unity中的本地路径如 Assets/xx.wav，运行时会自动和配置的音频地址前缀做拼接得到最终线上地址
        /// </summary>
        public string src = "";
        /// <summary>
        /// 是否循环播放,默认为 false
        /// </summary>
        public bool loop = false;
        /// <summary>
        /// 开始播放的位置（单位：s），默认为 0
        /// </summary>
        public float startTime = 0;
        /// <summary>
        /// 是否自动开始播放，默认为 false
        /// </summary>
        public bool autoplay = false;
        /// <summary>
        /// 音量。范围 0~1。默认为 1
        /// </summary>
        public float volume = 1;
        /// <summary>
        /// 是否遵循系统静音开关，默认为true。当此参数为false时，即使用户打开了静音开关，也能继续发出声音。
        /// </summary>
        public bool obeyMuteSwitch = true;
        /// <summary>
        /// 下载音频，设置为true后，会完全下载后再触发OnCanplay，方便后续音频复用，避免延迟，默认开启。
        /// </summary>
        public bool needDownload = true;
    }
    
    [Preserve]
    public class OnKeyboardCompleteResult : CallbackBase
    {
        /// <summary>
        /// 键盘输入的当前值。
        /// </summary>
        public string value;
    }

    [Preserve]
    public class InnerAudioContextErrorResult : CallbackBase
    {
        /// <summary>
        /// 音频播放错误事件的错误码。
        /// </summary>
        public int errCode;
    }

    [Preserve]
    public class GetClipboardDataOption : ActionParamBaseSuc<GetClipboardDataSuccessResult>
    {
    }

    [Preserve]
    public class GetClipboardDataSuccessResult : CallbackBase
    {
        /// <summary>
        /// 剪贴板的内容。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class SetClipboardDataOption : ActionParamBase
    {
        /// <summary>
        /// 剪贴板的内容。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class OnTouchStartCallbackResult
    {
        /// <summary> 
        /// 触发此次事件的触摸点列表
        /// </summary>
        public List<Touch> changedTouches;

        /// <summary> 
        /// 事件触发时的时间戳
        /// </summary>
        public long timeStamp;

        /// <summary> 
        /// 当前所有触摸点的列表
        /// </summary>
        public List<Touch> touches;
    }

    [Preserve]
    [Serializable]
    public class Touch
    {
        /// <summary> 
        /// 触点相对于可见视区左边沿的 X 坐标。
        /// </summary>
        [Preserve] public float clientX;

        /// <summary> 
        /// 触点相对于可见视区下边沿的 Y 坐标。
        /// </summary>
        [Preserve] public float clientY;

        /// <summary> 
        /// Touch 对象的唯一标识符，只读属性。一次触摸动作(我们值的是手指的触摸)在平面上移动的整个过程中, 该标识符不变。可以根据它来判断跟踪的是否是同一次触摸过程。
        /// </summary>
        [Preserve] public int identifier;

        /// <summary> 
        /// 触点相对于页面左边沿的 X 坐标。
        /// </summary>
        [Preserve] public float pageX;

        /// <summary> 
        /// 触点相对于页面下边沿的 Y 坐标。
        /// </summary>
        [Preserve] public float pageY;
    }

    [Preserve]
    public class GetSystemInfoOption : ActionParamBaseSuc<GetSystemInfoSuccessResult>
    {
    }

    [Preserve]
    public class GetSystemInfoSuccessResult : CallbackBase
    {
        /// <summary>
        /// 手机品牌。
        /// </summary>
        public string brand;

        /// <summary>
        /// 手机型号。
        /// </summary>
        public string model;

        /// <summary>
        /// 设备像素比。
        /// </summary>
        public double pixelRatio;

        /// <summary>
        /// 屏幕宽度。
        /// </summary>
        public double screenWidth;

        /// <summary>
        /// 屏幕高度。
        /// </summary>
        public double screenHeight;

        /// <summary>
        /// 可使用窗口宽度。
        /// </summary>
        public double windowWidth;

        /// <summary>
        /// 可使用窗口高度。
        /// </summary>
        public double windowHeight;

        /// <summary>
        /// 系统语言。
        /// </summary>
        public string language;

        /// <summary>
        /// 系统地区。
        /// </summary>
        public string region;

        /// <summary>
        /// 系统语言书写方式。
        /// </summary>
        public string script;

        /// <summary>
        /// 渲染引擎版本号。
        /// </summary>
        public string coreVersion;

        /// <summary>
        /// 渲染引擎版本号。
        /// </summary>
        public string COREVersion;

        /// <summary>
        /// 操作系统版本。
        /// </summary>
        public string system;

        /// <summary>
        /// 客户端平台。
        /// </summary>
        public string platform;

        /// <summary>
        /// 快应用中心版本。
        /// </summary>
        public string version;

        /// <summary>
        /// 状态栏高度。
        /// </summary>
        public double statusBarHeight;

        /// <summary>
        /// 运行平台版本名称。
        /// </summary>
        public string platformVersionName;

        /// <summary>
        /// 运行平台标准版本号。
        /// </summary>
        public int platformVersionCode;

        /// <summary>
        /// 安全区域，详情可参见SafeArea参数说明。
        /// </summary>
        public SafeArea safeArea;
    }

    [Preserve]
    [Serializable]
    public class SafeArea
    {
        /// <summary>
        /// 安全区域右下角纵坐标。
        /// </summary>
        [Preserve] public double bottom;

        /// <summary>
        /// 安全区域左上角横坐标。
        /// </summary>
        [Preserve] public double left;

        /// <summary>
        /// 安全区域右下角横坐标。
        /// </summary>
        [Preserve] public double right;

        /// <summary>
        /// 安全区域左上角纵坐标。
        /// </summary>
        [Preserve] public double top;

        /// <summary>
        /// 安全区域的高度，大小为实际高度（以px为单位）/设备屏幕密度。
        /// </summary>
        [Preserve] public double height;

        /// <summary>
        /// 安全区域的宽度，大小为实际宽度（以px为单位）/设备屏幕密度。
        /// </summary>
        [Preserve] public double width;
    }

    [Preserve]
    public class OnErrorResult : CallbackBase
    {
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string message;

        /// <summary>
        /// 错误调用堆栈。
        /// </summary>
        public string stack;
    }

    [Preserve]
    public class ExitApplicationOption : ActionParamBase
    {
    }

    [Preserve]
    public class GetLaunchOptionsSyncResult
    {
        // /// <summary>
        // /// 启动快游戏时携带的参数，query。
        // /// </summary>
        [Preserve] public string query;

        /// <summary>
        /// 来源信息。从另一个快游戏、桌面图标等方式启动，详情请参见ReferrerInfo。
        /// </summary>
        [Preserve] public ReferrerInfo referrerInfo;
    }

    [Preserve]
    [Serializable]
    public class ReferrerInfo
    {
        /// <summary>
        /// 来源类型：shortcut：桌面图标。url：网页。app：原生应用。quickapp：快应用或快游戏。deeplink：链接。other：其它。
        /// </summary>
        [Preserve] public string type;

        /// <summary>
        /// 若来源类型为app或quickapp时，通过packageName标识调用方的包名信息。
        /// </summary>
        [Preserve] public ExtraData extraData;
    }

    [Preserve]
    [Serializable]
    public class ExtraData
    {
        /// <summary>
        /// 调用方的包名信息。
        /// </summary>
        [Preserve] public string packageName;
    }

    [Preserve]
    public class NavigateToQuickAppOption<T> : ActionParamBase
    {
        /// <summary>
        /// 快游戏包名。
        /// </summary>
        public string packageName;

        /// <summary>
        /// 传递给快游戏的数据，可在GetLaunchOptionsSync()获取extraData传递的数据。
        /// </summary>
        public T extraData;
    }

    [Preserve]
    public class OpenDeeplinkOption : ActionParamBase
    {
        /// <summary>
        /// 支持跳转网页http(s)，例如https://developer.huawei.com/consumer/cn/。http(s)默认使用内置webview加载网页，若默认策略不能处理，将尝试使用系统中的应用来处理请求，若没有系统应用，将抛弃请求。
        /// </summary>
        public string uri;
    }

    [Preserve]
    public class
        HasShortcutInstalledOption : ActionParamBase<HasShortcutInstalledSuccessResult, HasShortcutInstalledFailResult>
    {
    }

    [Preserve]
    public class HasShortcutInstalledSuccessResult : CallbackBase
    {
        /// <summary>
        /// 是否有快游戏的桌面图标。
        /// true：已创建。
        /// false：未创建。
        /// </summary>
        public bool hasShortcut;
    }

    [Preserve]
    public class HasShortcutInstalledFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string errorMsg;
    }

    [Preserve]
    public class InstallShortcutOption : ActionParamBase<InstallShortcutSuccessResult, InstallShortcutFailResult>
    {
        /// <summary>
        /// 提示语。
        /// </summary>
        public string message;
    }

    [Preserve]
    public class InstallShortcutSuccessResult : CallbackBase
    {
        /// <summary>
        /// 成功回调提示语。
        /// </summary>
        public string message;
    }

    [Preserve]
    public class InstallShortcutFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码描述信息。
        /// </summary>
        public string erromsg;

        /// <summary>
        /// 错误码
        /// </summary>
        public string errocode;
    }

    [Preserve]
    public class DownloadFileOption : ActionParamBase<DownloadFileSuccessResult, DownloadFileFailResult>
    {
        /// <summary>
        /// 下载资源的url。
        /// </summary>
        public string url;

        /// <summary>
        /// HTTP请求的Header。
        /// </summary>
        public List<QgKeyValuePair> header;

        /// <summary>
        /// 指定文件下载后存储的路径+文件名。
        /// 如果没有设置filePath：调用完成后生成一个临时路径，临时路径可以在success回调中通过res.tempFilePath获取。
        /// 如果设置filePath：返回路径为设置的路径，且目录必须是`${qg.env.USER_DATA_PATH}/`手机上的用户个人目录，如qg.env.USER_DATA_PATH+ "/sdcard/Pictures/Screenshots/test.zip"，如果设置的路径已存在，下载的内容将覆盖原路径的内容。
        /// </summary>
        public string filePath;
    }
    
    [Preserve]
    [Serializable]
    public class QgKeyValuePair
    {
        /// <summary>
        /// 键值对的key
        /// </summary>
        public string key;
        
        /// <summary>
        /// 键值对的value
        /// </summary>
        public string value;
    }
    
    [Preserve]
    public class DownloadFileSuccessResult : CallbackBase
    {
        /// <summary>
        /// 临时文件路径 (本地路径)。没传入filePath指定文件存储路径时会返回，下载后的文件会存储到一个临时文件。
        /// </summary>
        public string tempFilePath;

        /// <summary>
        /// 用户文件路径 (本地路径)。传入filePath时会返回，跟传入的filePath一致。
        /// </summary>
        public string filePath;

        /// <summary>
        /// 开发者服务器返回的HTTP状态码。
        /// </summary>
        public int statusCode;
    }

    [Preserve]
    public class DownloadFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class DownloadTaskOnProgressUpdateResult : CallbackBase
    {
        /// <summary>
        /// 下载进度百分比。
        /// </summary>
        public int progress;
        
        /// <summary>
        /// 已经下载的数据长度，单位Bytes。
        /// </summary>
        public long totalBytesWritten;
        
        /// <summary>
        /// 预期需要下载的数据总长度，单位Bytes。
        /// </summary>
        public long totalBytesExpectedToWrite;
    }
    
    [Preserve]
    public class FileSystemBaseResult
    {
        /// <summary>
        /// 判断调用是否成功。
        /// </summary>
        public bool isSuccess;
        
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class AccessOption : ActionParamBaseFail<AccessFailResult>
    {
        /// <summary>
        /// 要判断是否存在的文件/目录路径。
        /// </summary>
        public string path;
    }

    [Preserve]
    public class AccessFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class CopyFileOption : ActionParamBaseFail<CopyFileFailResult>
    {
        /// <summary>
        /// 源文件路径，只可以是本地文件，如果非本地文件，需调用FileSystemManager.saveFile接口将文件保存到本地。
        /// </summary>
        public string srcPath;

        /// <summary>
        /// 目标文件路径。
        /// </summary>
        public string destPath;
    }

    [Preserve]
    public class CopyFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class MkdirOption : ActionParamBaseFail<MkdirFailResult>
    {
        /// <summary>
        /// 创建的目录路径。
        /// </summary>
        public string dirPath;

        /// <summary>
        /// 是否在递归创建该目录的上级目录后再创建该目录。如果对应的上级目录已经存在，则不创建该上级目录。默认值为false。
        /// </summary>
        public bool recursive;
    }

    [Preserve]
    public class MkdirFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class RmdirOption : ActionParamBaseFail<RmdirFailResult>
    {
        /// <summary>
        /// 要删除的目录路径。
        /// </summary>
        public string dirPath;

        /// <summary>
        /// 是否递归删除目录。如果为 true，则删除该目录和该目录下的所有子目录以及文件。默认值为false。
        /// </summary>
        public bool recursive;
    }

    [Preserve]
    public class RmdirFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class ReaddirOption : ActionParamBase<ReaddirSuccessResult, ReaddirFailResult>
    {
        /// <summary>
        /// 要读取的目录路径。
        /// </summary>
        public string dirPath;
    }

    [Preserve]
    public class ReaddirSuccessResult : CallbackBase
    {
        /// <summary>
        /// 指定目录下的文件名数组。
        /// </summary>
        public List<string> files;
    }

    [Preserve]
    public class ReaddirFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class ReaddirSyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 指定目录下的文件名数组。
        /// </summary>
        public List<string> files;
    }

    [Preserve]
    public class ReadFileStringOption: ActionParamBase<ReadFileStringSuccessResult, ReadFileFailResult>
    {
        /// <summary>
        /// 要读取的文件的路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class ReadFileBinaryOption: ActionParamBase<ReadFileBinarySuccessResult, ReadFileFailResult>
    {
        /// <summary>
        /// 要读取的文件的路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class ReadFileStringSuccessResult : CallbackBase
    {
        /// <summary>
        /// 文件内容。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class ReadFileBinarySuccessResult : CallbackBase
    {
        /// <summary>
        /// 文件内容。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class ReadFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class ReadFileStringSyncOption : ReadFileStringSyncResult
    {
        /// <summary>
        /// 要读取的文件的路径。
        /// </summary>
        public string filePath;
    }
    
    [Preserve]
    public class ReadFileStringSyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 文件内容。
        /// </summary>
        public string data;
    }
    
    [Preserve]
    public class ReadFileBinarySyncOption : ReadFileBinarySyncResult
    {
        /// <summary>
        /// 要读取的文件的路径。
        /// </summary>
        public string filePath;
    }
    
    [Preserve]
    public class ReadFileBinarySyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 文件内容。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class RenameOption : ActionParamBaseFail<RenameFailResult>
    {
        /// <summary>
        /// 源文件路径，可以是普通文件或目录。
        /// </summary>
        public string oldPath;

        /// <summary>
        /// 新文件路径。
        /// </summary>
        public string newPath;
    }

    [Preserve]
    public class RenameFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class StatStatsOption : ActionParamBase<StatSuccessStatsResult, StatFailResult>
    {
        /// <summary>
        /// 文件/目录路径。
        /// </summary>
        public string path;

        /// <summary>
        /// 是否递归获取目录下的每个文件的stats信息。默认值为false。
        /// </summary>
        public bool recursive;
    }

    [Preserve]
    public class StatFileStatsOption : ActionParamBase<StatSuccessFileStatsResult, StatFailResult>
    {
        /// <summary>
        /// 文件/目录路径。
        /// </summary>
        public string path;

        /// <summary>
        /// 是否递归获取目录下的每个文件的stats信息。默认值为false。
        /// </summary>
        public bool recursive;
    }

    [Preserve]
    public class StatSuccessStatsResult : CallbackBase
    {
        /// <summary>
        /// 当recursive为false时，返回一个Stats对象。
        /// </summary>
        public Stats stat;
    }

    [Preserve]
    public class StatSuccessFileStatsResult : CallbackBase
    {
        /// <summary>
        /// 当recursive 为true，且path是一个目录的路径时，res.stats 是一个Array，数组的每一项是一个对象，每个对象包含path和stats。
        /// </summary>
        public List<FileStats> stats;
    }
    
    [Preserve]
    public class StatStatsSyncOption
    {
        /// <summary>
        /// 文件/目录路径。
        /// </summary>
        public string path;
    }
    
    [Preserve]
    public class StatStatsSyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 当recursive为false时，返回一个Stats对象。
        /// </summary>
        public Stats stat;
    }
    
    [Preserve]
    public class StatFileStatsSyncOption
    {
        /// <summary>
        /// 文件/目录路径。
        /// </summary>
        public string path;
    }
    
    [Preserve]
    public class StatFileStatsSyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 当recursive 为true，且path是一个目录的路径时，res.stats 是一个Array，数组的每一项是一个对象，每个对象包含path和stats。
        /// </summary>
        public List<FileStats> stats;
    }

    [Preserve]
    [Serializable]
    public class Stats
    {
        /// <summary>
        /// 文件的类型和存取的权限，对应POSIX stat.st_mode。
        /// </summary>
        [Preserve] public string mode;

        /// <summary>
        /// 文件大小，单位：B，对应POSIX stat.st_size。
        /// </summary>
        [Preserve] public int size;

        /// <summary>
        /// 文件最近一次被存取或被执行的时间，UNIX时间戳，对应POSIX stat.st_atime。
        /// </summary>
        [Preserve] public long lastAccessedTime;

        /// <summary>
        /// 文件最后一次被修改的时间，UNIX时间戳，对应POSIX stat.st_mtime。
        /// </summary>
        [Preserve] public long lastModifiedTime;
    }

    [Preserve]
    [Serializable]
    public class FileStats
    {
        /// <summary>
        /// 文件/目录路径。
        /// </summary>
        [Preserve] public string path;

        /// <summary>
        /// Stats 对象，即描述文件状态的对象。
        /// </summary>
        [Preserve] public Stats stats;
    }

    [Preserve]
    public class StatFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class UnlinkOption : ActionParamBaseFail<UnlinkFailResult>
    {
        /// <summary>
        /// 要删除的文件路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class UnlinkFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class UnzipOption : ActionParamBaseFail<UnzipFailResult>
    {
        /// <summary>
        /// 源文件路径，只可以是 zip 压缩文件。
        /// </summary>
        public string zipFilePath;

        /// <summary>
        /// 目标目录路径。
        /// </summary>
        public string targetPath;
    }

    [Preserve]
    public class UnzipFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class WriteFileStringOption : ActionParamBaseFail<WriteFileFailResult>
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;

        /// <summary>
        /// 要写入的文本数据。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class WriteFileBinaryOption : ActionParamBaseFail<WriteFileFailResult>
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;

        /// <summary>
        /// 要写入的二进制数据。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class WriteFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class WriteFileStringSyncOption
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;
        
        /// <summary>
        /// 要写入的文本数据。
        /// </summary>
        public string data;
    }
    
    [Preserve]
    public class WriteFileBinarySyncOption
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;
        
        /// <summary>
        /// 要写入的二进制数据。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class SaveFileOption : ActionParamBase<SaveFileSuccessResult, SaveFileFailResult>
    {
        /// <summary>
        /// 临时存储文件路径。
        /// </summary>
        public string tempFilePath;

        /// <summary>
        /// 要存储的文件路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class SaveFileSuccessResult : CallbackBase
    {
        /// <summary>
        /// 存储后的文件路径。
        /// </summary>
        public string savedFilePath;
    }

    [Preserve]
    public class SaveFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class SaveFileSyncResult : FileSystemBaseResult
    {
        /// <summary>
        /// 存储后的文件路径。
        /// </summary>
        public string savedFilePath;
    }

    [Preserve]
    public class AppendFileStringOption : ActionParamBaseFail<AppendFileFailResult>
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;

        /// <summary>
        /// 要写入的文本数据。
        /// </summary>
        public string data;
    }

    [Preserve]
    public class AppendFileBinaryOption : ActionParamBaseFail<AppendFileFailResult>
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;

        /// <summary>
        /// 要写入的二进制数据。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class AppendFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
    
    [Preserve]
    public class AppendFileStringSyncOption
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;
        
        /// <summary>
        /// 要写入的文本数据。
        /// </summary>
        public string data;
    }
    
    [Preserve]
    public class AppendFileBinarySyncOption
    {
        /// <summary>
        /// 要写入的文件路径。
        /// </summary>
        public string filePath;
        
        /// <summary>
        /// 要写入的二进制数据。
        /// </summary>
        public byte[] data;
    }

    [Preserve]
    public class GetFileInfoOption : ActionParamBase<GetFileInfoSuccessResult, GetFileInfoFailResult>
    {
        /// <summary>
        /// 要读取的文件路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class GetFileInfoSuccessResult : CallbackBase
    {
        /// <summary>
        /// 文件大小，以字节为单位。
        /// </summary>
        public int size;
    }

    [Preserve]
    public class GetFileInfoFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }

    [Preserve]
    public class RemoveSavedFileOption : ActionParamBaseFail<RemoveSavedFileFailResult>
    {
        /// <summary>
        /// 要删除的文件路径。
        /// </summary>
        public string filePath;
    }

    [Preserve]
    public class RemoveSavedFileFailResult : CallbackBase
    {
        /// <summary>
        /// 错误码。
        /// </summary>
        public int errCode;

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errMsg;
    }
}