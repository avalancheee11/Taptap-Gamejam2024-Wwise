using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyHealth : MonoBehaviour, IDamageable
{    
    
    public static event Action OnEnemyDeadEvent;

    [Header("Config")]
    [SerializeField] private MonsterStats_SO stats;
    [SerializeField] private EnergyStats_SO energyStats;

    public int currentHealth{get; private set;}
    private EnemyBrain enemyBrain;
    //TODO:之后加动画
    // private Animator animator;
    // private EnemySelector enemySelector;

    private void Awake()
    {
        // animator = GetComponent<Animator>();
        enemyBrain = GetComponent<EnemyBrain>();
        // enemySelector = GetComponent<EnemySelector>(); //TODO:选择敌人时的鼠标图标
    }

     private void Start() {
        currentHealth = stats.health;
    }

   public void TakeDamage(int amount)
   {
            currentHealth -= amount;
            Debug.Log($"怪物受伤 - Monster took {amount} damage. Remaining health: {stats.health}");

        if (currentHealth <= 0)
        {
            enemyBrain.enabled = false;
            // enemySelector.NoSelectionCallback(); //hidding our selection sprite
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            OnEnemyDeadEvent?.Invoke();  //然后去PlayerAttack 去listen to this event
            Debug.Log("怪物死亡");
            Invoke("DestroyEnemy", 2f);
            Debug.Log("获得击败怪物能量："+stats.energyBall);
            energyStats.currentEnergy += stats.energyBall;

            
        } 
        else
        {
            DamageManager.Instance.ShowDamageText(amount, transform);
        }

   }


    private void DestroyEnemy()
        {
            Destroy(gameObject);
        }
}
