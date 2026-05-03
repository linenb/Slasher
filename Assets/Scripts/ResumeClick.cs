using UnityEngine;
using UnityEngine.InputSystem;

public class ResumeClick : MonoBehaviour
{
    public PauseManager pauseManager;

    void OnEnable()
    {
        InputSystem.onAfterUpdate += CheckClick;
    }

    void OnDisable()
    {
        InputSystem.onAfterUpdate -= CheckClick;
    }

    void CheckClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && PauseManager.IsPaused)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransform rt = GetComponent<RectTransform>();

            Canvas canvas = GetComponentInParent<Canvas>();
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            if (RectTransformUtility.RectangleContainsScreenPoint(rt, mousePos, cam))
            {
                pauseManager.Resume();
            }
        }
    }
}