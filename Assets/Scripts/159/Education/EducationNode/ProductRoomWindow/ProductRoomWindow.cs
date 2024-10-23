using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XHFrameWork;

public class ProductRoomWindow : BaseUI
{
    [SerializeField] private CanvasGroup normalPanel;
    [SerializeField] private CanvasGroup specialPanel;
    [SerializeField] private CanvasGroup containerPanel;
    [SerializeField] private CanvasGroup ownContainPanel;
    [SerializeField] private CustomUIDragObject makingPanel;
    [SerializeField] private CustomUIDragObject rubbishPanel;

    [SerializeField] private List<ProductItemNode> normalItemNodes;
    [SerializeField] private List<ProductItemNode> specialItemNodes;
    [SerializeField] private List<ItemContainerNode> containerNodes;
    [SerializeField] private List<ItemContainerNode> ownContainerNodes;
    [SerializeField] private Image dragIcon;
    [Header("制作面板")]
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private ItemContainerNode makingContainer;
    [SerializeField] private List<ProductItemNode> makingItemList;
    [SerializeField] private Text makingLabel;
    protected override bool needStatusObject => true;

    class UIState
    {
        public const int None = 0;
        public const int Wait = 1;
        public const int SelectContainer = 2;
        public const int SelectNormalItem = 3;
        public const int MakingItem = 4;
        public const int SelectSpecialItem = 5;
        public const int Confirm = 6;
    }

    private float delay;
    private float currentDelay;
    
    private ItemContainerDataRoot containerDataRoot => this.mainData.itemContainerDataRoot;

    private bool isPointerEnterMakingPanel;
    private bool isPointerEnterRubbishPanel;

    public override void start(IUIData uiData)
    {
        base.start(uiData);
        foreach (var node in this.normalItemNodes) {
            node.start();
        }

        foreach (var node in this.specialItemNodes) {
            node.start();
        }

        foreach (var node in this.containerNodes) {
            node.start();
        }

        foreach (var node in this.ownContainerNodes) {
            node.start();
        }

        foreach (var node in this.makingItemList) {
            node.start();
        }
        
        this.makingContainer.start();
        this.setCanvasGroupState(normalPanel, false);
        this.setCanvasGroupState(specialPanel, false);
        this.setCanvasGroupState(containerPanel, false);

        var normalItemList = this.mainData.itemDataRoot.dataList.FindAll(x => x.config.itemType == ItemType.Normal);
        for (int i = 0; i < this.normalItemNodes.Count; i++) {
            var node = this.normalItemNodes[i];
            var data = normalItemList.objectValue(i);
            node.setItemData(data);
        }
        
        var specialItemList = this.mainData.itemDataRoot.dataList.FindAll(x => x.config.itemType == ItemType.Special);
        for (int i = 0; i < this.specialItemNodes.Count; i++) {
            var node = this.specialItemNodes[i];
            var data = specialItemList.objectValue(i);
            node.setItemData(data);
        }

        for (int i = 0; i < this.containerNodes.Count; i++) {
            var node = this.containerNodes[i];
            var config = this.mainData.itemContainerConfigRoot.configList.objectValue(i);
            if (config != null) {
                var data = new ItemContainerData();
                data.reloadData(null, config);
                node.setContainerData(data);
            }
        }
        
        this.mainData.itemContainerDataRoot.onContainerDataChangeAction += DataRootOnContainerDataChangeAction;
        this.DataRootOnContainerDataChangeAction();

        this.makingPanel.onPointerEnterAction += MakingPanelOnPointerEnterAction;
        this.makingPanel.onPointerExitAction += MakingPanelOnPointerExitAction;

        this.rubbishPanel.onPointerEnterAction += RubbishPanelOnPointerEnterAction;
        this.rubbishPanel.onPointerExitAction += RubbishPanelOnPointerExitAction;
        
        this.statusActions[UIState.SelectContainer] = this.runSelectContainer;
        this.leaveActions[UIState.SelectContainer] = this.leaveSelectContainer;
        
        this.statusActions[UIState.SelectNormalItem] = this.runSelectNormalItem;
        this.leaveActions[UIState.SelectNormalItem] = this.leaveSelectNormalItem;
        
        this.statusActions[UIState.MakingItem] = this.runMakingItem;
        this.updateActions[UIState.MakingItem] = this.updateMakingItem;
        this.leaveActions[UIState.MakingItem] = this.leaveMakingItem;
        
        this.statusActions[UIState.SelectSpecialItem] = this.runSelectSpecialItem;
        this.leaveActions[UIState.SelectSpecialItem] = this.leaveSelectSpecialItem;

        this.statusActions[UIState.Confirm] = this.runConfirm;
        this.leaveActions[UIState.Confirm] = this.leaveConfirm;

        foreach (var node in this.ownContainerNodes) {
            node.onBeginDragContainerDataAction += NodeOnBeginDragContainerDataActionByOwn;
            node.onEndDragContainerDataAction += NodeOnEndDragContainerDataActionByOwn;
            node.onStayDragContainerDataAction += NodeOnStayDragContainerDataActionByOwn;
        }

        this.dragIcon.gameObject.setActiveByCheck(false);
        this.makingLabel.gameObject.setActiveByCheck(false);
        this.returnBtn.onClick.AddListener(this.clickReturnBtnAction);
        this.checkState();
    }

