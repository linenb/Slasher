using UnityEngine;
using TMPro;

public class UpgradeTooltip : MonoBehaviour
{
    public static UpgradeTooltip instance;

    public GameObject panel;
    public TextMeshProUGUI descriptionText;

    void Awake()
    {
        instance = this;
        Hide();
    }

    public void Show(string text, Vector2 position)
    {
        panel.SetActive(true);
        descriptionText.text = text;
        panel.transform.position = position;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}