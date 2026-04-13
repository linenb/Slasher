using UnityEngine;

public class KnifeDetector : MonoBehaviour
{
    public static KnifeDetector instance;

    [Header("References")]
    public Transform foxTransform;         

    [Header("Settings")]
    public float proximityThreshold = 2f;  
    public bool KnifeIsClose { get; private set; } = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        GameObject knife = GameObject.FindGameObjectWithTag("Knife");

        if (knife == null || foxTransform == null)
        {
            KnifeIsClose = false;
            return;
        }

        float distance = Vector3.Distance(knife.transform.position, foxTransform.position);
        KnifeIsClose = distance <= proximityThreshold;
    }
}