using UnityEngine;

public class FallingObjectManager : MonoBehaviour
{
    public static FallingObjectManager instance;

    public float speedMultiplier = 1f; // fall
    public float liftMultiplier = 1f;  // lift

    public float baseSpeedMultiplier = 1f;
    public float baseLiftMultiplier = 1f;

    public void ResetToBase()
    {
        speedMultiplier = baseSpeedMultiplier;
        liftMultiplier = baseLiftMultiplier;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // reset kad nesikauptų
        speedMultiplier = 1f;
        liftMultiplier = 1f;
    }
}