using UnityEngine;

[CreateAssetMenu(fileName = "EnergyStats", menuName = "EnergyStats/Energy")]
public class EnergyStats_SO : ScriptableObject
{
    [Header("Energy Values")]
    public float currentEnergy;
    public float maxEnergy = 1000f;       // 最大能量
    public float walkEnergyDrain = 1f;   // 走路时每秒消耗的能量
    public float runEnergyDrain = 2.5f;  // 跑步时每秒消耗的能量
    public float gatherEnergyDrain = 5f; // 采集时每秒消耗的能量
}
