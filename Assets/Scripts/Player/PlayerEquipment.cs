using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEquipment : MonoBehaviour
{
    public EquipmentType currentEquipment = EquipmentType.Tool;

    public void EquipWeapon()
    {
        currentEquipment = EquipmentType.Weapon;
        Debug.Log("Weapon equipped武器装备中.");
    }

    public void EquipTool()
    {
        currentEquipment = EquipmentType.Tool;
        Debug.Log("Tool equipped锄头工具装备中.");
    }

    public void EquipFishingNet()
    {
        currentEquipment = EquipmentType.FishingNet;
        Debug.Log("Fishing net equipped渔网装备中.");
    }
}
