using UnityEngine;

/// <summary>
/// Renders one balloon as a procedural circle.
/// Works entirely in LOCAL space — parented to the Knife, so it follows it naturally.
/// The "string" is drawn from the balloon down to localPosition.y = 0 (the knife pivot).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BalloonVisual : MonoBehaviour
{
    [Header("Appearance")]
    public Color  balloonColor  = Color.red;
    public float  balloonRadius = 0.5f;     // in LOCAL (knife) units

    [Header("Bob")]
    public float bobAmplitude = 0.12f;     // local units
    public float bobSpeed     = 2.2f;
    public float bobOffset    = 0f;

    [Header("String")]
    public float stringLocalLength = 0.7f; // local units downward from balloon centre

    // ── private ──────────────────────────────────────────────────────────────
    private SpriteRenderer sr;
    private LineRenderer   stringLine;
    private Vector3        baseLocalPos;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 10;
        BuildString();
    }

    void Start()
    {
        // Read radius HERE — after BalloonAttachment has assigned it
        sr.sprite = CreateCircleSprite(64, balloonColor);
        float diameter = balloonRadius * 2f;
        transform.localScale = new Vector3(diameter, diameter, 1f);

        baseLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Bob in local Y
        float yOff = Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobAmplitude;
        transform.localPosition = baseLocalPos + new Vector3(0f, yOff, 0f);

        // String: world positions derived from local space
        if (stringLine != null)
        {
            // Bottom of balloon circle
            Vector3 balloonBottom = transform.TransformPoint(new Vector3(0f, -0.5f, 0f));
            // Tip of string, stringLocalLength below the balloon in local space
            Vector3 stringTip     = transform.TransformPoint(new Vector3(0f, -0.5f - stringLocalLength, 0f));
            stringLine.SetPosition(0, balloonBottom);
            stringLine.SetPosition(1, stringTip);
        }
    }

    void BuildString()
    {
        GameObject stringGO = new GameObject("BalloonString");
        // Make it a sibling (child of knife, not child of balloon) so scale doesn't double-apply
        stringGO.transform.SetParent(transform.parent);
        stringGO.transform.localPosition = Vector3.zero;
        stringGO.transform.localScale    = Vector3.one;

        stringLine = stringGO.AddComponent<LineRenderer>();
        stringLine.positionCount  = 2;
        stringLine.startWidth     = 0.04f;
        stringLine.endWidth       = 0.04f;
        stringLine.useWorldSpace  = true;
        stringLine.material       = new Material(Shader.Find("Sprites/Default"));
        stringLine.startColor     = new Color(0.25f, 0.25f, 0.25f, 1f);
        stringLine.endColor       = new Color(0.25f, 0.25f, 0.25f, 1f);
        stringLine.sortingOrder   = 9;
    }

    void OnDestroy()
    {
        if (stringLine != null)
            Destroy(stringLine.gameObject);
    }

    // ── Procedural circle sprite ─────────────────────────────────────────────
    static Sprite CreateCircleSprite(int res, Color color)
    {
        Texture2D tex = new Texture2D(res, res, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;

        float center = res / 2f;
        float r      = center - 1f;

        for (int y = 0; y < res; y++)
        for (int x = 0; x < res; x++)
        {
            float dx   = x - center, dy = y - center;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);
            if (dist <= r)
            {
                float hl = Mathf.Clamp01(((-dx - dy) / r) * 0.5f + 0.5f)
                         * Mathf.Clamp01(1f - dist / r);
                Color c = Color.Lerp(color, Color.white, hl * 0.35f);
                c.a = 1f;
                tex.SetPixel(x, y, c);
            }
            else tex.SetPixel(x, y, Color.clear);
        }
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, res, res), new Vector2(0.5f, 0.5f), res);
    }
}
