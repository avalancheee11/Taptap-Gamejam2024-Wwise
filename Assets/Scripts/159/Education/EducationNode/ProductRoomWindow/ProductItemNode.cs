using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductItemNode : CustomUIDragObject
{
    [SerializeField] private Text countLabel;
    [SerializeField] private Text nameLabel;
    [SerializeField] private Image icon;
    private ItemData itemData;
    public event Action<ItemData> onBeginDragItemDataAction;
    public event Action<ItemData> onEndDragItemDataAction;
    public event Action<ItemData> onStayDragItemDataAction;
    
    public void start()
    {
        this.setItemData(null);
        this.refreshView();
    }

    public void stop()
    {
        this.setItemData(null);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (this.itemData != null && this.itemData.count > 0) {
            this.onBeginDragItemDataAction?.Invoke(this.itemData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (this.itemData != null && this.itemData.count > 0) {
            this.onEndDragItemDataAction?.Invoke(this.itemData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (this.itemData != null && this.itemData.count > 0) {
            this.onStayDragItemDataAction?.Invoke(this.itemData);
        }
    }
    
    public void setItemData(ItemData itemData)
    {
        if (this.itemData == itemData) {
            return;
        }
        if (this.itemData != null) {
            this.itemData.onCountChangeAction -= ItemDataOnCountChangeAction;
        }
        this.itemData = itemData;
        if (this.itemData != null) {
            this.itemData.onCountChangeAction += ItemDataOnCountChangeAction;
        }

        this.refreshView();
    }

    private void ItemDataOnCountChangeAction()
    {
        this.countLabel.text = this.itemData.count.toMPString();
    }

    void refreshView()
    {
        if (this.itemData == null) {
            this.gameObject.setActiveByCheck(false);
            return;
        }
        this.gameObject.setActiveByCheck(true);
        this.countLabel.text = this.itemData.count.toMPString();
        this.nameLabel.text = this.itemData.config.name;
        this.icon.sprite = this.itemData.config.icon;
    }
}
