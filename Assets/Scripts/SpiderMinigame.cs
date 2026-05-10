using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class SpiderMinigame : MonoBehaviour, IPointerDownHandler
{
    private GameObject minigameUI;

    void Awake()
    {
        // This finds inactive objects too
        minigameUI = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "SpiderMinigameUI");
    }

    //  OnMouseDown for UI elements
    public void OnPointerDown(PointerEventData eventData)
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
            SpiderClickMinigame.instance.StartGame();
        else
            Debug.LogError("SpiderClickMinigame instance NOT FOUND!");
    }
}