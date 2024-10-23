using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private Image energyBar;

    private void Update()
    {
        // 更新能量条
        energyBar.fillAmount = playerEnergy.GetEnergyPercentage();
    }
}