    protected override void stop()
    {
        this.baseState = UIState.None;
        foreach (var node in this.ownContainerNodes) {
            node.onBeginDragContainerDataAction -= NodeOnBeginDragContainerDataActionByOwn;
            node.onEndDragContainerDataAction -= NodeOnEndDragContainerDataActionByOwn;
            node.onStayDragContainerDataAction -= NodeOnStayDragContainerDataActionByOwn;
        }
        this.returnBtn.onClick.RemoveListener(this.clickReturnBtnAction);
        this.makingPanel.onPointerEnterAction -= MakingPanelOnPointerEnterAction;
        this.makingPanel.onPointerExitAction -= MakingPanelOnPointerExitAction;
        this.rubbishPanel.onPointerEnterAction -= RubbishPanelOnPointerEnterAction;
        this.rubbishPanel.onPointerExitAction -= RubbishPanelOnPointerExitAction;
        this.mainData.itemContainerDataRoot.onContainerDataChangeAction -= DataRootOnContainerDataChangeAction;
        this.makingContainer.stop();
        foreach (var node in this.makingItemList) {
            node.stop();
        }
        foreach (var node in this.normalItemNodes) {
            node.stop();
        }

        foreach (var node in this.specialItemNodes) {
            node.stop();
        }

        foreach (var node in this.containerNodes) {
            node.stop();
        }

        foreach (var node in this.ownContainerNodes) {
            node.stop();
        }
        base.stop();
    }
    
    private void DataRootOnContainerDataChangeAction()
    {
        for (int i = 0; i < this.ownContainerNodes.Count; i++) {
            var node = this.ownContainerNodes[i];
            var data = this.mainData.itemContainerDataRoot.containerDataList.objectValue(i);
            node.setContainerData(data);
        }
    }

    private void MakingPanelOnPointerEnterAction(PointerEventData obj)
    {
        this.isPointerEnterMakingPanel = true;
    }
    
    private void MakingPanelOnPointerExitAction(PointerEventData obj)
    {
        this.isPointerEnterMakingPanel = false;
    }
    
    private void RubbishPanelOnPointerEnterAction(PointerEventData obj)
    {
        this.isPointerEnterRubbishPanel = true;
    }
    
    private void RubbishPanelOnPointerExitAction(PointerEventData obj)
    {
        this.isPointerEnterRubbishPanel = false;
    }

    void runSelectContainer()
    {
        this.setCanvasGroupState(containerPanel, true);
        foreach (var node in this.containerNodes) {
            node.onBeginDragContainerDataAction += NodeOnBeginDragContainerDataActionBySelectContainer;
            node.onEndDragContainerDataAction += NodeOnEndDragContainerDataActionBySelectContainer;
            node.onStayDragContainerDataAction += NodeOnStayDragContainerDataActionBySelectContainer;
        }
    }

    void leaveSelectContainer()
    {
        foreach (var node in this.containerNodes) {
            node.onBeginDragContainerDataAction -= NodeOnBeginDragContainerDataActionBySelectContainer;
            node.onEndDragContainerDataAction -= NodeOnEndDragContainerDataActionBySelectContainer;
            node.onStayDragContainerDataAction -= NodeOnStayDragContainerDataActionBySelectContainer;
        }
        this.setCanvasGroupState(containerPanel, false);
    }

    void runSelectNormalItem()
    {
        this.setCanvasGroupState(this.normalPanel, true);
        foreach (var node in this.normalItemNodes) {
            node.onBeginDragItemDataAction += NodeOnBeginDragItemDataAction;
            node.onEndDragItemDataAction += NodeOnEndDragItemDataAction;
            node.onStayDragItemDataAction += NodeOnStayDragItemDataAction;
        }
    }

    void leaveSelectNormalItem()
    {
        foreach (var node in this.normalItemNodes) {
            node.onBeginDragItemDataAction -= NodeOnBeginDragItemDataAction;
            node.onEndDragItemDataAction -= NodeOnEndDragItemDataAction;
            node.onStayDragItemDataAction -= NodeOnStayDragItemDataAction;
        }
        this.setCanvasGroupState(this.normalPanel, false);
    }

