using UnityEngine;
using TMPro;
using System.Collections;

public class SpiderClickMinigame : MonoBehaviour
{
    public static SpiderClickMinigame instance;

    public float duration = 10f;
    private float timer;

    public int clickCount = 0;
    private bool isActive = false;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI clickText;

    void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        clickCount = 0;
        timer = duration;
        isActive = true;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            EndGame();
        }

        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();

        if (clickText != null)
            clickText.text = "Clicks: " + clickCount;
    }

    public void RegisterClick()
    {
        if (!isActive) return;

        clickCount++;
    }

    void EndGame()
    {
        isActive = false;

        RewardPlayer();
        TriggerSpiderHelp();

        gameObject.SetActive(false);
    }

    void RewardPlayer()
    {
        int level = XPSystem.instance.currentLevel;

        float levelFactor = Mathf.Sqrt(level);
        float randomFactor = Random.Range(0.8f, 1.2f);

        int totalReward = Mathf.RoundToInt(
            clickCount *
            TearSystem.instance.clickValue *
            levelFactor *
            randomFactor
        );

        TearScoreManager.instance.Add(totalReward);

        TearSystem.instance.SpawnPopup(totalReward, TearSystem.instance.eyePoint.position);
    }

    void TriggerSpiderHelp()
    {
        if (FallingObjectManager.instance == null) return;

        int level = XPSystem.instance.currentLevel;

        // Scale strength with performance
        float performanceFactor = Mathf.Clamp(clickCount / 50f, 0.5f, 2f);

        // Level scaling
        float levelFactor = Mathf.Sqrt(level);

        float liftBoost = 0.5f * performanceFactor * levelFactor;

        StartCoroutine(ApplyLiftOverTime(liftBoost));
    }



IEnumerator ApplyLiftOverTime(float boost)
{
    float duration = 2f; // how long spider helps
    float timer = 0f;

    // Apply boost
    FallingObjectManager.instance.liftMultiplier += boost;

    while (timer < duration)
    {
        timer += Time.deltaTime;
        yield return null;
    }

    // Remove boost after
    FallingObjectManager.instance.liftMultiplier -= boost;
}
}