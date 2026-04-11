using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;

    public Button button;

    UpgradeData data;

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

        button.interactable =
            TearScoreManager.instance.HasEnough(cost) &&
            level < data.maxLevel;

        Refresh();
    }

    void Refresh()
    {
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