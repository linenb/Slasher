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

    public float spawnInterval = 3f;

    public int currency = 0;
    public int bottleCapacity = 10;

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

        GameObject tear = Instantiate(tearPrefab, eyePoint.position, Quaternion.identity);

        Tear tearScript = tear.GetComponent<Tear>();
        tearScript.target = bottlePoint;
        tearScript.system = this;
    }

    public void AddCurrency(int amount)
    {
        currency = Mathf.Min(currency + amount, bottleCapacity);

        UpdateBottle();
        UpdateText();
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

        Debug.Log("Bottle clicked!");
        scoreManager.AddScore(currency);

        currency = 0;

        UpdateBottle();
        UpdateText();
    }
}