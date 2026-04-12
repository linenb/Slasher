using UnityEngine;
using UnityEngine.InputSystem;

public class FoxClick : MonoBehaviour
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
                OnFoxClicked();
            }
        }
    }

    void OnFoxClicked()
    {
        int value = 1;

        bool isGolden = Random.value < tearSystem.goldenTearChance;

        if (isGolden)
            value = 1000;

        // Give points instantly
        TearScoreManager.instance.Add(value);

        // Slight randomness for popup position
        Vector3 offset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(0f, 0.3f),
            0
        );

        tearSystem.SpawnPopup(value, tearSystem.eyePoint.position + offset);

        // Spawn visual tear
        tearSystem.SpawnClickTear();
    }
}