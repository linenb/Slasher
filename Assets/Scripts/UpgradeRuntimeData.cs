using System.Collections.Generic;
using UnityEngine;

public class UpgradeRuntimeData : MonoBehaviour
{
    public static UpgradeRuntimeData instance;

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

    public void ResetAll()
    {
        levels.Clear();
    }
}