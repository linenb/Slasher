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
        Debug.Log("CONTINUE PRESSED");

        if (!File.Exists(savePath))
        {
            Debug.Log("NO SAVE FOUND → NEW GAME");
            SceneManager.LoadScene("MainGame");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        StartCoroutine(LoadSceneAndApply(data));
    }
    IEnumerator LoadSceneAndApply(SaveData data)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MainGame");

        while (!op.isDone)
            yield return null;

        yield return null; // extra safety frame

        if (TearScoreManager.instance == null || XPSystem.instance == null)
        {
            Debug.LogError("Systems not ready after scene load!");
            yield break;
        }

        TearScoreManager.instance.score = data.tears;
        XPSystem.instance.currentLevel = data.level;
        XPSystem.instance.currentXP = data.xp;

        Debug.Log("Loaded into game");
    }
}
