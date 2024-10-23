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

    //创建窗口
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

    //打开窗口
    public void OpenWnd()
    {
        
        
        SelfTransform.gameObject.SetActive(true);
    }

    //关闭窗口
    public void CloseWnd()
    {
        isOpened = false;
        SelfTransform.gameObject.SetActive(false);
    }


}
