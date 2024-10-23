using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_BossStats" , menuName = "Enemy Stats/Boss Data" )]

public class BossStats_SO : ScriptableObject
{
     [Header("BOSS信息 Stats Info")]
      public Boss bossPrefab;
      public string bossName;

    [Header("Health 生命值/血量")]
    public int health;
    public int maxHealth;

     [Header("Attack 攻击力")]
    public int attack=30;


    [Header("type 类型")]
    public string type = "Monster";
    [Header("CoolDown 冷却时间/攻击间隔")]
    public float timeBtwAttacks=2f;

       [Header("可攻击范围(红色：即玩家进入这个范围会遭受攻击)")]
    public float attackRange;

      [Header("掉落物品ID")]
    public string dropItemId;

    [Header("掉落物")]
    public ItemStats_SO item;
    
    [Header("掉落能量")]
    public int energyBall=100;

    [Header("----以下是专属于BOSS子弹的属性----")]

    [Header("Bullet Speed 子弹速度")]
    public float bulletSpeed;


   [Header("子弹 Bullet Prefab")]
    public GameObject bulletPrefab;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("子弹最大运动距离")]
public float maxDistance = 20f; // 
}
