using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject batPrefab;
    public float spawnInterval = 8f;
    public float spawnChance = 0.6f;

    public float spawnYMin = -2f;
    public float spawnYMax = 2f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (Random.value < spawnChance)
            {
                SpawnBat();
            }
        }
    }

    void SpawnBat()
    {
        float y = Random.Range(spawnYMin, spawnYMax);

        bool fromLeft = Random.value > 0.5f;

        Vector3 pos = new Vector3(fromLeft ? -10f : 10f, y, 0);
        GameObject bat = Instantiate(batPrefab, pos, Quaternion.identity);

        BatMinigame script = bat.GetComponent<BatMinigame>();

        // flip direction
        if (!fromLeft)
        {
            script.speed *= -1;
            bat.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void ForceSpawnBat()
    {
        SpawnBat();
    }
}