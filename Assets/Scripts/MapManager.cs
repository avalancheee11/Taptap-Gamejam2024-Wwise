using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapStats_SO currentMapData; // 当前场景的地图数据
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (player == null)
        {
            Debug.LogError("玩家对象未找到，请确保场景中有一个带有 Player 脚本的对象。");
            return;
        }


        // 根据地图配置设置玩家的出生点
        SetPlayerSpawnPoint();

        // 开始怪物刷新
        // StartCoroutine(SpawnMonsters());
    }

    void SetPlayerSpawnPoint()
    {
        Vector3 spawnPoint = currentMapData.playerSpawnPoint;
        // 设置玩家出生点逻辑，例如传送玩家到指定坐标
        player.transform.position = spawnPoint;
    }

    // IEnumerator SpawnMonsters()
    // {
    //     foreach (var entry in currentMapData.monsterSpawnProbabilities)
    //     {
    //         MonsterStats_SO monsterData = entry.Key;
    //         float probability = entry.Value;

    //         // 根据概率决定是否生成怪物
    //         if (Random.value <= probability)
    //         {
    //             // 刷新怪物逻辑
    //             SpawnMonster(monsterData);
    //         }
    //     }

    //     yield return null; // 可以根据需要控制刷怪频率
    // }

    void SpawnMonster(MonsterStats_SO monsterData)
    {
        // 生成怪物的具体逻辑
        Instantiate(monsterData.monsterPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        // 返回一个怪物刷新的随机坐标，具体实现可以根据地图需求调整
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}
