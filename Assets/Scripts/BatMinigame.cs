using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BatMinigame : MonoBehaviour, IPointerDownHandler
{
    public float speed = 300f;      // UI pixels per second (was 3f world units)
    public float bobHeight = 30f;   // UI pixels (was 0.5f world units)
    public float bobSpeed = 3f;

    private RectTransform rt;
    private Vector2 startPos;
    private float timeOffset;
    private RectTransform canvasRect;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
        timeOffset = Random.Range(0f, 100f);

        // Find canvas size for off-screen check
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
            canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move horizontally in UI space
        Vector2 pos = rt.anchoredPosition;
        pos.x += speed * Time.deltaTime;
        
        // Bob up/down
        float yOffset = Mathf.Sin((Time.time + timeOffset) * bobSpeed) * bobHeight;
        pos.y = startPos.y + yOffset;
        
        rt.anchoredPosition = pos;

        // Destroy if off screen
        if (canvasRect != null)
        {
            float halfWidth = canvasRect.rect.width / 2f;
            if (Mathf.Abs(rt.anchoredPosition.x) > halfWidth + 150f)
                Destroy(gameObject);
        }
    }

    // This replaces OnMouseDown for UI elements
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PauseManager.IsPaused) return;

        AudioManager.instance.PlayBatScreech();
        TriggerBatEffect();
        Destroy(gameObject);
    }

    void TriggerBatEffect()
    {
        if (TearSystem.instance != null)
            TearSystem.instance.BatScareExplosion();
    }
}