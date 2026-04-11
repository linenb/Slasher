using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public UpgradeData[] upgrades;
    public GameObject upgradePrefab;
    public Transform container;

    void Start()
    {
        foreach (var upgrade in upgrades)
        {
            GameObject obj = Instantiate(upgradePrefab, container);
            obj.GetComponent<UpgradeButtonUI>().Setup(upgrade);
        }
    }
}