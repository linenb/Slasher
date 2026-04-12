using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Idle/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public GameObject Bottle;

    public int baseCost;
    public float costMultiplier = 1.5f;
    public int maxLevel = 10;

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

    public int GetCost(int level)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }
}