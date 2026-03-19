using UnityEngine;
using UnityEngine.InputSystem;

public class BottleClick : MonoBehaviour
{
    public TearSystem tearSystem;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Clicked via new input system");
                tearSystem.CollectBottle();
            }
        }
    }
}