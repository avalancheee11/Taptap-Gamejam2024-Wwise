using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SellingSystem;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    public event EventHandler OnItemListChanged;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                if(instance == null)
                {
                    GameObject obj = new GameObject("Inventory");
                    instance = obj.AddComponent<Inventory>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private List<Item>itemList;
    private Action<Item> useItemAction;
    public List<Item> GetItemList()
    {
        return itemList;
    }
    public Inventory()
    {
        itemList = new List<Item>();
        Debug.Log("Inventory");
    //    UnitTest();
    }

    public void AddItemWithUI(Item item)
    {
        Debug.Log($"Item added: {item.id}, amount: {item.amount},  Image : {item.IconInBag}");

        for (int i = 0; i < itemList.Count; ++i)
        {
            if (itemList[i].id == item.id && itemList[i].type == item.type)
            {
                itemList[i].amount += item.amount;
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        itemList.Add(item);
     //   Debug.Log($"Item added: {item.id}, amount: {item.amount},  Image : {item.IconInBag}");
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddItem(string id, int amount = 1, string type = "normal")
    {
        for (int i = 0; i < itemList.Count; ++i)
        {
            if (itemList[i].id == id && itemList[i].type == type)
            {
                itemList[i].amount += amount;
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        Item newItem = new Item
        {
            id = id,
            amount = amount,
            type = type
        };

        itemList.Add(newItem);
        Debug.Log($"Item added: {id}, amount: {amount}, Image : {newItem.IconInBag}");
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddItems(List<Item> list)
    {
        foreach (var item in list)
        {
            AddItem(item.id, item.amount, item.type);
        }

    }

    public void RemoveItem(string id, int amount = 1, string type = "normal")
    {
        for (int i = 0; i < itemList.Count; ++i)
        {
            if (itemList[i].id == id && itemList[i].type == type)
            {
                itemList[i].amount -= amount;
                if (itemList[i].amount <= 0)
                {
                    itemList.RemoveAt(i);
                }
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

    }

    public int QueryItem(string id, string type = "normal")
    {
        foreach (var item in itemList)
        {
            if (item.id == id && item.type == type)
            {
                return item.amount;
            }
        }
        return 0;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public void SetUseActionItem(Action<Item> itemAction)
    {
        useItemAction = itemAction;
    }

    public void UnitTest()
    {
        Debug.Log($"QueryItemResult1 (normal): {QueryItem("Test1")}");
        AddItem("Test1", 2);
        AddItem("Test1");
        AddItem("SpecialTest1", 1, "special");
        Debug.Log($"QueryItemResult2 (normal): {QueryItem("Test1")}");
        Debug.Log($"QueryItemResult2 (special): {QueryItem("SpecialTest1", "special")}");

        RemoveItem("Test1", 1);
        RemoveItem("Test1");
        AddItem("Test2");
        RemoveItem("SpecialTest1", 1, "special");
        Debug.Log($"HasItem Test1 (normal): {HasItem("Test1")}");
        Debug.Log($"HasItem SpecialTest1 (special): {HasItem("SpecialTest1", "special")}");

        RemoveItem("Test1", 2);
        RemoveItem("Test2");
        Debug.Log($"ClearUnitTest: {itemList.Count}");
    }

    public bool HasItem(string itemId, string type = "normal")
    {
        return QueryItem(itemId, type) > 0;
    }
}
