using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringItem : MonoBehaviour,IDamageable
{
    // private GameObject player; // Reference to the player object

    [SerializeField] private GatherStats_SO gatherStats; 
    // public int currentHealth{get; private set;}

    // [SerializeField] private string itemType; // 采集物的类型: "plant", "mineral", "fish"
    private int currentHealth = 1; // 初始生命值，采集物只需1次攻击
    // public float interactionRadius;

    private void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player"); // Assuming the player has a "Player" tag
    }

    public GatherStats_SO GetStats()
    {
        return gatherStats;
    }

    // private void Update()
    // {
    //     // Check distance between player and gathering item
    //     if (player != null && gatherStats != null)
    //     {
    //         float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    //         if (distanceToPlayer <= gatherStats.interactionRadius)
    //         {
    //             Debug.Log("Player is within interaction radius, can gather the item.");
    //             AllowPlayerToGather(); // Allow the player to gather when in range
    //         }
    //     }
    // }

     public void TakeDamage(int amount)
   {
            currentHealth -= amount;
            Debug.Log($"采集物品正在被采集中...");

        if (currentHealth <= 0)
        {                  
           Gathering();                 
        }      
   }
        private void Gathering()
    {
         //TODO:待添加掉落物逻辑：将采集物掉落的物品和数量添加进背包，UI上显示将掉落物品的图标飞进下方的行囊条框框里。

        // Debug.Log($"{itemType} 已采集");
        Debug.Log($"已采集");

        // 这里可以加一些采集成功的动画效果
        AllowPlayerToGather();
        Destroy(gameObject); // 采集后销毁
    }

    private void AllowPlayerToGather()
    {
        //PlayerGather playerGather = player.GetComponent<PlayerGather>();
        //if (playerGather != null && gatherStats != null)
        //  {
        //     playerGather.SetGatherStats(gatherStats);
        // }

     /*   Debug.Log(" AllowPlayerToGather()");
        //Gather Reward
        Item GatherReward = new Item();
        GatherReward.amount = 1;
        GatherReward.id = gatherStats.item.itemId;
        GatherReward.IconInBag = gatherStats.item.icon;
        Inventory.Instance.AddItemWithUI(GatherReward);*/


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGather playerGather = other.GetComponent<PlayerGather>();
            if (playerGather != null)
            {
                // 确保 gatherStats 不为空
                if (gatherStats != null)
                {
                    playerGather.SetGatherStats(gatherStats);
                    Debug.Log($"Gather stats set for {gatherStats.name}.");
                }
                else
                {
                    Debug.LogError("gatherStats is null on this gatherable item.");
                }
            }
            else
            {
                Debug.LogError("playerGather == null");
            }
        }
        else
        {
            Debug.Log("No Player Tag");
        }
    }
    
    // private void OnCollisionEnter(Collision collision)
    // {
    //         if (collision.gameObject.CompareTag("Player"))
    //         {
    //             PlayerGather playerGather = collision.gameObject.GetComponent<PlayerGather>();
    //             if (playerGather != null)
    //             {
    //                 // Ensure gatherStats is not null
    //                 if (gatherStats != null)
    //                 {
    //                     playerGather.SetGatherStats(gatherStats);
    //                     Debug.Log($"Gather stats set for {gatherStats.name}.");

    //                 }
    //                 else
    //                 {
    //                     Debug.LogError("gatherStats is null on this gatherable item.");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError("playerGather == null");
    //             }

    //         }
    //         else
    //         {
    //             Debug.Log("No Player Tag");
    //         }
    // }

}
