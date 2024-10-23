using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGather : MonoBehaviour
{
    [Header("Config")]
    // [SerializeField] private GameObject gatherBox; // 攻击盒子，用于检测碰撞
    private bool isGathering = false;
    public float energy;
    private PlayerEnergy playerEnergy;
    private PlayerEquipment playerEquipment;
    [SerializeField] private GatherStats_SO gatherStats;

    private void Start()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerEquipment = GetComponent<PlayerEquipment>();
        isGathering = false;
    }

    // 允许外部设置 gatherStats
    public void SetGatherStats(GatherStats_SO newGatherStats)
    {
        gatherStats = newGatherStats;
        Debug.Log($"Selected gather item: {gatherStats.name}");
    }

    public void Gather()
    {
        // allTransforms = 
     /*   var all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        foreach (var item in all)
        {
            if (item.scene.isLoaded && item.GetComponent<GatheringItem>())
            {
                GatheringItem gatherItem = item.GetComponent<GatheringItem>();
                gatherStats = gatherItem.GetStats();
                Debug.Log("FindGatherSuccess: " + item.name);
                break;
            }
                
        }*/


        if (gatherStats == null)
        {
            Debug.Log("No gatherStats selected, cannot perform gathering.");
            return;
        }

        if (playerEquipment.currentEquipment == EquipmentType.Tool && !isGathering)
        {
            Debug.Log("Gathering resources...");
            StartCoroutine(PerformGather());
        }
        else
        {
            Debug.Log("No gather item selected or invalid equipment.");
        }

    }

    public void Fish()
    {
        if (playerEquipment.currentEquipment == EquipmentType.FishingNet && !isGathering)
        {
            Debug.Log("Fishing...");
            StartCoroutine(PerformFish());
        }
    }

    private void ResetGatherStats()
    {
        gatherStats = null;
        Debug.Log("Gather stats reset.");
    }

        private IEnumerator PerformGather()
    {
        if (gatherStats == null)
            {
                Debug.Log("No gatherStats selected, cannot perform gathering.");
                yield break; // 退出协程
            }
        isGathering = true;
        playerEnergy.SetGathering(true);
        //         // 播放采集动画
        yield return new WaitForSeconds(2.0f); // 假设采集动画时长为1秒
        // GatherSystem.GatherTest(item.GetComponent<GatherItem>(), this);
        // Debug.Log($"{gatherStats.name} caught!");

        // 调用掉落物逻辑
        DropItem(gatherStats);

        isGathering = false;
        playerEnergy.SetGathering(false);

        // 重置 gatherStats
        ResetGatherStats();
    }

    private IEnumerator PerformFish()
    {
        isGathering = true;
        playerEnergy.SetGathering(true);
        // 播放捕鱼动画
        yield return new WaitForSeconds(2.0f); // 假设捕鱼动画时长为2秒
        // 执行捕鱼逻辑
        Debug.Log("Fish caught!");
        // break;

        // 调用掉落物逻辑
        DropItem(gatherStats);

        isGathering = false;
        playerEnergy.SetGathering(false);
    }

    // 掉落物逻辑
    private void DropItem(GatherStats_SO gatherStats)
    {
        if (gatherStats.item != null)
        {
            Debug.Log($"获得掉落物，装进背包 Item dropped: {gatherStats.item.itemName}");
            Item gatherItem = new Item();
            gatherItem.amount = 1;
            gatherItem.id = gatherStats.item.itemId;
            gatherItem.IconInBag = gatherStats.item.icon;
            Inventory.Instance.AddItemWithUI(gatherItem);
        }
        else
        {
            Debug.Log("No item to drop.");
        }
    }

}
