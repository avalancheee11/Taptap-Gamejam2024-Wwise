using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomDataHandler : SimpleDataHandler<ShopCustomConfigRoot, ShopCustomDataRoot, ShopCustomDataHandler>
{
    public override string module => DataModule.ShopCustom;
}
