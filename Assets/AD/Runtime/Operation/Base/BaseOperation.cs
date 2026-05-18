using System;
using System.Collections.Generic;
using AD.Decorator;
using AD.NodeTree;
using AD.Tools;

namespace AD.Operation
{
    public abstract class BaseOperation
    {
        /// <summary>
        /// 是否需要隐私政策
        /// </summary>
        /// <returns></returns>
        public virtual AdInstanceType AllowedInstanceType(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return AdInstanceType.Health;
                default:
                    return AdInstanceType.Health | AdInstanceType.Privacy;
            }
        }

        public virtual AdType AllowedAdType(EPlatform platform)
        {
            return AdType.Banner | AdType.Custom | AdType.Reward;
        }

        public virtual AdButtonType AllowedButtonType(EPlatform platform)
        {
            AdButtonType type = AdButtonType.添加桌面 | AdButtonType.隐私政策;
            if (!this.NeedPrivacy(platform))
                type &= ~AdButtonType.隐私政策;
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.VIVO:
                case EPlatform.OPPO:
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                    break;
                case EPlatform.HuaWei:
                    type |= AdButtonType.注销账号;
                    type &= ~AdButtonType.添加桌面;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }

            return type;
        }

        public virtual void InitDecorator(EPlatform platform, ADParamDecoratorManager decoratorManager)
        {
        }


        public virtual string GetShieldTime(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return TimeCheck.GetNotShieldTime();
                case EPlatform.OPPO:
                    return TimeCheck.GetOppoTime();
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                case EPlatform.VIVO:
                    return TimeCheck.GetVIVOBlockTime();
                default:
                    return TimeCheck.GetVIVOBlockTime();
            }
        }

        public virtual string GetBlackShieldTime(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return TimeCheck.GetNotShieldTime();
                case EPlatform.OPPO:
                    return TimeCheck.GetOPPOBlackTime();
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                case EPlatform.VIVO:
                    return TimeCheck.GetVIVOBlockTime();
                default:
                    return TimeCheck.GetVIVOBlockTime();
            }
        }

        public virtual string GetModifyPackageShieldTime(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.None:
                case EPlatform.HuaWei:
                    return TimeCheck.GetNotShieldTime();
                case EPlatform.OPPO:
                    return TimeCheck.GetOppoTime();
                case EPlatform.XiaoMi:
                case EPlatform.RongYao:
                case EPlatform.VIVO:
                    return TimeCheck.GetVIVOBlockTime();
                default:
                    return TimeCheck.GetVIVOBlockTime();
            }
        }

        public virtual string GetModifyPackageBlackShieldTime(EPlatform platform)
        {
            return GetModifyPackageShieldTime(platform);
        }

        public abstract INode CreateRule(EPlatform platform);
    }
}