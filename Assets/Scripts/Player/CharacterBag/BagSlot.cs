using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BagSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    private int selfIndex;
    private Image selfImage;
    public Item selfItem { get; private set; }
    private Text selfNum;

    public bool HasItem()
    {
        return selfItem != null;
    }
       
    public void Inital(int index)
    {
        selfIndex = index;
        selfImage = transform.Find("Image").GetComponent<Image>();
        selfNum = transform.Find("Amount").GetComponent<Text>();
        Clear();
    }

    public bool ItemCanAdd()
    {
        if(selfItem.type == "Test1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetItem(Item itemInfo)
    {
        selfItem = itemInfo;
        selfNum.gameObject.SetActive(true);
        Sprite sp = itemInfo.IconInBag;
        selfImage.sprite = sp;
        selfImage.color = Color.white;
        Debug.Log("SetItem" + itemInfo.IconInBag.name);
        if(itemInfo.amount > 1)
        {
            selfNum.text = itemInfo.amount.ToString();
        }
        else
        {
            selfNum.text = "";
        }
    }

    public void AddItem(int count)
    {
        selfItem.amount += count;
        selfNum.text = selfItem.amount.ToString();
    }

    public void Clear()
    {
        selfImage.color = Color.clear;
        selfNum.text = "";
        selfItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<BagWnd>().SetCurrentSlot(this);
        if (HasItem())
        {

            WndManager.Instance.GetWnd<BagWnd>().ShowItemInfo(selfItem, transform.position + new Vector3(300, -200, 0));
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HasItem())
        {
            WndManager.Instance.GetWnd<BagWnd>().HideItemInfo();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //开始拖动时
        selfImage.color = Color.clear;
        selfNum.text = "";
        selfNum.gameObject.SetActive(false);
        WndManager.Instance.GetWnd<BagWnd>().SetWorkItem(this, selfItem, selfImage.sprite);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Clear();
        WndManager.Instance.GetWnd<BagWnd>().EndDragItem();
    }


    public void OnDrag(PointerEventData eventData)
    {
        WndManager.Instance.GetWnd<BagWnd>().SetWorkItemPos(eventData.position);
    }
}
