using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomJson;
using CustomJson.MiniJSON;
using GameLib;
using UnityEngine;
using XHFrameWork;

public class DataBaseManager : SingletonData<DataBaseManager>
{
    public enum SaveType
    {
        PlayerPref,
        StreamAssets,
    }

    public Dictionary<string, IDataHandler> dataHandlers { get; private set; }
    public Dictionary<string, IConfigHandler> configHandlers { get; private set; }
    public IDatabaseAccess databaseAccess;
    
    public static SaveType saveType = SaveType.StreamAssets;
    public bool loadOver { get; private set; }
    public string XdMdm = "NQ0BxeS1JRmaXzY7uT25uDfXQ0GbaL2dm9AbHcXYjwrY7p0un1x0hRk1FkntZuQgPt4yxVyyjnjYCyCB6zAFOeS4Icp6R9944UoMkePaWbEiTcsZVaykcBm3eQz3JUsOyjoyl3w48lusp33BwrbEe3M8BXYgvVte";
    private static bool SupportSecret = true;
    
    protected override void OnInit()
    {
        
    }

    public void load()
    {
        this.loadOver = false;
        if (saveType == SaveType.PlayerPref) {
            this.databaseAccess = new PlayerPrefAccess();
        }
        else if (saveType == SaveType.StreamAssets) {
            this.databaseAccess = new StreamingAccess();
        }

        this.dataHandlers = new Dictionary<string, IDataHandler>();
        this.configHandlers = new Dictionary<string, IConfigHandler>();

        //加载配置
        foreach (var moduleName in DataModule.ScriptModuleNameList) {
            var className = DataModule.DataHandlerClassNames.objectValue(moduleName);
            if (string.IsNullOrEmpty(className)) {
                continue;
            }

            var type = Type.GetType(className);
            var configHandler = type.getStaticProperty<IConfigHandler>("Instance");
            configHandler.LoadConfig();
            this.configHandlers[moduleName] = configHandler;
        }
        
        foreach (var moduleName in DataModule.ConfigModuleNameList) {
            var className = DataModule.DataHandlerClassNames.objectValue(moduleName);
            if (string.IsNullOrEmpty(className)) {
                continue;
            }

            var type = Type.GetType(className);
            var configHandler = type.getStaticProperty<IConfigHandler>("Instance");
            configHandler.LoadConfig();
            this.configHandlers[moduleName] = configHandler;
        }
        
        this.databaseAccess.openDBAsync(() =>
        {
            foreach (var moduleName in DataModule.DataModuleNameList) {
                var className = DataModule.DataHandlerClassNames.objectValue(moduleName);
                if (string.IsNullOrEmpty(className)) {
                    Debug.LogError("找不到目标配置的className");
                    continue;
                }

                var type = Type.GetType(className);
                var dataHandler = type.getStaticProperty<IDataHandler>("Instance");
                var str = this.getValue(moduleName);
                if (string.IsNullOrEmpty(str)) {
                    dataHandler.reloadData(null);
                }
                else {
                    var dic = Json.Deserialize(str) as Dictionary<string, object>;
                    dataHandler.reloadData(dic);
                }

                this.dataHandlers[moduleName] = dataHandler;
            }

            this.databaseAccess.closeDB();

            this.loadOver = true;
        });
    }

    public void start()
    {
        
    }

    public void update(float dt)
    {
        foreach (var dataHandler in this.dataHandlers) {
            if (dataHandler.Value.saveByUpdate) {
                dataHandler.Value.saveByUpdate = false;
                var dic = dataHandler.Value.toCache();
                var str  = Json.Serialize(dic);
                this.setValue(dataHandler.Key, str);
            }
        }
        
        this.databaseAccess.update(dt);
    }

    public void stop()
    {
        
    }

    public string getValue(string key)
    {
        var value = this.databaseAccess.getValue(key);
        if (value.IsNullOrEmpty()) {
            return value;
        }
        if (SupportSecret) {
            try {
                //
                var dic = MiniJson.JsonDecode(value) as Dictionary<string, object>;
                if (dic != null) {
                    return value;
                }
            }
            catch (Exception e) {
                
            }
            return XXTEA.Decrypt(value, this.XdMdm);
        }

        return value;
    }
    
    public void setValue(string key, string value)
    {
// #if UNITY_EDITOR
//         this.databaseAccess.setValue(key, value);
//         return;
// #endif
        var str = value;
        if (SupportSecret) {
            str = XXTEA.Encrypt(value, this.XdMdm);
        }
        this.databaseAccess.setValue(key, str);
    }

    public Dictionary<string, object> getParameterByString(string value)
    {
        if (value.IsNullOrEmpty()) {
            return new Dictionary<string, object>();
        }
        if (SupportSecret) {
            try {
                //
                var dic = MiniJson.JsonDecode(value) as Dictionary<string, object>;
                if (dic != null) {
                    return dic;
                }
            }
            catch (Exception e) {
                
            }
            var v = XXTEA.Decrypt(value, this.XdMdm);
            var d = MiniJson.JsonDecode(v) as Dictionary<string, object>;
            if (d != null) {
                return d;
            }
        }

        return new Dictionary<string, object>();
    }

    public void cacheModule(string module)
    {
        this.dataHandlers.objectValue(module).saveByUpdate = true;
    }
}
