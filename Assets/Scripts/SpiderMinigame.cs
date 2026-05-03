using UnityEngine;
using System.Linq;

public class SpiderMinigame : MonoBehaviour
{
    private GameObject minigameUI;

    void Awake()
    {
        // This finds inactive objects too
        minigameUI = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "SpiderMinigameUI");
    }

    void OnMouseDown()
    {
        if (PauseManager.IsPaused) return;
        StartMinigame();
        Destroy(gameObject);
    }

    void StartMinigame()
    {
        if (minigameUI == null)
        {
            Debug.LogError("SpiderMinigameUI NOT FOUND!");
            return;
        }

        minigameUI.SetActive(true);

        if (SpiderClickMinigame.instance != null)
        {
            SpiderClickMinigame.instance.StartGame();
        }
        else
        {
            Debug.LogError("SpiderClickMinigame instance NOT FOUND!");
        }
    }

    void SetUIReference()
    {
        if (minigameUI == null)
        {
            minigameUI = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(obj => obj.name == "SpiderMinigameUI");
        }
    }
}