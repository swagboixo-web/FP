using UnityEngine;
using UnityEngine.SceneManagement; // Allows us to load and unload scenes
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Campaign Settings")]
    public int currentLevel = 1;
    public int totalMedalsEarned = 0;

    void Awake()
    {
        // Singleton pattern to keep this running across all scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this function when the player clicks "PLAY" on the Main Menu
    public void StartCampaign()
    {
        StartCoroutine(LoadLevelSequence(1));
    }

    // Call this function when an enemy boss is defeated to go to the next stage
    public void AdvanceNextLevel()
    {
        currentLevel++;
        if (currentLevel <= 9)
        {
            StartCoroutine(LoadLevelSequence(currentLevel));
        }
        else
        {
            Debug.Log("CAMPAIGN COMPLETE! You saved the bedroom!");
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Beautiful asynchronous loading screen sequence
    private IEnumerator LoadLevelSequence(int levelBuildIndex)
    {
        // 1. Load the Loading Screen Scene first
        SceneManager.LoadScene("LoadingScreen");

        // Wait for 2 seconds so the player can read the level tips/narrative arc
        yield return new WaitForSeconds(2f);

        // 2. Load the actual gameplay world map asynchronously in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync("level_" + levelBuildIndex);

        while (!operation.isDone)
        {
            yield return null; // Wait until fully loaded
        }
    }
}
