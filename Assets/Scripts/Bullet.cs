using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BossStats_SO stats; 
    private float speed = 2f;
    private int damage = 10;
    private Vector3 direction;  // 子弹移动方向

        public void SetBulletData(float bulletSpeed, int bulletDamage, Vector3 bulletDirection)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        direction = bulletDirection; // 使用传递的方向（玩家）
    }


    void Update()
    {
        // 让子弹沿着固定方向移动
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // 检查子弹是否超出最大运动距离
        float traveledDistance = Vector3.Distance(transform.position, transform.position + direction);
        if (traveledDistance >= stats.maxDistance) // 确保 maxDistance 被定义
        {
            Destroy(gameObject);
        }
    }

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player") ) // 根据需要添加标签
    {
        HitTarget(other.transform);
        Destroy(gameObject);
    }
}

    void HitTarget(Transform player)
    {
        Damage(player);
        Destroy(gameObject);
    }

    void Damage(Transform player)
{
    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
    
    if (playerHealth != null)
    { Debug.Log("子弹准备伤害玩家");
        playerHealth.TakeDamage(damage);
    }
    else
    {
        Debug.LogError("PlayerHealth component is missing on the player object.");
    }
}

}
