using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;

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
        Refresh();
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

        button.interactable =
            TearScoreManager.instance.HasEnough(cost) &&
            level < data.maxLevel;
 
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
        UpgradeManager.instance.BuyUpgrade(data);
    }
}