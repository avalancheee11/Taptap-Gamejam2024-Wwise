using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XHFrameWork;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats_SO stats;


    public LayerMask terrainLayer; // Terrain layer for ground check
    public SpriteRenderer sr;

    private PlayerEquipment playerEquipment;
    private PlayerGather playerGather;
    private PlayerAttack playerAttack;

    public GameObject EquipWeapon;
    public GameObject EquipTool;
    public GameObject EquipFishingNet;

    private Vector3 Pos1;
    private Vector3 Pos2;
    private Vector3 Pos3;

    private SellingManager sellingManager;

    private void Start()
    {
        playerEquipment = GetComponent<PlayerEquipment>();
        playerGather = GetComponent<PlayerGather>();
        playerAttack = GetComponent<PlayerAttack>();
        Pos1 = EquipWeapon.transform.position;
        Pos2 = EquipTool.transform.position;
        Pos3 = EquipFishingNet.transform.position;

        // 获取当前的 SellingManager 实例
        sellingManager = SellingManager.Instance;

        // 在阶段三前隐藏渔网
        if (sellingManager.currentStage < 3)
        {
            EquipFishingNet.SetActive(false);
        }

        // // 默认装备武器
        // EquipWeapon.transform.position = Pos2;  // 武器显示在 UI 的位置2
        // EquipTool.transform.position = Pos1;
        // EquipFishingNet.transform.position = Pos3;
        // playerEquipment.EquipWeapon();  // 默认装备武器

        Transform canvasPos = GameObject.Find("UICanvas").transform;
        WndManager.Instance.Initial(canvasPos);
        WndManager.Instance.ChangeWndOpenStatus<BagWnd>();
        WndManager.Instance.CloseWnd<BagWnd>();

    }

    void Update()
    {
        HandleEquipmentSwitch();
        HandlePrimaryAction();
        HandleSecondaryAction();
        HandleInteraction();
        HandleOtherKeys();
        HandleBagWindow();
    }

    void HandleBagWindow()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            WndManager.Instance.ChangeWndOpenStatus<BagWnd>();
        }

    }

    // ***** 鼠标 ********
    // Handle equipment switching (scrolling or pressing 1, 2, 3)
    //TODO:鼠标滚轮和1、2、3按键【切换装备】（需要UI和动画变化）--没做完，还要根据装备的设定写
    // void HandleEquipmentSwitch()
    // {
    // // // Scroll to switch equipment
    // float scroll = Input.GetAxis("Mouse ScrollWheel");
    // if (scroll > 0f)
    // {
    //     currentEquipment = Mathf.Clamp(currentEquipment + 1, 1, 3); // Increment equipment
    // }
    // else if (scroll < 0f)
    // {
    //     currentEquipment = Mathf.Clamp(currentEquipment - 1, 1, 3); // Decrement equipment
    // }


    // 切换装备
    void HandleEquipmentSwitch()
    {
        int currentStage = SellingManager.Instance.GetCurrentStage();

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            playerEquipment.EquipWeapon();
            EquipWeapon.transform.position = Pos2;
            EquipFishingNet.transform.position = Pos1;
            EquipTool.transform.position = Pos3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerEquipment.EquipTool();
            EquipWeapon.transform.position = Pos1;
            EquipFishingNet.transform.position = Pos3;
            EquipTool.transform.position = Pos2;
        }
    
        // 只有当前阶段为3及以上时，才能切换到渔网
        if (sellingManager.currentStage >= 3 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerEquipment.EquipFishingNet();
                EquipWeapon.transform.position = Pos3;
                EquipFishingNet.transform.position = Pos2;
                EquipTool.transform.position = Pos1;
            }
        else
            {
                Debug.Log("渔网装备只能在第三阶段使用！");
            }
        
        Debug.Log("Current Equipment当前装备为: " + playerEquipment.currentEquipment);

    }

    // 鼠标左键：执行主要动作
    void HandlePrimaryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (playerEquipment.currentEquipment)
            {
                case EquipmentType.Weapon:
                    playerAttack.Attack();
                    break;
                case EquipmentType.Tool:
                    playerGather.Gather();
                    break;
                case EquipmentType.FishingNet:
                    playerGather.Fish();
                    break;
            }
        }
    }

    // Handle secondary action (right mouse button)
    //TODO：鼠标右键-次要动作，互动动作
    void HandleSecondaryAction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Secondary action executed互动动作"); // Implement feature-based interaction (e.g., light interactions)
        }
    }

    // Handle interaction/pick up (space bar)
    //TODO：互动，拾取（对话，宝箱等等）---可能会改成F按键？还是space？待定
    void HandleInteraction()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
          if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Interacting or picking up item 交互按键");
            // Implement interaction logic with objects in the world
        }
    }

    // Handle other keys (pause, map, etc.)
    //TODO：****其他游戏界面按钮*****
    void HandleOtherKeys()
    {
        // Pause game (Esc key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Game Paused 游戏暂停");
            // Implement game pause functionality
            // 游戏暂停有Esc和UI按钮
        }

        // Open inventory (Tab key)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Inventory Opened打开背包");
            // Implement inventory opening functionality
        }

        //TODO：地图打开后，鼠标滚轮进行大小缩放---地图功能不要了 Open map (M key)
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Map Opened");
            // Implement map opening functionality

        }

        //TODO： Open character panel (E key) 打开人物面板
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Character Panel Opened 打开人物面板");
            // Implement character panel opening functionality
        }
    }

}