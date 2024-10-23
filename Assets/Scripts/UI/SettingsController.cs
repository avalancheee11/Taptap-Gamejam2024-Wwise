using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Button openButton;
    public RectTransform SettingsPanel;
    public Button closeButton;

    private bool isSettingsVisible = false;

    void Start()
    {
        // ���ð�ť�ĵ���¼�  
        openButton.onClick.AddListener(OpenSettings);
        closeButton.onClick.AddListener(CloseSettings);
    }

    public void OpenSettings()
    {
        // �����������leftλ������Ϊ*��ʹ�����Ļ������  
        SettingsPanel.anchoredPosition = new Vector2(0, 0);
        isSettingsVisible = true;
    }

    public void CloseSettings()
    {
        // �����������leftλ�����ã�ʹ�����Ļ�����ʧ  
        SettingsPanel.anchoredPosition = new Vector2(0, 450);
        isSettingsVisible = false;
    }

}