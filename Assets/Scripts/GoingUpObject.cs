using UnityEngine;
public class RisingObject : MonoBehaviour
{
    public float riseSpeed = 5f;
    void Update()
    {
        transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
        // Destroy object when it goes off screen
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}