using UnityEngine;
using TMPro;

public class TearSystem : MonoBehaviour
{
    public GameObject tearPrefab;
    public Transform eyePoint;
    public Transform bottlePoint;
    public SpriteRenderer bottleRenderer;
    public Sprite[] bottleLevels;
    public TextMeshProUGUI bottleText;
    public GameObject popupPrefab;
    public Canvas canvas;
    public TearScoreManager scoreManager;
    public GameObject clickTearPrefab;


    public float goldenTearChance = 0f;

    public float spawnInterval = 3f;

    public int currency = 0;
    public int bottleCapacity = 10;
    public int clickValue = 1;
    public int xpPerTear = 1;
    public int tearsPerSpawn = 1;

    [Header("Proximity Speed Upgrade")]
    public float tearSpeedMultiplier = 1f;

    [Header("Base Values (DO NOT CHANGE AT RUNTIME)")]
    public float baseSpawnInterval = 3f;
    public int baseBottleCapacity = 10;
    public int baseClickValue = 1;
    public float baseGoldenChance = 0f;
    public int baseTearsPerSpawn = 1;
    public float baseTearSpeedMultiplier = 1f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {

            TrySpawnTear();
            timer = 0f;
        }
    }

    void TrySpawnTear()
    {
        if (currency >= bottleCapacity) return;
         
        bool knifeClose = KnifeDetector.instance != null && KnifeDetector.instance.KnifeIsClose;
        float currentSpeed = knifeClose ? tearSpeedMultiplier : 1f;
        //Debug.Log($"Knife close: {knifeClose} | Speed multiplier: {currentSpeed}"); 

        for(int i = 0; i < tearsPerSpawn; i++ )
        {        
        GameObject tear = Instantiate(tearPrefab, eyePoint.position, Quaternion.identity);

        Tear tearScript = tear.GetComponent<Tear>();

        bool isGolden = Random.value < goldenTearChance;

        tearScript.isGolden = isGolden;
        tearScript.target = bottlePoint;
        tearScript.system = this;
        tearScript.speed *= currentSpeed;
    
        }
    }

    public void AddCurrency(int amount)
    {
        currency = Mathf.Min(currency + amount, bottleCapacity);

        UpdateBottle();
        UpdateText();
        if (XPSystem.instance != null)
            XPSystem.instance.AddXP(amount * xpPerTear);
    }

    void UpdateBottle()
    {
        float percent = (float)currency / bottleCapacity;

        int index = Mathf.FloorToInt(percent * bottleLevels.Length);

        index = Mathf.Clamp(index, 0, bottleLevels.Length - 1);

        bottleRenderer.sprite = bottleLevels[index];
    }

    void UpdateText()
    {
        bottleText.text = currency + " / " + bottleCapacity;
    }

    public void SpawnPopup(int value, Vector3 worldPosition)
    {
        GameObject popup = Instantiate(popupPrefab, canvas.transform);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        popup.GetComponent<RectTransform>().localPosition = localPoint;

        popup.GetComponent<PopupText>().Setup(value);
    }

    public void CollectBottle()
    {
        if (currency <= 0) return;

        TearScoreManager.instance.Add(currency * clickValue);

        currency = 0;

        UpdateBottle();
        UpdateText();
    }

    public void SpawnClickTear()
    {
        GameObject tear = Instantiate(clickTearPrefab, eyePoint.position, Quaternion.identity);

        ClickTear clickTear = tear.GetComponent<ClickTear>();

        bool isGolden = Random.value < goldenTearChance;

        clickTear.Init(isGolden);
    }

    public void ResetToBase()
    {
        spawnInterval = baseSpawnInterval;
        bottleCapacity = baseBottleCapacity;
        clickValue = baseClickValue;
        goldenTearChance = baseGoldenChance;
        tearsPerSpawn = baseTearsPerSpawn;
        tearSpeedMultiplier = baseTearSpeedMultiplier;

        currency = 0;

        UpdateBottle();
        UpdateText();
    }

    // =========================
    // BAT MINIGAME SCALING
    // =========================
    public void BatScareExplosion()
    {
        if (XPSystem.instance == null) return;

        int level = XPSystem.instance.currentLevel;

        // LEVEL SCALING (smooth)
        float levelFactor = Mathf.Sqrt(level);

        // TIER BONUS (every 33 levels)
        int tier = (level - 1) / 33;
        float tierBonus = 1f + (tier * 0.5f);

        // RANDOMNESS (juice)
        float randomFactor = Random.Range(0.8f, 1.3f);

        // FINAL TOTAL VALUE
        int totalValue = Mathf.RoundToInt(
            clickValue * 15 * levelFactor * tierBonus * randomFactor
        );

        int burstCount = 20;

        for (int i = 0; i < burstCount; i++)
        {
            bool isGolden = Random.value < goldenTearChance;

            int value = isGolden ? totalValue : totalValue / burstCount;

            TearScoreManager.instance.Add(value);

            GameObject tear = Instantiate(clickTearPrefab, eyePoint.position, Quaternion.identity);
            tear.GetComponent<ClickTear>().Init(isGolden);

            SpawnPopup(value, eyePoint.position);
        }
    }

    public static TearSystem instance;

    void Awake()
    {
        instance = this;
    }
}