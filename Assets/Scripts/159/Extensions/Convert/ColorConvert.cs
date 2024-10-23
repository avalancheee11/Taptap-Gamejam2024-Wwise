using UnityEngine;

public static class ColorConvert
{
    public static bool isDarkColor(this Color color)
    {
        return color.r < 0.5f && color.g < 0.5f && color.b < 0.5f;
    }

}