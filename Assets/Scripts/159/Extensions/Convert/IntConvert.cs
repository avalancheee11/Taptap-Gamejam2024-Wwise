using System;
using UnityEngine;
using Random = UnityEngine.Random;
public static class IntConvert
{
    /// <summary>
    /// 从0到i-1
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static int toCCRandomIndex(this int i)
    {
        //return Random.Range(0, i);
        return i.toRandomIndex(RandomSync.CCRANDOM);
    }
    public static int toWDRandomIndex(this int i) { return i.toRandomIndex(RandomSync.WDRANDOM); }
    public static int toRandomIndex(this int i, int count) { return i <= 0 ? 0 : count % i; }

    public static float toRadian(this int i) { return i.toFloat().toRadian(); }
    public static float toAngle(this int i) { return i.toFloat().toAngle(); }

    public static bool isNotFound(this int i) { return i == IC.NotFound; }

    
    //00:00:00格式
    public static string formatTimeHour(this int scond)
    {
        if (scond > 0)
        {
            int hours = scond / (60 * 60);
            int minute = (scond - hours * 60 * 60) / 60;
            int s = scond - hours * 60 * 60 - minute * 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minute, s);
        }
        else
        {
            return "00:00:00";
        }
    }
    
    public static DateTime GetDateTime(this int timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(IC.GreenwichDateTime);  
        DateTime targetDt = dtStart.AddSeconds(timeStamp.toLong());  
        return targetDt;
    } 
    
    public static float ReMapNumber(this int oXY, float originMin, float originMax, float targetMin, float targetMax, float defaultValue = 1)
    {
        if (Math.Abs(originMax - originMin) < float.Epsilon || originMin > originMax) {
            return defaultValue;
        }
        float result = 0;
        result = (targetMax - targetMin) / (originMax - originMin) * (oXY - originMin) + targetMin;
        return result;
    }
}

public static class IC
{
    public static readonly int NotFound = -1;
    public static readonly int MinusTwo = -2;

    public static readonly string Type = "int";

    public const int NO = 0;
    public const int YES = 1;

    public static DateTime GreenwichDateTime = new DateTime(1970, 1, 1);

    public const int OneSecond = 1;
    public const int OneMinute = 60;
    public const int OneHour = 3600;
    public const int OneDay = 86400;
    public const int OneWeek = 604800;
    public const int OneYear = OneDay * 365;
}
