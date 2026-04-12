using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    public GameObject gameOverPanel;           // The full Game Over UI panel
    public TearScoreManager tearScoreManager;  // To read final tear count
    public TearSystem tearSystem;

    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Make sure game over panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return; // Prevent double-triggering
        isGameOver = true;

        Debug.Log("Game Over triggered!");

        // Pause the game
        Time.timeScale = 0f;

        // Show game over screen with final score
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            GameOverUI ui = gameOverPanel.GetComponent<GameOverUI>();
            if (ui != null && tearScoreManager != null)
                ui.ShowFinalScore(tearScoreManager.score + tearSystem.currency);
        }
    }
    
    public void RestartGame()
    {
        // Resume time before reloading
        Time.timeScale = 1f;

        UpgradeRuntimeData.instance.ResetAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}