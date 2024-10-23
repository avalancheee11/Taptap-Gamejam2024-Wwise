using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 新增的ItemSlot类，用于管理单个物品槽位
[System.Serializable]
public class ItemSlot: MonoBehaviour
{
    public Image itemImage;
    public Text amountText;
    public Text nameText; // 可选
    public void Start()
    {
        itemImage = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();
    }

    public void UpdateSlot(Item item)
    {
        if (item != null)
        {
            itemImage.sprite = item.GetSprite();
            itemImage.enabled = true;
            amountText.text = item.amount.ToString();
            if (nameText != null)
            {
                nameText.text = item.id; // 假设Item.id是物品名称
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        amountText.text = "";
        if (nameText != null)
        {
            nameText.text = "";
        }
    }
}

