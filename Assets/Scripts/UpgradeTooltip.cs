using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeTooltip : MonoBehaviour
{
    public static UpgradeTooltip instance;

    public GameObject panel;
    public TextMeshProUGUI descriptionText;

    private RectTransform panelRect;
    private Canvas rootCanvas;
    private Canvas tooltipCanvas;

    public bool IsVisible => panel != null && panel.activeSelf;

    void Awake()
    {
        instance = this;

        panelRect = panel.GetComponent<RectTransform>();

        // Find the top/root canvas
        Canvas[] canvases = GetComponentsInParent<Canvas>();
        rootCanvas = canvases[canvases.Length - 1];

        // Add a dedicated canvas to the tooltip panel
        tooltipCanvas = panel.GetComponent<Canvas>();

        if (tooltipCanvas == null)
            tooltipCanvas = panel.AddComponent<Canvas>();

        tooltipCanvas.overrideSorting = true;
        tooltipCanvas.sortingOrder = 999;

        // Needed for UI rendering
        if (panel.GetComponent<GraphicRaycaster>() == null)
            panel.AddComponent<GraphicRaycaster>();

        // Prevent tooltip from blocking mouse events
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        cg.blocksRaycasts = false;
        cg.interactable = false;

        // Ensure panel has an Image component
        Image img = panel.GetComponent<Image>();

        if (img != null)
        {
            Color c = img.color;

            // Prevent invisible background
            if (c.a <= 0f)
            {
                c.a = 0.85f;
                img.color = c;
            }
        }

        Hide();
    }

    public void Show(string text, Vector2 screenPosition)
    {
        descriptionText.text = text;

        panel.SetActive(true);

        // Make sure tooltip draws above everything
        panel.transform.SetAsLastSibling();

        UpdatePosition(screenPosition);
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        float tooltipWidth = panelRect.rect.width;

        // Flip tooltip if near right edge
        float offsetX =
            (screenPosition.x + tooltipWidth + 15f > Screen.width)
            ? -(tooltipWidth + 15f)
            : 15f;

        screenPosition += new Vector2(offsetX, -10f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            screenPosition,
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : rootCanvas.worldCamera,
            out Vector2 localPoint
        );

        panelRect.anchoredPosition = localPoint;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}