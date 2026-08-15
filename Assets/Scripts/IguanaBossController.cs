using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IguanaBossController : MonoBehaviour, IDamageable
{
    [Header("Boss Stats & UI")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider bossHealthBar;
    public TextMeshProUGUI healthText;
    public GameObject damageTextPrefab;

    [Header("Level Transition")]
    [Tooltip("The exact name of the scene to load when the boss dies")]
    public string nextSceneName = "WinScene";
    public float delayBeforeLoading = 4f;

    [Header("Epic AI Settings")]
    public float moveSpeed = 12f;
    public float stoppingDistance = 4f;
    public float retreatDistance = 20f;
    public float attackCooldown = 2.5f;

    [Header("Attack Hitboxes")]
    public GameObject tongueHitbox;
    public GameObject biteHitbox;

    private IguanaCharacter iguanaCharacter;
    private Transform playerTarget;

    private float nextAttackTime;
    private bool isAwake = false;
    private bool isDead = false;

    private enum BossState { Waiting, Chasing, Attacking, Retreating }
    private BossState currentState = BossState.Waiting;

    void Start()
    {
        iguanaCharacter = GetComponent<IguanaCharacter>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = currentHealth;
            bossHealthBar.gameObject.SetActive(false);
        }

        // Ensure hitboxes are off when the game starts
        if (tongueHitbox != null) tongueHitbox.SetActive(false);
        if (biteHitbox != null) biteHitbox.SetActive(false);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTarget = p.transform;
    }

    void Update()
    {
        if (!isAwake || isDead || playerTarget == null) return;

        Vector3 lookDirection = playerTarget.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 8f);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        switch (currentState)
        {
            case BossState.Chasing:
                if (distanceToPlayer > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
                    iguanaCharacter.Move(1f, 0f);
                }
                else
                {
                    iguanaCharacter.Move(0f, 0f);
                    if (Time.time >= nextAttackTime)
                    {
                        // Start the sequenced attack!
                        StartCoroutine(AttackSequence());
                    }
                }
                break;

            case BossState.Retreating:
                if (distanceToPlayer < retreatDistance)
                {
                    Vector3 retreatTarget = transform.position + (transform.position - playerTarget.position).normalized * 5f;
                    transform.position = Vector3.MoveTowards(transform.position, retreatTarget, (moveSpeed * 0.6f) * Time.deltaTime);
                    iguanaCharacter.Move(-1f, 0f);
                }
                else
                {
                    iguanaCharacter.Move(0f, 0f);
                    currentState = BossState.Chasing;
                }
                break;
        }
    }

    private IEnumerator AttackSequence()
    {
        // 1. Set state to attacking
        currentState = BossState.Attacking;

        // 2. Trigger the Slap Animation
        iguanaCharacter.Attack();

        // 3. Wait a split second for the arm to actually swing forward in the animation
        yield return new WaitForSeconds(0.2f);

        // 4. Turn on the damage hitbox!
        if (biteHitbox != null) biteHitbox.SetActive(true);

        // 5. Keep the hitbox active while the arm is swinging
        yield return new WaitForSeconds(0.4f);

        // 6. Turn the hitbox off safely
        if (biteHitbox != null) biteHitbox.SetActive(false);

        // 7. Finish and Retreat
        nextAttackTime = Time.time + attackCooldown;
        currentState = BossState.Retreating;
    }

    public void StartDuel()
    {
        isAwake = true;
        currentState = BossState.Chasing;
        if (bossHealthBar != null) bossHealthBar.gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth = Mathf.Max(0, currentHealth - damage);

        UpdateHealthUI();
        iguanaCharacter.Hit();

        //FLOATING TEXT FEATURE
        if (damageTextPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1f, 1f), 3f, Random.Range(-1f, 1f));
            GameObject textObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

            FloatingDamageText floatingText = textObj.GetComponent<FloatingDamageText>();
            if (floatingText != null)
            {
                floatingText.Setup(damage);
            }
        }

        if (currentHealth <= 0) Die();
    }

    private void UpdateHealthUI()
    {
        if (bossHealthBar != null) bossHealthBar.value = currentHealth;
        if (healthText != null) healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void Die()
    {
        isDead = true;
        iguanaCharacter.Move(0f, 0f);
        iguanaCharacter.Death();
        GetComponent<Collider>().enabled = false;

        // Clean up UI and weapons
        if (tongueHitbox != null) tongueHitbox.SetActive(false);
        if (biteHitbox != null) biteHitbox.SetActive(false);
        if (bossHealthBar != null) bossHealthBar.gameObject.SetActive(false);

        // Start the countdown to load the next scene!
        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        // Wait for the death animation to play out
        yield return new WaitForSeconds(delayBeforeLoading);

        // Load the victory/loading screen
        SceneManager.LoadScene(nextSceneName);
    }
}