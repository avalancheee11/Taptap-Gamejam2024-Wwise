using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : UnitSimpleData<ItemConfig>
{
    public int count { get; private set; }
    public event Action onCountChangeAction;

    public override void reloadDataByConfig(ItemConfig config)
    {
        base.reloadDataByConfig(config);
        this.count = 10;
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ItemConfig config)
    {
        base.reloadDataByData(parameters, config);
        this.count = parameters.intValue("count");
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheData["count"] = this.count;
        return base.toCache();
    }

    public void addCount(int value)
    {
        this.count += value;
        this.onCountChangeAction?.Invoke();
    }

    public void reduceCount(int value)
    {
        this.count -= value;
        this.onCountChangeAction?.Invoke();
    }
}
