using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopToProductionRoom : MonoBehaviour
{
    public void GoProductionRoom()
    {
        SceneManager.LoadScene("ProductionRoom");
    }
}
