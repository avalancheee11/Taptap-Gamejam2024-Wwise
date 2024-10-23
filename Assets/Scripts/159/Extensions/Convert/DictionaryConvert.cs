using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DictionaryConvert
{
    public static A[] getKeys<A,B>(this Dictionary<A,B> dictionary)
    {
        A[] keys = new A[dictionary.Keys.Count];
        dictionary.Keys.CopyTo(keys, 0);
        return keys;
    }

    public static B[] getValues<A,B>(this Dictionary<A,B> dictionary)
    {
        B[] values = new B[dictionary.Values.Count];
        dictionary.Values.CopyTo(values, 0);
        return values;
    }

    public static List<A> getKeysList<A,B>(this Dictionary<A,B> dictionary)
    {
        return new List<A>(dictionary.Keys);
    }

    public static List<B> getValuesList<A,B>(this Dictionary<A,B> dictionary)
    {
        return new List<B>(dictionary.Values);
    }

    public static void addEntriesFromDictionary<A,B>(this Dictionary<A,B> dictionary, Dictionary<A,B> entries)
    {
        if (dictionary == null || entries == null) {
            return;
        }
        if (dictionary == entries) {
            return;
        }
        foreach (KeyValuePair<A,B> kvp in entries) {
            dictionary[kvp.Key] = kvp.Value;
        }
    }

    public static B objectValue<A,B>(this Dictionary<A,B>dictionary, A key, B defaultValue = default(B))
    {
        if (dictionary == default(Dictionary<A,B>)) return defaultValue;
        B val;
        if (!dictionary.TryGetValue(key, out val)) {
            val = defaultValue;
        }
        return val;
    }

    public static Dictionary<string, object> dictionaryValue<A,B>(this Dictionary<A,B>dictionary, A key, Dictionary<string, object> defaultValue = null)
    {
        return dictionary.dictionaryValue<A,B,object>(key, defaultValue);
    }

    public static Dictionary<string, C> dictionaryValue<A,B,C>(this Dictionary<A,B>dictionary, A key, Dictionary<string, C> defaultValue = null)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toDictionary<C>(defaultValue);
    }

    public static List<object> listValue<A,B>(this Dictionary<A,B>dictionary, A key, List<object> defaultValue = null)
    {
        return dictionary.listValue<A,B,object>(key, defaultValue);
    }

    public static List<C> listValue<C>(this Dictionary<string,object>dictionary, string key, List<C> defaultValue = null)
    {
        return dictionary.listValue<string,object,C>(key, defaultValue);
    }

    public static List<C> listValue<A,B,C>(this Dictionary<A,B>dictionary, A key, List<C> defaultValue = null)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toList<C>(defaultValue);
    }

    public static int intValue<A,B>(this Dictionary<A,B>dictionary, A key, int defaultValue = 0)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toInt(defaultValue);
    }

    public static long longValue<A,B>(this Dictionary<A,B>dictionary, A key, long defaultValue = 0L)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toLong(defaultValue);
    }

    public static float floatValue<A,B>(this Dictionary<A,B>dictionary, A key, float defaultValue = 0f)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A,B>(dictionary, key).toFloat(defaultValue);
    }

    public static double doubleValue<A, B>(this Dictionary<A, B> dictionary, A key, double defaultValue = 0f)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toDouble(defaultValue);
    }

    public static bool boolValue<A,B>(this Dictionary<A,B>dictionary, A key, bool defaultValue = false)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toBool(defaultValue);
    }

    public static string stringValue<A,B>(this Dictionary<A,B>dictionary, A key, string defaultValue = null)
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toString(defaultValue);
    }

    public static Vector2 vector2Value<A,B>(this Dictionary<A,B>dictionary, A key, Vector2 defaultValue = default(Vector2))
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toVector2(defaultValue);
    }

    public static Vector3 vector3Value<A,B>(this Dictionary<A,B>dictionary, A key, Vector3 defaultValue = default(Vector3))
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toVector3(defaultValue);
    }

    public static Vector4 vector4Value<A,B>(this Dictionary<A,B>dictionary, A key, Vector4 defaultValue = default(Vector4))
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toVector4(defaultValue);
    }

    public static Rect rectValue<A,B>(this Dictionary<A,B>dictionary, A key, Rect defaultValue = default(Rect))
    {
        if (dictionary == null) return defaultValue;
        return objectValue<A, B>(dictionary, key).toRect(defaultValue);
    }

    public static Dictionary<A,B> copy<A,B>(this Dictionary<A,B>dictionary)
    {
        return new Dictionary<A, B>(dictionary);
    }

    public static bool containsKey<A,B>(this Dictionary<A,B>dictionary, A key)
    {
        return dictionary == null ? false : dictionary.ContainsKey(key);
    }

    public static bool tryGetBoolValue(this Dictionary<string,object>dictionary, string key)
    {
        if (dictionary == null) {
            return false;
        }
        object obj;
        if (dictionary.TryGetValue(key, out obj)) {
            if (obj.toBool()) {
                return true;
            }
        }
        return false;
    }

    public static bool isEmpty(this Dictionary<string,object>dictionary)
    {
        return (dictionary == null || dictionary.Count == 0);
    }

    public static Dictionary<A,B> filterBy<A,B>(this Dictionary<A,B>dictionary, Dictionary<A,B> filter)
    {
        if (dictionary == null || filter == null) {
            return dictionary;
        }
        foreach (var item in filter) {
            dictionary.Remove(item.Key);
        }
        return dictionary;
    }

    public static List<int> intListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<int>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                list.Add(item.toInt());
            }
        }
        return list;
    }

    public static List<float> floatListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<float>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                list.Add(item.toFloat());
            }
        }
        return list;
    }

    public static List<double> doubleListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<double>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                list.Add(item.toDouble());
            }
        }
        return list;
    }

    public static List<string> stringListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<string>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                list.Add(item.toString());
            }
        }
        return list;
    }

    public static List<T> objectListValue<T>(this Dictionary<string, object> dictionary, string key) where T : NSConfigObject, new()
    {
        var list = new List<T>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var o = new T();
                o.initialize(item.toDictionary());
                list.Add(o);
            }
        }
        return list;
    }

    public static A objectData<A, B>(this Dictionary<string, object> dictionary, string key, Dictionary<int, B> configs, B defaultConfig) where A : UnitSimpleData<B>, new() where B : NSConfigObject, new()
    {
        var data = new A();
        var para = dictionary.dictionaryValue(key);
        if (para == null) {
            data.reloadData(null, defaultConfig);
            return data;
        }

        var config = configs.objectValue(para.intValue("id"));
        if (config == null) {
            data.reloadData(null, defaultConfig);
            return data;
        }

        data.reloadData(para, config);
        return data;
    }

    public static A objectConfig<A>(this Dictionary<string, object> dictionary, string key) where A : NSConfigObject, new()
    {
        var para = dictionary.dictionaryValue(key);
        var con = new A();
        if (para != null) {
            con.initialize(para);
        }
        else {
            con.initialize(new Dictionary<string, object>());
        }

        return con;
    }

    public static List<List<int>> intListListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<List<int>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null)
        {
            foreach (var item in valueList)
            {
                var itemList = new List<int>();
                var intList = item.toList();
                foreach (var intItem in intList)
                {
                    itemList.Add(intItem.toInt());
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static List<List<bool>> boolListListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<List<bool>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null)
        {
            foreach (var item in valueList)
            {
                var itemList = new List<bool>();
                var intList = item.toList();
                foreach (var intItem in intList)
                {
                    itemList.Add(intItem.toBool());
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static List<List<float>> floatListListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<List<float>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var itemList = new List<float>();
                var intList = item.toList();
                foreach (var intItem in intList) {
                    itemList.Add(intItem.toFloat());
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static List<List<double>> doubleListListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<List<double>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null)
        {
            foreach (var item in valueList)
            {
                var itemList = new List<double>();
                var intList = item.toList();
                foreach (var intItem in intList)
                {
                    itemList.Add(intItem.toDouble());
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static List<List<string>> stringListListValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = new List<List<string>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var itemList = new List<string>();
                var intList = item.toList();
                foreach (var intItem in intList) {
                    itemList.Add(intItem.toString());
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static List<List<T>> objectListListValue<T>(this Dictionary<string, object> dictionary, string key) where T : NSConfigObject, new()
    {
        var list = new List<List<T>>();
        var valueList = dictionary.listValue(key);
        if (valueList != null)
        {
            foreach (var item in valueList)
            {
                var itemList = new List<T>();
                var objList = item.toList();
                foreach (var objItem in objList)
                {
                    var o = new T();
                    o.initialize(objItem.toDictionary());
                    itemList.Add(o);
                }
                list.Add(itemList);
            }
        }
        return list;
    }

    public static Dictionary<int, T> objectDictValueByList<T>(this Dictionary<string, object> dictionary, string key) where T : NSConfigObject, new()
    {
        var dict = new Dictionary<int, T>();
        var valueList = dictionary.listValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var o = new T();
                o.initialize(item.toDictionary());
                dict[o.id] = o;
            }
        }
        return dict;
    }

    public static Dictionary<int, T> objectDictValueByDict<T>(this Dictionary<string, object> dictionary, string key) where T : NSConfigObject, new()
    {
        var dict = new Dictionary<int, T>();
        var valueList = dictionary.dictionaryValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var o = new T();
                o.initialize(item.Key, item.Value.toDictionary());
                dict[o.id] = o;
            }
        }
        return dict;
    }

    public static Dictionary<string, T> objectDictValueByStringDict<T>(this Dictionary<string, object> dictionary, string key) where T : NSConfigObject, new()
    {
        var dict = new Dictionary<string, T>();
        var valueList = dictionary.dictionaryValue(key);
        if (valueList != null) {
            foreach (var item in valueList) {
                var o = new T();
                o.initialize(item.Key, item.Value.toDictionary());
                dict[item.Key] = o;
            }
        }
        return dict;
    }

    public static Vector2 vector2ListValue<A, B>(this Dictionary<A, B> dictionary, A key, Vector2 defaultValue = default(Vector2))
    {
        if (dictionary == null) return defaultValue;
        var list = listValue<A, B>(dictionary, key);
        if (list == null) return defaultValue;
        return new Vector2(list.floatValue(0), list.floatValue(1));
    }

    public static Dictionary<int, int> intDictValue(this Dictionary<string, object> dictionary, string key)
    {
        var intDict = new Dictionary<int, int>();
        var valueDict = dictionary.dictionaryValue(key);
        if (valueDict != null)
        {
            foreach (var item in valueDict)
            {
                intDict[item.Key.toInt()] = item.Value.toInt();
            }
        }
        return intDict;
    }

    public static Dictionary<int, float> intFloatDictValue(this Dictionary<string, object> dictionary, string key)
    {
        var intDict = new Dictionary<int, float>();
        var valueDict = dictionary.dictionaryValue(key);
        if (valueDict != null) {
            foreach (var item in valueDict) {
                intDict[item.Key.toInt()] = item.Value.toFloat();
            }
        }
        return intDict;
    }

    public static Dictionary<string, int> stringIntDictValue(this Dictionary<string, object> dictionary, string key)
    {
        var strDict = new Dictionary<string, int>();
        var valueDict = dictionary.dictionaryValue(key);
        if (valueDict != null) {
            foreach (var item in valueDict) {
                strDict[item.Key] = item.Value.toInt();
            }
        }
        return strDict;
    }

    public static Dictionary<string, string> stringStringDictValue(this Dictionary<string, object> dictionary, string key)
    {
        var strDict = new Dictionary<string, string>();
        var valueDict = dictionary.dictionaryValue(key);
        if (valueDict != null) {
            foreach (var item in valueDict) {
                strDict[item.Key] = item.Value.toString();
            }
        }
        return strDict;
    }
    
    public static RangeObject<int> intRangeValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = dictionary.intListValue(key);
        if (list.Count >= 2) {
            return new RangeObject<int>(list.intValue(0), list.intValue(1));
        }
        else if (list.Count == 1) {
            return new RangeObject<int>(list.intValue(0));
        }
        else {
            return new RangeObject<int>(dictionary.intValue(key));
        }
    }

    public static RangeObject<int> intRangeValueByDict(this Dictionary<string, object> dictionary, string key)
    {
        var p = dictionary.dictionaryValue(key);
        return new RangeObject<int>(p.intValue("min"), p.intValue("max"));
    }

    public static RangeObject<float> floatRangeValue(this Dictionary<string, object> dictionary, string key)
    {
        var list = dictionary.floatListValue(key);
        if (list.Count >= 2) {
            return new RangeObject<float>(list.floatValue(0), list.floatValue(1));
        }
        else if (list.Count == 1) {
            return new RangeObject<float>(list.floatValue(0));
        }
        else {
            return new RangeObject<float>(dictionary.floatValue(key));
        }
    }

    public static RangeObject<float> floatRangeValueByDict(this Dictionary<string, object> dictionary, string key)
    {
        var p = dictionary.dictionaryValue(key);
        return new RangeObject<float>(p.floatValue("min"), p.floatValue("max"));
    }

    public static string serialize(this List<object> list)
    {
        string sz = string.Empty;
        foreach (var value in list)
        {
            string valueStr = null;
            if (value is Dictionary<string, object>)
            {
                valueStr = (value.toDictionary()).serialize();
            }
            else if (value is List<object>)
            {
                valueStr = (value.toList()).serialize();
            }
            else if (value is string)
            {
                valueStr = "\"" + value.toString() + "\"";
            }
            else
            {
                valueStr = value.toString();
            }
            sz += string.Format("{0},", valueStr);
        }
        sz = "[" + sz + "]";
        return sz;
    }

    public static string serialize(this Dictionary<string,object> dictionary)
    {
        string sz = string.Empty;

        foreach (var kvp in dictionary)
        {
            string valueStr = null;
            if (kvp.Value is Dictionary<string, object>)
            {
                valueStr = (kvp.Value.toDictionary()).serialize();
            }
            else if (kvp.Value is List<object>)
            {
                valueStr = (kvp.Value.toList()).serialize();
            }
            else if (kvp.Value is string)
            {
                valueStr ="\""+ kvp.Value.toString() +"\"";
            }
            else if (kvp.Value is bool)
            {
                valueStr = ((bool)kvp.Value) ? "true" : "false";
            }
            else
            {
                valueStr = kvp.Value.toString();
            }
            sz += string.Format("\"{0}\":{1},", kvp.Key, valueStr);
        }
        sz = "{" + sz + "}";
        return sz;
    }

    public static Vector2 vec2Value(this Dictionary<string,object> dictionary, string key)
    {
        var floatList = dictionary.floatListValue(key);
        if (floatList.Count < 2) {
            return Vector2.zero;
        }
        return new Vector2(floatList.floatValue(0), floatList.floatValue(1));
    }

    public static Vector2 vec2Value(this Dictionary<string, object> dictionary, string key,Vector2 defaultValue)
    {
        var floatList = dictionary.floatListValue(key);
        if (floatList.Count < 2)
        {
            return defaultValue;
        }
        return new Vector2(floatList.floatValue(0), floatList.floatValue(1));
    }

    public static Vector4 vec4Value(this Dictionary<string,object> dictionary, string key)
    {
        var floatList = dictionary.floatListValue(key);
        if (floatList.Count < 4) {
            return Vector4.zero;
        }
        return new Vector4(floatList.floatValue(0), floatList.floatValue(1), floatList.floatValue(2), floatList.floatValue(3));
    }

    public static List<Vector2> vec2ListValue(this Dictionary<string,object> dictionary,string key)
    {
        var ret = new List<Vector2>();
        var list = dictionary.floatListListValue(key);
        if (list == null) return ret;
        foreach (var item in list)
        {
            var vec = new Vector2(item.floatValue(0), item.floatValue(1));
            ret.Add(vec);
        }
        return ret;
    }
}
