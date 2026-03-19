using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float pixelsPerSecond = 5f; // VERY slow (try 1–10)
    private Camera cam;
    //private bool hitPlayer = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Convert pixels to world units
        float unitsPerPixel = (cam.orthographicSize * 2f) / Screen.height;
        float moveAmount = pixelsPerSecond * unitsPerPixel;
        transform.position += Vector3.down * moveAmount * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Character")
        {
            //hitPlayer = true;
            PointSystem.instance.LosePoint();
            Destroy(gameObject);
        }
    }
}