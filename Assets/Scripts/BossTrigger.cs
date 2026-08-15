using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Camera Swap")]
    public GameObject mainCamera; 
    public GameObject bossCamera; 

    [Header("The Combatants")]
    public MosquitoController playerController;
    public IguanaBossController iguanaBoss;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player and we haven't triggered this yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. Swap the cameras!
            if (mainCamera != null) mainCamera.SetActive(false);
            if (bossCamera != null) bossCamera.SetActive(true);

            // 2. Free the player to fly freely
            if (playerController != null)
                playerController.bossFightActive = true;

            // 3. Wake up the Iguana!
            if (iguanaBoss != null)
                iguanaBoss.StartDuel();
        }
    }
}