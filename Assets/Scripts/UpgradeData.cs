using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Idle/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;

    public int baseCost;
    public float costMultiplier = 1.5f;

    [Header("Linear Cost (optional)")]
    [Tooltip("If > 0, cost = baseCost + (level * linearCostIncrease), ignoring costMultiplier")]
    public int linearCostIncrease = 0;

    public int maxLevel = 10;
    public bool goldenTear;
    //  EFFECTS

    [Header("Bottle")]
    public int bottleCapacityIncrease;

    [Header("Spawn")]
    public float spawnSpeedMultiplier; // mažina spawnInterval

    [Header("Click")]
    public int clickValueIncrease; // kiek prideda paspaudus bottle

    [Header("Knife fall")]
    public float fallingSpeedMultiplier;
    public float liftSpeedMultiplier;

    [Header("Tears Per Spawn")]
    public int tearsPerSpawnIncrease;

    [Header("Proximity Speed")]
    [Tooltip("Adds this to tearSpeedMultiplier when knife is close. +0.1 recommended.")]
    public float tearSpeedMultiplierIncrease;
    
    public int GetCost(int level)
    {
        if (linearCostIncrease > 0)
            return baseCost + (level * linearCostIncrease);

        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }
}