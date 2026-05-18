using System;
using UnityEngine;

namespace HWWASM
{
    /// <summary>
    /// 华为快游戏对外暴露的API。
    /// </summary>
    public class QG
    {
        /// <summary>
        /// 游戏登录。
        /// [qg.gameLogin(Object object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-account-0000001083874630#section73325514166)
        /// </summary>
        /// <param name="gameLoginOption">登录参数</param>
        public static void GameLogin(GameLoginOption gameLoginOption)
        {
            QGSDKManagerHandler.Instance.GameLogin(gameLoginOption);
        }

        /// <summary>
        /// 创建激励视频广告。
        /// [qg.createRewardedVideoAd(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-ad-0000001130711971#section9772146486)
        /// </summary>
        /// <param name="createRewardedVideoAdOption">创建激励视频广告参数</param>
        /// <returns>激励视频广告对象</returns>
        public static RewardedVideoAd CreateRewardedVideoAd(CreateRewardedVideoAdOption createRewardedVideoAdOption)
        {
            return QGSDKManagerHandler.Instance.CreateRewardedVideoAd(createRewardedVideoAdOption);
        }

        /// <summary>
        /// 判断当前华为帐号所属国家或地区是否支持华为IAP支付。
        /// [qg.isEnvReady(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section1623962461015)
        /// </summary>
        /// <param name="isEnvReadyOption">参数</param>
        public static void IsEnvReady(IsEnvReadyOption isEnvReadyOption)
        {
            QGSDKManagerHandler.Instance.IsEnvReady(isEnvReadyOption);
        }

        /// <summary>
        /// 判断华为帐号和快游戏RPK版本是否满足沙盒条件。
        /// [qg.isSandboxActivated(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section181371925102211)
        /// </summary>
        /// <param name="isSandboxActivatedOption">参数</param>
        public static void IsSandboxActivated(IsSandboxActivatedOption isSandboxActivatedOption)
        {
            QGSDKManagerHandler.Instance.IsSandboxActivated(isSandboxActivatedOption);
        }

        /// <summary>
        /// 查询用户的已购买数据，包括消耗型商品、非消耗型商品和订阅类商品、且一次请求只能查询一种类型的商品。
        /// [qg.obtainOwnedPurchases(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section3284913305)
        /// </summary>
        /// <param name="obtainOwnedPurchasesOption">参数</param>
        public static void ObtainOwnedPurchases(ObtainOwnedPurchasesOption obtainOwnedPurchasesOption)
        {
            QGSDKManagerHandler.Instance.ObtainOwnedPurchases(obtainOwnedPurchasesOption);
        }

        /// <summary>
        /// 查询在AGC控制台配置的商品详情。
        /// <para>若您使用了华为PMS系统进行商品定价，可通过此接口从华为PMS获取商品详情，以保证和华为支付收银台显示的商品信息一致，避免出现应用内商品价格与华为支付收银台价格不一致的现象。</para>
        /// [qg.obtainProductInfo(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section125017344614)
        /// </summary>
        /// <param name="obtainProductInfoOption">参数</param>
        public static void ObtainProductInfo(ObtainProductInfoOption obtainProductInfoOption)
        {
            QGSDKManagerHandler.Instance.ObtainProductInfo(obtainProductInfoOption);
        }

        /// <summary>
        /// 创建在AGC控制台配置的商品购买订单，支持购买消耗型商品、非消耗型商品、订阅类商品。
        /// <para>您在AGC控制台创建商品后，使用此接口调用华为支付收银台，显示商品、价格和支付方式。华为会根据国际汇率变动进行商品价格档调整，为了保证应用内商品价格显示一致，请您同时使用obtainProductInfo接口获取商品详情，避免从自有服务端获取价格，以免在汇率变动价格调整时，导致价格显示不一致。</para>
        /// [qg.createPurchaseIntent(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section416683091)
        /// </summary>
        /// <param name="createPurchaseIntentOption">参数</param>
        public static void CreatePurchaseIntent(CreatePurchaseIntentOption createPurchaseIntentOption)
        {
            QGSDKManagerHandler.Instance.CreatePurchaseIntent(createPurchaseIntentOption);
        }

