using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="PlayerEnergyStats", menuName = "Energy Stats")]

public class PlayerEnergyStats_SO : ScriptableObject
{
     [Header("Config")]


    [Header("玩家走路消耗能量")]
    public int moveCost;

    
    [Header("玩家跑步消耗能量")]
    public int runCost;

    
    [Header("玩家采集消耗能量")]
    public int gatherCost;
}
