using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : new()
{
    Stack<T> datas = new Stack<T>();

    public T Get()
    {
        T result;
        if (datas.Count <= 0)
        {
            result = new T();
        }
        else
        {
            result = datas.Pop();
        }
        return result;
    }

    public void Release(ref T t)
    {
        if (t == null)
        {
            Debug.LogError($"invalid data {t.GetType()}!");
            return;
        }
        Debug.Assert(!datas.Contains(t));
        datas.Push(t);
        t = default;
    }
}
