using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string id;
    public int amount;

    // 增加一个普通/特殊的type标识
    public string type;
    internal string itemspname;
    internal string information;
    public Sprite IconInBag;

    public Sprite GetSprite()
    {
        //TODO: ӳ��ÿ�����ƺͶ�Ӧ��ͼ��
        switch (id)
        {
            default: return null;
        }
    }

    public Color GetColor()
    {
        //TODO: ӳ��ÿ�����ƺͶ�Ӧ����ɫ
        switch (id)
        {
            default: return new Color(1, 0, 0);
        }
    }
}
