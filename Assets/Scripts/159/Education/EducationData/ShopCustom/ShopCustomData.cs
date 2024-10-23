using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomData : UnitSimpleData<ShopCustomConfig>
{
    public List<int> formulaList { get; private set; }
    public int specialItem { get; private set; }

    public override void initialize()
    {
        base.initialize();
        this.formulaList = new List<int>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ShopCustomConfig config)
    {
        base.reloadDataByData(parameters, config);
        this.formulaList = parameters.intListValue("formulaList");
        this.specialItem = parameters.intValue("specialItem");
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheData["specialItem"] = this.specialItem;
        this.cacheData["formulaList"] = this.formulaList.convertToObjectList();
        return base.toCache();
    }

    public void setFormulaList(List<int> values)
    {
        this.formulaList.Clear();
        this.formulaList.AddRange(values);
    }

    public void setSpecialItem(int value)
    {
        this.specialItem = value;
    }
}
