using System;
using AD.NodeTree;
using AD.Tools;

namespace AD.NodeTree
{
    public class IsHolidayNode : BaseNode
    {
        public override void InitTree(Tree tree)
        {
        }
        protected override void OnCall(AdSourceType type)
        {
            Complete(IsHoliday());
        }

        /// <summary>
        /// 返回当前时间是否处于节假日
        /// </summary>
        /// <returns></returns>
        public static bool IsHoliday()
        {
            DateTime now = DateTime.Now;
            return HolidayUtils.Holidays.Contains(new DateTime(now.Year, now.Month, now.Day));
        }
    }
}