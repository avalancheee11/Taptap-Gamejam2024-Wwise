using System;
using System.Collections.Generic;

public class SimpleConfigRoot<T> : NSConfigObject where T : NSConfigObject, new()
{
    public Dictionary<int, T> configs { get; private set; }
    public List<T> configList { get; private set; }
    protected virtual string key => "config";
    public T getConfig(int id) => this.configs.objectValue(id);

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);

        this.configs = parameters.objectDictValueByDict<T>(this.key);
        this.configList = this.configs.getValuesList();
        this.configList.Sort((x, y) => { return x.id.CompareTo(y.id); });
    }
}