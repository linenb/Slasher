using UnityEngine;

public class BatMinigame : MonoBehaviour
{
    public float speed = 3f;
    public float bobHeight = 0.5f;
    public float bobSpeed = 3f;

    private Vector3 startPos;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Move horizontally
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Bob up/down (sine wave)
        float yOffset = Mathf.Sin((Time.time + timeOffset) * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, startPos.y + yOffset, transform.position.z);

        // Destroy if off screen
        if (Mathf.Abs(transform.position.x) > 10f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        TriggerBatEffect();
        Destroy(gameObject);
    }

    void TriggerBatEffect()
    {
        // call tear explosion on fox
        if (TearSystem.instance != null)
        {
            TearSystem.instance.BatScareExplosion();
        }
    }
}