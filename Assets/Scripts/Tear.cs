using UnityEngine;

public class Tear : MonoBehaviour
{
    public Transform target;
    public TearSystem system;
    public bool isGolden;
    public int value = 1;

    public float speed = 4f;

    void Update()
    {
        if (isGolden)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            value = 1000;
        }
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            system.AddCurrency(value);
            system.SpawnPopup(value, transform.position);
            value = 1;
            Destroy(gameObject);
        }
    }
}