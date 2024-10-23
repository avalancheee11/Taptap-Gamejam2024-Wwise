using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerDataHandler : SimpleDataHandler<ItemContainerConfigRoot, ItemContainerDataRoot, ItemContainerDataHandler>
{
    public override string module => DataModule.ItemContainer;
}
