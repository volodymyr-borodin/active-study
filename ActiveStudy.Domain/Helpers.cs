using System;

namespace ActiveStudy.Domain
{
    public static class Helpers
    {
        public static DateTime NearestMonday(this DateTime day)
        {
            while (day.DayOfWeek != DayOfWeek.Monday)
            {
                day = day.AddDays(-1);
            }

            return day;
        }

        public static DateOnly NearestMonday(this DateOnly day)
        {
            while (day.DayOfWeek != DayOfWeek.Monday)
            {
                day = day.AddDays(-1);
            }

            return day;
        }
    }
}