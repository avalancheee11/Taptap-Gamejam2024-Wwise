using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWnd 
{
    protected Transform SelfTransform
    {
        get;
        private set;
    }

    public bool isOpened;

    public abstract void Inital();

    //��������
    public void CreateWnd(string wndName, Transform Canvas)
    {
        isOpened = true;
        GameObject originWnd = Resources.Load<GameObject>("Prefabs/CharacterBag/Wnd/" + wndName);
        Debug.Log("ResourceLoad: "+ originWnd.name);
        GameObject clonewnd = GameObject.Instantiate(originWnd);
        SelfTransform = clonewnd.transform;
        SelfTransform.SetParent(Canvas, false);
        isOpened = true;
    }
    public void ChangeWnd()
    {
        if (SelfTransform.gameObject.activeSelf) CloseWnd();
        else OpenWnd();
    }

    //�򿪴���
    public void OpenWnd()
    {
        
        
        SelfTransform.gameObject.SetActive(true);
    }

    //�رմ���
    public void CloseWnd()
    {
        isOpened = false;
        SelfTransform.gameObject.SetActive(false);
    }


}
