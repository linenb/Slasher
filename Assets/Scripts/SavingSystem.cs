using UnityEngine;
using System.IO;
[System.Serializable]
public class SaveData
{
    public int level;
    public int xp;
    public int tears;
    public int bottleTears;
    public int damageUpgrade;
    public int speedUpgrade;
}
public class SavingSystem : MonoBehaviour
{
    public static SavingSystem Instance;

    private string savePath;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Application.persistentDataPath + "/save.json";
    }

    private void Start()
    {
        // Start autosave every 60 seconds
        InvokeRepeating(nameof(SaveGame), 60f, 60f);
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // Example values from another script
        data.tears = TearScoreManager.instance.score;
        data.level = XPSystem.instance.currentLevel;
        data.xp = XPSystem.instance.currentXP;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Apply loaded data
            TearScoreManager.instance.score = data.tears;
            XPSystem.instance.currentLevel = data.level;
            XPSystem.instance.currentXP = data.xp;

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}
