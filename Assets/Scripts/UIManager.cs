using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
     public static UIManager Instance;

[Header("Stats")]
   [SerializeField] private PlayerStats_SO stats;
    [SerializeField] private EnergyStats_SO energyStats; // 关联的能量属性

    [Header("Bars")]
   [SerializeField] private Image healthBar;
   [SerializeField] private Image energyBar;
    
    [Header("Text")]
    [SerializeField] private Text healthText;  // 使用 legacy Text 组件
    [SerializeField] private Text energyText;  // 使用 legacy Text 组件

   
//    [Header("Text")]
   
//    [SerializeField] private TextMeshProUGUI healthTMP;
//    [SerializeField] private TextMeshProUGUI energyTMP;

   // 把 Level，Health血量，Mana法力（体力）和Exp的数值显示在 UI 上。
   // 数值匹配Bar - image -> Fill Amount
   // 数值匹配 TMP -> Text Input


void Awake() {
           if (Instance == null)
        {
            Instance = this;
            // 其他初始化代码
        }
        else
        {
            Destroy(gameObject);
        }
        healthBar.fillAmount = 1;
        energyBar.fillAmount=1;
}

    private void Update() {
        UpdatePlayerUI();
    }

    // private void UpdatePlayerUI(){
    //     // Filling color images
    //     healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, stats.health / 100, 10f * Time.deltaTime); 
    //     energyBar.fillAmount= Mathf.Lerp(energyBar.fillAmount, energyStats.currentEnergy / energyStats.maxEnergy, 10f * Time.deltaTime);
    //     Debug.Log("ui stats.health:"+stats.health);
    //     Debug.Log("ui stats.health:"+stats.maxHealth);
    //     // Texts
    //     // levelTMP.text = $"Level {stats.Level}";
    //     // healthTMP.text = $" {stats.health} / {stats.maxHealth}";
    //     // energyBarTMP.text = $"{energyStats.currentEnergy} / {energyStats.maxEnergy}";

    // }

    public void UpdatePlayerUI(){
    // Check for maxHealth to avoid division by zero
    if (stats.maxHealth > 0) {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)stats.health / stats.maxHealth, 10f * Time.deltaTime);
        healthText.text = $"{stats.health}/{stats.maxHealth}";

    }

    if (energyStats.maxEnergy > 0) {
        energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, (float)energyStats.currentEnergy / energyStats.maxEnergy, 10f * Time.deltaTime);
        int currentEnergyInt = Mathf.FloorToInt(energyStats.currentEnergy); // 或者 Mathf.RoundToInt()
        int maxEnergyInt = Mathf.FloorToInt(energyStats.maxEnergy); // 将最大值也转换为整数
        energyText.text = $"{currentEnergyInt}/{maxEnergyInt}";


    }

    // Debug.Log($"ui stats.health: {stats.health} / {stats.maxHealth}");
}
}
