using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 采集物不可放入背包
public enum GatherType
{
    Plant,
    Mineral,
    Fish
}

[System.Serializable]
public class ItemDrop
{
    // public ItemStats_SO item; // 这是掉落的物品
    // public int quantity;      // 掉落物的数量
}

[CreateAssetMenu(fileName = "_GatherStats" , menuName = "Gather Stats/Data" )]

// 采集物：植物，矿物和鱼
public class GatherStats_SO : ScriptableObject
{
  [Header("采集物Config")]

  public string id;

  public string name;


  [Header("采集物种类 type")]
  public GatherType gatherType;

  
  [Header("采集物 Prefab(是模型)")]
  public GameObject gatherItemPrefab;

  [Header("刷新概率 Probability（暂无）")]
  public float refreshProbability;

  [Header("消耗能量:目前我们先统一所有的采集物都是同样的消耗能量，数值设定详见energyStats--要改成配置在地图中")]
  // public int energy;
  [Header("交互范围")]
  public float interactionRadius =2f;
    [Header("掉落物")]
    // public List<ItemDrop> rewards; // 包含物品和数量的列表
    public ItemStats_SO item;


  [Header("需求工具 Tool")]
  public EquipmentType equipmentType; //植物和矿物是Tool，鱼是FishingNet；
}