using UnityEngine;

public class bossbite : MonoBehaviour
{
    [Header("Attack Damage")]
    public int damageAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to find the PlayerHealth script on the hit player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Iguana hit the player for " + damageAmount + " damage!");
            }

            // Turn off hitbox immediately after hitting so it doesn't deal damage twice in one attack
            gameObject.SetActive(false);
        }
    }
}