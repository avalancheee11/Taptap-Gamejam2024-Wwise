using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl1111 : MonoBehaviour
{
    public float speed = 3;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        

        Vector3 dir = new Vector3(h, 0, v);

        transform.Translate(dir * speed * Time.deltaTime);
    }

}
