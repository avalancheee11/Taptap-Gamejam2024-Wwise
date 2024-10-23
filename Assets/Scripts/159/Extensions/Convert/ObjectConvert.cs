using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class ObjectConvert
{
    public static Dictionary<int, string> MPDict = new Dictionary<int, string>(){
        {3, "K"},
        {6, "M"},
        {9, "B"},
        {12, "T"},
        {15, "P"},
        {18, "E"},
        {21, "Z"},
        {24, "Y"},
    };

    public static int toInt(this object obj, int defaultValue = 0)
    {
        return  obj == null ? defaultValue : Convert.ToInt32(obj);
    }

    public static long toLong(this object obj, long defaultValue = 0L)
    {
        return  obj == null ? defaultValue : Convert.ToInt64(obj);
    }

    public static float toFloat(this object obj, float defaultValue = 0f)
    {
        return obj == null ? defaultValue : (float)Convert.ToDouble(obj);
    }

    public static double toDouble(this object obj, double defaultValue = 0)
    {
        return obj == null ? defaultValue : Convert.ToDouble(obj);
    }

    public static bool toBool(this object obj, bool defaultValue = false)
    {
        return obj == null ? defaultValue : Convert.ToBoolean(obj);
    }

    public static string toString(this object obj, string defaultValue = null)
    {
        return obj == null ? defaultValue : Convert.ToString(obj);
    }

    public static Dictionary<string,T> toDictionary<T>(this object obj, Dictionary<string,T> defaultValue = null)
    {
        return obj == null ? defaultValue : obj as Dictionary<string,T>;
    }

    public static Dictionary<string,object> toDictionary(this object obj, Dictionary<string,object> defaultValue = null)
    {
        return obj.toDictionary<object>(defaultValue);
    }

    public static List<T> toList<T>(this object obj, List<T> defaultValue = null)
    {
        return obj == null ? defaultValue : obj as List<T>;
    }

    public static List<object> toList(this object obj, List<object> defaultValue = null)
    {
        return obj.toList<object>(defaultValue);
    }

    public static Vector2 toVector2(this object obj, Vector2 defaultValue = default(Vector2))
    {
        return obj == null ? defaultValue : (Vector2)obj;
    }

    public static Vector3 toVector3(this object obj, Vector3 defaultValue = default(Vector3))
    {
        return obj == null ? defaultValue : (Vector3)obj;
    }

    public static Vector4 toVector4(this object obj, Vector4 defaultValue = default(Vector4))
    {
        return obj == null ? defaultValue : (Vector4)obj;
    }

    public static Rect toRect(this object obj, Rect defaultValue = default(Rect))
    {
        return obj == null ? defaultValue : (Rect)obj;
    }

    public static List<object> toObjectList(this object obj, List<object> defaultValue = null)
    {
        return obj == null ? defaultValue : (obj is List<object> ? obj as List<object> : new List<object> { obj });
    }

    public static List<object> convertToObjectList<T>(this List<T> list)
    {
        var l = new List<object>();
        foreach (var o in list) {
            l.Add(o);
        }

        return l;
    }

    public static T deepCopy<T>(T obj)
    {
        object retval;
        using (MemoryStream ms = new MemoryStream()) {
            BinaryFormatter bf = new BinaryFormatter();
            //序列化成流
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            //反序列化成对象
            retval = bf.Deserialize(ms);
            ms.Close();
        }
        return (T)retval;
    }

    public static void replaceValue<T>(ref T a, ref T b)
    {
        if (a is Dictionary<string, object>) {
            var da = a.toDictionary();
            var db = b.toDictionary();

            if (db == null) {
                return;
            }
            foreach (string k in db.Keys) {
                if (da.ContainsKey(k)) {
                    var dak = da[k];
                    var dbk = db[k];
                    replaceValue(ref dak, ref dbk);
                    da[k] = dak;
                }
                else {
                    da[k] = db[k];
                }
            }

        }
        else if (a is List<object>) {
            var la = a.toList();
            var lb = b.toList();

            if (lb == null) {
                return;
            }
            int count = Mathf.Max(la.Count, lb.Count);
            for (int t = 0; t < count; t++) {
                var lao = t < la.Count ? la[t] : null;
                var lbo = t < lb.Count ? lb[t] : null;
                if (lbo == null) {
                    continue;
                }
                if (t >= la.Count) {
                    la.Add(lbo);
                }
                else {
                    if (lao != null) {
                        replaceValue(ref lao, ref lbo);
                        la[t] = lao;
                    }
                    else {
                        la[t] = lbo;
                    }
                }
            }
        }
        else {
            a = b;
        }
    }

    public static MethodInfo getMethodInfo(this object obj, string methodString, bool simple = false)
    {
        return obj.getMethodInfo(methodString, null, simple);
    }

    public static MethodInfo getMethodInfo(this object obj, string methodString, object[] os)
    {
        Type[] ts = null;
        if (os != null) {
            ts = new Type[os.Length];
            int t = 0;
            foreach (object o in os) {
                ts[t] = o.GetType();
                t++;
            }
        }
        return obj.getMethodInfo(methodString, ts, false);
    }

    public static MethodInfo getMethodInfo(this object obj, string methodString, Type[] ts, bool simple)
    {
        if (methodString == null) {
            return null;
        }

        if (ts == null) {
            ts = new Type[0];
        }

        //GetType(命名空间.类名)  
        Type type = obj.GetType();
//        Debug.LogFormat("getMethodInfo, type:{0}, method:{1}", type.Name, methodString);
        //GetMethod(需要调用的方法名称) 
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        return simple ? type.GetMethod(methodString, flags) : type.GetMethod(methodString, flags, null, ts, null);
    }

    public static void invokeMethod(this object obj, string methodString, object[] ps)
    {
        MethodInfo method = ps == null ? obj.getMethodInfo(methodString) : obj.getMethodInfo(methodString, ps);
        if (method == null) {
            return;
        }
        method.Invoke(obj, ps);
    }

    public static T invokeStaticMethod<T>(this Type type, string methodString) where T : class
    {
        if (type == null || methodString == null) {
            return default(T);
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        MethodInfo method = type.GetMethod(methodString, flags);
        if (method == null) {
            return default(T);
        }
        return method.Invoke(null, null) as T;
    }

    public static T getStaticProperty<T>(this Type type, string propertyName) where T : class
    {
        if (type == null || propertyName == null) {
            return default(T);
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        PropertyInfo info = type.GetProperty(propertyName, flags);
        if (info == null) {
            return default(T);
        }
        return info.GetValue(null, null) as T;
    }

    public static T getDelegate<T>(this object obj, string methodString) where T : class
    {
        var methodInfo = obj.getMethodInfo(methodString);
        if (methodInfo == null) {
            return null;
        }
        return Delegate.CreateDelegate(obj.GetType(), methodInfo) as T;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    public static bool isNull(this object o) { return o == null || o.Equals(null);}


    public static string toMPNString(this float number)
    {
        if (number < 0) {
            return "-" + Mathf.Ceil(Mathf.Abs(number)).toMPLString();
        } else {
            return "+" + Mathf.Ceil(number).toMPLString();
        }
    }
    /// <summary>
    /// tos the Metric prefix
    /// </summary>
    /// <returns>The Metric prefix string.</returns>
    /// <param name="obj">Object.</param>
    /// <param name="defaultValue">Default value.</param>
    /// 
    public static string toMPLString(this object obj)
    {
        return obj.toMPString(99999);
    }

    public static string toMPString(this object obj, double cut = 999)
    {
        double number = obj.toDouble();
        if (number is < 10 and > 0) {
            return number.ToString("0.##");
        }
        if (number < cut) {
            // var n = (Mathf.RoundToInt(obj.toFloat() * 10)) / 10f;
            // return n.toString();
            //return number.ToString();
            return number.toLong().ToString();
        }

        int length = (int)System.Math.Log10(number);
        length = length - length % 3;
        var sn = number / Mathf.Pow(10f, length);
        string k = string.Empty;
        if (length >= 27) {
            //要转AA BB CC
            //ASCII 65 -> A
            int AAsciiCode = 65;
            //计算3的位数
            int l = (length - 27)/3;
            //计算AABBCC的位数
            int w = Mathf.FloorToInt((float)l/26f);
            int ii = (w > 0) ? 2 : 0;
            while (w > 26) {
                w = Mathf.FloorToInt((float)w/26f);
                ii++;
            }
            var lll = ii == 0 ? 10 : ii;
            byte[] byteArray = new byte[lll];  
            if (ii == 0) {
                //默认带A 
                byteArray[0] = (byte)(AAsciiCode);
                byteArray[1] = (byte)(AAsciiCode + l);
            } else {
                while (ii > 0) {
                    int code = l%26;
                    byteArray[ii-1] = (byte)(AAsciiCode + code);
                    l = Mathf.FloorToInt((float)l / 26f) - 1;
                    ii--;
                }
            }
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();  
            k = asciiEncoding.GetString(byteArray);
            k = k.Replace("\0", "");
        } else {
            k = MPDict.stringValue(length);
        }
        //判断小数点位数 
        //确保保证 是4位数 
        //10000 => 10.0k
        //100000 => 1.00M
        //1000000 => 10.0M 
        string snumber = sn.toString();
        //没有小数点
        if (!snumber.Contains(".")) {
            if (snumber.Length == 1) {
                return string.Format("{0}.00{1}", snumber, k);
            } else if (snumber.Length == 2) {
                return string.Format("{0}.0{1}", snumber, k);
            } else {
                return string.Format("{0}{1}", snumber, k);
            }
        } else {
            //拆分
            string[] strArr = snumber.Split('.');
            if (strArr.Length == 1 && snumber.IndexOf('.') == 0) {
                string decimalStr = strArr[0];
                decimalStr = decimalStr.PadRight(2, '0');
                decimalStr = decimalStr.Substring(0, 2);
                return string.Format("{0}{1}", string.Format("0.{0}", decimalStr), k);
            } else {
                //小数点前部分
                string numStr = strArr[0];
                //小数点后部分
                string decimalStr = strArr.Length == 1 ? "0": strArr[1];
                //如果前部分大于等于3以上 则后面部分全部不显示 不足3则根据后部分补0裁剪到合适
                if (numStr.Length == 1) {
                    decimalStr = decimalStr.PadRight(2, '0');
                    decimalStr = decimalStr.Substring(0, 2);
                    return string.Format("{0}{1}", string.Format("{0}.{1}", strArr[0], decimalStr), k);
                } else if (numStr.Length == 2) {
                    decimalStr = decimalStr.PadRight(1, '0');
                    decimalStr = decimalStr.Substring(0, 1);
                    return string.Format("{0}{1}", string.Format("{0}.{1}", strArr[0], decimalStr), k);
                } else {
                    return string.Format("{0}{1}", numStr, k);
                }
            }

        } 
    }

    /// <summary>
    /// 计算战斗力
    /// </summary>
    /// <returns>The power string.</returns>
    /// <param name="obj">Object.</param>
    public static string toPowerString(this object obj)
    {
        return obj.toString();
        //暂时屏蔽
//        long number = obj.toLong();
//        if (number < 9999) {
//            return number.toString();
//        }
//        else if (number < 99999) {
//            long n = 10000 + (number - 10000) * 5 / 100;
//            return n.toString();
//        }
//        else if (number < 999999) {
//            long n = 10000 + (100000 - 10000) * 5 / 100 + (number - 100000) * 5 / 1000;
//            return n.toString();
//        } else {
//            long n = 10000 + (100000 - 10000) * 5 / 100 + (1000000 - 100000) * 5 / 1000 + (number - 1000000) * 1 / 10000;
//            return n.toString();
//        } 
    }
    
    public static string cutMidStr(this string source, string startStr, string stopStr)
    {
        if (source.IsNullOrEmpty()) {
            return string.Empty;
        }

        if (!source.Contains(startStr)) {
            return string.Empty;
        }
        
        var startIndex = source.IndexOf(startStr);
        if (startIndex < 0) {
            return string.Empty;
        }

        string tmpstr = source.Substring(startIndex + startStr.Length);
        if (!source.Contains(stopStr)) {
            return string.Empty;
        }
        var stopIndex = tmpstr.IndexOf(stopStr);
        if (stopIndex < 0) {
            return string.Empty;
        }
        var result = tmpstr.Remove(stopIndex);

        return result;
    }
}