using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button pauseButton;
    public CanvasGroup gameUICanvasGroup;

    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    private void Start()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(Pause);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

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