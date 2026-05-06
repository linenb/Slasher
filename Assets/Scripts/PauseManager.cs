using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [Header("References")]
    public GameObject pauseMenuPanel;
    public Button pauseButton;
    public CanvasGroup gameUICanvasGroup;

    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Reset all state on every fresh load
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);

        if (gameUICanvasGroup != null)
        {
            gameUICanvasGroup.interactable = true;
            gameUICanvasGroup.blocksRaycasts = true;
        }
    }

    private void Update()
    {
        if (!IsGameActive()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    private bool IsGameActive()
    {
        if (SceneManager.GetActiveScene().name != "MainGame") return false;
        if (GameManager.instance != null && GameManager.instance.IsGameOver) return false;
        return true;
    }

    public void Pause()
    {
        if (IsPaused || !IsGameActive()) return;
        IsPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);

            PauseMenuUI ui = pauseMenuPanel.GetComponent<PauseMenuUI>();
            if (ui != null) ui.OnPauseShown();
        }

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        if (gameUICanvasGroup != null)
        {
            gameUICanvasGroup.interactable = false;
            gameUICanvasGroup.blocksRaycasts = false;
        }
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);

        if (gameUICanvasGroup != null)
        {
            gameUICanvasGroup.interactable = true;
            gameUICanvasGroup.blocksRaycasts = true;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}