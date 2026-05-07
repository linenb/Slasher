using TMPro;
using UnityEngine;

public class TearScoreManager : MonoBehaviour
{
    public static TearScoreManager instance;

    public int score = 0;
    public TextMeshProUGUI pointText;
    public TearSystem tearSystem;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void Add(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void Spend(int amount)
    {
        score -= amount;
        score = Mathf.Max(score, 0);
        UpdateUI();
    }

    public bool HasEnough(int amount)
    {
        return score >= amount;
    }

    void UpdateUI()
    {
        if (pointText != null)
            pointText.text = "" + score;
    }
}