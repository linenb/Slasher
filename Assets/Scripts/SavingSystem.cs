using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public int level;
    public int xp;
    public int tears;
    public int bottleTears;
    public List<int> upgradeLevels;

}
public class SavingSystem : MonoBehaviour
{
    public static SavingSystem Instance;
    public string savePath;
    public SaveData loadedData;
    public System.DateTime LastSaveTime { get; private set; }

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

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log("SAVE PATH: " + savePath);
    }

    private void Start()
    {
        LastSaveTime = System.DateTime.Now;
        InvokeRepeating(nameof(AutoSave), 60f, 60f);
    }
    private void AutoSave()
    {
        if (SceneManager.GetActiveScene().name != "MainGame")
            return;

        if (GameManager.instance != null && GameManager.instance.IsGameOver)
            return;

        SaveGame();
    }
    public void SaveGame()
    {
        Debug.Log("SAVE GAME CALLED");

        if (TearScoreManager.instance == null)
            Debug.LogError("TearScoreManager is NULL");

        if (XPSystem.instance == null)
            Debug.LogError("XPSystem is NULL");

        SaveData data = new SaveData();

        data.tears = TearScoreManager.instance.score;
        data.level = XPSystem.instance.currentLevel;
        data.xp = XPSystem.instance.currentXP;

        data.upgradeLevels = new List<int>();
        if (UpgradePanel.instance == null)
{
    Debug.LogError("UpgradePanel is NULL at save time");
    return;
}

if (UpgradePanel.instance.upgrades == null)
{
    Debug.LogError("UpgradePanel.upgrades is NULL");
    return;
}
        data.upgradeLevels = new List<int>();

        foreach (var upgrade in UpgradeRuntimeData.instance.AllUpgrades)
        {
            data.upgradeLevels.Add(UpgradeRuntimeData.instance.GetLevel(upgrade));
        }

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("SAVE WRITTEN TO: " + savePath);
        Debug.Log("FILE EXISTS NOW: " + File.Exists(savePath));
        Debug.Log("Saved!");
        LastSaveTime = System.DateTime.Now;
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file");
            SceneManager.LoadScene("MainGame");
            return;
        }

        string json = File.ReadAllText(savePath);
        loadedData = JsonUtility.FromJson<SaveData>(json);

        SceneManager.LoadScene("MainGame");
    }
    public void ApplyLoad()
    {
        if (loadedData == null)
            return;

        if (TearScoreManager.instance == null || XPSystem.instance == null)
        {
            Debug.LogError("Systems not ready yet");
            return;
        }

        TearScoreManager.instance.score = loadedData.tears;
        XPSystem.instance.currentLevel = loadedData.level;
        XPSystem.instance.currentXP = loadedData.xp;

        for (int i = 0; i < loadedData.upgradeLevels.Count; i++)
        {
            var upgrade = UpgradeRuntimeData.instance.AllUpgrades[i];
            int level = loadedData.upgradeLevels[i];

            UpgradeRuntimeData.instance.SetLevel(upgrade, level);
            UpgradeManager.instance.ApplyUpgradeAtLevel(upgrade, level);
        }

        loadedData = null;

        Debug.Log("Load applied safely");
    }
}
