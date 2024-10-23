using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataHandler : SimpleDataHandler<ItemConfigRoot, ItemDataRoot, ItemDataHandler>
{
    public override string module => DataModule.Item;
}
