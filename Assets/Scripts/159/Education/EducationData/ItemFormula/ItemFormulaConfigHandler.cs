using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFormulaConfigHandler : SimpleConfigHandler<ItemFormulaConfigRoot, ItemFormulaConfigHandler>
{
    public override string module => DataModule.ItemFormula;
}
