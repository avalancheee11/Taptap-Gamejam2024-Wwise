using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataUtils
{
    Dictionary<string, NSObject> _objects;
    private Dictionary<string, Dictionary<string, object>> resourceObjs;
    
    void OnInitObject()
    {
        _objects = new Dictionary<string, NSObject>();
        this.resourceObjs = new Dictionary<string, Dictionary<string, object>>();
    }
    
    void objectClear()
    {
        UnityEngine.Debug.Log("objectClear.objects" + _objects.Count);
        _objects.Clear();
        this.resourceObjs.Clear();
    }

    public RuntimeAnimatorController getAnimator(string aniName, string path = "animator")
    {
        return this.getObjectByResource<RuntimeAnimatorController>(aniName, path);
    }
    
    public T getObject<T>(string name) where T : NSObject
    {
        UnityEngine.Debug.Log("DataUtils.getObject:" + name);
        NSObject o;
        if (!_objects.TryGetValue(name, out o)) {
            o = this.getActivator<T>(name);
            _objects[name] = o;
        }
        return o.shallowCopy() as T;
    }

    public T getObjectByResource<T>(string name, string path) where T : UnityEngine.Object
    {
        var ts = typeof(T).ToString();
        var os = this.resourceObjs.objectValue(ts);
        if (os == null) {
            os = new Dictionary<string, object>();
            this.resourceObjs[ts] = os;
        }

        var fullPath = Path.Combine(path, name);
        var o = os.objectValue(fullPath);
        if (o == null) {
            o = Resources.Load<T>(fullPath);
        }
        return o as T;
    }
}
