using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFormulaData : UnitSimpleData<ItemFormulaConfig>
{
    public List<int> sourceItemList { get; private set; }

    public override void reloadDataByConfig(ItemFormulaConfig config)
    {
        base.reloadDataByConfig(config);
        this.sourceItemList = new List<int>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ItemFormulaConfig config)
    {
        base.reloadDataByData(parameters, config);
        this.sourceItemList = parameters.intListValue("sourceItemList");
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheData["sourceItemList"] = this.sourceItemList.convertToObjectList();
        return base.toCache();
    }

    public void setSourceItems(List<int> itemIdList)
    {
        this.sourceItemList.Clear();
        this.sourceItemList.AddRange(itemIdList);
    }
}
