using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvesting : MonoBehaviour
{
    // ������ҿ�����������ɼ��߼�������ƶ����߼�������أ�  
    // ���ߣ������ֱ������Ҫ�ĵط���ȡ������������  
    // private PlayerController playerController;  

    // �ɼ������Ƿ񼤻�ı�ʶ  
    private bool isHarvesting = false;

    // �ɼ����ʯ��ֲ��ȣ�������  
    // �������ͨ�����߼�⣨Raycast����������ʽ��ȡ  
    private GameObject currentHarvestable = null;

    // �ɼ��ٶȣ�ÿ��ɼ�����������ȣ�  
    public float harvestRate = 1.0f;

    // �ɼ��������ʱ�䣨�����ܴ�����  
    // ������Ը��ݲɼ�������ͻ��������趨  
    public float totalHarvestTime = 5.0f;

    // �Ѿ��ɼ���ʱ������  
    private float elapsedHarvestTime = 0.0f;

    // �����߼�  
    void Update()
    {
        // �������Ƿ���������������װ���˲ɼ�����  
        if (Input.GetMouseButtonDown(0) && IsHarvestingToolEquipped())
        {
            StartHarvesting();
        }

        // ������ڲɼ���������ɼ��߼�  
        if (isHarvesting)
        {
            ContinueHarvesting();

            // ����Ƿ�ɼ����  
            if (IsHarvestComplete())
            {
                StopHarvesting();
            }
        }

        // �������ɿ�������������ֹͣ�ɼ�  
        if (Input.GetMouseButtonUp(0))
        {
            StopHarvesting();
        }
    }

    // ��ʼ�ɼ����߼�  
    private void StartHarvesting()
    {
        // ʹ�����߼���������ʽ�ҵ���ǰ�����ԵĲɼ���  
        // ��������Ѿ�ͨ��ĳ�ַ�ʽ������currentHarvestable  
        if (currentHarvestable != null)
        {
            isHarvesting = true;
            elapsedHarvestTime = 0.0f;

            // ���Ųɼ���������������ѡ��  
            // PlayHarvestAnimation();  
            // PlayHarvestSound();  
        }
    }

    // �����ɼ����߼�  
    private void ContinueHarvesting()
    {
        // �����Ѳɼ���ʱ��  
        elapsedHarvestTime += Time.deltaTime;

        // ���ݲɼ��ٶȺ��Ѳɼ�ʱ����²ɼ����ȣ�����ʡ���˾���Ľ�����ʾ�߼���  
        // UpdateHarvestProgress(elapsedHarvestTime / totalHarvestTime);  
    }

    // ���ɼ��Ƿ���ɵ��߼�  
    private bool IsHarvestComplete()
    {
        return elapsedHarvestTime >= totalHarvestTime;
    }

    // ֹͣ�ɼ����߼�  
    private void StopHarvesting()
    {
        isHarvesting = false;

        // ����ɼ���ɣ������ٲɼ���������״̬  
        if (currentHarvestable != null && IsHarvestComplete())
        {
            Destroy(currentHarvestable);
            // ���ߣ�currentHarvestable.GetComponent<Harvestable>().OnHarvested();  

            // ���òɼ�״̬  
            elapsedHarvestTime = 0.0f;
            currentHarvestable = null;

            // ���Ųɼ���ɶ�������������ѡ��  
            // PlayHarvestCompleteAnimation();  
            // PlayHarvestCompleteSound();  
        }
    }

    // �������Ƿ�װ���˲ɼ����ߵ��߼�����Ҫ���������Ϸ�߼���ʵ�֣�  
    private bool IsHarvestingToolEquipped()
    {
        // ����Ӧ����һ���߼����ж�����Ƿ�װ������ȷ�Ĺ���  
        // ��������ҵı�����װ�������ֳ���Ʒ  
        return true; // ��ʱ����true�Խ��в���  
    }

    // ���߼���������ʽ�ҵ��ɼ�����߼�����Ҫʵ�֣�  
    // private void FindHarvestableInFrontOfPlayer()  
    // {  
    //     // ʵ�����߼���߼����ҵ������ǰ�Ĳɼ��ﲢ����currentHarvestable  
    // }  
}
