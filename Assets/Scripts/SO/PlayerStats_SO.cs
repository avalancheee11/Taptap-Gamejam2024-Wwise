using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="PlayerStats", menuName = "Player Stats")]

public class PlayerStats_SO : ScriptableObject
{
    [Header("Config")]
    [Header("Player Prefab")]
    public string prefab;

    [Header("Health 血条/生命值")]
    public int health;
    public int maxHealth;

    [Header("Attack 基础攻击力取决于武器")]
    // public int attack;
    
    [Header("Speed 移动速度")]
    public float speed;
    
    [Header("Speed 跑步速度")]
    public int runSpeed;

    
    [Header("type 类型")]
    public string type = "Player";

    [Header("Power 能量值")]
    public int energy;
    public int maxEnergy;


   
    // [Range(1f, 100f)]public float ExpMutiplier;


}
