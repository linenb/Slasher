using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5f;
    private bool hitPlayer = false;
    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        // Destroy object when it goes off screen
        if (transform.position.y < -6f)
        {
            if (!hitPlayer)
            {
                PointSystem.instance.AddPoint();
            } 
            else 
            {
                hitPlayer = false;
            }
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Touched: " + other.gameObject.name + " | Tag: " + other.tag);

        // Use object name instead of tag as a quick test
        if (other.gameObject.name == "Square")
        {
            hitPlayer = true;
            PointSystem.instance.LosePoint();
            Destroy(gameObject);
        }
    }
}