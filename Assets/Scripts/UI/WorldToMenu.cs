using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldToMenu : MonoBehaviour
{
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
