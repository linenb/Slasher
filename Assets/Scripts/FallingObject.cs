using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float pixelsPerSecond = 5f; // VERY slow (try 1–10)
    private Camera cam;
    //private bool hitPlayer = false;
    public float liftForce = 2f;
    private bool goingUp = false;

    public void PullUp()
    {
        goingUp = true;
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Convert pixels to world units
        float unitsPerPixel = (Camera.main.orthographicSize * 2f) / Screen.height;
        float moveAmount = pixelsPerSecond * unitsPerPixel;

        // Always falling
        transform.position += Vector3.down * moveAmount * Time.deltaTime;

        // If rope pulled, move up
        if (goingUp)
        {
            transform.position += Vector3.up * liftForce * Time.deltaTime;

            // stop after small lift
            goingUp = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Character")
        {
            //hitPlayer = true;
            PointSystem.instance.LosePoint();
            pixelsPerSecond = 0;
            //Destroy(gameObject);
        }
    }
}