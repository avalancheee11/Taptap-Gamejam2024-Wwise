using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopToWorld : MonoBehaviour
{
    public void GoWorld()
    {
        SceneManager.LoadScene(1);
    }
}
