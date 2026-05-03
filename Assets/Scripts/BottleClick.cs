using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BottleClick : MonoBehaviour
{
    public TearSystem tearSystem;

    void Update()
    {
        if (PauseManager.IsPaused) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                tearSystem.CollectBottle();
            }
        }
    }
}
