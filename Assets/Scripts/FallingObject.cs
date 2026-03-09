using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Destroy object when it goes off screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}