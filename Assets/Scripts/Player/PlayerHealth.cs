using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 为了编辑Player Health的数据的code
public class PlayerHealth : MonoBehaviour, IDamageable

{
    // private PlayerAnimations playerAnimations;
    // public float CurrentHealth {get; private set;}

    [Header("Config")]
    [SerializeField] private PlayerStats_SO stats;
    // [SerializeField] private GameObject damageEffect; // 受击特效
    // [SerializeField] private HealthBarUI healthBar; // 血条UI


    private void Awake() {
        // playerAnimations = GetComponent<PlayerAnimations>();
       
    }

    private void Start()
    {
        ResetHealth();
    }


    private void Update() {
        //测试之用
        if(Input.GetKeyDown(KeyCode.P)){
            TakeDamage(20);
            Debug.Log("测试：受到伤害, health-20");
        }


        if(stats.health <= 0f){
            PlayerDead();
        }
    }


    public void TakeDamage(int amount)
    {
       if (stats == null)
            {
                Debug.LogError("PlayerStats_SO is not assigned in PlayerHealth.");
                return; // 确保 stats 被赋值
            }
        // 扣血
        stats.health -= amount;
      

        if(stats.health <= 0){
              Debug.Log("玩家血量为零");
            stats.health = 0;
            PlayerDead();
        }

        UIManager.Instance.UpdatePlayerUI();
        // 播放受击特效
        // Instantiate(damageEffect, transform.position, Quaternion.identity);
        
        // 更新血条
        // healthBar.UpdateHealth(stats.health, stats.maxHealth);
        
        Debug.Log($"Unit took {amount} damage. Remaining health: {stats.health}");
     
        DamageManager.Instance.ShowDamageText(amount, transform);
        // 如果血量为 0，执行死亡逻辑

    }

    public void DecreaseEnergy(int amount)
    {
       if(stats.energy <= 0){
        Debug.Log("能量已消耗完。返回商店。");
         stats.energy = 0;
        PlayerDead();
       }

        stats.energy -= amount;
        // DamageManager.Instance.ShowDamageText(amount, transform);
        Debug.Log("能量消耗中.... DecreaseEnergy");

    }

    //TODO: 大地图冒险结束，游戏暂停，显示UI，返回商店:加载返回商店场景
    private void PlayerDead(){
        Debug.Log("角色死亡。大地图冒险结束，返回商店。Dead");
        // playerAnimations.SetDeadAnimation();
    
        
    }

     public void ResetHealth(){
            stats.health = stats.maxHealth;
        }

    public float GetHealthPercentage()
    {
        return stats.health / stats.maxHealth;
    }

}
