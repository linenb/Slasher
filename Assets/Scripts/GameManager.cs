using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    public GameObject gameOverPanel;
    public TearScoreManager tearScoreManager;
    public TearSystem tearSystem;

    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        Debug.Log("Game Over triggered!");

        Time.timeScale = 0f;

        // Force resume pause menu if it was open
        if (PauseManager.instance != null && PauseManager.IsPaused)
            PauseManager.instance.Resume();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            GameOverUI ui = gameOverPanel.GetComponent<GameOverUI>();
            if (ui != null && tearScoreManager != null)
            {
                ui.ShowFinalScore(tearScoreManager.score + tearSystem.currency);
                ui.ShowFinalXP(XPSystem.instance.currentLevel);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UpgradeRuntimeData.instance.ResetAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
