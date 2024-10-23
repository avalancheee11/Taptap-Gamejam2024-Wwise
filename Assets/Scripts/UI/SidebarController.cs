using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarController : MonoBehaviour
{
    public Button openButton;
    public RectTransform sidebarPanel;
    public Button closeButton;

    private bool isSidebarVisible = false;

    public Text adventureText;

    void Start()
    {
        // ���ð�ť�ĵ���¼�  
        openButton.onClick.AddListener(OpenSidebar);
        closeButton.onClick.AddListener(CloseSidebar);
        UpdateUIText();
    }

    public void OpenSidebar()
    {
        // �����������leftλ������Ϊ*��ʹ�����Ļ������  
        sidebarPanel.anchoredPosition = new Vector2(-230, sidebarPanel.anchoredPosition.y);
        isSidebarVisible = true;
        UpdateUIText();
    }

    public void CloseSidebar()
    {
        // �����������leftλ�����ã�ʹ�����Ļ�����ʧ  
        sidebarPanel.anchoredPosition = new Vector2(-1000, sidebarPanel.anchoredPosition.y);
        isSidebarVisible = false;
    }

    /// 更新UI
    public void UpdateUIText()
    {
        int adventureTimes = SellingManager.Instance.GetCurrentStageAdventrueCount();
        int remainingAdventureTimes = SellingManager.Instance.GetRemainingAdventrueCount();
        adventureText.text = $"{remainingAdventureTimes}/{adventureTimes}";
    }
}