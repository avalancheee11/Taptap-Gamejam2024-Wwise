using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum ItemType
// {
//     Normal,
//     Special,
// }

//掉落物以图标显示，具体视觉上为直接图标飞入背包
[CreateAssetMenu(fileName = "Item_", menuName = "Collection/Item", order = 0)]

// 两种掉落物
public class ItemStats_SO : ScriptableObject
{
    public string itemId;
    // public int amount;
    public ItemType itemType; //Normal是普通材料，Special 是特殊材料

    public string itemName;
    // public Sprite sprite; //没有实体，所以不用
    public Sprite icon; // 具体的飞入背包的图标prefab
    public string effect; //掉落物特效
    public string description;


}
// 能放进背包里的就是材料和特殊物品
