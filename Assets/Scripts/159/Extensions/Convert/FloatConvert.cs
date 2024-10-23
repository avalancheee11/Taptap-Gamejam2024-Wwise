using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class FloatConvert
{
    public static float toRadian(this float f)
    {
        return f * Mathf.Deg2Rad;
    }

    public static float toAngle(this float f)
    {
        return f * Mathf.Rad2Deg;
    }

    public static float toPI(this float f)
    {
        return Mathf.PI * f;
    }

    public static float toCCRandom01(this float f)
    {
        //return f * Random.Range(0f, 1f);
        return f * RandomSync.CCRANDOM_0_1;
    }

    public static float toWDRandom01(this float f)
    {
        return f * RandomSync.WDRANDOM_0_1;
    }

    public static float toCCRandomMINUS11(this float f)
    {
        return f * RandomSync.CCRANDOM_MINUS1_1;
    }

    public static float toWDRandomMINUS11(this float f)
    {
        return f * RandomSync.WDRANDOM_MINUS1_1;
    }

    public static Vector2 toV2(this float f)
    {
        return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
    }

    public static Vector3 toV3(this float f)
    {
        return new Vector3(Mathf.Cos(f), Mathf.Sin(f));
    }

    public static bool isZero(this float f)
    {
        return f <= Mathf.Epsilon;
    }

    public static bool isAbsZero(this float f)
    {
        return  Mathf.Abs(f) <= Mathf.Epsilon;
    }

    public static float toRound(this float f, int round)
    {
        return (float)(Mathf.Round(f * 10 * round)) / (10 * round);
    }
    
    public static float ReMapNumber(this float oXY, float originMin, float originMax, float targetMin, float targetMax, float defaultValue = 1)
    {
        if (Math.Abs(originMax - originMin) < float.Epsilon || originMin > originMax) {
            return defaultValue;
        }
        float result = 0;
        result = (targetMax - targetMin) / (originMax - originMin) * (oXY - originMin) + targetMin;
        return result;
    }
}

public class FC
{
    public static readonly float PIX4 = 4f.toPI();
    public static readonly float PIX2 = 2f.toPI();
    public static readonly float PI = 1f.toPI();
    public static readonly float PI_2 = 0.5f.toPI();
    public static readonly float PI_4 = 0.25f.toPI();

    public static readonly string Type = "float";
}
