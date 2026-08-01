using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // This function will run when the button is clicked
    public void ClickStartCampaign()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartCampaign();
        }
        else
        {
            Debug.LogError("GameManager instance is missing in the scene!");
        }
    }
}