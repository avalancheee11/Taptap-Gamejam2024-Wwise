using System;
using System.Collections.Generic;
using UnityEngine;

public static class ListConvert
{
    public static T objectValue<T>(this List<T>list, int index)
    {
        if (list == default(List<T>)) return default(T);
        if (index >= 0 && index < list.Count) {
            return list[index];
        }
        return default(T);
    }

    public static Dictionary<string,object> dictionaryValue<T>(this List<T>list, int index)
    {
        return list.dictionaryValue<T, object>(index);
    }

    public static Dictionary<string,B> dictionaryValue<A,B>(this List<A>list, int index)
    {
        if (list == null) return null;
        return objectValue<A>(list, index).toDictionary<B>();
    }

    public static List<T> listValue<T>(this List<List<T>> list, int index, List<T> defaultValue = null)
    {
        if (list == null) return defaultValue;
        if (index < 0 || index >= list.Count) return defaultValue;
        return list[index];
    }

    public static List<B> listValue<A,B>(this List<A>list, int index)
    {
        if (list == null) return null;
        return objectValue<A>(list, index).toList<B>();
    }

    public static int intValue<T>(this List<T>list, int index)
    {
        if (list == null) return 0;
        return objectValue<T>(list, index).toInt();
    }

    public static int countItem<T>(this List<T> list,Func<T,bool> prediction)
    {
        if (prediction == null || list == null) return IC.NotFound;
        int ret = 0;
        foreach (var item in list)
        {
            if (prediction(item)) ret++;
        }
        return ret;
    }

    

    public static float floatValue<T>(this List<T>list, int index)
    {
        if (list == null) return 0f;
        return objectValue<T>(list, index).toFloat();
    }

    public static bool boolValue<T>(this List<T>list, int index)
    {
        if (list == null) return false;
        return objectValue<T>(list, index).toBool();
    }

    public static string stringValue<T>(this List<T>list, int index)
    {
        if (list == null) return null;
        return objectValue<T>(list, index).toString();
    }

    public static UnityEngine.Vector2 vector2Value<T>(this List<T>list, int index)
    {
        if (list == null) return UnityEngine.Vector2.zero;
        return objectValue<T>(list, index).toVector2();
    }

    public static UnityEngine.Vector3 vector3Value<T>(this List<T>list, int index)
    {
        if (list == null) return UnityEngine.Vector3.zero;
        return objectValue<T>(list, index).toVector3();
    }

    public static UnityEngine.Vector4 vector4Value<T>(this List<T>list, int index)
    {
        if (list == null) return UnityEngine.Vector4.zero;
        return objectValue<T>(list, index).toVector4();
    }

    public static UnityEngine.Rect rectValue<T>(this List<T>list, int index)
    {
        if (list == null) return UnityEngine.Rect.zero;
        return objectValue<T>(list, index).toRect();
    }

    public static List<T> copy<T>(this List<T> thisList)
    {
        if (thisList == null)
            return null;
        return new List<T>(thisList);
    }

    public static int indexOfInt(this List<object> thisList, int i)
    {
        return thisList.FindIndex(s => s.toInt() == i);
    }

    public static bool containsInt(this List<object> thisList, int i)
    {
        return !thisList.indexOfInt(i).isNotFound();
    }

    public static int indexOfFloat(this List<object> thisList, float i)
    {
        return thisList.FindIndex(s => (s.toFloat() - i).isAbsZero());
    }

    public static bool containsFloat(this List<object> thisList, float i)
    {
        return !thisList.indexOfFloat(i).isNotFound();
    }

    public static bool isEmpty(this List<object> thisList)
    {
        if (thisList == null || thisList.Count == 0) {
            return true;
        }
        else {
            return false;
        }
    }

    public static List<object> addList(this List<object> thisList, List<object> otherList)
    {
        if (thisList.isEmpty()) {
            return otherList;
        }
        if (otherList.isEmpty()) {
            return thisList;
        }
        if (thisList == otherList) {
            return thisList;
        }

        List<object> newList = new List<object>();
        int count = thisList.Count > otherList.Count ? thisList.Count : otherList.Count;
        for (int i = 0; i < count; i++) {
            var dict = thisList.dictionaryValue(i);
            if (dict == null) {
                dict = otherList.dictionaryValue(i);
            }
            else {
                dict.addEntriesFromDictionary(otherList.dictionaryValue(i));
            }
            if (dict != null) {
                newList.Add(dict);
            }
        }
        return newList;
    }

