using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_MonsterStats" , menuName = "Enemy Stats/Data" )]
public class MonsterStats_SO : ScriptableObject
{
    [Header("怪物信息 Stats Info")]

    [Header("怪物 Prefab")]
    // public string prefab;
    public EnemyBrain monsterPrefab;

    [Header("Health 生命值/血量")]
    public int health;
    public int maxHealth;


    [Header("Attack 攻击力")]
    public int attack;

    [Header("Speed 移动速度")]
    public float speed;

    [Header("Run Speed 跑步速度(暂时没用)")]
    public int runSpeed;

    [Header("type 类型")]
    public string type = "Monster";

    [Header("掉落能量")]
    public int energyBall;



    [Header("----以下是专属于怪物的属性----")]

    [Header("Initial Position 初始位置")]
    public Vector3 initialPosition;

    [Header("追击速度(可以设置当追击时速度更快)")]
    public float chaseSpeed;

    [Header("追击范围（深蓝色）")]
    public float range;

    [Header("Patrol Radius 巡逻范围(浅蓝色)")]
    public float patrolRadius=6f;

    [Header("可攻击范围(黄色：即玩家进入这个范围会遭受攻击)")]
    public float attackRange;

    [Header("CoolDown 冷却时间/攻击间隔")]
    public float timeBtwAttacks=2f;

    [Header("掉落物品ID(还没写)")]
    public string dropItemId;

    // [Header("巡逻移动间隔时间 Wander time(可选)")]
    // public float wanderTime=3f;

    [Header("怪物刷新概率 Spawn Probability")]
    [Range(0f, 1f)] // 刷新概率范围 0 到 1
    public float spawnProbability = 1f; // 默认 100% 概率

//    [Header("Money 击败可获得金钱")]
//    public int earnMoney;


}
