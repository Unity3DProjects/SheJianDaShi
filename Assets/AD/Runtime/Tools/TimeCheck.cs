using System;
using System.Linq;
using UnityEngine;

namespace AD.Tools
{
    public static class TimeCheck
    {
        public const string Format = "yyyy-MM-dd HH:mm:ss";

        public static bool IsExpired(string date)
        {
            if (date == null)
            {
                throw new Exception("请检查广告日期");
            }

#if UNITY_EDITOR
            return true;
#endif
            return DateTime.Now > date.GetDateTime();
        }

        public static bool IsExpired(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                throw new Exception("请检查广告日期");
            }

#if UNITY_EDITOR
            return true;
#endif
            return DateTime.Now > dateTime;
        }


        internal static string GetString(this DateTime dateTime, string format = Format)
        {
            return dateTime.ToString(format);
        }

        internal static DateTime GetDateTime(this string date, string format = Format)
        {
            try
            {
                return DateTime.ParseExact(date, format, null);
            }
            catch (Exception e)
            {
                throw new InvalidCastException("广告日期设置错误：字符串无法转换为 DateTime");
            }
        }

        public static string GetNotShieldTime()
        {
            return DateTime.Now.AddDays(-1).ToString(Format);
        }

        /// <summary>
        /// 设置VIVO屏蔽时间
        /// </summary>
        public static string GetVIVOBlockTime()
        {
            // 获取当前时间  
            DateTime currentTime = DateTime.Now;
            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
            DateTime targetTime;
            if (!HolidayMoveTime(out targetTime, currentTime, 2))
            {
                switch (currentTime.DayOfWeek)
                {
                    case DayOfWeek.Thursday:
                        targetTime = currentTime.AddDays(1);
                        break;
                    case DayOfWeek.Friday:
                        targetTime = currentTime.AddDays(4);
                        break;
                    case DayOfWeek.Saturday:
                        targetTime = currentTime.AddDays(3);
                        break;
                    default:
                        targetTime = currentTime.AddDays(2);
                        break;
                }
            }

            return new DateTime(targetTime.Year, targetTime.Month, targetTime.Day, 19, 0, 0).ToString(Format);
        }


        /// <summary>
        /// 获取OPPO黑包时间
        /// </summary>
        /// <returns></returns>
        public static string GetOPPOBlackTime()
        {
            // 获取当前时间  
            DateTime currentTime = DateTime.Now;
            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
            DateTime targetTime;
            if (!HolidayMoveTime(out targetTime, currentTime, 2))
            {
                switch (currentTime.DayOfWeek)
                {
                    case DayOfWeek.Thursday:
                        targetTime = currentTime.AddDays(1);
                        break;
                    case DayOfWeek.Friday:
                        targetTime = currentTime.AddDays(4);
                        break;
                    case DayOfWeek.Saturday:
                        targetTime = currentTime.AddDays(3);
                        break;
                    default:
                        targetTime = currentTime.AddDays(3);
                        break;
                }
            }

            return new DateTime(targetTime.Year, targetTime.Month, targetTime.Day, 19, 0, 0).ToString(Format);
        }

        /// <summary>
        /// 如果节假日移动，返回 true
        /// </summary>
        private static bool HolidayMoveTime(out DateTime targetTime, DateTime today, int shieldDays)
        {
            if (shieldDays < 0) throw new Exception();
            var futureTime = today.AddDays(shieldDays);
            if (!HolidayUtils.Holidays.Contains(futureTime))
            {
                targetTime = default;
                return false;
            }

            var todayBeforeHoliday = today;
            while (!HolidayUtils.Holidays.Contains(todayBeforeHoliday))
                todayBeforeHoliday = todayBeforeHoliday.AddDays(1);
            todayBeforeHoliday = todayBeforeHoliday.AddDays(-1);
            if (today >= todayBeforeHoliday)
            {
                do futureTime = futureTime.AddDays(1);
                while (HolidayUtils.Holidays.Contains(futureTime));

                targetTime = futureTime;
            }
            else
            {
                targetTime = todayBeforeHoliday;
            }

            return true;
        }

        /// <summary>
        /// 获取OPPO白包时间
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static string GetOppoTime()
        {
            // 获取当前时间  
            DateTime currentTime = DateTime.Now;
            if (currentTime.Hour < 16)
                return new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 19, 0, 0).ToString(Format);

            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
            var futureTime = currentTime;
            bool delay, delayByHoliday = false;
            do
            {
                delay = true;
                futureTime = futureTime.AddDays(1);
                while (HolidayUtils.Holidays.Contains(futureTime))
                {
                    delay = false;
                    delayByHoliday = true;
                    var nextTime = futureTime.AddDays(1);
                    if (!HolidayUtils.Holidays.Contains(nextTime)) break;
                    futureTime = nextTime;
                }
            } while (!delay);


            if (!delayByHoliday)
            {
                switch (currentTime.DayOfWeek)
                {
                    case DayOfWeek.Friday:
                        futureTime = currentTime.AddDays(3);
                        break;
                    case DayOfWeek.Saturday:
                        futureTime = currentTime.AddDays(2);
                        break;
                    case DayOfWeek.Sunday:
                        futureTime = currentTime.AddDays(1);
                        break;
                }
            }

            return new DateTime(futureTime.Year, futureTime.Month, futureTime.Day, 19, 0, 0).ToString(Format);
        }
    }
}