using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}