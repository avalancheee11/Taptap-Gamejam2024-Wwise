using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomConfig : NSConfigObject
{
    public string url { get; private set; }
    public List<int> colorGroupList { get; private set; }
    public List<int> specialList { get; private set; }
    public List<int> containerList { get; private set; }
    public int colorCount { get; private set; }

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);
        this.url = parameters.stringValue("url");
        this.colorGroupList = parameters.intListValue("colorGroupList");
        this.specialList = parameters.intListValue("specialList");
        this.containerList = parameters.intListValue("containerList");
        this.colorCount = parameters.intValue("colorCount");
    }

    public ShopCustomData getData()
    {
        var data = new ShopCustomData();
        data.reloadData(null, this);
        return data;
    }
}
