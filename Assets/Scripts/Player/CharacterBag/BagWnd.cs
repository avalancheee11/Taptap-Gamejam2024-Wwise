using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagWnd : BaseWnd
{
    private List<BagSlot> allSlots;//所有的格子数组
    private Transform info; //信息提升面板父类
    private Text infoTitle; //面板标题
    private Text infoDesc; //面板描述
    private Image workItem; //拖动时临时存放的图片格子
    private Button btn_close; //关闭按钮

    private Item _currentItemInfo; //当前格子数据
    private BagSlot _originSlot; //被拖动的格子
    private BagSlot _currentSlot; //当前格子


    public void SetWorkItem(BagSlot originSlot, Item itemInfo, Sprite itemSprite)
    {
        _originSlot = originSlot;
        _currentItemInfo = itemInfo;
        workItem.gameObject.SetActive(true);//显示临时图片
        workItem.sprite = itemSprite;//把被拖动格子图片赋值
    }

    public void SetWorkItemPos(Vector3 pos)
    {
        workItem.transform.position = pos;
    }

    public void EndDragItem()
    {
        if (_currentSlot.HasItem())
        {
            _originSlot.SetItem(_currentSlot.selfItem);
            _currentSlot.SetItem(_currentItemInfo);
        }
        else //没物品
        {
            _currentSlot.SetItem(_currentItemInfo);

        }

        workItem.gameObject.SetActive(false);
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        Debug.Log("Inventory_OnItemListChanged");
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
    {
        List<Item> ItemList = Inventory.Instance.GetItemList();
        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].Clear();
            if (i < ItemList.Count)
            {
                allSlots[i].SetItem(ItemList[i]);
                Debug.Log("SetItemInfo: " + ItemList[i].amount);
            }
        }

        Debug.Log("SlotLen: " + ItemList.Count);
    }

    public override void Inital()
    {

        allSlots = new List<BagSlot>();
        Transform slotRoot = SelfTransform.Find("BG/Slots");
        for(int i = 0; i < slotRoot.childCount; i++)
        {
            GameObject tmpSlot = slotRoot.GetChild(i).gameObject;
            BagSlot bagSlot = tmpSlot.AddComponent<BagSlot>();
            bagSlot.Inital(i);
            allSlots.Add(bagSlot);
        }
        //btn_close = SelfTransform.Find("BG/btn_close").GetComponent<Button>();
        //btn_close.onClick.AddListener(CloseBagWnd);
        //Debug.Log("背包窗口已打开");
        info = SelfTransform.Find("BG/Info");
        infoTitle = info.Find("Title").GetComponent<Text>();
        infoDesc = info.Find("Desc").GetComponent<Text>();
        workItem = SelfTransform.Find("BG/tmpImage").GetComponent<Image>();

        Inventory.Instance.OnItemListChanged += Inventory_OnItemListChanged;
        Debug.Log("BagWndInitSuccess");
    }

    public void PickUp(Item itemInfo)
    {
        for(int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i].HasItem() && allSlots[i].selfItem.id == itemInfo.id) //如果有物品且可以叠加
            {
                allSlots[i].AddItem(itemInfo.amount);
                break;
            }else if (!allSlots[i].HasItem()) //如果没有物品
            {
                allSlots[i].SetItem(itemInfo);
                break;
            }
        }
    }

    private void CloseBagWnd()
    {
        CloseWnd(); 
    }


    public void SetCurrentSlot(BagSlot slot)
    {
        _currentSlot = slot;
    }

    public void ShowItemInfo(Item itemInfo, Vector3 pos)
    {
        info.position = pos;
        infoTitle.text = itemInfo.id;
        infoDesc.text = itemInfo.information;
        info.gameObject.SetActive(true);
    }

    public void HideItemInfo()
    {
        info.gameObject.SetActive(false);
    }


}
