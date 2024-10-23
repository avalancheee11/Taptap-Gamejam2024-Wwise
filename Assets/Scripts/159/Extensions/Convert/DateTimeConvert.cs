using System;
using UnityEngine;
using System.Globalization;

public static class DataTimeConvert
{
    public static DateTime toDateTime(this string dateString, string format = "yyyy-MM-dd")
    {
        DateTime dateTime;
        DateTime.TryParseExact(dateString, format, null, DateTimeStyles.AssumeLocal, out dateTime);
        return dateTime;
    }

    public static DateTime toUTCDateTime(this string dateString, string format = "yyyy-MM-dd")
    {
        DateTime dateTime;
        DateTime.TryParseExact(dateString, format, null, DateTimeStyles.AssumeUniversal, out dateTime);
        return dateTime;
    }

    public static string timestampDefaultForamt(this int timestamp)
    {
        var dateTime = DTC.UnixDateTime.AddSeconds(TimeZoneInfo.Local.BaseUtcOffset.Hours * IC.OneHour + timestamp);
        return dateTime.dateTimeDefaultFormat();
    }

    public static string timestampDefaultForamt(this int timestamp, string format = "yyyy-MM-dd HH:mm:ss")
    {
        var dateTime = DTC.UnixDateTime.AddSeconds(TimeZoneInfo.Local.BaseUtcOffset.Hours * IC.OneHour + timestamp);
        return dateTime.ToString(format);
    }

    public static string dateTimeDefaultFormat(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string greenwichTimestampString(this DateTime dateTime)
    {
        return dateTime.greenwichTimestampString(DTC.UnixDateTime);
    }

    public static string greenwichTimestampString(this DateTime dateTime, DateTime unixDateTime)
    {
        return dateTime.greenwichTimestamp(unixDateTime).ToString();
    }

    public static int greenwichTimestampInt(this DateTime dateTime)
    {
        return dateTime.greenwichTimestampInt(DTC.UnixDateTime);
    }

    public static int greenwichTimestampInt(this DateTime dateTime, DateTime unixDateTime)
    {
        return (int)dateTime.greenwichTimestamp(unixDateTime);
    }

    public static double greenwichTimestamp(this DateTime dateTime)
    {
        return dateTime.greenwichTimestamp(DTC.UnixDateTime);
    }

    /// <summary>
    /// Dates the time to greenwich timestamp.
    /// 获取1970时间戳 精确到后3位小数
    /// </summary>
    /// <returns>The time to greenwich timestamp.</returns>
    /// <param name="dateTime">Date time.</param>
    public static double greenwichTimestamp(this DateTime dateTime, DateTime unixDateTime)
    {
        return Math.Round((dateTime - unixDateTime).TotalSeconds, 3);
    }

    // This presumes that weeks start with Monday.
    // Week 1 is the 1st week of the year with a Thursday in it.
    public static int getIso8601WeekOfYear(this DateTime time)
    {
        // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
            time = time.AddDays(3);
        }

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public static int getWeekEndTime(this DateTime time)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        int days = day == DayOfWeek.Sunday ? 0 : (DayOfWeek.Saturday + 1 - day);
        var end = time.AddDays(days);
        return new DateTime(end.Year, end.Month, end.Day).greenwichTimestampInt() + IC.OneDay;
    }
    
    public static int getSecond(int second)
    {
        return second % 60;
    }

    public static int getMinute(int second)
    {
        var m = second / 60;
        return m % 60;
    }
    
    public static int getHour(int second)
    {
        var m = second / 60;
        var h = m / 60;
        return h;
    }

    public static string getTimeStringBySecond(int second)
    {
        if (second < 60 * 60) {
            return $"{getMinute(second):00}:{getSecond(second):00}";
        }
        return $"{getHour(second)}:{getMinute(second):00}:{getSecond(second):00}";
    }
}

public static class DTC
{
    public static DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static DateTime LocalDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
}
