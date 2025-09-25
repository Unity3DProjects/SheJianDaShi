using System;

namespace _ADD
{
    public static class TimeCheck
    {
        public static bool TimeReach => IsExpired(2024, 09, 24, 20, 0, 0);
        private static bool IsExpired(int year, int mouth, int date, int hour, int minute, int second)
        {
            return DateTime.Now > new DateTime(year, mouth, date, hour, minute, second);
        }
    }
}