        /// <summary>
        /// 消耗已支付成功的消耗型商品。
        /// <para>在商品支付成功后，应用需要在发放商品成功之后调用此接口对消耗型商品执行消耗操作。您需把已发货的购买Token传至您的服务器，后续即使消耗失败也可以从您的服务器拉取数据进行对比，避免出现异常。</para>
        /// [qg.consumeOwnedPurchase(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section2946161093810)
        /// </summary>
        /// <param name="consumeOwnedPurchaseOption">参数</param>
        public static void ConsumeOwnedPurchase(ConsumeOwnedPurchaseOption consumeOwnedPurchaseOption)
        {
            QGSDKManagerHandler.Instance.ConsumeOwnedPurchase(consumeOwnedPurchaseOption);
        }

        /// <summary>
        /// 查询消耗型商品的历史消耗商品信息或订阅类商品的订阅收据。
        /// <para>若是消耗型商品，此接口返回商品列表中执行过发货和消耗操作的商品信息。</para>
        /// <para>若是订阅类商品，此接口返回在应用中所有的订阅收据。</para>
        /// <para>此接口不会返回非消耗型商品的购买信息，您需使用obtainOwnedPurchases接口获取非消耗型商品的购买记录。</para>
        /// [qg.obtainOwnedPurchaseRecord(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section16230152592811)
        /// </summary>
        /// <param name="obtainOwnedPurchaseRecordOption">参数</param>
        public static void ObtainOwnedPurchaseRecord(ObtainOwnedPurchaseRecordOption obtainOwnedPurchaseRecordOption)
        {
            QGSDKManagerHandler.Instance.ObtainOwnedPurchaseRecord(obtainOwnedPurchaseRecordOption);
        }

        /// <summary>
        /// 打开华为应用内支付相关页面。
        /// <para>可跳转至华为编辑订阅页。</para>
        /// <para>可跳转至华为管理订阅页。</para>
        /// [qg.startIapActivity(object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-payment-0000001453629405#section88391175564)
        /// </summary>
        /// <param name="startIapActivityOption">参数</param>
        public static void StartIapActivity(StartIapActivityOption startIapActivityOption)
        {
            QGSDKManagerHandler.Instance.StartIapActivity(startIapActivityOption);
        }

        /// <summary>
        /// 隐藏软键盘。
        /// [qg.hideKeyboard(Object object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section169301539475)
        /// </summary>
        /// <param name="hideKeyboardOption">参数</param>
        public static void HideKeyboard(HideKeyboardOption hideKeyboardOption)
        {
            QGSDKManagerHandler.Instance.HideKeyboard(hideKeyboardOption);
        }

        /// <summary>
        /// 显示软键盘。
        /// [qg.showKeyboard(Object object)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section816150185312)
        /// </summary>
        /// <param name="showKeyboardOption">参数</param>
        public static void ShowKeyboard(ShowKeyboardOption showKeyboardOption)
        {
            QGSDKManagerHandler.Instance.ShowKeyboard(showKeyboardOption);
        }

        /// <summary>
        /// 更新键盘输入框显示的默认值。
        /// [qg.updateKeyboard（Object object）](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section15453121412595)
        /// </summary>
        /// <param name="updateKeyboardOption">参数</param>
        public static void UpdateKeyboard(UpdateKeyboardOption updateKeyboardOption)
        {
            QGSDKManagerHandler.Instance.UpdateKeyboard(updateKeyboardOption);
        }

        /// <summary>
        /// 监听键盘输入事件。
        /// [qg.onKeyboardInput(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section03051834550)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnKeyboardInput(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OnKeyboardInput(action);
        }

