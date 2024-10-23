using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomDataRoot : SimpleDataRoot<ShopCustomConfig, ShopCustomData, ShopCustomConfigRoot>
{
    public List<ShopCustomData> customDataList { get; private set; }

    public override void initialize()
    {
        base.initialize();
        this.customDataList = new List<ShopCustomData>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ShopCustomConfigRoot config)
    {
        base.reloadDataByData(parameters, config);
        this.loadDataListByConfigRoot(this.customDataList, "customDataList", parameters, config);
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheUserDataListByList<ShopCustomConfig, ShopCustomData>("customDataList", this.customDataList);
        return base.toCache();
    }

    public void addShopCustomData(ShopCustomData data)
    {
        if (this.customDataList.Contains(data)) {
            return;
        }

        this.customDataList.Add(data);
    }

    public void removeShopCustomData(ShopCustomData data)
    {
        if (!this.customDataList.Contains(data)) {
            return;
        }

        this.customDataList.Remove(data);
    }
}
