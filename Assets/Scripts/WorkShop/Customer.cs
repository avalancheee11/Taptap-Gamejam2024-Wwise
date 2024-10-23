using System;
using UnityEngine;
using UnityEngine.UI;
using static SellingSystem;


public class Customer : MonoBehaviour
{
    public SellingSystem.Order currentOrder;
    [SerializeField] private SellingSystem.Order predefinedOrder;
    public Text itemDescription;

    // 结算 UI 部分 - 考虑之后写自动绑定
    public GameObject orderPanel;
    public Text rewardText;
    public GameObject submitPanel;
    public Button submitButton;
    public Text colorMatchText;
    public Text specificColorMatchText;
    public Text materialMatchText;

    private bool isSettling = false;
    private FireItem currentFireItem;
    // 添加 OnOrderCompleted 事件
    public event Action<Customer, int> OnOrderCompleted;
    private SellingSystem.IInventorySystem inventorySystem; // 唯一实现了该接口的背包系统
    public void Start()
    {
        // 获取背包系统引用 （理论上来说背包是单例）
        //inventorySystem = FindObjectOfType<InventorySystem>();
        Inventory.Instance.UnitTest();

        // 绑定提交订单
        submitButton.onClick.AddListener(SubmitOrder);
        // 更新UI
        UpdateOrderDisplay();
    }

    // 获取订单
    public void SetOrder(SellingSystem.Order order)
    {
        currentOrder = order;
        UpdateOrderDisplay();
        
    }

    // UI部分的显示 - 待更新
    private void UpdateOrderDisplay()
    {
        if (currentOrder != null)
        {
            itemDescription.text = string.IsNullOrEmpty(currentOrder.materialName)?currentOrder.orderText:$"{currentOrder.orderText}";
            // 之后可能还有图标什么的吧，困惑，ui里放着就行
        }
        // 测试
        //ShowOrder();
    }

    // 检测是否有物品触碰到
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!isSettling && other.CompareTag("FireItem"))
        {
            StartSettlement(other.gameObject);
        }
    }

    // 火焰调用
     public void CheckFireItem(FireItem fireItem)
    {
        if (!isSettling)
        {
            currentFireItem = fireItem;
            StartSettlement(fireItem.gameObject);
        }
    }


    // 开始结算订单UI
    private void StartSettlement(GameObject providedItem)
    {
        if (SellingManager.Instance.IsCustomerLeaving())
        {
            //不允许同时提交多个订单
            return;
        }
        isSettling = true;
        submitPanel.SetActive(true);

        // 检测FireItem
        FireItem fireItem = providedItem.GetComponent<FireItem>();
        string providedColorCategory = fireItem.colorCategory;
        string providedSpecificColor = fireItem.specificColor;
        string providedMaterial = fireItem.material;

        // 算钱
        int reward = SellingSystem.CalculateReward(currentOrder, providedColorCategory, providedSpecificColor, providedMaterial);

        // 更新UI - UI 显示部分待更新
        colorMatchText.text = currentOrder.colorCategory == providedColorCategory ? "颜色类别: +50" : "颜色类别: 未完成";
        specificColorMatchText.text = currentOrder.specificColor == providedSpecificColor ? "具体颜色: +30" : "具体颜色: 未完成";
        // 如果有/无材料要求
        if (!string.IsNullOrEmpty(currentOrder.materialName))
        {
            materialMatchText.text = currentOrder.materialName == providedMaterial ? "材料: +50" : "材料: 未完成";
            materialMatchText.gameObject.SetActive(true);
        }
        else
        {
            materialMatchText.gameObject.SetActive(false);
        }

        rewardText.text = $"最终奖励: {reward}";
        submitButton.interactable = true;
        submitButton.onClick.AddListener(SubmitOrder);
        currentOrder.reward = reward;

        // 先销毁火焰
        if (currentFireItem != null)
            {
                // 摧毁火焰
                currentFireItem.DestroyOnSuccess();
                currentFireItem = null;
            }
    }


    // 提交订单
    public void SubmitOrder()
    {
        if (currentOrder != null && isSettling && !SellingManager.Instance.IsCustomerLeaving())
        {
           /* //从背包中移除物品，通知SellingManager收钱和刷新客人，销毁客人
            #if !UNITY_EDITOR
            inventorySystem.RemoveItem(currentOrder.itemName, 1);
            #endif
            Debug.Log($"订单完成！获得 {currentOrder.reward} 金币");*/

            submitPanel.SetActive(false);
            orderPanel.SetActive(false);
            isSettling = false;
            // 触发 OnOrderCompleted 事件
            OnOrderCompleted?.Invoke(this, currentOrder.reward);
            
        }
    }

    // 测试
    public void ShowOrder()
    {
        Debug.Log("colorCat: " + currentOrder.colorCategory);
        Debug.Log("colorSP: " + currentOrder.specificColor);
        Debug.Log("Material: " + currentOrder.materialName);
    }

    // 预制订单
    public void SetPredefinedOrder()
    {
        if (predefinedOrder != null)
        {
            currentOrder = SellingSystem.CreatePredefinedOrder(predefinedOrder);
            UpdateOrderDisplay();
        }
        else
        {
            Debug.LogWarning("Predefined order is not set for this customer.");
        }
    }


/*
     // 新添加的方法来处理鼠标点击
    private void OnMouseDown()
    {
        #if UNITY_EDITOR
        // 在编辑器模式下，点击客户会自动完成订单
        currentOrder.isCompleted = true;
        submitPanel.SetActive(true);
        //currentOrder.isCompleted = true;
        #endif
    }
    */

}

