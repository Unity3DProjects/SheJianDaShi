using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AD.Tools
{
    public class HolidayUtils
    {
        private const string Format = "yyyy-MM-dd";

        private static ISet<string> stringDateTimes = new HashSet<string>()
        {
            // 元旦：1月1日至3日
            "2026-01-01", "2026-01-02", "2026-01-03",
            // 春节：2月15日至23日
            "2026-02-15", "2026-02-16", "2026-02-17", "2026-02-18", "2026-02-19",
            "2026-02-20", "2026-02-21", "2026-02-22", "2026-02-23",
            // 清明节：4月4日至6日
            "2026-04-04", "2026-04-05", "2026-04-06",
            // 劳动节：5月1日至5日
            "2026-05-01", "2026-05-02", "2026-05-03", "2026-05-04", "2026-05-05",
            // 端午节：6月19日至21日
            "2026-06-19", "2026-06-20", "2026-06-21",
            // 中秋节：9月25日至27日
            "2026-09-25", "2026-09-26", "2026-09-27",
            // 国庆节：10月1日至7日
            "2026-10-01", "2026-10-02", "2026-10-03", "2026-10-04", "2026-10-05",
            "2026-10-06", "2026-10-07",
        };

        private static ISet<DateTime> DateTimes = new HashSet<DateTime>()
        {
            new DateTime(2025, 1, 1), // 元旦
            new DateTime(2025, 1, 28), // 春节
            new DateTime(2025, 1, 29), // 春节
        };

        private static HashSet<DateTime> holidays;

        /// <summary>
        /// 节假日
        /// </summary>
        public static HashSet<DateTime> Holidays
        {
            get
            {
                if (holidays != null) return holidays;
                holidays = new HashSet<DateTime>();
                foreach (var dateTime in stringDateTimes)
                {
                    if (dateTime.GetDateTime(Format) == new DateTime(2026, 1, 4))
                    {
                        Debug.Log(dateTime);
                    }
                    holidays.Add(dateTime.GetDateTime(Format));
                }
                foreach (var dateTime in DateTimes)
                    holidays.Add(dateTime);
                return holidays;
            }
        }

        public static void Move2Next(ref DateTime time)
        {
        }
    }
}