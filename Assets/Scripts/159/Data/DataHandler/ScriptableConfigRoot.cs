using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableConfigRoot<T> : ScriptableObject where T : ScriptableObject, new()
{
    [SerializeField]private List<T> configList;

    protected Dictionary<string,T> configs;

    //初始化抽象方法 需要子类去实现这个方法
    public void OnInit()
    {
        this.configs = new Dictionary<string, T>();
        for (int i = 0; i < this.configList.Count; i++) {
            this.configs[this.configList[i].name] = this.configList[i];
        }
    }
}