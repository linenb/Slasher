using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to the Knife prefab root (same GameObject as FallingObject).
///
/// Positions balloons in the knife's LOCAL space, so they always sit above the
/// knife tip regardless of world scale or position.
///
/// Call  BalloonAttachment.SetBalloonCount(n)  from UpgradeManager whenever the
/// "lighter knife" upgrade level increases — one balloon per level.
/// </summary>
public class BalloonAttachment : MonoBehaviour
{
    // ── Singleton ───────────────────────────────────────────────────────────
    public static BalloonAttachment instance;

    // ── Inspector tweaks ────────────────────────────────────────────────────
    [Header("Balloon Layout (LOCAL space — relative to knife pivot)")]

    [Tooltip("Y position of balloons in the knife's local space. " +
             "Positive = towards the tip. Tweak this until balloons sit above the blade.")]
    public float localVerticalOffset = 2.5f;

    [Tooltip("X spread between balloons in local space.")]
    public float localHorizontalSpread = 0.6f;

    [Tooltip("Alternate Y stagger so balloons look clustered, not in a row (local units).")]
    public float localStagger = 0.3f;

    [Tooltip("Balloon radius in LOCAL space (will be scaled by the knife's transform).")]
    public float balloonLocalRadius = 0.5f;

    [Tooltip("Colours cycling per balloon level")]
    public Color[] balloonColors = new Color[]
    {
        new Color(1f,  0.2f, 0.2f),
        new Color(0.3f, 0.6f, 1f),
        new Color(0.3f, 0.9f, 0.3f),
        new Color(1f,  0.85f, 0.1f),
        new Color(1f,  0.4f, 0.9f),
        new Color(0.6f, 0.2f, 1f),
        new Color(1f,  0.6f, 0.1f),
    };

    // ── Runtime state ────────────────────────────────────────────────────────
    private readonly List<GameObject> spawnedBalloons = new List<GameObject>();
    private static int pendingCount = 0;

    // ── Static API ───────────────────────────────────────────────────────────
    public static void SetBalloonCount(int count)
    {
        if (instance != null)
            instance.ApplyBalloonCount(count);
        else
            pendingCount = count;
    }

    // ── Unity lifecycle ──────────────────────────────────────────────────────
    void Awake() { instance = this; }

    void Start()
    {
        if (pendingCount > 0)
            ApplyBalloonCount(pendingCount);
    }

    void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    // ── Scene gizmo — visible in the Scene view so you can tune the offset ──
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        // Draw a small cross at the balloon anchor point in local space
        Vector3 worldAnchor = transform.TransformPoint(new Vector3(0f, localVerticalOffset, 0f));
        float s = 0.1f;
        Gizmos.DrawLine(worldAnchor - Vector3.right * s, worldAnchor + Vector3.right * s);
        Gizmos.DrawLine(worldAnchor - Vector3.up    * s, worldAnchor + Vector3.up    * s);
        Gizmos.DrawWireSphere(worldAnchor, 0.05f);
    }

    // ── Internal ─────────────────────────────────────────────────────────────
    void ApplyBalloonCount(int targetCount)
    {
        targetCount = Mathf.Max(0, targetCount);

        while (spawnedBalloons.Count < targetCount)
            SpawnOneBalloon(spawnedBalloons.Count);

        while (spawnedBalloons.Count > targetCount)
        {
            int last = spawnedBalloons.Count - 1;
            Destroy(spawnedBalloons[last]);
            spawnedBalloons.RemoveAt(last);
        }

        LayoutBalloons();
    }

    void SpawnOneBalloon(int index)
    {
        GameObject go = new GameObject($"Balloon_{index}");
        go.transform.SetParent(transform);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale    = Vector3.one;   // keep it in local space of knife

        BalloonVisual visual = go.AddComponent<BalloonVisual>();
        visual.balloonColor  = balloonColors[index % balloonColors.Length];
        visual.bobOffset     = index * 0.9f;
        visual.balloonRadius = balloonLocalRadius;

        spawnedBalloons.Add(go);
    }

    void LayoutBalloons()
    {
        int count = spawnedBalloons.Count;
        if (count == 0) return;

        float totalWidth = (count - 1) * localHorizontalSpread;
        float startX     = -totalWidth / 2f;

        for (int i = 0; i < count; i++)
        {
            float x      = startX + i * localHorizontalSpread;
            float yExtra = (i % 2 == 0) ? 0f : localStagger;
            // localPosition — lives in the knife's own local space
            spawnedBalloons[i].transform.localPosition =
                new Vector3(x, localVerticalOffset + yExtra, 0f);
        }
    }
}
