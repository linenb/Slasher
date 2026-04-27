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

    [Header("Character Sprites")]
    public SpriteRenderer characterRenderer;
    public Sprite[] characterSprites; // Assign in Inspector: sprite for tier 0, 1, 2...

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
        int tierBefore = (currentLevel - 1) / 33;

        currentXP += amount;

        while (currentXP >= GetXPRequired(currentLevel))
        {
            currentXP -= GetXPRequired(currentLevel);
            currentLevel++;
        }

        int tierAfter = (currentLevel - 1) / 33;

        if (tierAfter != tierBefore)
        {
            UpdateCharacterSprite(tierAfter);
        }

        UpdateUI();
    }

    void UpdateCharacterSprite(int tier)
    {
        if (characterRenderer == null || characterSprites == null) return;

        // Clamp so we don't go out of bounds if player exceeds all defined tiers
        int index = Mathf.Min(tier, characterSprites.Length - 1);
        characterRenderer.sprite = characterSprites[index];
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
