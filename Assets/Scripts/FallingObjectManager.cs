using UnityEngine;

public class FallingObjectManager : MonoBehaviour
{
    public static FallingObjectManager instance;

    // Lift
    public float liftMultiplier = 1f;

    // Fall speed components
    public float levelSpeedMultiplier = 1f;
    public float upgradeSpeedMultiplier = 1f;

    // Final combined speed
    public float speedMultiplier
    {
        get
        {
            return levelSpeedMultiplier * upgradeSpeedMultiplier;
        }
    }

    public float baseLiftMultiplier = 1f;

    public void ResetToBase()
    {
        levelSpeedMultiplier = 1f;
        upgradeSpeedMultiplier = 1f;

        liftMultiplier = baseLiftMultiplier;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // reset kad nesikauptų
        levelSpeedMultiplier = 1f;
        upgradeSpeedMultiplier = 1f;

        liftMultiplier = 1f;
    }
}