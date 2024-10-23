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
        // ��ʼ����ť����¼�  
        startGameButton.onClick.AddListener(OnLoadGame);

        // ���Զ���  
        playerData = LoadGameData();

        if (playerData == null)
        {
            // ���û�д浵����ʼ��һ���µ��������  
            playerData = new PlayerData();
            playerData.level = 1; // ��ʼ�ؿ�  
            playerData.position = new Vector3(0, 0, 0); // ��ʼλ��  
            playerData.inventory = new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" }; // ��ʼ����  
            playerData.sceneActions = new List<string> { "Action1", "Action2" }; // ��ʼ������Ϣ  
        }
        else
        {
            Debug.Log("Loaded game data: " + playerData.ToString());
        }
    }

    void Update()
    {
        // �Զ��浵  
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= autoSaveInterval)
        {
            SaveGameData();
            elapsedTime = 0f;
        }
    }

    void OnApplicationQuit()
    {
        // �˳���Ϸʱ�浵  
        SaveGameData();
    }

    public void OnLoadGame()
    {
        // ��ʼ��Ϸ��ť���ʱ����  
        if (playerData != null)
        {
            // ��ӽ��������Ӧ�õ���Ϸ�Ĵ���  
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