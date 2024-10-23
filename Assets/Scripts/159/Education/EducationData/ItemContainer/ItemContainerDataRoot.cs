using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerDataRoot : SimpleDataRoot<ItemContainerConfig, ItemContainerData, ItemContainerConfigRoot>
{
    public List<ItemContainerData> containerDataList { get; private set; }
    public ItemContainerData makingContainerData { get; private set; }
    public bool containerIsFull => this.containerDataList.Count >= 3;
    public event Action onContainerDataChangeAction;
    public event Action onMakingContainerChangeAction;

    public override void initialize()
    {
        base.initialize();
        this.containerDataList = new List<ItemContainerData>();
    }

    public override void reloadDataByConfig(ItemContainerConfigRoot config)
    {
        base.reloadDataByConfig(config);
        this.containerDataList = new List<ItemContainerData>();
    }

    public override void reloadDataByData(Dictionary<string, object> parameters, ItemContainerConfigRoot config)
    {
        base.reloadDataByData(parameters, config);
        this.loadDataListByConfigRoot(this.containerDataList, "containerDataList", parameters, config);
        this.makingContainerData = parameters.objectData<ItemContainerData, ItemContainerConfig>("makingContainerData", config.configs, new ItemContainerConfig());
    }

    public override Dictionary<string, object> toCache()
    {
        this.cacheData["makingContainerData"] = this.makingContainerData?.toCache();
        this.cacheUserDataListByList<ItemContainerConfig, ItemContainerData>("containerDataList", this.containerDataList);
        return base.toCache();
    }

    public void addItemContainerData(ItemContainerData containerData)
    {
        this.containerDataList.Add(containerData);
        this.onContainerDataChangeAction?.Invoke();
    }
    
    public void removeItemContainerData(ItemContainerData containerData)
    {
        if (this.containerDataList.Remove(containerData)) {
            this.onContainerDataChangeAction?.Invoke();
        }
    }

    public void setMakingContainer(ItemContainerData data)
    {
        this.makingContainerData = data;
        this.onMakingContainerChangeAction?.Invoke();
    }
}
