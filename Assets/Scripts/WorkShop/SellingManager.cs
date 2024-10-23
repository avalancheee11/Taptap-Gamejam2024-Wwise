using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SellingManager : Singleton<SellingManager>
{
    [SerializeField] private List<GameObject> customerPrefabs;
    [SerializeField] private float customerMoveSpeed = 100f;
    [SerializeField] private float timeBetweenCustomers = 2f;
    private bool isCustomerLeaving = false;
    private const int MaxCustomers = 3;
    private const int MaxStages = 4;

    [Serializable]
    private struct StageData
    {
        public int customerCount;
        public int requiredMoney;
        public List<Customer> predefinedCustomers; // Only used for stage 1
        public int initialAdventureCount; // 存储初始冒险次数
        public int currentAdventureCount; // 当前剩余的冒险次数
    }

    [SerializeField] private StageData[] stageData = new StageData[]
    {
        new StageData { customerCount = 2, requiredMoney = 80, predefinedCustomers = new List<Customer>(), initialAdventureCount = 2 },
        new StageData { customerCount = 3, requiredMoney = 100, predefinedCustomers = new List<Customer>(), initialAdventureCount = 3},
        new StageData { customerCount = 5, requiredMoney = 200, predefinedCustomers = new List<Customer>(), initialAdventureCount = 5 },
        new StageData { customerCount = 7, requiredMoney = 300, predefinedCustomers = new List<Customer>(), initialAdventureCount = 7}
    };

    public int currentStage = 0;
    private int currentCustomerIndex = 0;
    private int earnedMoney = 0;

    private List<Customer> activeCustomers = new List<Customer>();
    private bool isEvaluatingStage = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeAdventureCounts();
        StartStage();
        ShopManager.Instance.UpdatePlayerStatusUI();
        ShopManager.Instance.UpdateAdventureText();
    }

    private void StartStage()
    {
        //Debug.Log("start");
        SpawnInitialCustomers();
        StartCoroutine(ManageCustomers());
    }

    // 初始化当前冒险次数
      private void InitializeAdventureCounts()
    {
        for (int i = 0; i < stageData.Length; i++)
        {
            stageData[i].currentAdventureCount = stageData[i].initialAdventureCount;
        }
    }

    // 阶段初始一次性生成
    private void SpawnInitialCustomers()
    {
        int initialCustomers = Mathf.Min(MaxCustomers, stageData[currentStage].customerCount);
        for (int i = 0; i < initialCustomers; i++)
        {
            Debug.Log("index" + i);
            SpawnCustomerAtPosition(i);
            Debug.Log("done");
        }
        currentCustomerIndex = initialCustomers;
    }

    // 阶段1 固定 - 其他阶段随机，按照位置生成顾客
    private void SpawnCustomerAtPosition(int positionIndex)
    {
        Debug.Log("count " + ShopManager.Instance.customerPositions.Count());
        while (ShopManager.Instance.customerPositions.Count() == 0)
        {
            StartCoroutine(DelayedSpawn(positionIndex));
            return;
        }
        Customer newCustomer;
        if (currentStage == 0 && positionIndex < stageData[0].predefinedCustomers.Count)
        {            
            newCustomer = Instantiate(stageData[0].predefinedCustomers[positionIndex], ShopManager.Instance.customerPositions[positionIndex].position, Quaternion.identity);
            newCustomer.SetPredefinedOrder();
        }
        else
        {
            GameObject randomPrefab = customerPrefabs[UnityEngine.Random.Range(0, customerPrefabs.Count)];
            newCustomer = Instantiate(randomPrefab, ShopManager.Instance.customerPositions[positionIndex].position, Quaternion.identity).GetComponent<Customer>();
            // 刷新订单
            newCustomer.SetOrder(SellingSystem.GenerateRandomOrder());
        }

        activeCustomers.Add(newCustomer);
        newCustomer.OnOrderCompleted += HandleOrderCompleted;
    }

    private IEnumerator DelayedSpawn(int positionIndex)
    {
        // 持续等待直到 customerPositions 准备好
        while (ShopManager.Instance.customerPositions.Count() == 0)
        {
            //Debug.Log("Waiting for customer positions...");
            yield return new WaitForSeconds(0.5f);
        }

        // customerPositions 已经准备好，继续生成
        SpawnCustomerAtPosition(positionIndex);
    }

    // 及时补充角色
    private IEnumerator ManageCustomers()
    {
        while (true)
        {
            if (!isEvaluatingStage && activeCustomers.Count < MaxCustomers && currentCustomerIndex < stageData[currentStage].customerCount)
            {
                yield return StartCoroutine(SpawnCustomer());
                yield return new WaitForSeconds(timeBetweenCustomers);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnCustomer()
    {
        Customer newCustomer;
        GameObject randomPrefab = customerPrefabs[UnityEngine.Random.Range(0, customerPrefabs.Count)];
        Transform entrance = ShopManager.Instance.entrancePosition;
        newCustomer = Instantiate(randomPrefab, entrance.position, Quaternion.identity).GetComponent<Customer>();
        newCustomer.SetOrder(SellingSystem.GenerateRandomOrder());

        currentCustomerIndex++;

        yield return StartCoroutine(MoveCustomerToPosition(newCustomer, GetNextAvailablePosition()));

        activeCustomers.Add(newCustomer);
        newCustomer.OnOrderCompleted += HandleOrderCompleted;
    }

    // 获取下一个可用的位置
    private Vector3 GetNextAvailablePosition()
    {
        return ShopManager.Instance.customerPositions[activeCustomers.Count].position;
    }

    // 客人移动到指定位置 + 或许加动画
    private IEnumerator MoveCustomerToPosition(Customer customer, Vector3 targetPosition)
    {
        while (customer.transform.position != targetPosition)
        {
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPosition, customerMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void HandleOrderCompleted(Customer customer, int moneyEarned)
    {
        // 加钱
        ShopManager.Instance.EarnMoney(moneyEarned);
        ShopManager.Instance.UpdatePlayerStatusUI();
        StartCoroutine(RemoveCustomer(customer));
    }

    private IEnumerator RemoveCustomer(Customer customer)
    {
        isCustomerLeaving = true;
        customer.OnOrderCompleted -= HandleOrderCompleted;

        yield return StartCoroutine(MoveCustomerToPosition(customer, ShopManager.Instance.entrancePosition.position));
        activeCustomers.Remove(customer);
        Destroy(customer.gameObject);

        yield return StartCoroutine(ShiftCustomers());
        isCustomerLeaving = false;

        if (activeCustomers.Count == 0)
        {
            // 当前阶段清0结算
            CheckStageCompletion();
        }
    }

    // 统统往前排
    private IEnumerator ShiftCustomers()
    {
        for (int i = 0; i < activeCustomers.Count; i++)
        {
            Vector3 targetPosition = ShopManager.Instance.customerPositions[i].position;
            yield return StartCoroutine(MoveCustomerToPosition(activeCustomers[i], targetPosition));
        }
    }

    private void CheckStageCompletion()
    {
        if (currentCustomerIndex >= stageData[currentStage].customerCount && !isEvaluatingStage)
        {
            EvaluateStagePerformance();
        }
    }

    // 外部触发检测，例如说制作工坊材料不够
    public void TriggerStageEvaluation()
    {
        if (!isEvaluatingStage)
        {
            EvaluateStagePerformance();
        }
    }

    private void EvaluateStagePerformance()
    {
        Debug.Log(ShopManager.Instance);
        if (ShopManager.Instance.earnedMoney >= stageData[currentStage].requiredMoney)
        {
            if (currentStage < MaxStages - 1)
            {
                if (ShopManager.Instance.earnedMoney >= (stageData[currentStage].requiredMoney * 1.5f))
                {
                    ShopManager.Instance.completedUI(0);//超额完成
                }
                else
                {
                    ShopManager.Instance.completedUI(1); //正常完成
                }
            }
            else
            {
                GameCompleted();
            }
        }
        else
        {
            if (ShopManager.Instance.earnedMoney < stageData[currentStage].requiredMoney)
            {
                ShopManager.Instance.completedUI(2);// 没达到目标
            }
            else 
            {
                // 材料不够
                ShopManager.Instance.completedUI(3); 
            }
        }
    }

    // 下一关
    public void AdvanceToNextStage()
    {
        currentStage++;
        ResetStageVariables();
        Debug.Log($"Advancing to Stage {currentStage + 1}");
        StartStage();
    }

    // 重新开始（可能后面加个UI吧。。确认了再重新开始）
    public void RestartStage()
    {
        ResetStageVariables();
        Debug.Log($"Restarting Stage {currentStage + 1}");
        //TODO 之后要添加上UI的部分
        StartStage();
    }

    // 刷新状态
    private void ResetStageVariables()
    {
        currentCustomerIndex = 0;
        // 钱刷新
        ShopManager.Instance.earnedMoney = 0;
        foreach (var customer in activeCustomers)
        {
            Destroy(customer.gameObject);
        }
        activeCustomers.Clear();
        StopAllCoroutines();
        // 重置冒险次数
        stageData[currentStage].currentAdventureCount = stageData[currentStage].initialAdventureCount;
        ShopManager.Instance.UpdateAdventureText();
    }

    private void GameCompleted()
    {
        // TODO 之后需要补充
        Debug.Log("Congratulations! All stages completed!");
    }

    // 外部获取当前阶段
    public int GetCurrentStage()
    {
        return currentStage;
    }

    // 返回当前阶段金钱目标
    public int GetCurrentGoal()
    {
        int money = stageData[currentStage].requiredMoney;
        return money;
    }

    // 是否有顾客在离开中...
    public bool IsCustomerLeaving()
    {
        return isCustomerLeaving;
    }

    // 获取当前冒险次数
    public int GetRemainingAdventrueCount()
    {
        return stageData[currentStage].currentAdventureCount;
    }

    // 获取总冒险次数
    public int GetCurrentStageAdventrueCount()
    {
        return stageData[currentStage].initialAdventureCount;
    }

    // 去冒险 - 可能之后增加一个打开的UI吧
    public void GoOnAdventrue()
    {
        if (GetRemainingAdventrueCount() > 0)
        {
            // 减少冒险次数
            stageData[currentStage].currentAdventureCount--;
            // 待补充
        }

    }
}