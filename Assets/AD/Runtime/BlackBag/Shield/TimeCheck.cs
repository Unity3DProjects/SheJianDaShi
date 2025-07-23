using System;

namespace AD.Runtime.BlackBag.Shield
{
    public static class TimeCheck 
    {
        private static DateTime? date;
        private static DateTime? blackDate;


        public static DateTime? Date => GetDate(ADManager.Instance.Date);

        public static DateTime? BlackDate => GetDate(ADManager.Instance.BlackDate);

        /// <summary>
        /// false表示在屏蔽中
        /// </summary>
        public static bool TimeReach => IsExpired(Date);

        /// <summary>
        /// false表示在屏蔽中
        /// </summary>
        public static bool BlackTimeReach => IsExpired(BlackDate);

        private static bool IsExpired(int year, int mouth, int date, int hour, int minute, int second)
        {
            return DateTime.Now > new DateTime(year, mouth, date, hour, minute, second);
        }

        private static bool IsExpired(DateTime? dateTime)
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

        private static DateTime GetDate(string date)
        {
            try
            {
                var _date = Convert.ToDateTime(date);
                return _date;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("广告日期设置错误：字符串无法转换为 DateTime");
            }
        }
    }
}