using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ShopManager : Singleton<ShopManager>
{
    // 更新UI，管理系统
    public Text income;
    public Text goal;
    public Button adventure;
    public Text adventureText;
    public Button production;
    public Button bag;
    public GameObject Bag;
    public Button exitBag;
    public Button all;
    public Button collection;
    public Button special;

    public GameObject NormalCompleted;
    public GameObject ExcessCompleted;
    public GameObject NormalFailure;
    public GameObject SpecialFailure;
    public Button restart;
    public Button nextLevel;
    public int earnedMoney = 0;
    public Transform entrancePosition;
    public Transform[] customerPositions;

    public ItemSlot[] itemSlots; // 固定的物品槽位
    private const int TOTAL_SLOTS = 18;
     private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return null;
        InitializeComponents();
        SetupListeners();
        // 等待确保 SellingManager 已经初始化
        yield return new WaitUntil(() => SellingManager.Instance != null);
        // 立刻更新所有状态
        UpdateAllUI();
        Debug.Log("count update " + customerPositions.Count());
    }

    private void InitializeComponents()
    {
        // 获取所有组件引用
        GameObject playerStatus = GameObject.Find("PlayerStatus");
        income = playerStatus.transform.Find("Income").GetComponent<Text>();
        goal = playerStatus.transform.Find("Goal").GetComponent<Text>();

        GameObject buttons = GameObject.Find("ButtonsInScene");
        adventure = buttons.transform.Find("AdventurePanel/Adventure").GetComponent<Button>();
        adventureText = buttons.transform.Find("AdventurePanel/AdventureTimes").GetComponent<Text>();
        production = buttons.transform.Find("ProductionPanel/Production").GetComponent<Button>();
        bag = buttons.transform.Find("InventoryPanel/Bag").GetComponent<Button>();
        
        Bag = buttons.transform.Find("BagPanel").gameObject;
        exitBag = Bag.transform.Find("Exit").GetComponent<Button>();
        all = Bag.transform.Find("All").GetComponent<Button>();
        collection = Bag.transform.Find("Collection").GetComponent<Button>();
        special = Bag.transform.Find("Special").GetComponent<Button>();

        Transform itemSlotsPanel = Bag.transform.Find("ItemSlots");
        itemSlots = itemSlotsPanel.GetComponentsInChildren<ItemSlot>();

        GameObject completedPanels = GameObject.Find("CompletedPanels");
        NormalCompleted = completedPanels.transform.Find("NormalComplete").gameObject;
        ExcessCompleted = completedPanels.transform.Find("ExcessComplete").gameObject;
        NormalFailure = completedPanels.transform.Find("NormalFailure").gameObject;
        SpecialFailure = completedPanels.transform.Find("SpecialFailure").gameObject;
        restart = completedPanels.transform.Find("RestartButton").GetComponent<Button>();
        nextLevel = completedPanels.transform.Find("NextLevelButton").GetComponent<Button>();

        entrancePosition = GameObject.Find("Entrance").transform;
        Transform positionsParent = GameObject.Find("CustomerPositions")?.transform;
        // 获取所有Transform（不包括父物体）
        customerPositions = positionsParent.GetComponentsInChildren<Transform>()
            .Where(t => t != positionsParent) // 排除父物体
            .ToArray();
    }

    private void SetupListeners()
    {
        // 设置按钮监听
        adventure.onClick.AddListener(Adventure);
        production.onClick.AddListener(Production);
        restart.onClick.AddListener(Restart);
        nextLevel.onClick.AddListener(NextLevel);
        bag.onClick.AddListener(OpenBag);
        exitBag.onClick.AddListener(CloseBag);
        all.onClick.AddListener(AllInBag);
        collection.onClick.AddListener(CollectionBag);
        special.onClick.AddListener(SpecialBag);
    }

    private void UpdateAllUI()
    {
       // AllInBag();
        UpdateAdventureText();
        ResetSceneButtons();
        UpdatePlayerStatusUI();
    }

    // 赚到钱
    public void EarnMoney(int money)
    {
        earnedMoney += money;
    }

    public void UpdatePlayerStatusUI()
    {
        income.text = $"{earnedMoney}";
        int money = SellingManager.Instance.GetCurrentGoal();
        goal.text = $"{money}";
    }

    // 去冒险
    public void Adventure()
    {
        int currentStage = SellingManager.Instance.GetCurrentStage();
        int remainingAdventureTimes = SellingManager.Instance.GetRemainingAdventrueCount();
        if (remainingAdventureTimes == 0)
        {
            return;
        }
        if (currentStage == 0)
        {
            // TODO 等待修改成正式的关卡
            SceneManager.LoadScene("Level_0");
        }
        else if (currentStage == 1)
        {
            SceneManager.LoadScene("Level_1");
        }
        else if (currentStage == 2)
        {
            SceneManager.LoadScene("Level_2");
        }
        else
        {
            SceneManager.LoadScene("Level_3");
        }

        UpdateAdventureText();
        SellingManager.Instance.GoOnAdventrue();
    }

    // 去制作间（待定场景）
    public void Production()
    {
        SceneManager.LoadScene("ProductionRoom");
    }

    // 更新冒险UI
    public void UpdateAdventureText()
    {
        int remainingAdventureTimes = SellingManager.Instance.GetRemainingAdventrueCount();
        int adventureTimes = SellingManager.Instance.GetCurrentStageAdventrueCount();
        adventureText.text = $"{remainingAdventureTimes}/{adventureTimes}";
    }

    // 打开背包
    public void OpenBag()
    {
        Bag.SetActive(true);
        BagAllButton.Instance.OpenBageImage();
        AllInBag();
    }

    // 关闭背包
    public void CloseBag()
    {
        Bag.SetActive(false);
    }

    // 所有栏目
    public void AllInBag()
    {
        List<Item> allItems = Inventory.Instance.GetItemList();
        if (allItems.Count > 0)
        {
            DisplayItems(allItems);
        }
    }
    
    // 采集物栏目
    public void CollectionBag()
    {
        BagAllButton.Instance.ChangeSourceImage();
        // 按照顺序加载采集物
        List<Item> normalItems = Inventory.Instance.GetItemList().Where(item => item.type == "normal").ToList();
        if (normalItems.Count > 0)
        {
            DisplayItems(normalItems);
        }
    }

    // 特殊物品栏目
    public void SpecialBag()
    {
        BagAllButton.Instance.ChangeSourceImage();
        // 按照顺序加载特殊物品
        List<Item> specialItems = Inventory.Instance.GetItemList().Where(item => item.type == "special").ToList();
        if (specialItems.Count > 0)
        {
            DisplayItems(specialItems);
        }
    }

    private void DisplayItems(List<Item> items)
    {
        // 清空所有槽位
        foreach (var slot in itemSlots)
        {
            slot.ClearSlot();
        }

        // 填充槽位
        for (int i = 0; i < Mathf.Min(items.Count, itemSlots.Length); i++)
        {
            itemSlots[i].UpdateSlot(items[i]);
        }

        // 如果物品数量超过槽位数量？虽然不应当
        if (items.Count > itemSlots.Length)
        {
            Debug.LogWarning("物品数量超过可用槽位数量");
        }
    }



    // 播放不同的UI，并关闭其他的按钮
    public void completedUI(int completedKind)
    {
        if (completedKind == 0)
        {
            ExcessCompleted.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
        else if (completedKind == 1)
        {
            NormalCompleted.SetActive(true);
            nextLevel.gameObject.SetActive(true);
        }
        else if (completedKind == 2)
        {
            NormalFailure.SetActive(true);
            restart.gameObject.SetActive(true);
        }
        else
        {
            SpecialFailure.SetActive(true);
            restart.gameObject.SetActive(true);
        }

    }

    // 恢复按钮正常
    public void ResetSceneButtons()
    {
        ExcessCompleted.SetActive(false);
        NormalCompleted.SetActive(false);
        NormalFailure.SetActive(false);
        SpecialFailure.SetActive(false);
        restart.gameObject.SetActive(false);
        nextLevel.gameObject.SetActive(false);
    }

    // 点击按钮
    public void NextLevel()
    {
        ResetSceneButtons();
        SellingManager.Instance.AdvanceToNextStage();
        UpdatePlayerStatusUI();
    }

    // 重开
    public void Restart()
    {
        ResetSceneButtons();
        SellingManager.Instance.RestartStage();
        UpdatePlayerStatusUI();
    }


}
