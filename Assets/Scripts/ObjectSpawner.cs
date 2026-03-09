using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public float spawnRate = 1.5f;
    public float spawnWidth = 8f;
    public float spawnHeight = 6f;

    void Start()
    {
        InvokeRepeating("SpawnObject", 1f, spawnRate);
    }

    void SpawnObject()
    {
        float randomX = Random.Range(-spawnWidth, spawnWidth);
        Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

        Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
    }
}