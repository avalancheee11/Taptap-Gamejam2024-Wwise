using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Map Data", menuName = "Map/Map Data")]

public class MapStats_SO : ScriptableObject
{
    [Header("怪物刷新配置 Monster Spawn Config")]
    // 存储怪物和它的刷新概率
    public Dictionary<MonsterStats_SO, float> monsterSpawnProbabilities = new Dictionary<MonsterStats_SO, float>();
   public string mapName;

    [Header("玩家出生点 Player Spawn Point")]
    public Vector3 playerSpawnPoint; // 固定的玩家出生坐标

    // 可以扩展地图的其他配置，如怪物数量上限等
}
