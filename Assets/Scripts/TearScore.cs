using TMPro;
using UnityEngine;

public class TearScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI PointCounter;

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        PointCounter.text = "Tears: " + score;
    }
}