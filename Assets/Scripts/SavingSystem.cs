using System.Collections;
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
    public int damageUpgrade;
    public int speedUpgrade;
    
}
public class SavingSystem : MonoBehaviour
{
    public static SavingSystem Instance;
    public TMP_Text saveText;
    public string savePath;
    private SaveData loadedData;

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
        // Start autosave every 60 seconds
        InvokeRepeating(nameof(SaveGame), 60f, 60f);
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

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("SAVE WRITTEN TO: " + savePath);
        Debug.Log("FILE EXISTS NOW: " + File.Exists(savePath));
        Debug.Log("Saved!");
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

        loadedData = null;

        Debug.Log("Load applied safely");
    }
}
