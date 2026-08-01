using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void TryAgain()
    {
        // Reloads the main gameplay level
        SceneManager.LoadScene("level_1");
    }

    public void LoadMainMenu()
    {
        // Returns to the main menu
        SceneManager.LoadScene("MainMenu");
    }
}