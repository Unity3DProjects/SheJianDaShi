using System;
using AD.Tools;

namespace AD.AdAdapter
{
    public interface IAdAdapter : IADInit<IADParam>
    {
        void Login(Action onSuccess, Action onFailure);

        /// <summary>
        /// 显示白包广告
        /// </summary>
        void ShowWhiteAD();

        /// <summary>
        /// 显示黑包广告
        /// </summary>
        void ShowBlackAD();

        void ShowBannerAD(string id = null);

        void ShowCustomAD(string id = null);

        /// <summary>
        /// 显示激励广告
        /// </summary>
        void ShowRewardAD(string receiverKey, string id = null);

        /// <summary>
        /// 添加到桌面
        /// </summary>
        void AddToDesktop();

        /// <summary>
        /// 退出
        /// </summary>
        void Quit();

        bool HasTitle { get; }
    }
}