using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContainerNode : CustomUIDragObject
{
    [SerializeField] private Image icon;
    [SerializeField] private Image formatIcon;

    private ItemContainerData data;

    public event Action<ItemContainerData> onBeginDragContainerDataAction;
    public event Action<ItemContainerData> onEndDragContainerDataAction;
    public event Action<ItemContainerData> onStayDragContainerDataAction;

    private static Color emptyColor = new Color(0, 0, 0, 0);
    
    public void start()
    {
        this.setContainerData(null);
        var mat = new Material(this.formatIcon.material.shader);
        this.formatIcon.material = mat;
    }

    public void stop()
    {
        this.setContainerData(null);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (this.data != null) {
            this.onBeginDragContainerDataAction?.Invoke(this.data);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (this.data != null) {
            this.onEndDragContainerDataAction?.Invoke(this.data);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (this.data != null) {
            this.onStayDragContainerDataAction?.Invoke(this.data);
        }
    }

    public void setContainerData(ItemContainerData data)
    {
        if (this.data != null) {
            this.data.onFormulaChangeAction -= DataOnFormulaChangeAction;
            this.data.onSpecialItemChangeAction -= DataOnSpecialItemChangeAction;
        }
        this.data = data;
        if (this.data != null) {
            this.data.onFormulaChangeAction += DataOnFormulaChangeAction;
            this.data.onFormulaChangeAction += DataOnFormulaChangeAction;
        }
        this.refreshView();
    }

    private void DataOnSpecialItemChangeAction()
    {
        this.refreshView();
    }

    private void DataOnFormulaChangeAction()
    {
        this.refreshView();
    }

    void refreshView()
    {
        if (this.data == null) {
            this.gameObject.setActiveByCheck(false);
            return;
        }
        this.gameObject.setActiveByCheck(true);
        this.icon.sprite = this.data.config.icon;
        var core = this.data.formulaDataList.Count >= 1 ? this.data.formulaDataList[0].config.color : emptyColor;
        var inter = this.data.formulaDataList.Count >= 2 ? this.data.formulaDataList[1].config.color : emptyColor;
        var outer = this.data.formulaDataList.Count >= 3 ? this.data.formulaDataList[2].config.color : emptyColor;

        this.formatIcon.sprite = this.data.config.interIcon;
        this.formatIcon.material.SetTexture("_Mask", this.data.config.interIcon.texture);
        this.formatIcon.material.SetColor("_Core_Color", core);
        this.formatIcon.material.SetColor("_Inner_Color", inter);
        this.formatIcon.material.SetColor("_Outer_Color", outer);
    }
}
