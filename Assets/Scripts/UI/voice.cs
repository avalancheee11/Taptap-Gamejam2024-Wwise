using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����UI�����ռ�  

public class SoundToggle : MonoBehaviour
{
    public AudioSource audioSource; // ����AudioSource���  
    public Button toggleButton; // ����UI��ť  

    // ������Inspector��ֱ�Ӹ�ֵ��Ҳ����ͨ���������  
    void Start()
    {
        // ���û����Inspector�и�ֵ������ͨ���������AudioSource��Button  
        audioSource = GetComponent<AudioSource>();  
        toggleButton = GameObject.Find("SettingsPanel").GetComponent<Button>();  

        // ���ð�ť�ĵ���¼�������  
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleSound);
        }
        else
        {
            Debug.LogError("Toggle button not found!");
        }
    }

    // �л������ķ���  
    void ToggleSound()
    {
        // ���AudioSource�Ƿ����  
        if (audioSource != null)
        {
            // ����������ڲ��ţ���ֹͣ�������򲥷�  
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); // Ҳ����ʹ��Stop()����ȫֹͣ����  
            }
            else
            {
                audioSource.Play();
            }

            // ���°�ť���ı���ͼ�꣬�Է�ӳ��ǰ״̬����ѡ��  
            // ���磺toggleButton.GetComponentInChildren<Text>().text = audioSource.isPlaying ? "Pause" : "Play";  
        }
        else
        {
            Debug.LogError("Audio source not found!");
        }
    }

    // �����Ҫ�����ٶ���ʱ��������������������OnDestroy����  
    void OnDestroy()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.RemoveListener(ToggleSound);
        }
    }
}