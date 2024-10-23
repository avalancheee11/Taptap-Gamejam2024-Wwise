using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CustomUIDragObject :CustomUIComponent, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action<PointerEventData> onBeginDragAction;
    public event Action<PointerEventData> onDragAction;
    public event Action<PointerEventData> onEndDragAction;
    public event Action<PointerEventData> onPointerEnterAction;
    public event Action<PointerEventData> onPointerExitAction;
    public event Action<PointerEventData> onClickAction;
    public event Action<PointerEventData> onDownAction;
    public event Action<PointerEventData> onUpAction;
    
    private CanvasGroup _canvasGroup;

    public CanvasGroup canvasGroup
    {
        get
        {
            if (this._canvasGroup == null) {
                this._canvasGroup =  this.gameObject.GetComponent<CanvasGroup>();
            }
            return this._canvasGroup;
        }
    }
    
    private Selectable _selectableObj;

    public Selectable selectableObj
    {
        get
        {
            if (this._selectableObj == null) {
                this._selectableObj = this.gameObject.GetComponent<Selectable>();
                if (this._selectableObj == null) {
                    var btn = this.gameObject.AddComponent<Button>();
                    btn.transition = Selectable.Transition.None;
                    this._selectableObj = btn;
                }
            }
            return this._selectableObj;
        }
    }

    protected virtual bool canNavigation => true;

    public virtual void OnDrag(PointerEventData eventData)
    {
        this.onDragAction?.Invoke(eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        this.onBeginDragAction?.Invoke(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        this.onEndDragAction?.Invoke(eventData);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        this.onPointerEnterAction?.Invoke(eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        this.onPointerExitAction?.Invoke(eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        this.onClickAction?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.onDownAction?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.onUpAction?.Invoke(eventData);
    }
}