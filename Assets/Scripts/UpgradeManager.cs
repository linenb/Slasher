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

        ApplyUpgradeAtLevel(data);

        Debug.Log($"Bought {data.upgradeName} Lv.{level + 1}");
    }

    public void ApplyUpgradeAtLevel(UpgradeData data)
    {
        int level = UpgradeRuntimeData.instance.GetLevel(data);

        // Bottle size
        if (data.bottleCapacityIncrease > 0)
        {
            tearSystem.bottleCapacity += data.bottleCapacityIncrease * level;

            if (bottleTransform == null) return;

            float scaleIncrease = 0f;

            if (level <= 3)
            {
                scaleIncrease += 0.1f;
            }
            else if (level == 4)
            {
                scaleIncrease -= 0.3f;
                tearSystem.bottleLevels = tearSystem.finalBottleLevels;
                tearSystem.UpdateBottle();
            }

                scaleIncrease += 0.01f;

            bottleTransform.localScale += Vector3.one * scaleIncrease;
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
    public void ApplyUpgradeAtLevel(UpgradeData data, int level)
{
    // RESET per upgrade first (IMPORTANT if needed)

    // BOTTLE (absolute state)
    if (data.bottleCapacityIncrease > 0)
    {
        tearSystem.bottleCapacity =
            tearSystem.baseBottleCapacity + (data.bottleCapacityIncrease * level);

        float scale = 1f + (0.1f * level);
        bottleTransform.localScale = originalBottleScale * scale;

        if (level >= 4)
        {
            tearSystem.bottleLevels = tearSystem.finalBottleLevels;
            tearSystem.UpdateBottle();
        }
    }

    // GOLDEN TEAR (STATE)
    if (data.goldenTear)
    {
        tearSystem.goldenTearChance = 0.05f * level;

        if (level <= 0)
            characterSpriteRenderer.sprite = normalSprite;
        else if (level < data.maxLevel)
            characterSpriteRenderer.sprite = goldenSprite1;
        else
            characterSpriteRenderer.sprite = goldenSprite2;
    }

    // SPAWN SPEED (ABSOLUTE)
    if (data.spawnSpeedMultiplier > 0)
    {
        tearSystem.spawnInterval =
            tearSystem.baseSpawnInterval * Mathf.Pow(data.spawnSpeedMultiplier, level);
    }

    // CLICK VALUE (STATE BASED)
    if (data.clickValueIncrease > 0)
    {
        tearSystem.clickValue = (int)Mathf.Pow(2, level);
    }

    // KNIFE FALL SPEED (ABSOLUTE)
    if (data.fallingSpeedMultiplier > 0)
    {
        FallingObjectManager.instance.speedMultiplier =
            Mathf.Clamp(1f - (data.fallingSpeedMultiplier * level), 0.2f, 10f);
    }

    // LIFT SPEED (ABSOLUTE)
    if (data.liftSpeedMultiplier > 0)
    {
        FallingObjectManager.instance.liftMultiplier =
            1f + (data.liftSpeedMultiplier * level);
    }

    // TEARS PER SPAWN (STATE)
    if (data.tearsPerSpawnIncrease > 0)
    {
        tearSystem.tearsPerSpawn = level;
    }

    // PROXIMITY SPEED (STATE)
    if (data.tearSpeedMultiplierIncrease > 0)
    {
        tearSystem.tearSpeedMultiplier =
            1f + (data.tearSpeedMultiplierIncrease * level);
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