using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : FSMAction
{
    [Header("Config")]
    // [SerializeField]  private int attack;
    // [SerializeField]  private float timeBtwAttacks;
    [SerializeField] private MonsterStats_SO stats;

    private EnemyBrain enemyBrain;
    private float timer;

    void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain> ();       
    }

    public override void Act(){
        AttackPlayer();
    }

    private void AttackPlayer(){
        if(enemyBrain.Player == null) return;
        timer -= Time.deltaTime;

        //攻击player
        // 一种写法：使用接口
        if(timer <= 0f){
            IDamageable player = enemyBrain.Player.GetComponent<IDamageable>(); 
            player.TakeDamage(stats.attack);
            timer = stats.timeBtwAttacks;
        }
        
        //  // 另一种写法（就要去health中再加一个PlayerHasHealth
        //  if(timer <= 0f){
        //     // IDamageable player = enemyBrain.Player.GetComponent<IDamageable>(); 
        //     PlayerHealth health = enemyBrain.Player.GetComponent<PlayerHealth>();
        //     if(health.PlayerHasHealth == false) return;
        //     health.TakeDamage(damage);  
        //     timer = timeBtwAttacks;
        // }
    }
  
}
