using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None, Normal, Special,
}

public class ItemConfig : NSConfigObject
{
    public string name { get; private set; }
    public int group { get; private set; }
    public int red { get; private set; }
    public int yellow { get; private set; }
    public int blue { get; private set; }
    public ItemType itemType { get; private set; }
    public string url { get; private set; }
    public Sprite icon => CacheManager.Instance.loadSpriteByAssetBundle(this.url);
    public int max { get; private set; }

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);
        this.name = parameters.stringValue("name");
        this.red = parameters.intValue("red");
        this.yellow = parameters.intValue("yellow");
        this.blue = parameters.intValue("blue");
        this.url = parameters.stringValue("url");
        this.group = parameters.intValue("group");
        this.max = parameters.intValue("max", 9999);
        this.itemType = (ItemType) Enum.Parse(typeof(ItemType), parameters.stringValue("itemType", ItemType.None.toString()));
    }

    public ItemData getData()
    {
        var data = new ItemData();
        data.reloadData(null, this);
        return data;
    }
}
