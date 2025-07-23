
namespace AD.Runtime.BlackBag
{
    public abstract class BaseOperation
    {
        /// <summary>
        /// 是否需要隐私政策
        /// </summary>
        /// <returns></returns>
        public virtual bool NeedPrivacy(EPlatform platform)
        {
            return true;
        }

        /// <summary>
        /// 能否展示广告
        /// </summary>
        public virtual bool CheckCanShowAD(EPlatform platform)
        {
            return false;
        }

        /// <summary>
        /// 能否显示宝箱
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckCanShowBox(EPlatform platform)
        {
            return false;
        }
    }
}