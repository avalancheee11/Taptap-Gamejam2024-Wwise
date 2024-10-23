using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomJson;
using UnityEngine;

public class PlayerPrefAccess : IDatabaseAccess
{
    private string saveKey => "playerDatas";
    private Dictionary<string, string> _datas;
    
    public PlayerPrefAccess()
    {
        this._datas = new Dictionary<string, string>();
    }

    public bool hasDemoFile => false;
    public bool saveByUpdate { get; set; }

    public void openDBAsync(Action callBack)
    {
        if (PlayerPrefs.HasKey(this.saveKey)) {
            var str = PlayerPrefs.GetString(this.saveKey);
            var dic = MiniJson.JsonDecode(str) as Dictionary<string, object>;
            foreach (var kvp in dic) {
                this._datas[kvp.Key] = kvp.Value.toString();
            }
        }
        
        callBack?.Invoke();
    }

    public void closeDB()
    {
        
    }

    public void clearDB()
    {
        PlayerPrefs.DeleteAll();
        this.saveByUpdate = true;
    }

    public string getValue(string key)
    {
        return this._datas.objectValue(key);
    }

    public void setValue(string key, string value)
    {
        this._datas[key] = value;
        this.saveByUpdate = true;
    }

    public void removeValue(string key)
    {
        this._datas.Remove(key);
        this.saveByUpdate = true;
    }

    public void saveDB()
    {
        if (this._datas.Count == 0) {
            return;
        }

        this._datas["lastSaveTime"] = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMinutes.toLong().toString();
        //json方法写入
        var str = MiniJson.JsonEncode(this._datas);
        PlayerPrefs.SetString(this.saveKey, str);
        PlayerPrefs.Save();
    }

    public Dictionary<string, object> exportDB()
    {
        var data = new Dictionary<string, object>();
        foreach (var kvp in this._datas) {
            data[kvp.Key] = kvp.Value;
        }
        return data;
    }

    public void update(float dt)
    {
        if (this.saveByUpdate) {
            PlayerPrefs.Save();
            this.saveByUpdate = false;
        }
    }
}
