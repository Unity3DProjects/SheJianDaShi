namespace AD.Runtime.AdAdapter.Interface
{
    public interface IAdAdapter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init(IADParam normalParam);

        /// <summary>
        /// 显示白包广告
        /// </summary>
        void ShowWhiteAD();

        /// <summary>
        /// 显示黑包广告
        /// </summary>
        void ShowBlackAD();

        /// <summary>
        /// 显示激励广告
        /// </summary>
        void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param);

        /// <summary>
        /// 添加到桌面
        /// </summary>
        void AddToDesktop();

        /// <summary>
        /// 退出
        /// </summary>
        void Quit();
    }
}