using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Tool,
    FishingNet
}
[CreateAssetMenu(fileName = "Equip_", menuName = "Equipment", order = 0)]

public class EquipmentStats_SO : ScriptableObject
{
    [Header("Config")]
    public Sprite Icon;
    public EquipmentType equipmentType;
    public int damage=20;
    public float attackRange=5;


}
