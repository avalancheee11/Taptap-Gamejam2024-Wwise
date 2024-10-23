using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public Button startGameButton;
    public string saveFilePath = "PlayerSaveData.dat";

    private PlayerData playerData;
    private float autoSaveInterval = 30f;
    private float elapsedTime = 0f;

    void Start()
    {
        // 初始化按钮点击事件  
        startGameButton.onClick.AddListener(OnLoadGame);

        // 尝试读档  
        playerData = LoadGameData();

        if (playerData == null)
        {
            // 如果没有存档，初始化一个新的玩家数据  
            playerData = new PlayerData();
            playerData.level = 1; // 初始关卡  
            playerData.position = new Vector3(0, 0, 0); // 初始位置  
            playerData.inventory = new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" }; // 初始背包  
            playerData.sceneActions = new List<string> { "Action1", "Action2" }; // 初始操作信息  
        }
        else
        {
            Debug.Log("Loaded game data: " + playerData.ToString());
        }
    }

    void Update()
    {
        // 自动存档  
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= autoSaveInterval)
        {
            SaveGameData();
            elapsedTime = 0f;
        }
    }

    void OnApplicationQuit()
    {
        // 退出游戏时存档  
        SaveGameData();
    }

    public void OnLoadGame()
    {
        // 开始游戏按钮点击时读档  
        if (playerData != null)
        {
            // 添加将玩家数据应用到游戏的代码  
            Debug.Log("Game loaded with data: " + playerData.ToString());
            SceneManager.LoadScene(playerData.level);
        }
    }

    private PlayerData LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read);
            PlayerData loadedData = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return loadedData;
        }
        else
        {
            return null;
        }
    }

    private void SaveGameData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, playerData);
        stream.Close();
        Debug.Log("Game data saved.");
    }

    [System.Serializable]
    public class PlayerData
    {
        public int level;
        public Vector3 position;
        public List<string> inventory;
        public List<string> sceneActions;

        public override string ToString()
        {
            return $"Level: {level}, Position: {position},: Inventory {string.Join(", ", inventory)}, Scene Actions: {string.Join(", ", sceneActions)}";
        }
    }
}