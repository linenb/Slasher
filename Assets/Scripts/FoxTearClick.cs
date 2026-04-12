using UnityEngine;

public class ClickTear : MonoBehaviour
{
    private Rigidbody2D rb;

    public float lifeTime = 2f;

    public void Init(bool isGolden)
    {
        rb = GetComponent<Rigidbody2D>();

        // Optional golden visual
        if (isGolden)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        // Random direction (upward bias)
        Vector2 dir = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(0.6f, 1.5f)
        ).normalized;

        float force = Random.Range(4f, 7f);

        rb.AddForce(dir * force, ForceMode2D.Impulse);

        // Spin for juice
        rb.AddTorque(Random.Range(-200f, 200f));

        Destroy(gameObject, lifeTime);
    }
}