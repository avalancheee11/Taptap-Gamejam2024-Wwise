using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagWnd : BaseWnd
{
    private List<BagSlot> allSlots;//���еĸ�������
    private Transform info; //��Ϣ������常��
    private Text infoTitle; //������
    private Text infoDesc; //�������
    private Image workItem; //�϶�ʱ��ʱ��ŵ�ͼƬ����
    private Button btn_close; //�رհ�ť

    private Item _currentItemInfo; //��ǰ��������
    private BagSlot _originSlot; //���϶��ĸ���
    private BagSlot _currentSlot; //��ǰ����


    public void SetWorkItem(BagSlot originSlot, Item itemInfo, Sprite itemSprite)
    {
        _originSlot = originSlot;
        _currentItemInfo = itemInfo;
        workItem.gameObject.SetActive(true);//��ʾ��ʱͼƬ
        workItem.sprite = itemSprite;//�ѱ��϶�����ͼƬ��ֵ
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
        else //û��Ʒ
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
        //Debug.Log("���������Ѵ�");
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
            if (allSlots[i].HasItem() && allSlots[i].selfItem.id == itemInfo.id) //�������Ʒ�ҿ��Ե���
            {
                allSlots[i].AddItem(itemInfo.amount);
                break;
            }else if (!allSlots[i].HasItem()) //���û����Ʒ
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
