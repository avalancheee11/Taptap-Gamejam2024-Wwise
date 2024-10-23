using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFormulaConfigRoot : SimpleConfigRoot<ItemFormulaConfig>
{
    public ItemFormulaConfig getConfigByColor(int red, int yellow, int blue)
    {
        var config = this.configList.Find(x => x.red == red && x.yellow == yellow && x.blue == blue);
        if (config == null) {
            return this.configs[22];
        }

        return config;
    }
}
