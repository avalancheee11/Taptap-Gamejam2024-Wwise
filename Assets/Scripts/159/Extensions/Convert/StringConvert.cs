using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class StringConvert
{
    public static Vector2 sizeValue(this string str)
    {
        if (str.isNull()) {
            return Vector2.zero;
        }
        string[] strings = str.Split(',');

        string xString = strings[0];
        xString = xString.Replace("{", "");
        float x = float.Parse(xString);

        string yString = strings[1];
        yString = yString.Replace("}", "");
        float y = float.Parse(yString);

        return new Vector2(x, y);
    }

    public static Vector3 pointValue(this string str)
    {
        Vector2 vec = str.sizeValue();
        return new Vector3(vec.x, vec.y);
    }

    public static string MD5(this string str) 
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));

        System.Text.StringBuilder md5Text = new System.Text.StringBuilder();
        for (int i = 0; i < result.Length; i++) {
            md5Text.Append(result[i].ToString("x2"));
        }
        return md5Text.ToString();
    }

    public static string SHA1(this string str)
    {
        System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
        byte[] result = sha1.ComputeHash(System.Text.Encoding.Default.GetBytes(str));

        System.Text.StringBuilder sha1Text = new System.Text.StringBuilder();
        for (int i = 0; i < result.Length; i++) {
            sha1Text.Append(result[i].ToString("x2"));
        }
        return sha1Text.ToString();
    }

    public static bool equals(this string a, string b)
    {
        if (a == null || b == null) {
            return false;
        }
        return a.GetHashCode() == b.GetHashCode();
    }

    public static void writeString(this BinaryWriter writer, string s)
    {
        char[] cs = s.ToCharArray();
        int len = cs.Length;
        writer.Write(len.toProByte());
        writer.Write(cs);
    }

    public static string readString(this BinaryReader reader)
    {
        int len = reader.ReadByte().toProInt();
        var cs = reader.ReadChars(len);
        return new string(cs);
    }

    public static string capitalizeFirstLetter(this string str)
    {
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }

    /// <summary>
    /// 按照百分比裁剪字符长度
    /// </summary>
    /// <returns>The user name.</returns>
    /// <param name="userName">User name.</param>
    /// <param name="percent">Percent.</param>
    public static string CutUserName(this string str, float percent)
    {
        if (str.Length == 0)
            return str;
        int cutCount = (int)(percent * str.Length);
        return str.Substring(0, cutCount) + "...";
    }

    public static int GetUTF8Length(this string str)
    {   
        if (str.Length == 0)
            return 0;  
        int tempLen = System.Text.Encoding.UTF8.GetByteCount(str);
        return tempLen;   
    }

    public static DateTime GetDateTime(this string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(IC.GreenwichDateTime);  
        DateTime targetDt = dtStart.AddSeconds(timeStamp.toLong());  
        return targetDt;
    }

    /// <summary>
    /// 判断一个字符串是否为url
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsUrl(this string str)
    {
        try
        {
            string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
            return Regex.IsMatch(str, Url);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool IsNumeric(this string value)
    {
        try
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool IsEmail(this string str)
    {
        try
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static string removeQuotes(this string str)
    {
        if (str.isNull()) return str;
        if (str.Length > 1 && str[0]=='\"' && str[str.Length - 1] == '\"')
        {
            return str.Substring(1, str.Length - 2);
        }
        return str;
    }

    public static bool IsNullOrEmpty(this string str)
    {
        if (str == "0") {
            return true;
        }

        return string.IsNullOrEmpty(str);
    }
    
    public static string findMidStrEx(this string source, string startStr, string stopStr)
    {
        var startIndex = source.IndexOf(startStr);
        if (startIndex < 0) {
            return string.Empty;
        }

        string tmpstr = source.Substring(startIndex + startStr.Length);
        var stopIndex = tmpstr.IndexOf(stopStr);
        if (stopIndex < 0) {
            return string.Empty;
        }
        var result = tmpstr.Remove(stopIndex);

        return result;
    }
    
    public static Color convertToColor(this string _str)
    {
        if (_str.IsNullOrEmpty()) return Color.white;

        Color nowColor;
        string thisStr = _str[0] != '#' ? "#" + _str : _str;
        ColorUtility.TryParseHtmlString(thisStr, out nowColor);
        return nowColor;
    }
}
