using UnityEngine;

public class Tear : MonoBehaviour
{
    public Transform target;
    public TearSystem system;

    public float speed = 4f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            system.AddCurrency();
            Destroy(gameObject);
        }
    }
}