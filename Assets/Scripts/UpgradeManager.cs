using UnityEngine;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public TearSystem tearSystem;

    private Vector3 originalBottleScale;
    private Transform bottleTransform;
    public SpriteRenderer characterSpriteRenderer;
    public Sprite normalSprite;
    public Sprite goldenSprite1;
    public Sprite goldenSprite2;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject bottle = GameObject.Find("Bottle");

        if (bottle != null)
        {
            bottleTransform = bottle.transform;
            originalBottleScale = bottleTransform.localScale;
        }
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
        int level = UpgradeRuntimeData.instance.GetLevel(data);

        // Bottle size
        if (data.bottleCapacityIncrease > 0)
        {
            tearSystem.bottleCapacity +=
                tearSystem.bottleCapacity * 2 + data.bottleCapacityIncrease * level;

            GameObject bottle = GameObject.Find("Bottle");
            if (bottleTransform != null)
            {
                bottleTransform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        // Golden tear chance
        if (data.goldenTear)
        {
            tearSystem.goldenTearChance += 0.05f;
            characterSpriteRenderer.sprite = goldenSprite1;
            if (level == data.maxLevel)
            {
                characterSpriteRenderer.sprite = goldenSprite2;
            }
        }

        // Spawn speed (faster spawning)
        if (data.spawnSpeedMultiplier > 0)
        {
            tearSystem.spawnInterval /= data.spawnSpeedMultiplier;
        }

        // Click value
        if (data.clickValueIncrease > 0)
        {
            tearSystem.clickValue = (int)Mathf.Pow(2, level);
        }

        // Knife fall speed (SLOW DOWN FIXED)
        if (data.fallingSpeedMultiplier > 0)
        {
            FallingObjectManager.instance.speedMultiplier = Mathf.Max(
                0.2f,
                FallingObjectManager.instance.speedMultiplier * (1f - data.fallingSpeedMultiplier)
            );

            Debug.Log("New fall speed multiplier: " + FallingObjectManager.instance.speedMultiplier);
        }

        // Lift speed
        if (data.liftSpeedMultiplier > 0)
        {
            FallingObjectManager.instance.liftMultiplier *= data.liftSpeedMultiplier;
        }
        // Tears per spawn
        if (data.tearsPerSpawnIncrease > 0)
        {
            tearSystem.tearsPerSpawn += data.tearsPerSpawnIncrease;
        }
        // Proximity tear speed boost
        if (data.tearSpeedMultiplierIncrease > 0)
        {
            tearSystem.tearSpeedMultiplier += data.tearSpeedMultiplierIncrease;
        }
    }


    public void ResetAllEffects()
    {
        if (tearSystem != null)
            tearSystem.ResetToBase();

        if (FallingObjectManager.instance != null)
            FallingObjectManager.instance.ResetToBase();

        if (bottleTransform != null)
            bottleTransform.localScale = originalBottleScale;
    }

}