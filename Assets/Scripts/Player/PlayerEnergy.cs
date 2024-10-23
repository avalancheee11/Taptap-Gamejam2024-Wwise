using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private EnergyStats_SO energyStats; // 关联的能量属性
    // private float currentEnergy;

    private bool isRunning = false;  // 是否在跑步
    private bool isGathering = false;  // 是否在采集
    private bool isWalking = false;   // 是否在走路

    private void Start()
    {
        energyStats.currentEnergy = energyStats.maxEnergy; // 初始化能量为最大值
    }

    private void Update()
    {
        UpdateEnergy();
    }


//TODO：还需要更改为人物的降低能量值跟不同地图有关 
    private void UpdateEnergy()
    {
        // 根据不同的活动类型减少能量
        if (isGathering)
        {
            DrainEnergy(energyStats.gatherEnergyDrain);
        }
        else if (isRunning)
        {
            DrainEnergy(energyStats.runEnergyDrain);
        }
        else if (isWalking)
        {
            DrainEnergy(energyStats.walkEnergyDrain);
        }

        // 确保能量不会小于0或大于最大值
        energyStats.currentEnergy = Mathf.Clamp(energyStats.currentEnergy, 0f, energyStats.maxEnergy);
    }

    // 减少能量值
    private void DrainEnergy(float drainRate)
    {
        energyStats.currentEnergy -= drainRate * Time.deltaTime;
        // Debug.Log("现在能量为：" + energyStats.currentEnergy);
    }

    // 切换是否走路
    public void SetWalking(bool walking)
    {
        isWalking = walking;
    }

    // 切换是否跑步
    public void SetRunning(bool running)
    {
        isRunning = running;
    }

    // 切换是否采集
    public void SetGathering(bool gathering)
    {
        isGathering = gathering;
    }

    // 返回当前能量百分比，用于UI显示
    public float GetEnergyPercentage()
    {
        return energyStats.currentEnergy / energyStats.maxEnergy;
    }
}
