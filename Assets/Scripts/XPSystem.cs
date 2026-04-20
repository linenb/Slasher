using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSystem : MonoBehaviour
{
    public static XPSystem instance;

    public int currentLevel = 1;
    public int currentXP = 0;
    public int baseXPRequired = 10;
    public float levelScalingFactor = 1.5f;

    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public int GetXPRequired(int level)
    {
        return Mathf.RoundToInt(baseXPRequired * Mathf.Pow(level, levelScalingFactor));
    }

    public int GetXPToNextLevel()
    {
        return GetXPRequired(currentLevel) - currentXP;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= GetXPRequired(currentLevel))
        {
            currentXP -= GetXPRequired(currentLevel);
            currentLevel++;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int required = GetXPRequired(currentLevel);

        if (xpSlider != null)
        {
            xpSlider.minValue = 0;
            xpSlider.maxValue = required;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
            levelText.text = "Level " + currentLevel;

        if (xpText != null)
            xpText.text = currentXP + " / " + required;
    }

    public void ShowTooltip()
    {
        if (tooltipPanel == null || tooltipText == null) return;

        tooltipText.text = "To next level:\n" + GetXPToNextLevel() + " tears";
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void ResetProgress()
    {
        currentLevel = 1;
        currentXP = 0;

        UpdateUI();

        Debug.Log("XP RESET");
    }
}
