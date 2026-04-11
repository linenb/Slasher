using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public TearSystem tearSystem;

    void Awake()
    {
        instance = this;
    }

    public void BuyUpgrade(UpgradeData data)
    {
        if (data == null) return;

        int level = UpgradeRuntimeData.instance.GetLevel(data);
        int cost = data.GetCost(level);

        if (!TearScoreManager.instance.HasEnough(cost))
        {
            Debug.Log("Not enough points");
            return;
        }

        if (level >= data.maxLevel)
        {
            Debug.Log("Max level reached");
            return;
        }

        TearScoreManager.instance.Spend(cost);

        UpgradeRuntimeData.instance.IncreaseLevel(data);

        ApplyUpgrade(data);

        Debug.Log($"Bought {data.upgradeName} Lv.{level + 1}");
    }

    void ApplyUpgrade(UpgradeData data)
    {
        // bottle size
        if (data.bottleCapacityIncrease > 0)
        {
            tearSystem.bottleCapacity += data.bottleCapacityIncrease;
        }

        // spawn speed
        if (data.spawnSpeedMultiplier > 0)
        {
            tearSystem.spawnInterval /= data.spawnSpeedMultiplier;
        }

        // click value
        if (data.clickValueIncrease > 0)
        {
            tearSystem.clickValue += data.clickValueIncrease;
        }

        // knife speed
        if (data.fallingSpeedMultiplier > 0)
        {
            FallingObjectManager.instance.speedMultiplier /= data.fallingSpeedMultiplier;
        }

        if (data.liftSpeedMultiplier > 0)
        {
            FallingObjectManager.instance.liftMultiplier *= data.liftSpeedMultiplier;
        }
    }
}