using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Tears Score Display")]
    public TextMeshProUGUI totalTearsText;   // Text inside the card/panel below the GIF

    [Header("Level Display")]
    public TextMeshProUGUI totalXPText;   // Text inside the card/panel below the GIF

    [Header("Animators")]
    public SpriteAnimator gameOverAnimator;  // Animator for the game over GIF
    public SpriteAnimator restartAnimator;   // Animator for the restart button GIF

    void OnEnable()
    {
        // Start animations when panel becomes visible
        if (gameOverAnimator != null) gameOverAnimator.Play();
        if (restartAnimator != null)  restartAnimator.Play();
    }

    void OnDisable()
    {
        if (gameOverAnimator != null) gameOverAnimator.Stop();
        if (restartAnimator != null)  restartAnimator.Stop();
    }

    /// <summary>
    /// Called by GameManager with the final tear count.
    /// </summary>
    public void ShowFinalScore(int tears)
    {
        if (totalTearsText != null)
            totalTearsText.text = "Tears collected: " + tears;
    }

    /// <summary>
    /// Called by GameManager with the final tear count.
    /// </summary>
    public void ShowFinalXP(int xp)
    {
        if (totalXPText != null)
            totalXPText.text = "Final level: " + xp;
    }

    /// <summary>
    /// Hook this to the Restart button's OnClick event in the Inspector.
    /// </summary>
    public void OnRestartClicked()
    {
        if (GameManager.instance != null)
            GameManager.instance.RestartGame();
    }
}