using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    // ��ǿ�ʯ�Ƿ��ѱ�����  
    private bool isMined = false;

    // ��ʯ������ʱ���õķ���  
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�������Ƿ�����ң�������ָ�����ھ򹤾ߣ�  
        if (other.CompareTag("Player"))
        {
            // ȷ����ʯֻ����һ��  
            if (!isMined)
            {
                isMined = true;

                // �����ڿ󶯻�����������ѡ��  
                // PlayMiningAnimation();  
                // PlayMiningSound();  

                // ���ٿ�ʯ����  
                Destroy(gameObject);

                // ��ѡ��������ҵ���Դ�����  
                // PlayerResources.Instance.AddOre(1);  
            }
        }
    }
}