    void runMakingItem()
    {
        this.currentDelay = 0;
        this.delay = 1f;
        this.makingLabel.gameObject.setActiveByCheck(true);
    }

    void updateMakingItem(float dt)
    {
        this.currentDelay += dt;
        if (this.currentDelay >= this.delay) {
            this.currentDelay = 0;
            
            var r = 0;
            var y = 0;
            var b = 0;
            foreach (var itemData in this.containerDataRoot.makingContainerData.itemList) {
                r += itemData.config.red;
                y += itemData.config.yellow;
                b += itemData.config.blue;
            }

            var formulaConfig = ItemFormulaConfigHandler.Instance.configRoot.getConfigByColor(r, y, b);
            if (formulaConfig != null) {
                var data = formulaConfig.getData();
                var itemList = new List<int>();
                foreach (var itemData in this.containerDataRoot.makingContainerData.itemList) {
                    itemList.Add(itemData.config.id);
                }
                data.setSourceItems(itemList);
                this.containerDataRoot.makingContainerData.addFormula(data);
                this.containerDataRoot.makingContainerData.clearItemDataList();
            }
            this.checkState();
        }
    }

    void leaveMakingItem()
    {
        this.makingLabel.gameObject.setActiveByCheck(false);
    }

    void runSelectSpecialItem()
    {
        this.setCanvasGroupState(this.specialPanel, true);
        foreach (var node in this.specialItemNodes) {
            node.onBeginDragItemDataAction += NodeOnBeginDragItemDataAction;
            node.onEndDragItemDataAction += NodeOnEndDragItemDataAction;
            node.onStayDragItemDataAction += NodeOnStayDragItemDataAction;
        }
        this.confirmBtn.onClick.AddListener(this.clickConfirmBtnByConfirmState);
    }

    void leaveSelectSpecialItem()
    {
        this.confirmBtn.onClick.RemoveListener(this.clickConfirmBtnByConfirmState);
        foreach (var node in this.specialItemNodes) {
            node.onBeginDragItemDataAction -= NodeOnBeginDragItemDataAction;
            node.onEndDragItemDataAction -= NodeOnEndDragItemDataAction;
            node.onStayDragItemDataAction -= NodeOnStayDragItemDataAction;
        }
        this.setCanvasGroupState(this.specialPanel, false);
    }

    void runConfirm()
    {
        this.confirmBtn.onClick.AddListener(this.clickConfirmBtnByConfirmState);
    }

    void leaveConfirm()
    {
        this.confirmBtn.onClick.RemoveListener(this.clickConfirmBtnByConfirmState);
    }

