using System;
using AD.Runtime.BlackBag.Shield;

namespace AD.Runtime.BlackBag.Operation
{
    public class YeZi : BaseOperation
    {
        //烨子vivo宝箱
        private static TimeSpan? yeziStartTime = new TimeSpan(9, 0, 0);
        private static TimeSpan? yeziEndTime = new TimeSpan(18, 0, 0);

        public override bool CheckCanShowAD(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return TimeCheck.TimeReach;
                case EPlatform.OPPO:
                    return TimeCheck.BlackTimeReach;
                default:
                    return TimeCheck.TimeReach;
            }
        }

        public override bool CheckCanShowBox(EPlatform platform)
        {
            switch (platform)
            {
                case EPlatform.VIVO:
                    return HolidayBlock.CheckCanShowAd(yeziStartTime, yeziEndTime);
                case EPlatform.OPPO:
                    return false;
                default:
                    return HolidayBlock.CheckCanShowAd(yeziStartTime, yeziEndTime);
            }
        }
    }
}