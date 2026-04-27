using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float pixelsPerSecond = 5f; // VERY slow (try 1–10)

    private Camera cam;

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
        // Gauti multiplierius (saugiai)
        float fallMult = 1f;
        float liftMult = 1f;

        if (FallingObjectManager.instance != null)
        {
            fallMult = FallingObjectManager.instance.speedMultiplier;
            liftMult = FallingObjectManager.instance.liftMultiplier;
        }

        // Clamp (apsauga nuo per didelio greičio)
        fallMult = Mathf.Max(fallMult, 0.2f); // keeps the lower bound, removes upper cap
        liftMult = Mathf.Clamp(liftMult, 0.2f, 3f);

        // Convert pixels to world units
        float unitsPerPixel = (cam.orthographicSize * 2f) / Screen.height;

        // FALL (su multiplier)
        float moveAmount = pixelsPerSecond * fallMult * unitsPerPixel;
        transform.position += Vector3.down * moveAmount * Time.deltaTime;

        // LIFT (su multiplier)
        if (goingUp)
        {
            transform.position += Vector3.up * liftForce * liftMult * Time.deltaTime;
            goingUp = false;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
        Debug.Log("HIT!");

        pixelsPerSecond = 0;

        AnimalDeathAnimation death = other.GetComponent<AnimalDeathAnimation>();

        if (death != null)
        {
                death.PlayDeath();
                GameObject[] knives = GameObject.FindGameObjectsWithTag("Knife");
                Debug.Log("Rasta peilių: " + knives.Length); // ar randa?
                foreach (GameObject knife in knives)
                {
                    Destroy(knife);
                }
            }
        }
    }
}