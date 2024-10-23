using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CacheManager : SingletonData<CacheManager>
{
    private GameObject cacheObject;

    private Dictionary<string, Stack<GameObject>> gameobjectStack;

    protected override void OnInit()
    {
        this.gameobjectStack = new Dictionary<string, Stack<GameObject>>();
        this.cacheObject = new GameObject("cacheObject");
        this.cacheObject.AddComponent<Canvas>();
        GameObject.DontDestroyOnLoad(this.cacheObject);
        this.cacheObject.SetActive(false);
        
        this.initAssetBundle();
    }

    public void clear()
    {
        this.clearAssetBundle();

        Resources.UnloadUnusedAssets();
    }

    public GameObject popGameObject(string key, GameObject prafab, Transform parent)
    {
        var s = this.gameobjectStack.objectValue(key);
        if (s == null) {
            s = new Stack<GameObject>();
            this.gameobjectStack[key] = s;
        }
        
        if (s.Count == 0) {
            return GameObject.Instantiate(prafab, parent);
        }

        var o = s.Pop();
        o.transform.SetParent(parent);
        o.transform.localScale = Vector3.one;
        o.transform.localPosition = Vector3.zero;
        o.transform.localEulerAngles = Vector3.zero;
        return o;
    }

    // public GameObject popGameObjectByAssetBundle(string key, string prefabName, Transform parent)
    // {
    //     var s = this.gameobjectStack.objectValue(key);
    //     if (s == null) {
    //         s = new Stack<GameObject>();
    //         this.gameobjectStack[key] = s;
    //     }
    //     
    //     if (s.Count == 0) {
    //         var prefab = CacheManager.Instance.loadResourceByAssetBundle<GameObject>(prefabName + ".prefab");
    //         return GameObject.Instantiate(prefab, parent);
    //     }
    //
    //     var o = s.Pop();
    //     o.transform.SetParent(parent);
    //     o.transform.localScale = Vector3.one;
    //     o.transform.localPosition = Vector3.zero;
    //     o.transform.localEulerAngles = Vector3.zero;
    //     return o;
    // }
    
    public void pushGameobject(string key, GameObject obj)
    {
        obj.transform.SetParent(this.cacheObject.transform);
        var s = this.gameobjectStack.objectValue(key);
        if (s == null) {
            s = new Stack<GameObject>();
            this.gameobjectStack[key] = s;
        }
        s.Push(obj);
    }

    public T popCompent<T>(string key, T prafab, Transform parent) where T : Component
    {

        var o = this.popGameObject(key, prafab.gameObject, parent);
        var com = o.GetComponent<T>();
        foreach (var child in com.GetComponentsInChildren<CustomUIComponent>(true)) {
            if (child == null) {
                continue;
            }
            if (child.unused) {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        
        foreach (var child in com.GetComponentsInChildren<CustomUIComponent>(true)) {
            child.startComponent();
        }
        return com;
    }

    public void pushCompent<T>(string key, T o) where T : Component
    {
        foreach (var child in o.GetComponentsInChildren<CustomUIComponent>(true)) {
            child.stopComponent();
        }
        this.pushGameobject(key, o.gameObject);
    }
}
