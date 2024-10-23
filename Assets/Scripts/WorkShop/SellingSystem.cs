using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SellingSystem : MonoBehaviour
{
    // 订单类
    [System.Serializable]
    public class Order
    {
        // 之后需要根据这个颜色（或许是色值？来切换对应的颜色）
        public string colorCategory;
        public string specificColor;
        public string materialName;
        public string orderText;
        public int reward;
        public bool isCompleted;
        public int level; // 当前物品等级 订单0为低级， 1为高级

        // 创建订单
        public Order(string colorCat, string specificClr, string mat, string text, int rw, int lvl)
        {
            colorCategory = colorCat;
            specificColor = specificClr;
            materialName = mat;
            orderText = text;
            reward = 0;
            isCompleted = false;
            level = lvl;
        }
    }

    // 背包系统接口
    public interface IInventorySystem
    {
        // 检测是否有对应的item
        bool HasItem(string itemName);
        // 移除物品
        void RemoveItem(string itemName, int quantity);

        //做测试用接口
        void UnitTest();
    }

    // 颜色类型// 待修改

    // 订单库 
    // 颜色库
    private static Dictionary<string, List<string>> colorCategories = new Dictionary<string, List<string>>
    {
        { "红", new List<string> { "低红", "中红", "高红" } },
        { "黄", new List<string> { "浅黄", "中黄", "深黄" } },
        //{ "蓝", new List<string> { "淡蓝", "中蓝", "深蓝" } },
        //{ "橙", new List<string> { "浅橙", "中橙", "深橙" } },
        //{ "绿", new List<string> { "淡绿", "中绿", "深绿" } },
        //{ "紫", new List<string> { "浅紫", "中紫", "深紫" } },
        { "黑", new List<string> { "黑" } }
    };
    // 素材库
    private static string[] materials = {"材料1", "材料2", "材料3"};
    // 颜色文本库
    private static Dictionary<string, List<string>> textsByColor = new Dictionary<string, List<string>>
    {
        { "低红", new List<string> { "低红文本1", "低红文本2" } },
        { "中红", new List<string> { "中红文本1", "中红文本2", "中红文本3" } },
        { "高红", new List<string> { "高红文本1", "高红文本2", "高红文本3" } },
        { "浅黄", new List<string> { "浅黄文本1", "浅黄文本2", "浅黄文本3" } },
        { "中黄", new List<string> { "中黄文本1", "中黄文本2", "中黄文本3" } },
        { "深黄", new List<string> { "深黄文本1", "深黄文本2", "深黄文本3" } },
        { "黑", new List<string> { "黑色文本1", "黑色文本2" } }
        //{ "低黄", new List<string> { "黄色文本1", "黄色文本2", "黄色文本3" } },
        //{ "橙", new List<string> { "橙色文本1", "橙色文本2", "橙色文本3" } },
        //{ "蓝", new List<string> { "蓝色文本1", "蓝色文本2", "蓝色文本3" } },
        //{ "紫", new List<string> { "紫色文本1", "紫色文本2", "紫色文本3" } }
    };

    // 材料文本
    private static Dictionary<string, List<string>> textsByMaterial = new Dictionary<string, List<string>>
    {
        { "材料1", new List<string> { "材料1文本1", "材料1文本2" } },
        { "材料2", new List<string> { "材料2文本1", "材料2文本2" } },
        { "材料3", new List<string> { "材料3文本1", "材料3文本2" } },
        // ... 为所有材料添加对应的文本
    };


    // 刷随机订单
    public static Order GenerateRandomOrder()
    {
        string randomColorCategory = colorCategories.Keys.ToList()[Random.Range(0, colorCategories.Count)];
        string randomSpecificColor = colorCategories[randomColorCategory][Random.Range(0, colorCategories[randomColorCategory].Count)];

        bool isHighLevelOrder = Random.value <= 0.6f; // 60%的概率是高级订单
        string material = "";
        string orderText;
        int level = 0;
        //Debug.Log("高级订单吗？ " + isHighLevelOrder);
        // TODO 更改背景颜色
        if (isHighLevelOrder)
        {
            material = materials[Random.Range(0, materials.Length)];
            orderText = textsByMaterial[material][Random.Range(0, textsByMaterial[material].Count)];
            level = 1; // 变成高级订单了捏
        }
        else
        {
            orderText = textsByColor[randomSpecificColor][Random.Range(0, textsByColor[randomSpecificColor].Count)]; 
        }
    
        // TODO 价格怎么算啊
        int baseReward = 0;
        Order newOrder  = new Order(randomColorCategory, randomSpecificColor, material, orderText, baseReward, level);
        //Debug.Log($"New order created: Category={newOrder.colorCategory}, SpecificColor={newOrder.specificColor}, Material={newOrder.materialName}, Text={newOrder.orderText}, Level={newOrder.level}");
        return newOrder;
    }

    // 调用计算订单价格
    public static int CalculateReward(Order order, string providedColorCategory, string providedSpecificColor, string providedMaterial)
    {
        int reward = 0;

        if (order.colorCategory == providedColorCategory)
        {
            reward += 50;  // 颜色大类一致
            if (order.specificColor == providedSpecificColor)
            {
                reward += 30;  // 具体颜色完全一致
            }
        }

        if (!string.IsNullOrEmpty(order.materialName) && order.materialName == providedMaterial)
        {
            reward += 50;  // 材料一致
        }

        return reward;
    }
    
    // 预制单
    public static Order CreatePredefinedOrder(Order predefinedOrder)
    {
        return new Order(
            predefinedOrder.colorCategory,
            predefinedOrder.specificColor,
            predefinedOrder.materialName,
            predefinedOrder.orderText,
            predefinedOrder.reward,
            predefinedOrder.level
        );
    }
}
