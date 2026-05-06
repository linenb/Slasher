using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    void OnEnable()
    {
        OnPauseShown();
    }

    void OnDisable()
    {
        // Hook here if you later add animators, like GameOverUI does
    }

    /// <summary>
    /// Called by PauseManager when the panel is shown.
    /// Add any setup here (e.g. display current stats, play animations).
    /// </summary>
    public void OnPauseShown()
    {
        // Ready for future additions, e.g.:
        // if (someAnimator != null) someAnimator.Play();
    }

    /// <summary>
    /// Hook this to the Resume button's OnClick() in the Inspector.
    /// </summary>
    public void OnResumeClicked()
    {
        if (PauseManager.instance != null)
            PauseManager.instance.Resume();
    }

    /// <summary>
    /// Hook this to the Main Menu button's OnClick() in the Inspector.
    /// </summary>
    public void OnMainMenuClicked()
    {
        if (GameManager.instance != null)
            GameManager.instance.RestartGame();
    }
}
