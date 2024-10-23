using System;
using System.Collections.Generic;

public class SimpleDataRootType
{
    public const int Dict = 0;
    public const int List = 1;
}

public class SimpleDataRoot<A, B, C> : UnitSimpleData<C> where A : NSConfigObject, new() where B : UnitSimpleData<A>, new() where C : SimpleConfigRoot<A>, new()
{
    public Dictionary<int, B> datas { get; private set; }
    public List<B> dataList { get; private set; }
    protected virtual string key => "data";
    public B getData(int id) => this.datas.objectValue(id);
    public virtual int dataRootType => SimpleDataRootType.Dict;

    public override void initialize()
    {
        base.initialize();

        this.datas = new Dictionary<int, B>();
    }

    public override void reloadDataByConfig(C config)
    {
        base.reloadDataByConfig(config);
        
        if (this.dataRootType == SimpleDataRootType.Dict) {
            this.loadUserDataByDict(null, this.key, this.datas, this.config.configs);
        }
        else if (this.dataRootType == SimpleDataRootType.List) {
            this.loadUserDataByList(null, this.key, this.datas, this.config.configs);
        }
        
        this.dataList = this.datas.getValuesList();
        this.dataList.Sort((x, y) => x.id.CompareTo(y.id));
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, C config)
    {
        base.reloadDataByData(parameters, config);
        
        if (this.dataRootType == SimpleDataRootType.Dict) {
            this.loadUserDataByDict(parameters, this.key, this.datas, this.config.configs);
        }
        else if (this.dataRootType == SimpleDataRootType.List) {
            this.loadUserDataByList(parameters, this.key, this.datas, this.config.configs);
        }
        
        this.dataList = this.datas.getValuesList();
        this.dataList.Sort((x, y) => x.id.CompareTo(y.id));
    }

    public override Dictionary<string, object> toCache()
    {
        var dict = new Dictionary<string, object>();
        foreach (var kvp in this.datas) {
            dict[kvp.Key.ToString()] = kvp.Value.toCache();
        }
        this.cacheData[key] = dict;
        return base.toCache();
    }
}

