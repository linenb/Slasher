using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public GameObject spiderPrefab;
    public RectTransform canvasRect;

    public float minSpawnTime = 10f;
    public float maxSpawnTime = 20f;

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnSpider();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnSpider()
    {
        Canvas liveCanvas = canvasRect.GetComponent<Canvas>();
        if (liveCanvas == null) return;

        float halfWidth = canvasRect.rect.width / 2f;
        float x = Random.Range(-halfWidth + 100f, halfWidth - 100f);
        float y = Random.Range(-200f, 200f);

        GameObject spider = Instantiate(spiderPrefab, liveCanvas.transform);
        RectTransform rt = spider.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x, y);
    }

    public void ForceSpawnSpider()
    {
        SpawnSpider();
    }
}