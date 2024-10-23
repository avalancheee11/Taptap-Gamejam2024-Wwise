using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryIntSimpleData : UnitSimpleData<NSConfigObject>
{
    private Dictionary<int, int> dic;
    
    public override void initialize()
    {
        base.initialize();
        this.dic = new Dictionary<int, int>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, NSConfigObject config)
    {
        base.reloadDataByData(parameters, config);
        this.dic = parameters.intDictValue("dic");
    }

    public override Dictionary<string, object> toCache()
    {
        var d = new Dictionary<string, object>();
        foreach (var kvp in this.dic) {
            d[kvp.Key.toString()] = kvp.Value;
        }

        this.cacheData["dic"] = d;
        return base.toCache();
    }

    public int getValue(int key)
    {
        return this.dic.objectValue(key);
    }

    public void setValue(int key, int value)
    {
        this.dic[key] = value;
    }

    public void addValue(int key, int value)
    {
        this.dic[key] = this.dic.objectValue(key) + value;
    }

    public void reduceValue(int key, int value)
    {
        this.dic[key] = this.dic.objectValue(key) - value;
    }
}
