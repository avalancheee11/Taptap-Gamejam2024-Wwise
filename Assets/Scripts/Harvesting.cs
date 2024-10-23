using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvesting : MonoBehaviour
{
    // 引用玩家控制器（如果采集逻辑与玩家移动等逻辑紧密相关）  
    // 或者，你可以直接在需要的地方获取这个组件的引用  
    // private PlayerController playerController;  

    // 采集工具是否激活的标识  
    private bool isHarvesting = false;

    // 采集物（矿石、植物等）的引用  
    // 这个可以通过射线检测（Raycast）或其他方式获取  
    private GameObject currentHarvestable = null;

    // 采集速度（每秒采集的数量或进度）  
    public float harvestRate = 1.0f;

    // 采集所需的总时间（或者总次数）  
    // 这个可以根据采集物的类型或数量来设定  
    public float totalHarvestTime = 5.0f;

    // 已经采集的时间或次数  
    private float elapsedHarvestTime = 0.0f;

    // 更新逻辑  
    void Update()
    {
        // 检查玩家是否按下了鼠标左键并且装备了采集工具  
        if (Input.GetMouseButtonDown(0) && IsHarvestingToolEquipped())
        {
            StartHarvesting();
        }

        // 如果正在采集，则继续采集逻辑  
        if (isHarvesting)
        {
            ContinueHarvesting();

            // 检查是否采集完成  
            if (IsHarvestComplete())
            {
                StopHarvesting();
            }
        }

        // 如果玩家松开了鼠标左键，则停止采集  
        if (Input.GetMouseButtonUp(0))
        {
            StopHarvesting();
        }
    }

    // 开始采集的逻辑  
    private void StartHarvesting()
    {
        // 使用射线检测或其他方式找到当前玩家面对的采集物  
        // 这里假设已经通过某种方式设置了currentHarvestable  
        if (currentHarvestable != null)
        {
            isHarvesting = true;
            elapsedHarvestTime = 0.0f;

            // 播放采集动画或声音（可选）  
            // PlayHarvestAnimation();  
            // PlayHarvestSound();  
        }
    }

    // 继续采集的逻辑  
    private void ContinueHarvesting()
    {
        // 增加已采集的时间  
        elapsedHarvestTime += Time.deltaTime;

        // 根据采集速度和已采集时间更新采集进度（这里省略了具体的进度显示逻辑）  
        // UpdateHarvestProgress(elapsedHarvestTime / totalHarvestTime);  
    }

    // 检查采集是否完成的逻辑  
    private bool IsHarvestComplete()
    {
        return elapsedHarvestTime >= totalHarvestTime;
    }

    // 停止采集的逻辑  
    private void StopHarvesting()
    {
        isHarvesting = false;

        // 如果采集完成，则销毁采集物或更新其状态  
        if (currentHarvestable != null && IsHarvestComplete())
        {
            Destroy(currentHarvestable);
            // 或者：currentHarvestable.GetComponent<Harvestable>().OnHarvested();  

            // 重置采集状态  
            elapsedHarvestTime = 0.0f;
            currentHarvestable = null;

            // 播放采集完成动画或声音（可选）  
            // PlayHarvestCompleteAnimation();  
            // PlayHarvestCompleteSound();  
        }
    }

    // 检查玩家是否装备了采集工具的逻辑（需要根据你的游戏逻辑来实现）  
    private bool IsHarvestingToolEquipped()
    {
        // 这里应该有一个逻辑来判断玩家是否装备了正确的工具  
        // 比如检查玩家的背包、装备栏或手持物品  
        return true; // 临时返回true以进行测试  
    }

    // 射线检测或其他方式找到采集物的逻辑（需要实现）  
    // private void FindHarvestableInFrontOfPlayer()  
    // {  
    //     // 实现射线检测逻辑，找到玩家面前的采集物并设置currentHarvestable  
    // }  
}
