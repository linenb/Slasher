using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FoxClick : MonoBehaviour
{
    public TearSystem tearSystem;
    public int xpPerTear = 1;

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
                OnFoxClicked();
            }
        }
    }

    void OnFoxClicked()
    {
        int value = tearSystem.clickValue;

        bool isGolden = Random.value < tearSystem.goldenTearChance;

        if (isGolden)
            value = 1000;

        TearScoreManager.instance.Add(value);
        if (XPSystem.instance != null)
            XPSystem.instance.AddXP(value * xpPerTear);

        Vector3 offset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(0f, 0.3f),
            0
        );

        tearSystem.SpawnPopup(value, tearSystem.eyePoint.position + offset);
        tearSystem.SpawnClickTear();
    }
}
