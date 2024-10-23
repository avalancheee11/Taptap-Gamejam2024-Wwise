using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    
    [SerializeField] private BossStats_SO stats;
    private Transform target;
    // private List<GameObject> enemiesInRange = new List<GameObject>();
    // private bool stunEffectActive = false; // 攻击范围内眩晕效果
    private float speedUp; // 攻击加速
    private bool isAttack = false;

    void Start()
    {
        // stats = GetComponent<BossStats_SO>();
        StartCoroutine(ShootContinuously());
        // speedUp = stats.CoolDown * (100f - 30f) / 100f;        
    }

    void Update()
    {
        UpdatePlayerInRange();
        // UpdateTarget();
    }

    // 我方眩晕效果 (called by Effect class)
    // public void EnableStunEffect()
    // {
    //     Debug.Log("开始眩晕");
    //     if (!stunEffectActive)
    //     {
    //         stunEffectActive = true;
          
    //         StartCoroutine(ApplyStunEffect());
    //     }
    // }

    // 🫧 在这里设置使player眩晕的时间
    // IEnumerator ApplyStunEffect()
    // {
    //     while (stunEffectActive)
    //     {
    //         foreach (GameObject enemy in enemiesInRange)
    //         {
    //             if (enemy != null)
    //             {
    //                 EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>(); //Enemy script
    //                 if (enemyScript != null)
    //                 {
    //                     Debug.Log("等待4秒");
    //                     enemyScript.ApplyStun(1f); // Stun for 1 second
    //                 }
    //             }
    //         }

    //         yield return new WaitForSeconds(5f); // Repeat every 5 seconds (4s detection + 1s stun)
    //     }
    // }



void UpdatePlayerInRange()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    if (player != null) // 确保玩家存在
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= stats.attackRange)
        {
            isAttack = true;
            target = player.transform; // 设置目标为玩家
            Debug.Log("BOSS可攻击玩家");
        }
        else
        {
            isAttack = false;
            target = null; // 超出攻击范围，清空目标
            // Debug.Log("不可攻击玩家");
        }
    }
}

    IEnumerator ShootContinuously()
    {
        while (true)
        {
            if (isAttack)
            {
                Shoot();
            }
            yield return new WaitForSeconds(stats.timeBtwAttacks);
        }
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(stats.bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            // 计算朝向玩家的方向
            Vector3 direction = (target.position - transform.position).normalized;
            bullet.SetBulletData(stats.bulletSpeed, stats.attack, direction); // 传递方向
        }

        Debug.Log("子弹发射");
    }


    //  Increase attack speed by a percentage 
    public void IncreaseAttackSpeed(int percentage)
    {
        // stats.CoolDown *= (100f - percentage) / 100f;
        stats.timeBtwAttacks = speedUp;
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);
    }
}
