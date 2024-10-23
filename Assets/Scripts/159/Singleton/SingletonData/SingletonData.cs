using System;

public abstract class SingletonData<T> where T : new()
{
    //初始化抽象方法 需要子类去实现这个方法
    protected abstract void OnInit();

    //实例
    private static T _instance;
    private static object syncRoot = new object();

    public static T Instance
    {  
        get
        {  
            lock (syncRoot)
            {
                if (null == _instance)
                    _instance = new T();
            }
            return _instance;  
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SingletonData"/> class.
    /// </summary>
    protected SingletonData()
    {
        UnityEngine.Debug.Log("init " + typeof(T));
        //子类必须实现这个方法
        OnInit();
    }
}
