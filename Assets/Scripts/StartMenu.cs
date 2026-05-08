using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour
{
    public Button continueButton;

    private string savePath;

    private void Start()
    {
        savePath = System.IO.Path.Combine(Application.persistentDataPath, "save.json");

        UpdateContinueButton();
    }

    void UpdateContinueButton()
    {
        if (continueButton == null)
            return;

        continueButton.interactable = File.Exists(savePath);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT GAME");
        Application.Quit();
    }

    public void OnContinuePressed()
    {
        SavingSystem.Instance.LoadGame();
    }
}