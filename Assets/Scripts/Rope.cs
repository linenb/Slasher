using UnityEngine;

public class Rope : MonoBehaviour
{
    public float pullDistance = 20f; // UI uses pixels!
    public float pullSpeed = 10f;

    private Vector3 startPos;
    private bool isPulling = false;

    public FallingObject knife;

    void Start()
    {
        startPos = transform.localPosition;
    }

    public void OnRopeClicked()
    {
        //Debug.Log("Rope clicked!");
        isPulling = true;

        if (knife != null)
        {
            knife.PullUp();
        }
    }

    void Update()
    {
        if (isPulling)
        {
            Vector3 target = startPos + Vector3.down * pullDistance;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, pullSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 1f)
            {
                isPulling = false;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, pullSpeed * Time.deltaTime);
        }
    }
}