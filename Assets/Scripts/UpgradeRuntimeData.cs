using System.Collections.Generic;
using UnityEngine;

public class UpgradeRuntimeData : MonoBehaviour
{
    public static UpgradeRuntimeData instance;
    public UpgradeData[] AllUpgrades;

    Dictionary<UpgradeData, int> levels = new Dictionary<UpgradeData, int>();

    void Awake()
    {
        instance = this;
    }

    public int GetLevel(UpgradeData data)
    {
        if (!levels.ContainsKey(data))
            levels[data] = 0;

        return levels[data];
    }

    public void IncreaseLevel(UpgradeData data)
    {
        if (!levels.ContainsKey(data))
            levels[data] = 0;

        levels[data]++;
    }
    public void SetLevel(UpgradeData data, int level)
    {
        if (levels.ContainsKey(data))
            levels[data] = level;
        else
            levels.Add(data, level);
    }
    public void ResetAll()
    {
        levels.Clear();
    }
}