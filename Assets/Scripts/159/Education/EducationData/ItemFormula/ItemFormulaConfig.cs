using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//二混一配置
public class ItemFormulaConfig : NSConfigObject
{
    public int red { get; private set; }
    public int yellow { get; private set; }
    public int blue { get; private set; }
    public Color color { get; private set; }
    public int group { get;private set; }

    public override void initializeByParameters(Dictionary<string, object> parameters)
    {
        base.initializeByParameters(parameters);
        this.red = parameters.intValue("red");
        this.yellow = parameters.intValue("yellow");
        this.blue = parameters.intValue("blue");
        this.color = parameters.stringValue("color", "#000000").convertToColor();
        this.group = parameters.intValue("group");
    }

    public ItemFormulaData getData()
    {
        var data = new ItemFormulaData();
        data.reloadData(null, this);
        return data;
    }
}
