using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    // Start is called before the first frame update
    public static ItemAssets Instance { get; private set; }

    public Transform pfItemWorld;
    private void Awake()
    {
        Instance = this;
    }

}
