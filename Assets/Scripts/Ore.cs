using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    // 标记矿石是否已被挖完  
    private bool isMined = false;

    // 矿石被挖完时调用的方法  
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是玩家（或其他指定的挖掘工具）  
        if (other.CompareTag("Player"))
        {
            // 确保矿石只被挖一次  
            if (!isMined)
            {
                isMined = true;

                // 播放挖矿动画或声音（可选）  
                // PlayMiningAnimation();  
                // PlayMiningSound();  

                // 销毁矿石对象  
                Destroy(gameObject);

                // 可选：增加玩家的资源或分数  
                // PlayerResources.Instance.AddOre(1);  
            }
        }
    }
}