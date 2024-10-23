using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListPool<T>
    {
        static readonly ObjectPool<List<T>> pool = new ObjectPool<List<T>>();

        public static List<T> Get()
        {
            return pool.Get();
        }

        public static List<T> Get(params T[] datas)
        {
            List<T> result = pool.Get();
            result.AddRange(datas);
            return result;
        }

        public static List<T> Get(IEnumerable<T> datas)
        {
            List<T> result = pool.Get();
            result.AddRange(datas);
            return result;
        }

        public static List<T> Get(params IEnumerable<T>[] datas)
        {
            List<T> result = pool.Get();
            for (int i = 0; i < datas.Length; i++)
            {
                result.AddRange(datas[i]);
            }
            return result;
        }

        public static void Release(ref List<T> list)
        {
            list.Clear();
            pool.Release(ref list);
        }
        
        public static void SimpleIterate(Action<T> action, ListProvideFunc listSource)
        {
            List<T> list = Get();
            listSource(ref list);
            DoSimpleIterate(action, list);
            Release(ref list);
        }

        public static void SimpleIterate(Action<T> action, params IEnumerable<T>[] datas)
        {
            List<T> list = Get(datas);
            DoSimpleIterate(action, list);
            Release(ref list);
        }

        public static void SimpleIterate(Action<T> action, params T[] datas)
        {
            List<T> list = Get(datas);
            DoSimpleIterate(action, list);
            Release(ref list);
        }

        public static void SimpleIterate(Action<T> action, List<T> datas)
        {
            List<T> list = Get(datas);
            DoSimpleIterate(action, list);
            Release(ref list);
        }

        static void DoSimpleIterate(Action<T> action, List<T> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                T data = datas[i];
                action(data);
            }
        }

        public delegate void ListProvideFunc(ref List<T> datas);
    }