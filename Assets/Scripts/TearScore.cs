using TMPro;
using UnityEngine;

public class TearScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI PointCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Update();
    }
    // Update is called once per frame
    void Update()
    {
        PointCounter.text = "Tears " + score;
    }
    public void AddScore(int amount)
    {
        score += amount;
        Update();
    }
}
