using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    private Image buttonImage;
    private Color normalColor;

    public Button button;

    [Header("Proximity Upgrade Extras (optional)")]
    [Tooltip("Assign only for the proximity speed upgrade button")]
    public TextMeshProUGUI proximityIndicatorText; 
    public TextMeshProUGUI purchaseCountText;

    UpgradeData data;
    bool isProximityUpgrade => data != null && data.tearSpeedMultiplierIncrease > 0;

    public void Setup(UpgradeData upgrade)
    {
        data = upgrade;

        button.onClick.AddListener(OnClick);

        buttonImage = button.GetComponent<Image>();
        normalColor = buttonImage.color;

        Refresh();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data == null || UpgradeTooltip.instance == null) return;
        if (string.IsNullOrEmpty(data.description)) return;

        UpgradeTooltip.instance.Show(data.description, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UpgradeTooltip.instance != null)
            UpgradeTooltip.instance.Hide();
    }

    void Update()
    {
        if (data == null)
        {
            Debug.LogError("DATA NULL");
            return;
        }

        if (UpgradeRuntimeData.instance == null)
        {
            Debug.LogError("RuntimeData NULL");
            return;
        }

        if (TearScoreManager.instance == null)
        {
            Debug.LogError("ScoreManager NULL");
            return;
        }

        if (button == null)
        {
            Debug.LogError("BUTTON NULL");
            return;
        }

        int level = UpgradeRuntimeData.instance.GetLevel(data);
        int cost = data.GetCost(level);
        bool canAfford = TearScoreManager.instance.HasEnough(cost);
        bool maxed = level >= data.maxLevel;

        button.interactable = true;

        if (!canAfford || maxed)
        {
            buttonImage.color = new Color(
                normalColor.r * 0.5f,
                normalColor.g * 0.5f,
                normalColor.b * 0.5f,
                normalColor.a
            );
        }
        else
        {
            buttonImage.color = normalColor;
        }

        if (proximityIndicatorText != null && isProximityUpgrade)
        {
            bool knifeClose = KnifeDetector.instance != null &&
                              KnifeDetector.instance.KnifeIsClose;
 
            bool showIndicator = canAfford && knifeClose && !maxed;
            proximityIndicatorText.gameObject.SetActive(showIndicator);
            proximityIndicatorText.text = "⚡ ACTIVE";
        }

        if (purchaseCountText != null && isProximityUpgrade)
        {
            purchaseCountText.text = "Bought: " + level;
        }

        // Keep tooltip position in sync with mouse while hovering
        if (UpgradeTooltip.instance != null && UpgradeTooltip.instance.IsVisible)
            UpgradeTooltip.instance.UpdatePosition(Input.mousePosition);

        Refresh();
    }

    void Refresh()
    {
        if (data == null || UpgradeRuntimeData.instance == null) return;

        int level = UpgradeRuntimeData.instance.GetLevel(data);

        nameText.text = data.upgradeName;
        levelText.text = "Lv. " + level + "/" + data.maxLevel;

        if (level >= data.maxLevel)
        {
            costText.text = "MAX";
        }
        else
        {
            costText.text = data.GetCost(level).ToString();
        }
    }

    void OnClick()
    {
        if (data == null) return;

        int level = UpgradeRuntimeData.instance.GetLevel(data);
        int cost = data.GetCost(level);

        bool canAfford = TearScoreManager.instance.HasEnough(cost);
        bool maxed = level >= data.maxLevel;

        if (!canAfford || maxed)
        {
            AudioManager.instance.PlayError();
            return;
        }

        bool success = UpgradeManager.instance.BuyUpgrade(data);

        if (success)
        {
            AudioManager.instance.PlayPurchase();
        }
    }
}
