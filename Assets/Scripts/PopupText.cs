using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifeTime = 1f;

    Vector3 randomOffset;
    TextMeshProUGUI text;
    Color color;

    float time;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
        color = text.color;
    }

    public void Setup(int value)
    {
        text.text = "+" + value;

        // pradinis "pop" dydis
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        time += Time.deltaTime;
        transform.position += (Vector3.up + randomOffset) * moveSpeed * Time.deltaTime;

        //  1. POP IN (greitas scale up)
        if (time < 0.15f)
        {
            float t = time / 0.15f;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.3f, EaseOutBack(t));
        }
        //  2. BOUNCE atgal
        else if (time < 0.3f)
        {
            float t = (time - 0.15f) / 0.15f;
            transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, t);
        }

        //  judėjimas aukštyn
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        //  fade out
        float fadeStart = 0.5f;
        if (time > fadeStart)
        {
            float t = (time - fadeStart) / (lifeTime - fadeStart);
            color.a = Mathf.Lerp(1f, 0f, t);
            text.color = color;
        }

        //  destroy
        if (time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    //  gražesnis easing
    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}