        /// <summary>
        /// 取消监听键盘输入事件。
        /// [qg.offKeyboardInput(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section154071347574)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffKeyboardInput(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OffKeyboardInput(action);
        }

        /// <summary>
        /// 监听用户点击键盘 confirm 按钮时的事件。
        /// [qg.onKeyboardConfirm(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section392018564012)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnKeyboardConfirm(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OnKeyboardConfirm(action);
        }

        /// <summary>
        /// 取消监听用户点击键盘 confirm 按钮时的事件。
        /// [qg.offKeyboardConfirm(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section66591019613)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffKeyboardConfirm(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OffKeyboardConfirm(action);
        }

        /// <summary>
        /// 监听键盘收起的事件。
        /// [qg.onKeyboardComplete(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section6651746869)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnKeyboardComplete(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OnKeyboardComplete(action);
        }

        /// <summary>
        /// 取消监听键盘收起的事件。
        /// [qg.offKeyboardComplete(function callback)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-keyboard-0000001130573699#section569918122086)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffKeyboardComplete(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OffKeyboardComplete(action);
        }

        public class LocalStorage
        {
            /// <summary>
            /// length是一个变量属性，不是函数，用于获取存储在 localStorage 对象中的数据项数量（只读）。
            /// </summary>
            public static int Length => QGSDKManagerHandler.Instance.LocalStorageLength();

            /// <summary>
            /// 清空localStorage中的数据。
            /// [localStorage.clear()](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-localstorage-0000001130711973#section877451974)
            /// </summary>
            public static void Clear()
            {
                QGSDKManagerHandler.Instance.LocalStorageClear();
            }

            /// <summary>
            /// 根据key删除单条存在localStorage中的数据。
            /// [localStorage.removeItem(string key)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-localstorage-0000001130711973#section1292932920814)
            /// </summary>
            /// <param name="key">要删除的数据的Key值。</param>
            public static void RemoveItem(string key)
            {
                QGSDKManagerHandler.Instance.LocalStorageRemoveItem(key);
            }

            /// <summary>
            /// 保存数据到localStorage。
            /// [localStorage.setItem(string key,string value)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-localstorage-0000001130711973#section214323414912)
            /// </summary>
            /// <param name="key">要存到localStorage中的数据的key。</param>
            /// <param name="value">要存到localStorage中的数据的值。</param>
            public static void SetItem(string key, string value)
            {
                QGSDKManagerHandler.Instance.LocalStorageSetItem(key, value);
            }

            /// <summary>
            /// 根据key查询单条存在localStorage中的数据。
            /// [localStorage.getItem(string key)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-localstorage-0000001130711973#section731992621014)
            /// </summary>
            /// <param name="key">存在localStorage中数据的key值。</param>
            /// <returns>存在localStorage中数据的key的对应的值</returns>
            public static string GetItem(string key)
            {
                return QGSDKManagerHandler.Instance.LocalStorageGetItem(key);
            }

            /// <summary>
            /// 根据index下标查询对应的key数据。
            /// [localStorage.key(number index)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-localstorage-0000001130711973#section711155914611)
            /// </summary>
            /// <param name="index">要获取数据的下标。</param>
            /// <returns>要获取数据的下标的对应值</returns>
            public static string Key(int index)
            {
                return QGSDKManagerHandler.Instance.LocalStorageKey(index);
            }
        }

        /// <summary>
        /// 修改渲染帧率。
        /// [qg.setPreferredFramesPerSecond(number fps)](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-frame-0000001130711965#section15639134185411)
        /// </summary>
        /// <param name="fps">有效值范围 1 ~ 60。</param>
        public static void SetPreferredFramesPerSecond(int fps)
        {
            QGSDKManagerHandler.Instance.SetPreferredFramesPerSecond(fps);
        }

        #region 音频
        /// <summary>
        /// 获取一个音频对象用于播放音频，播放的时候会返回一个audioID，可以通过此audioID操作对应的音频对象。每次调用create会创建一个新的实例对象，目前最多同时支持5个播放音频的对象。
        /// [qg.createInnerAudioContext()](https://developer.huawei.com/consumer/cn/doc/development/quickApp-References/quickgame-api-audio-0000001083874634#section855144817217)
        /// </summary>
        /// <returns>音频对象</returns>
        public static InnerAudioContext CreateInnerAudioContext(InnerAudioContextOption param = null)
        {
            return QGSDKManagerHandler.Instance.CreateInnerAudioContext(param);
        }

        /// <summary>
        /// 音频为网络请求，可能会有延迟，所以可以先调用这个接口预先下载音频缓存到本地，避免延迟
        /// </summary>
        /// <param name="pathList">音频的地址数组,如 { "www.xxx.com/0.wav", "www.xxx.com/1.wav" } </param>
        /// <param name="action">全部音频下载完成后回调，返回码为0表示下载完成</param>
        public static void PreDownloadAudios(string[] pathList, Action<int> action)
        {
            if (pathList == null || pathList.Length == 0)
            {
                Debug.LogError("pathList is null or length is 0");
                action.Invoke(-1);
                return;
            }
            QGSDKManagerHandler.Instance.PreDownloadAudios(pathList, action);
        }
        #endregion

        /// <summary>
        /// 获取CacheManager对象
        /// </summary>
        /// <returns></returns>
        public static CacheManager GetCacheManager()
        {
            return QGSDKManagerHandler.Instance.GetCacheManager();
        }

        /// <summary>
        /// 获取系统剪贴板的内容。
        /// [qg.getClipboardData(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-clipboard-0000001084194210#section18307358141320)
        /// </summary>
        /// <param name="getClipboardDataOption">参数</param>
        public static void GetClipboardData(GetClipboardDataOption getClipboardDataOption)
        {
            QGSDKManagerHandler.Instance.GetClipboardData(getClipboardDataOption);
        }

        /// <summary>
        /// 设置系统剪贴板的内容。
        /// [qg.setClipboardData(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-clipboard-0000001084194210#section1710725510154)
        /// </summary>
        /// <param name="setClipboardDataOption">参数</param>
        public static void SetClipboardData(SetClipboardDataOption setClipboardDataOption)
        {
            QGSDKManagerHandler.Instance.SetClipboardData(setClipboardDataOption);
        }

        /// <summary>
        /// 监听开始触摸事件
        /// </summary>
        /// <param name="result">参数</param>
        public static void OnTouchStart(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OnTouchStart(result);
        }

        /// <summary>
        /// 移除开始触摸事件的监听函数
        /// </summary>
        /// <param name="result">参数</param>
        public static void OffTouchStart(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OffTouchStart(result);
        }

        /// <summary>
        /// 监听触点移动事件
        /// </summary>
        /// <param name="result">参数</param>
        public static void OnTouchMove(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OnTouchMove(result);
        }

        /// <summary>
        /// 移除开始触摸事件的监听函数
        /// </summary>
        /// <param name="result">参数</param>
        public static void OffTouchMove(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OffTouchMove(result);
        }

        /// <summary>
        /// 监听触摸结束事件
        /// </summary>
        /// <param name="result">参数</param>
        public static void OnTouchEnd(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OnTouchEnd(result);
        }

        /// <summary>
        /// 移除触摸结束事件的监听函数
        /// </summary>
        /// <param name="result">参数</param>
        public static void OffTouchEnd(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OffTouchEnd(result);
        }

        /// <summary>
        /// 监听触点失效事件
        /// </summary>
        /// <param name="result">参数</param>
        public static void OnTouchCancel(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OnTouchCancel(result);
        }

        /// <summary>
        /// 移除触点失效事件的监听函数
        /// </summary>
        /// <param name="result">参数</param>
        public static void OffTouchCancel(Action<OnTouchStartCallbackResult> result)
        {
            QGSDKManagerHandler.Instance.OffTouchCancel(result);
        }

        /// <summary>
        /// 获取系统信息。
        /// [qg.getSystemInfo(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-sysinfo-0000001083874626#section16231152791216)
        /// </summary>
        /// <param name="getSystemInfoOption">参数</param>
        public static void GetSystemInfo(GetSystemInfoOption getSystemInfoOption)
        {
            QGSDKManagerHandler.Instance.GetSystemInfo(getSystemInfoOption);
        }

        /// <summary>
        /// 获取系统信息（同步方法）。
        /// [qg.getSystemInfoSync(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-sysinfo-0000001083874626#section14651153111516)
        /// </summary>
        public static GetSystemInfoSuccessResult GetSystemInfoSync()
        {
            return QGSDKManagerHandler.Instance.GetSystemInfoSync();
        }

        /// <summary>
        /// 监听全局错误事件。
        /// [qg.onError(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-sysevent-0000001084034252#section62601638132513)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnError(Action<OnErrorResult> action)
        {
            QGSDKManagerHandler.Instance.OnError(action);
        }

        /// <summary>
        /// 取消监听全局错误事件。
        /// [qg.offError(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-sysevent-0000001084034252#section37633402269)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffError(Action<OnErrorResult> action)
        {
            QGSDKManagerHandler.Instance.OffError(action);
        }

        /// <summary>
        /// 退出当前快游戏。
        /// [qg.exitApplication(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section827053913382)
        /// </summary>
        /// <param name="exitApplicationOption">参数</param>
        public static void ExitApplication(ExitApplicationOption exitApplicationOption)
        {
            QGSDKManagerHandler.Instance.ExitApplication(exitApplicationOption);
        }

        /// <summary>
        /// 获取快游戏启动时的参数。
        /// [qg.getLaunchOptionsSync()](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section4844135503910)
        /// </summary>
        /// <returns>快游戏启动时的参数</returns>
        public static GetLaunchOptionsSyncResult GetLaunchOptionsSync() 
        {
            return QGSDKManagerHandler.Instance.GetLaunchOptionsSync();
        }

        /// <summary>
        /// 监听后台事件，在快游戏切换到后台时触发回调。
        /// [qg.onHide(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section4172346204817)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnHide(Action action)
        {
            QGSDKManagerHandler.Instance.OnHide(action);
        }

        /// <summary>
        /// 取消监听后台事件。
        /// [qg.offHide(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section67289273509)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffHide(Action action)
        {
            QGSDKManagerHandler.Instance.OffHide(action);
        }

        /// <summary>
        /// 监听前台事件，在快游戏切换回前台时触发回调。
        /// [qg.onShow(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section16220131917512)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OnShow(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OnShow(action);
        }

        /// <summary>
        /// 取消监听前台事件。
        /// [qg.offShow(function callback)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section6163413205219)
        /// </summary>
        /// <param name="action">参数</param>
        public static void OffShow(Action<string> action)
        {
            QGSDKManagerHandler.Instance.OffShow(action);
        }

        /// <summary>
        /// 打开另一个快游戏。
        /// [qg.navigateToQuickApp(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-lifecycle-0000001083746128#section01284612546)
        /// </summary>
        /// <param name="navigateToQuickAppOption">参数</param>
        public static void NavigateToQuickApp<T>(NavigateToQuickAppOption<T> navigateToQuickAppOption)
        {
            QGSDKManagerHandler.Instance.NavigateToQuickApp(navigateToQuickAppOption);
        }

        /// <summary>
        /// 跳转至其它场景的页面。
        /// [qg.openDeeplink(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-deeplink-0000001130976505#section113471430152319)
        /// </summary>
        /// <param name="openDeeplinkOption">参数</param>
        public static void OpenDeeplink(OpenDeeplinkOption openDeeplinkOption)
        {
            QGSDKManagerHandler.Instance.OpenDeeplink(openDeeplinkOption);
        }

        /// <summary>
        /// 获取是否有快游戏的桌面图标。
        /// [qg.hasShortcutInstalled(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-shortcut-0000001130976501#section111516252313)
        /// </summary>
        /// <param name="hasShortcutInstalledOption">参数</param>
        public static void HasShortcutInstalled(HasShortcutInstalledOption hasShortcutInstalledOption)
        {
            QGSDKManagerHandler.Instance.HasShortcutInstalled(hasShortcutInstalledOption);
        }

        /// <summary>
        /// 创建快游戏的桌面图标。
        /// [qg.installShortcut(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-shortcut-0000001130976501#section19206171192917)
        /// </summary>
        /// <param name="installShortcutOption">参数</param>
        public static void InstallShortcut(InstallShortcutOption installShortcutOption)
        {
            QGSDKManagerHandler.Instance.InstallShortcut(installShortcutOption);
        }
        
        public class Env
        {
            /// <summary>
            /// 获取用户文件目录，您对这个目录有完全自由的读写权限。
            /// [qg.env.USER_DATA_PATH](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-global-0000001084194208#section1839774619714)
            /// </summary>
            public static string UserDataPath => QGSDKManagerHandler.Instance.USER_DATA_PATH();
        }

        /// <summary>
        /// 获取全局唯一的文件管理器，返回FileSystemManager对象。
        /// [qg.getFileSystemManager()](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-file-0000001084194212#section10341171144119)
        /// </summary>
        /// <returns>FileSystemManager对象</returns>
        public static FileSystemManager GetFileSystemManager()
        {
            return QGSDKManagerHandler.Instance.GetFileSystemManager();
        }

        /// <summary>
        /// 下载文件资源到本地。
        /// [qg.downloadFile(Object object)](https://developer.huawei.com/consumer/cn/doc/quickApp-References/quickgame-api-download-0000001083746136#section2193754182214)
        /// </summary>
        /// <param name="downloadFileOption">参数</param>
        public static DownloadTask DownloadFile(DownloadFileOption downloadFileOption)
        {
            return QGSDKManagerHandler.Instance.DownloadFile(downloadFileOption);
        }
    }
}