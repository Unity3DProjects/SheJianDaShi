using System;
using AD.NodeTree;

namespace AD.NodeTree
{
    public class IsWorkingTimeNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }
        protected override void OnCall(AdSourceType type)
        {
            Complete(IsWorkTime());
        }

        private static TimeSpan _startTime = new TimeSpan(8, 0, 0);
        private static TimeSpan _endTime = new TimeSpan(19, 0, 0);


        public static bool IsWorkTime()
        {
#if UNITY_EDITOR
            return false;
#endif
            return IsNowWorkDay() && IsNowInTimeSpan(_startTime, _endTime);
        }

        /// <summary>
        /// 时间段
        /// </summary>
        /// <param name="t1">开始时间点</param>
        /// <param name="t2">结束时间点</param>
        /// <returns>是否在时间范围内</returns>
        private static bool IsNowInTimeSpan(TimeSpan? t1, TimeSpan? t2)
        {
            DateTime now = DateTime.Now;
            TimeSpan currentTime = now.TimeOfDay;
            return currentTime >= t1 && currentTime <= t2;
        }

        /// <summary>
        /// 检查是否是工作日
        /// </summary>
        /// <returns></returns>
        private static bool IsNowWorkDay()
        {
            DateTime now = DateTime.Now;
            DayOfWeek dayOfWeek = now.DayOfWeek;
            return (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday);
        }
    }
}