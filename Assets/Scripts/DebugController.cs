using UnityEngine;
using TMPro;

public class DebugController : MonoBehaviour
{
    public static DebugController instance;

    [Header("UI")]
    public GameObject debugPanel;
    public TextMeshProUGUI debugText;

    private bool debugMode = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Toggle debug mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
            debugMode = !debugMode;
            Debug.Log("DEBUG MODE: " + (debugMode ? "ON" : "OFF"));
            UpdateUI();
        }

        if (!debugMode) return;

        // === DEBUG KEYS ===

        if (Input.GetKeyDown(KeyCode.T))
        {
            TearScoreManager.instance?.Add(100);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TearSystem.instance?.AddCurrency(100);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameManager.instance?.TriggerGameOver();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance?.RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeRuntimeData.instance?.ResetAll();
            UpgradeManager.instance?.ResetAllEffects();

            Debug.Log("DEBUG: Full reset (upgrades + visuals)");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            XPSystem.instance?.AddXP(5000);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            XPSystem.instance?.ResetProgress();
        }
    }

    void UpdateUI()
    {
        if (debugPanel != null)
            debugPanel.SetActive(debugMode);

        if (debugText != null)
        {
            debugText.text =
                "DEBUG MODE (Q to toggle)\n\n" +
                "[T] +100 Tears\n" +
                "[Y] +100 Bottle\n" +
                "[G] Game Over\n" +
                "[R] Restart Game\n" +
                "[U] Reset Upgrades\n" +
                "[X] +5000 XP\n" +
                "[L] Reset XP";
        }
    }
}