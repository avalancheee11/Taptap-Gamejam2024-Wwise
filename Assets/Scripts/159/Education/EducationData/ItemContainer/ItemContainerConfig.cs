using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerConfig : NSConfigObject
{
    public int capacity { get; private set; }
    public string url { get; private set; }
    public string interurl { get; private set; }
    public Sprite icon => CacheManager.Instance.loadSpriteByAssetBundle(this.url);
    public Sprite interIcon => CacheManager.Instance.loadSpriteByAssetBundle(this.interurl);

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);
        this.capacity = parameters.intValue("capacity");
        this.url = parameters.stringValue("url");
        this.interurl = parameters.stringValue("interurl");
    }
}