    void setCanvasGroupState(CanvasGroup canvasGroup, bool interactable)
    {
        canvasGroup.alpha = interactable ? 1 : 0.5f;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
    
    private void NodeOnBeginDragItemDataAction(ItemData obj)
    {
        this.dragIcon.sprite = obj.config.icon;
        this.dragIcon.gameObject.setActiveByCheck(true);
    }
    
    private void NodeOnEndDragItemDataAction(ItemData obj)
    {
        this.dragIcon.gameObject.setActiveByCheck(false);
        if (!this.isPointerEnterMakingPanel) {
            return;
        }

        if (this.baseState == UIState.SelectNormalItem) {
            obj.reduceCount(1);
            this.containerDataRoot.makingContainerData.addItemData(obj.config.getData());
        }
        else if (this.baseState == UIState.SelectSpecialItem) {
            obj.reduceCount(1);
            this.containerDataRoot.makingContainerData.setSpecialItem(obj.config);
        }

        this.checkState();
    }
    
    private void NodeOnStayDragItemDataAction(ItemData obj)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.dragIcon.transform.parent as RectTransform, Input.mousePosition, Camera.main, out  Vector2 localPoint);
        this.dragIcon.transform.localPosition = localPoint;
    }
    
    private void NodeOnBeginDragContainerDataActionBySelectContainer(ItemContainerData obj)
    {
        this.dragIcon.sprite = obj.config.icon;
        this.dragIcon.gameObject.setActiveByCheck(true);
    }
    
    private void NodeOnEndDragContainerDataActionBySelectContainer(ItemContainerData obj)
    {
        this.dragIcon.gameObject.setActiveByCheck(false);
        if (this.isPointerEnterMakingPanel) {
            var data = new ItemContainerData();
            data.reloadData(null, obj.config);
            this.containerDataRoot.setMakingContainer(data);
            this.checkState();
        }
    }
    
    private void NodeOnStayDragContainerDataActionBySelectContainer(ItemContainerData obj)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.dragIcon.transform.parent as RectTransform, Input.mousePosition, Camera.main, out  Vector2 localPoint);
        this.dragIcon.transform.localPosition = localPoint;
    }
    
    private void NodeOnBeginDragContainerDataActionByOwn(ItemContainerData obj)
    {
        this.dragIcon.sprite = obj.config.icon;
        this.dragIcon.gameObject.setActiveByCheck(true);
    }
    
    private void NodeOnStayDragContainerDataActionByOwn(ItemContainerData obj)
    {
        this.dragIcon.gameObject.setActiveByCheck(false);
        if (this.isPointerEnterRubbishPanel) {
            this.containerDataRoot.removeItemContainerData(obj);
            this.checkState();
        }
    }
    
    private void NodeOnEndDragContainerDataActionByOwn(ItemContainerData obj)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.dragIcon.transform.parent as RectTransform, Input.mousePosition, Camera.main, out  Vector2 localPoint);
        this.dragIcon.transform.localPosition = localPoint;
    }

    void clickConfirmBtnByConfirmState()
    {
        if (this.containerDataRoot.makingContainerData == null) {
            return;
        }
        this.containerDataRoot.addItemContainerData(this.containerDataRoot.makingContainerData);
        this.containerDataRoot.setMakingContainer(null);
        this.checkState();
    }

    void clickReturnBtnAction()
    {
        if (this.containerDataRoot.makingContainerData == null) {
            return;
        }

        if (this.containerDataRoot.makingContainerData.specialItem != null) {
            //返还特殊材料
            var data = this.mainData.itemDataRoot.datas.objectValue(this.containerDataRoot.makingContainerData.specialItem.id);
            if (data != null) {
                data.addCount(1);
            }
            this.containerDataRoot.makingContainerData.setSpecialItem(null);
            this.checkState();
            return;
        }

        if (this.containerDataRoot.makingContainerData.itemList.Count > 0) {
            //返还还没有混合的材料
            var item = this.containerDataRoot.makingContainerData.removeLastItemData();
            var data = this.mainData.itemDataRoot.datas.objectValue(item.id);
            if (data != null) {
                data.addCount(1);
            }
            
            this.checkState();
            return;
        }

        if (this.containerDataRoot.makingContainerData.formulaDataList.Count > 0) {
            //返还可以已经混合的材料
            var formulaData = this.containerDataRoot.makingContainerData.removeLastFormula();
            if (formulaData != null) {
                foreach (var itemId in formulaData.sourceItemList) {
                    var data = this.mainData.itemDataRoot.datas.objectValue(itemId);
                    if (data != null) {
                        data.addCount(1);
                    }
                }
            }
            this.checkState();
            return; 
        }
        //返还瓶子
        this.containerDataRoot.setMakingContainer(null);
        this.checkState();
    }

    void checkState()
    {
        //刷新混合材料
        this.returnBtn.gameObject.setActiveByCheck(this.canReturnItem());
        this.confirmBtn.gameObject.setActiveByCheck(this.baseState == UIState.Confirm || this.baseState == UIState.SelectSpecialItem);
        this.makingContainer.setContainerData(this.containerDataRoot.makingContainerData);
        if (this.containerDataRoot.makingContainerData == null) {
            foreach (var node in this.makingItemList) {
                node.setItemData(null);
            }
        }
        else {
            for (int i = 0; i < this.makingItemList.Count; i++) {
                var node = this.makingItemList[i];
                node.setItemData(this.containerDataRoot.makingContainerData.itemList.objectValue(i));
            }
            if (this.containerDataRoot.makingContainerData.isItemFull) {
                var r = 0;
                var y = 0;
                var b = 0;
                foreach (var itemData in this.containerDataRoot.makingContainerData.itemList) {
                    r += itemData.config.red;
                    y += itemData.config.yellow;
                    b += itemData.config.blue;
                }

                var formulaConfig = ItemFormulaConfigHandler.Instance.configRoot.getConfigByColor(r, y, b);
                if (formulaConfig != null) {
                    this.baseState = UIState.MakingItem;
                    return;
                }
            }
        }
        
        this.confirmBtn.gameObject.setActiveByCheck(false);
        this.returnBtn.gameObject.setActiveByCheck(false);
        
        if (this.containerDataRoot.containerIsFull) {
            this.baseState = UIState.Wait;
            return;
        }

        if (this.containerDataRoot.makingContainerData == null) {
            this.baseState = UIState.SelectContainer;
            return;
        }

        this.returnBtn.gameObject.setActiveByCheck(true);

        if (!this.containerDataRoot.makingContainerData.isFormulaFull) {
            this.baseState = UIState.SelectNormalItem;
            return;
        }
        
        this.confirmBtn.gameObject.setActiveByCheck(true);
        if (this.containerDataRoot.makingContainerData.specialItem == null) {
            this.baseState = UIState.SelectSpecialItem;
            return;
        }
        
        this.baseState = UIState.Confirm;
    }

    bool canReturnItem()
    {
        return this.containerDataRoot.makingContainerData != null;
    }
}
