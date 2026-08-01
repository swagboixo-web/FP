using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // REQUIRED to change scenes

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Components")]
    public Slider healthBar;
    public Animator animator;

    [Header("Game Over Settings")]
    [Tooltip("Type the EXACT name of your loading screen scene here")]
    public string loadingScreenName = "LoadingScreen"; // Change this if your scene is named differently

    private bool isInvincible = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible || isDead) return;

        currentHealth -= damageAmount;

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
            StartCoroutine(HitRoutine());
        }
    }

    private IEnumerator HitRoutine()
    {
        isInvincible = true;
        if (animator != null) animator.SetBool("GetHit", true);
        yield return new WaitForSeconds(0.2f);
        if (animator != null) animator.SetBool("GetHit", false);
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    private void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetBool("GetHit", false);
            animator.Play("death");
        }

        // Turn off player controls so they can't fly around while dead
        if (GetComponent<MosquitoController>() != null) GetComponent<MosquitoController>().enabled = false;
        if (GetComponent<PlayerCombat>() != null) GetComponent<PlayerCombat>().enabled = false;

        // Start the Game Over transition
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f); // Wait for player death animation
        SceneManager.LoadScene("GameOver"); // Loads the Game Over screen
    }
}