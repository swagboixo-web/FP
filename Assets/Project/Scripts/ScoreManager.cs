using UnityEngine;
using TMPro; // Crucial package needed to talk to TextMeshPro elements

public class ScoreManager : MonoBehaviour
{
    // Static instance allows any other script to easily send points here
    public static ScoreManager Instance;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore = 0;

    void Awake()
    {
        // Set up the Singleton pattern instance
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
        UpdateScoreUI();
    }

    // This is the function other scripts will call to add score
    public void AddPoints(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + currentScore;
        }
    }
}