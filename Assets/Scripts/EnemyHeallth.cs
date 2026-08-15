using UnityEngine;
using UnityEngine.UI; // 1. We need this to talk to the UI Slider
using System.Collections; // 2. We need this for the Bool Coroutine

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Components")]
    public Animator animator;
    public Slider healthBar; // 3. The new slot for the enemy's floating health bar!

    void Start()
    {
        currentHealth = maxHealth;

        // 4. Synchronize the health bar when the enemy spawns
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Stop the script if the enemy is already dead
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage! Health left: " + currentHealth);

        // 5. Visually drain the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 6. Use the Coroutine for the Bool instead of a Trigger
            if (animator != null)
            {
                StartCoroutine(HitRoutine());
            }
        }
    }

    // 7. This turns the GetHit Bool ON, waits a fraction of a second, and turns it OFF
    private IEnumerator HitRoutine()
    {
        animator.SetBool("GetHit", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("GetHit", false);
    }

    void Die()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        if (animator != null)
        {
            // 8. Force the transition straight to death, bypassing the Animator arrows
            animator.SetBool("GetHit", false); // Safety catch
            animator.Play("Death");
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // 9. Hide the health bar immediately upon death so it doesn't float over the corpse
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        Destroy(gameObject, 0.2f);
    }
}