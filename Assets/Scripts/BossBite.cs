using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    [Header("Hitbox Damage")]
    public int attackDamage = 25;

    // Check if we hit the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth pHealth = other.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(attackDamage);
            }

            // Turn off the hitbox immediately so it doesn't hit twice in one swing
            gameObject.SetActive(false);
        }
    }
}