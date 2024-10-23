using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CustomJson;
using CustomJson.MiniJSON;
using UnityEngine;

public abstract class DataHandler<A> : IDataHandler where A : new()
{
    public string module { get; }
    public bool configLoadOver { get; set; }
    public bool saveByUpdate { get; set; }

    //key 名称
    public Dictionary<string, object> cacheData;

    //实例
    private static A _instance = new A();

    public static A Instance => _instance;
    
    protected DataHandler()
    {
        this.cacheData = new Dictionary<string, object>();
        OnInit();
    }

    protected virtual void OnInit()
    {
        this.configLoadOver = false;
        this.loadConfig();
    }

    protected virtual void loadConfig()
    {
        
    }

    public void reloadData(Dictionary<string, object> parameters)
    {
        this.reloadDataByConfig();
        this.reloadDataByData(parameters);
    }

    public virtual void reloadDataByConfig()
    {
        
    }

    public virtual void reloadDataByData(Dictionary<string, object> parameters)
    {
        
    }
    
    public virtual Dictionary<string, object> toCache()
    {
        return this.cacheData;
    }

    public Dictionary<string, object> getCache()
    {
        return this.cacheData;
    }

    public void saveAction()
    {
        this.saveByUpdate = true;
    }

    public void dropAction()
    {
        this.cacheData.Clear();
        this.saveByUpdate = true;
    }
}

public class SimpleDataHandler<A, B, C> : DataHandler<C> where A : NSConfigObject, new() where B : UnitSimpleData<A>, new() where C : SimpleDataHandler<A, B, C>, new()
{
    public A configRoot { get; private set; }
    public B dataRoot { get; private set; }

    public virtual string module => String.Empty;
    private Dictionary<string, object> _config;

    protected override void OnInit()
    {
        this._config = new Dictionary<string, object>();
        //加载
        base.OnInit();
    }

    protected override void loadConfig()
    {
        base.loadConfig();
        var text = (this.module + ".json").loadResourceByAssetBundle<TextAsset>()?.text;
        if (text.IsNullOrEmpty()) {
            var url = Path.Combine(CustomGlobalConfig.ConfigStreamingAssetBasePath, this.module + ".json");
            url = Path.Combine(CustomGlobalConfig.ConfigPersistentDataPath, this.module + ".json");
            text = FileUtils.Instance.readFileStrByUnityWebRequest(url);
        }
        Debug.Log(this.GetType() + "  LoadData  Path:" + this.module + ".json");

        if (text.IsNullOrEmpty()) {
            this._config = new Dictionary<string, object>();
        }
        else {
            _config = (Dictionary<string, object>) Json.Deserialize(text);
        }
        this.configRoot = new A();
        this.configRoot.initialize(_config);
        this.configLoadOver = true;
    }

    public override void reloadDataByData(Dictionary<string, object> parameters)
    {
        base.reloadDataByData(parameters);
        this.cacheData = parameters;
        this.dataRoot = new B();
        this.dataRoot.reloadData(cacheData, this.configRoot);
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheData = this.dataRoot.toCache();
        return base.toCache();
    }
}