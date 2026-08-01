using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines (timing the attacks)

public class IguanaBossController : MonoBehaviour, IDamageable
{
    [Header("Boss Stats & UI")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider bossHealthBar;

    [Header("Epic AI Settings")]
    public float moveSpeed = 12f;
    public float stoppingDistance = 4f;
    public float retreatDistance = 20f;
    public float attackCooldown = 2.5f;

    [Header("Attack Hitboxes")]
    public GameObject tongueHitbox; // Drag Tongue.1 or Tongue.2 here
    public GameObject biteHitbox;   // Drag BiteHitBox here

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
        // 1. Set state to attacking so the Update loop doesn't run this twice
        currentState = BossState.Attacking;

        // 2. TONGUE LASH PHASE
        // If you have an Animator, trigger the tongue animation here!
        if (tongueHitbox != null) tongueHitbox.SetActive(true);
        yield return new WaitForSeconds(0.3f); // Tongue stays out for 0.3 seconds
        if (tongueHitbox != null) tongueHitbox.SetActive(false);

        // 3. WAIT A TINY BIT
        yield return new WaitForSeconds(0.2f); // Brief pause before the bite

        // 4. BITE PHASE
        iguanaCharacter.Attack(); // Triggers your original attack logic
        if (biteHitbox != null) biteHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Bite hitbox stays active for 0.5 seconds
        if (biteHitbox != null) biteHitbox.SetActive(false);

        // 5. FINISH AND RETREAT
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
        currentHealth -= damage;
        if (bossHealthBar != null) bossHealthBar.value = currentHealth;

        iguanaCharacter.Hit();
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        iguanaCharacter.Move(0f, 0f);
        iguanaCharacter.Death();
        GetComponent<Collider>().enabled = false;

        if (tongueHitbox != null) tongueHitbox.SetActive(false);
        if (biteHitbox != null) biteHitbox.SetActive(false);
        if (bossHealthBar != null) bossHealthBar.gameObject.SetActive(false);
    }
}