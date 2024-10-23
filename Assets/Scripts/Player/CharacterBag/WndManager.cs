using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WndManager
{
    private static WndManager instance;

    public static WndManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WndManager();
            }
            return instance;
        }
    }

    private Dictionary<string, BaseWnd> dic_allwinds;
    private Transform trans_canvas;
    // Start is called before the first frame update
    public void Initial(Transform canvas)
    {
        Debug.Log("WndManagerInitialSuccess" + canvas);
        dic_allwinds = new Dictionary<string, BaseWnd>();
        trans_canvas = canvas;
    }

    public void ChangeWndOpenStatus<T>() where T: BaseWnd, new()
    {
        string wndName = typeof(T).Name;

        if (dic_allwinds.containsKey(wndName))
        {
            dic_allwinds[wndName].ChangeWnd();
        }
        else
        {
            T wnd = new T();
            wnd.CreateWnd(wndName, trans_canvas);
            wnd.Inital(); //抽象方法，由子类进行实现
            Debug.Log("OpenWnd: wndName:" + wnd.ToString() + " " + wndName + " " + dic_allwinds.Count);
            dic_allwinds.Add(wndName, wnd);
            wnd.OpenWnd();
        }
    }

    public void OpenWnd<T>() where T:BaseWnd, new()
    {
        string wndName = typeof(T).Name;
        
        if(dic_allwinds.containsKey(wndName))
        {
            dic_allwinds[wndName].OpenWnd();
        }
        else
        {
            T wnd = new T();
            wnd.CreateWnd(wndName, trans_canvas);
            wnd.Inital(); //抽象方法，由子类进行实现
            Debug.Log("OpenWnd: wndName:" + wnd.ToString() + " " + wndName + " " + dic_allwinds.Count);
            dic_allwinds.Add(wndName, wnd);
            wnd.OpenWnd();
        }
    }

    public void CloseWnd<T>() where T : BaseWnd, new()
    {
        string wndName = typeof(T).Name;
        if (dic_allwinds.containsKey(wndName))
        {
            dic_allwinds[wndName].CloseWnd();
        }
    }

    public T GetWnd<T>() where T : BaseWnd
    {
        string wndName = typeof(T).Name;
        if (dic_allwinds.containsKey(wndName))
        {
            return dic_allwinds[wndName] as T;
        }
        return null;
    }
 }
