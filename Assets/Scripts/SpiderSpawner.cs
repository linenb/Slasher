using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public GameObject spiderPrefab;

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
        Vector3 pos = new Vector3(
            Random.Range(-6f, 6f),
            Random.Range(-3f, 3f),
            0
        );

        GameObject spider = Instantiate(spiderPrefab, pos, Quaternion.identity);

        // assign UI safely
        SpiderMinigame script = spider.GetComponent<SpiderMinigame>();
        script.SendMessage("SetUIReference", SendMessageOptions.DontRequireReceiver);
    }

    public void ForceSpawnSpider()
    {
        SpawnSpider();
    }
}