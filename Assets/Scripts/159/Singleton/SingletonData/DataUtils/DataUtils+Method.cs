using System;
using System.Collections.Generic;
using System.Reflection;

public partial class DataUtils
{
    Dictionary<string, MethodInfo> _methods;

    void OnInitMethod()
    {
        _methods = new Dictionary<string, MethodInfo>();
    }

    MethodInfo getMethodByParameter(object obj, string methodString, object[] os)
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
        return getMethodByParameter(obj, methodString, ts, false);
    }

    MethodInfo getMethodByParameter(object obj, string methodString, Type[] ts, bool simple)
    {
        string name = obj.GetType().Name + methodString;
        if (ts != null && ts.Length > 0) {
            foreach (var t in ts) {
                name += t.Name;
            }
        }
        MethodInfo o;
        if (_methods.TryGetValue(name, out o)) {
            return o;
        }
        var method = obj.getMethodInfo(methodString, ts, simple);
        _methods[name] = method;
        return method;
    }

    public static MethodInfo getMethod(object obj, string methodString, bool simple = false)
    {
        return Instance.getMethodByParameter(obj, methodString, null, simple);
    }

    public static MethodInfo getMethod(object obj, string methodString, object[] os)
    {
        return Instance.getMethodByParameter(obj, methodString, os);
    }

    void methodClear()
    {
        UnityEngine.Debug.Log("methodClear.methods" + _methods.Count);
        _methods.Clear();
    }
}

