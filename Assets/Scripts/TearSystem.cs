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

    public void AddCurrency()
    {
        currency = Mathf.Min(currency + 1, bottleCapacity);

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
}