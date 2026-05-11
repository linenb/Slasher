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

    [Header("Lighter Knife Balloons")]
    [Tooltip("Reference to the UpgradeData asset that represents the 'lighter knife' upgrade")]
    public UpgradeData lighterKnifeUpgradeData;

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

    public bool BuyUpgrade(UpgradeData data)
    {
        if (data == null) return false;

        int level = UpgradeRuntimeData.instance.GetLevel(data);
        int cost = data.GetCost(level);

        if (!TearScoreManager.instance.HasEnough(cost))
        {
            Debug.Log("Not enough points");
            return false;
        }

        if (level >= data.maxLevel)
        {
            Debug.Log("Max level reached");
            return false;
        }

        TearScoreManager.instance.Spend(cost);

        UpgradeRuntimeData.instance.IncreaseLevel(data);

        ApplyUpgradeAtLevel(data);

        Debug.Log($"Bought {data.upgradeName} Lv.{level + 1}");

        return true;
    }

    public void ApplyUpgradeAtLevel(UpgradeData data)
    {
        int level = UpgradeRuntimeData.instance.GetLevel(data);

        // Bottle size
        if (data.bottleCapacityIncrease > 0)
        {
            tearSystem.bottleCapacity =
                tearSystem.baseBottleCapacity +
                Mathf.RoundToInt(
                    data.bottleCapacityIncrease * level * level * 0.5f
                );

            if (bottleTransform == null) return;

            if (level <= 3)
            {
                float scale = 1f + (0.15f * level);

                bottleTransform.localScale =
                    originalBottleScale * scale;
            }
            else if (level == 4)
            {
                tearSystem.bottleLevels = tearSystem.finalBottleLevels;
                tearSystem.UpdateBottle();

                bottleTransform.localScale =
                    originalBottleScale * 1.5f;

                bottleTransform.localPosition = new Vector3(
                    bottleTransform.localPosition.x,
                    bottleTransform.localPosition.y - 0.2f,
                    bottleTransform.localPosition.z
                );
            }
        }

        // Golden tear chance
        if (data.goldenTear)
        {
            tearSystem.goldenTearChance = 0.05f * level;

            if (level <= 0)
            {
                characterSpriteRenderer.sprite = normalSprite;
                FoxAnimation.instance?.SetNormalFox();
            }
            else if (level < data.maxLevel)
            {
                characterSpriteRenderer.sprite = goldenSprite1;
                FoxAnimation.instance?.SetGoldenFox1();
            }
            else
            {
                characterSpriteRenderer.sprite = goldenSprite2;
                FoxAnimation.instance?.SetGoldenFox2();
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
            FallingObjectManager.instance.upgradeSpeedMultiplier *=
            (1f - data.fallingSpeedMultiplier);

            FallingObjectManager.instance.upgradeSpeedMultiplier =
                Mathf.Max(0.2f, FallingObjectManager.instance.upgradeSpeedMultiplier);

            Debug.Log("New fall speed multiplier: " + FallingObjectManager.instance.speedMultiplier);

            // Spawn one balloon per lighter-knife level
            if (lighterKnifeUpgradeData != null && data == lighterKnifeUpgradeData)
            {
                BalloonAttachment.SetBalloonCount(level);
                Debug.Log($"Lighter knife level {level} — showing {level} balloon(s) on knife.");
            }
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
            tearSystem.baseBottleCapacity +
            Mathf.RoundToInt(data.bottleCapacityIncrease * level * level * 0.5f);

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
        FallingObjectManager.instance.upgradeSpeedMultiplier =
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