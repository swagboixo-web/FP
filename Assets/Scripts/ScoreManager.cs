using UnityEngine;
using TMPro; // Crucial for TextMeshPro!

public class ScoreManager : MonoBehaviour
{
    // A "static instance" lets any other script easily find this ScoreManager
    public static ScoreManager Instance;

    [Header("UI Components")]
    public TextMeshProUGUI scoreText; // Drag your ScoreText object here

    private int score = 0;

    void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    // Call this function from other scripts to add points
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // Updates the actual text on the screen
    void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score;
    }
}