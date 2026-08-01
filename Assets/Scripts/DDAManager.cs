using UnityEngine;

public class DDAManager : MonoBehaviour
{
    public static DDAManager Instance;

    [Header("Base Settings")]
    public float baseEnemySpeed = 3f;
    public float baseSpawnInterval = 3f;

    [Header("Current Dynamic Values")]
    public float currentEnemySpeed;
    public float currentSpawnInterval;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        ResetDifficulty();
    }

    public void UpdateDifficulty(int currentScore)
    {
        // Every 50 points increases enemy speed and speeds up spawn timers
        float difficultyMultiplier = currentScore / 50f;

        currentEnemySpeed = baseEnemySpeed + (difficultyMultiplier * 0.3f);
        currentSpawnInterval = Mathf.Max(0.8f, baseSpawnInterval - (difficultyMultiplier * 0.15f));
    }

    public void ResetDifficulty()
    {
        currentEnemySpeed = baseEnemySpeed;
        currentSpawnInterval = baseSpawnInterval;
    }
}