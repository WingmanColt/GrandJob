using System;

namespace HireMe.Core.Extensions
{
    public static class TimeExtension
    {
        public static bool IsBetween(this DateTime now, DateTime start, DateTime end)
        {
            double start2 = TimeExtension.DateTimeToUnixTimestamp(start);
            TimeSpan startStamp = TimeSpan.FromSeconds(start2);

            double end2 = TimeExtension.DateTimeToUnixTimestamp(end);
            TimeSpan endStamp = TimeSpan.FromSeconds(end2);

            var time = now.TimeOfDay;
            // Scenario 1: If the start time and the end time are in the same day.
            if (startStamp <= endStamp)
                return time >= startStamp && time <= endStamp;
            // Scenario 2: The start time and end time is on different days.
            return time >= startStamp || time <= endStamp;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
        private static readonly long DatetimeMinTimeTicks =
        (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }
    }
}
