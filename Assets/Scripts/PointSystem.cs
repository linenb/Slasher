using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    public static PointSystem instance;
    public int points = 0;
    public TextMeshProUGUI pointsText;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + points;
    }

    public void AddPoint()
    {
        points++;
        Debug.Log("Dodged! Points: " + points);
    }

    public void LosePoint()
    {
        points--;
        Debug.Log("Hit! Points: " + points);

        if (points < 0)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();

            if (gm != null)
                gm.TriggerGameOver();
            else
                Debug.LogWarning("GameManager not found!");
        }
    }
}