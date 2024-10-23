using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---- 已重写替换为 GatherStats_SO，可适当时候删除 --------
public class GatherItem : MonoBehaviour
{
    public string id;
    public string modelId; //模型还是图片
    public float interactionRadius; //交互范围
    public Collider collider; //碰撞体
    public List<Item> rewards; //掉落物列表
    public float refreshProbability; //刷新概率
    public float energy; //消耗能量
    public string weaponType; //武器类型

}
