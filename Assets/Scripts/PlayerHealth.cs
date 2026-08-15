using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;

    // Changed to public so EnemySpawner can read player health for dynamic difficulty
    public int currentHealth;

    [Header("UI Components")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("Components")]
    public Animator animator;

    [Header("Game Over Settings")]
    [Tooltip("Type the EXACT name of your loading screen scene here")]
    public string loadingScreenName = "LoadingScreen";

    private bool isInvincible = false;
    private bool isDead = false;

    void Start()
    {
        
        int maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 10);
        currentHealth = maxHealth;
    }

    // Handles solid physical collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    // Handles trigger-based collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    // Deals continuous damage if the player stays touching an enemy after invincibility wears off
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible || isDead) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitRoutine());
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}";
        }
    }

    private IEnumerator HitRoutine()
    {
        isInvincible = true;
        if (animator != null) animator.SetBool("GetHit", true);
        yield return new WaitForSeconds(0.2f);
        if (animator != null) animator.SetBool("GetHit", false);
        yield return new WaitForSeconds(0.4f);
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

        if (GetComponent<MosquitoController>() != null) GetComponent<MosquitoController>().enabled = false;
        if (GetComponent<PlayerCombat>() != null) GetComponent<PlayerCombat>().enabled = false;

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }
}