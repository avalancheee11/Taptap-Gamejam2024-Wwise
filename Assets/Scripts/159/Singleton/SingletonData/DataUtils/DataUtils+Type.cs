using System;
using System.Collections.Generic;

public partial class DataUtils
{
    Dictionary<string, Type> _types;

    void OnInitType()
    {
        _types = new Dictionary<string, Type>();
    }

    Type getTypeByName(string name)
    {
        Type o;
        if (_types.TryGetValue(name, out o)) {
            return o;
        }
        Type type = Type.GetType(name);
        if (type == null) {
            return null;
        }
        _types[name] = type;
        return type;
    }

    public static Type getType(string name)
    {
        return Instance.getTypeByName(name);
    }

    public T getSkillActivator<T>(Type t, object parameters) where T : class
    {
        return getActivator<T>(t, new Type[1]{typeof(Dictionary<string,object>)}, new object[1]{parameters});
    }

    public T getActivator<T>(string key = default) where T : class
    {
        return this.getActivator<T>(this.getTypeByName(key.IsNullOrEmpty() ? typeof(T).toString() : key));
    }

    public T getActivator<T>(string key, object[] os) where T : class
    {
        return this.getActivator<T>(this.getTypeByName(key), os);
    }
    
    public T getActivator<T>(Type t) where T : class
    {
        //        return getActivator<T>(t, null, new object[1]{ null });
        return getActivator<T>(t, null, null);
    }

    public T getActivator<T>(Type t, object[] os) where T : class
    {
        return Activator.CreateInstance(t, os) as T;
    }
    
    public T getActivator<T>(Type t, Type[] pts, object[] os) where T : class
    {
        return Activator.CreateInstance(t, os) as T;
    }

    void typeClear()
    {
        UnityEngine.Debug.Log("typeClear.types" + _types.Count);
        _types.Clear();
    }
}
