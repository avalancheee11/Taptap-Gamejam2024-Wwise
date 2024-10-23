using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerData : UnitSimpleData<ItemContainerConfig>
{
    public List<ItemFormulaData> formulaDataList { get; private set; }
    public bool isFormulaFull => this.formulaDataList.Count >= this.config.capacity;
    public ItemData specialItem { get; private set; }

    //等待混合的材料
    public List<ItemData> itemList { get; private set; }
    public bool isItemFull => this.itemList.Count >= 2;

    public event Action onFormulaChangeAction;
    public event Action onSpecialItemChangeAction;
    public event Action onItemCountChangeAction;

    public override void initialize()
    {
        base.initialize();
        this.formulaDataList = new List<ItemFormulaData>();
        this.itemList = new List<ItemData>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ItemContainerConfig config)
    {
        base.reloadDataByData(parameters, config);
        this.loadDataListByConfigRoot(this.formulaDataList, "formulaDataList", parameters, ItemFormulaConfigHandler.Instance.configRoot);
        this.loadDataListByConfigRoot(this.itemList, "itemList", parameters, ItemDataHandler.Instance.configRoot);
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheUserDataListByList<ItemConfig, ItemData>("itemList", this.itemList);
        this.cacheUserDataListByList<ItemFormulaConfig, ItemFormulaData>("formulaDataList", this.formulaDataList);
        return base.toCache();
    }

    public void addItemData(ItemData itemData)
    {
        this.itemList.Add(itemData);
        this.onItemCountChangeAction?.Invoke();
    }

    public ItemData removeLastItemData()
    {
        if (this.itemList.Count == 0) {
            return null;
        }

        var data = this.itemList[this.itemList.Count - 1];
        this.itemList.Remove(data);
        this.onItemCountChangeAction?.Invoke();
        return data;
    }

    public void clearItemDataList()
    {
        this.itemList.Clear();
        this.onItemCountChangeAction?.Invoke();
    }

    public void addFormula(ItemFormulaData formulaData)
    {
        this.formulaDataList.Add(formulaData);
        this.onFormulaChangeAction?.Invoke();
    }

    public ItemFormulaData removeLastFormula()
    {
        if (this.formulaDataList.Count == 0) {
            return null;
        }

        var data = this.formulaDataList.objectValue(this.formulaDataList.Count - 1);
        this.formulaDataList.RemoveAt(this.formulaDataList.Count - 1);
        this.onFormulaChangeAction?.Invoke();
        return data;
    }

    public void setSpecialItem(ItemConfig itemConfig)
    {
        this.specialItem = new ItemData();
        this.specialItem.reloadData(null, itemConfig);
        this.onSpecialItemChangeAction?.Invoke();
    }
}
