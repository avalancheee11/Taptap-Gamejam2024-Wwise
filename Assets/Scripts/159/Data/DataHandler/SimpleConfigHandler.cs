using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CustomJson.MiniJSON;
using UnityEngine;

public interface IConfigHandler
{
    void LoadConfig();
}

public abstract class ConfigHandler<T> : IConfigHandler where T : new()
{ 
    public virtual string module => String.Empty;

    //实例
    private static T _instance = new T();

    public static T Instance {get {return _instance;}}

    protected Dictionary<string,object> _config;

    //初始化抽象方法 需要子类去实现这个方法
    protected abstract void OnInit();
    /// <summary>
    /// Initializes a new instance of the <see cref="DataHandler"/> class.
    /// </summary>
    protected ConfigHandler()
    {
        OnInit();
    }
    
    /// <summary>
    /// Loads the config.
    /// </summary>
    public void LoadConfig()
    {
        UnityEngine.Debug.Log("init " + typeof(T));
        var text = (this.module + ".json").loadResourceByAssetBundle<TextAsset>()?.text;
        if (text.IsNullOrEmpty()) {
            var url = Path.Combine(CustomGlobalConfig.ConfigStreamingAssetBasePath, this.module + ".json");
            url = Path.Combine(CustomGlobalConfig.ConfigPersistentDataPath, this.module + ".json");
            text = FileUtils.Instance.readFileStrByUnityWebRequest(url);
        }
        
        Debug.Log(this.GetType() + "  LoadConfig  Path:" + this.module + ".json");
        this.OnLoadConfig(text);    
    }
    
    /// <summary>
    /// Raises the load config event.
    /// </summary>
    protected virtual void OnLoadConfig(string text)
    {
        if (text.IsNullOrEmpty()) {
            _config = new Dictionary<string, object>();
        }
        else {
            _config = Json.Deserialize(text) as Dictionary<string, object>;
        }
    }
}

public class SimpleConfigHandler<A, C> : ConfigHandler<C> where A : NSConfigObject, new() where C : SimpleConfigHandler<A, C>, new()
{
    public A configRoot { get; private set; }
    
    protected override void OnInit()
    {
    }

    protected override void OnLoadConfig(string text)
    {
        base.OnLoadConfig(text);

        this.configRoot = new A();
        this.configRoot.initialize(_config);
    }
}

public class SimpleScriptHandler<A, B, C> : ConfigHandler<C> where B : ScriptableObject, new () where A : ScriptableConfigRoot<B>, new() where C : SimpleScriptHandler<A, B, C>, new()
{
    public A configRoot { get; private set; }
    
    protected override void OnInit()
    {
        this.configRoot = Resources.Load<ScriptableObject>("AnimeScriptObject/" + this.module) as A;
    }

    protected override void OnLoadConfig(string text)
    {
        base.OnLoadConfig(text);
        if (this.configRoot != null) {
            this.configRoot.OnInit();
        }
        else {
            Debug.LogError(this.module + " : AnimeScriptObject null" );
        }
    }
}