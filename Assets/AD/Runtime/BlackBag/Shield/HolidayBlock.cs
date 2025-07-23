using System;
using System.Collections.Generic;

namespace AD.Runtime.BlackBag.Shield
{
    public class HolidayBlock
    {
        #region 字段

        /// <summary>
        /// 节假日
        /// </summary>
        private static readonly HashSet<DateTime> holidays = new HashSet<DateTime>()
        {
            new DateTime(2025, 1, 1), // 元旦
            new DateTime(2025, 1, 28), // 春节
            new DateTime(2025, 1, 29), // 春节
            // ... 添加其他节假日
        };

        private static TimeSpan? _startTime = new TimeSpan(8, 0, 0);
        private static TimeSpan? _endTime = new TimeSpan(19, 0, 0);

        #endregion

        #region Public

        /// <summary>
        /// 检查能否显示广告
        /// </summary>
        /// <param name="startTime">工作日的开始时间</param>
        /// <param name="endTime">工作日的结束时间</param>
        /// <returns></returns>
        public static bool CheckCanShowAd(TimeSpan? startTime = null, TimeSpan? endTime = null)
        {
            if (startTime == null || endTime == null)
            {
                return NonTimeLock(_startTime, _endTime);
            }

            return NonTimeLock(startTime, endTime);
        }

        #endregion

        #region Private

        /// <summary>
        /// 判断方法
        /// </summary>
        /// <returns></returns>
        private static bool NonTimeLock(TimeSpan? t1, TimeSpan? t2)
        {
            if (IsHoliday()) return true;

            if (CheckToDay())
            {
                if (CheckTime(t1, t2))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 时间段
        /// </summary>
        /// <param name="t1">开始时间点</param>
        /// <param name="t2">结束时间点</param>
        /// <returns>是否在时间范围内</returns>
        private static bool CheckTime(TimeSpan? t1, TimeSpan? t2)
        {
            DateTime now = DateTime.Now;
            TimeSpan currentTime = now.TimeOfDay;
            return currentTime >= t1 && currentTime <= t2;
        }

        /// <summary>
        /// 检查是否是工作日
        /// </summary>
        /// <returns></returns>
        private static bool CheckToDay()
        {
            DateTime now = DateTime.Now;
            DayOfWeek dayOfWeek = now.DayOfWeek;
            return (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday);
        }


        /// <summary>
        /// 返回当前时间是否处于节假日
        /// </summary>
        /// <returns></returns>
        private static bool IsHoliday()
        {
            DateTime now = DateTime.Now;
            return holidays.Contains(now);
        }

        #endregion
    }
}