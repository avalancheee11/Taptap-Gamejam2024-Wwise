using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryTest : MonoBehaviour
{
    public ShopManager shopManager;

    private void Start()
    {
        if (shopManager == null)
        {
            shopManager = FindObjectOfType<ShopManager>();
            if (shopManager == null)
            {
                Debug.LogError("ShopManager not found in the scene!");
                return;
            }
        }

        // 添加测试物品
        AddTestItems();

        // 测试展示所有物品
        TestAllInBag();

        // 等待几秒后测试普通物品
        Invoke("TestCollectionBag", 2f);

        // 再等几秒后测试特殊物品
        Invoke("TestSpecialBag", 4f);
    }

    private void AddTestItems()
    {
        // 添加一些测试物品到Inventory
        Inventory.Instance.AddItem("Apple", 5, "normal");
        Inventory.Instance.AddItem("Banana", 3, "normal");
        Inventory.Instance.AddItem("Magic Potion", 2, "special");
        Inventory.Instance.AddItem("Stone", 10, "normal");
        Inventory.Instance.AddItem("Legendary Sword", 1, "special");
    }

    private void TestAllInBag()
    {
        Debug.Log("Testing All Items Display");
        List<Item> allItems = Inventory.Instance.GetItemList();
        Debug.Log($"Total items in inventory: {allItems.Count}");
        foreach (var item in allItems)
        {
            Debug.Log($"Item: {item.id}, Type: {item.type}, Amount: {item.amount}");
        }
        shopManager.AllInBag();
    }

    private void TestCollectionBag()
    {
        Debug.Log("Testing Normal Items Display");
        List<Item> normalItems = Inventory.Instance.GetItemList().Where(item => item.type == "normal").ToList();
        Debug.Log($"Normal items in inventory: {normalItems.Count}");
        shopManager.CollectionBag();
    }

    private void TestSpecialBag()
    {
        Debug.Log("Testing Special Items Display");
        List<Item> specialItems = Inventory.Instance.GetItemList().Where(item => item.type == "special").ToList();
        Debug.Log($"Special items in inventory: {specialItems.Count}");
        shopManager.SpecialBag();
    }

    // 可以添加更多测试方法，比如测试添加超过槽位数量的物品、移除物品等
    
}