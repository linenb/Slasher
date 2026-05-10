using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject batPrefab;
    public float spawnInterval = 8f;
    public float spawnChance = 0.6f;

    public float spawnYMin = -200f;
    public float spawnYMax = 200f;

    public RectTransform canvasRect;

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

        float canvasHalfWidth = canvasRect.rect.width / 2f;
        float startX = fromLeft ? -canvasHalfWidth - 100f : canvasHalfWidth + 100f;

         Canvas liveCanvas = canvasRect.GetComponentInParent<Canvas>();
        if (liveCanvas == null) liveCanvas = canvasRect.GetComponent<Canvas>();

        GameObject bat = Instantiate(batPrefab, liveCanvas.transform); // spawns INSIDE canvas
        RectTransform rt = bat.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(startX, y);

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