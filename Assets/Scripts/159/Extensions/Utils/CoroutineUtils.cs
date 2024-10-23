using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineUtils : MonoBehaviour
{
    private static object syncRoot = new object();

    private static CoroutineUtils _instance;

    public static CoroutineUtils Instance {
        get {
            lock (syncRoot)
            {
                if (_instance == null) {
                    GameObject go = new GameObject("CoroutineUtils");
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineUtils>();
                }
            }
            return _instance;
        }
    }
}