    private static System.Random rng = new System.Random();

    /// <summary>
    /// 左闭右开区间
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    public static void shuffle<T>(this IList<T> list,int startIndex = 0,int endIndex = int.MaxValue)
    {
        int n = Mathf.Min(endIndex, list.Count);
        while (n > startIndex + 1) {
            int k = startIndex + (n - startIndex).toCCRandomIndex();
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void sortByRandom<T>(this List<T> thisList)
    {
        var list = new List<T>(thisList);
        thisList.Clear();
        var count = list.Count;
        for (int t = 0; t < count; t++) {
            int idx = list.Count.toCCRandomIndex();
            thisList.Add(list[idx]);
            list.RemoveAt(idx);
        }
    }

    public static T pop<T>(this List<T> thisList,int index = -1)
    {
        if (index == -1)
        {
            index = thisList.Count - 1;
        }
        var ret = thisList[index];
        thisList.RemoveAt(index);
        return ret;
    }

    public static T getRandomOne<T>(this List<T> thisList, bool remove = false)
    {
        if (thisList.Count == 0)
        {
            return default(T);
        }
        int index = thisList.Count.toCCRandomIndex();
        var ret = thisList[index];
        if (remove) thisList.RemoveAt(index);
        return ret;
    }

    public static T getRandomOneByRemove<T>(this List<T> thisList)
    {
        return thisList.getRandomOne<T>(true);
    }

    public static T getRandomOneByWeight<T>(this List<T> thisList, bool remove = false) where T : IWeightObject 
    {
        if (thisList.Count == 0) return default(T);

        var totalWeight = 0;
        foreach (var item in thisList)
        {
            totalWeight += item.weight;
        }
        var randomNumber = totalWeight.toCCRandomIndex();
        T target = default(T);
        var index = 0;
        for (int i = 0; i < thisList.Count; i++)
        {
            var item = thisList[i];
            if (item.weight == 0) continue;

            randomNumber -= item.weight;
            if (randomNumber < 0)
            {
                target = item;
                index = i;
                break;
            }
        }
        if (target != null) {
            if (remove) thisList.RemoveAt(index);
        }
        else {
            UnityEngine.Debug.Log("权重随机失败，未获取到target");
        }
        return target;
    }

    public static T getRandomOneByWeightRemove<T>(this List<T> thisList) where T : IWeightObject
    {
        return thisList.getRandomOneByWeight<T>(true);
    }

    public static T[] getShuffedArrayByWeight<T>(this List<T> thisList) where T : IWeightObject
    {
        List<T> weightArray = new List<T>();
        foreach (var v in thisList)
        {
            for (int i = 0; i < v.weight; ++i)
            {
                weightArray.Add(v);
            }
        }

        for (int i = 0; i < weightArray.Count - 1; ++i)
        {
            int j = i + (weightArray.Count - i).toCCRandomIndex();
            var temp = weightArray[i];
            weightArray[i] = weightArray[j];
            weightArray[j] = temp;
        }

        return weightArray.ToArray();
    }

    public static T back<T>(this List<T> thisList)
    {
        return thisList[thisList.Count - 1];
    }
    /// <summary>
    ///交集
    /// </summary>
    public static List<T> interSect<T>(this List<T> list1,List<T> list2)
        //where T:IEquatable<T>
    {
        var ret = new List<T>();
        foreach (var ele in list1)
        {
            if (!ret.Contains(ele) && list2.Contains(ele))
            {
                ret.Add(ele);
            }
        }
        return ret;
    }

    public static T getRandomSubEle<T>(this List<List<T>> list,bool remove = false)
    {
        List<T> chosenList = null;
        var sumWeight = 0;
        foreach (var l in list)
        {
            var weight = l.Count;
            sumWeight += weight;
            if (sumWeight.toCCRandomIndex() < weight)
            {
                chosenList = l;
            }
        }
        if (chosenList == null) return default;
        return chosenList.getRandomOne(remove);
    }
    
    
    public static void release<T>(this List<T> list)
    {
        ListPool<T>.Release(ref list);
    }
}
