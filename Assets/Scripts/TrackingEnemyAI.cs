using UnityEngine;
using System.Collections;

public class TrackingEnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float fallbackMoveSpeed = 3f;
    public float attackDistance = 2f; // Distance at which the spider starts biting while walking

    [Header("Combat Settings")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    public int biteDamage = 1;

    [Header("Components")]
    public Animator animator;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. ALWAYS keep chasing and moving legs toward the player!
        ChasePlayer();

        // 2. Trigger the bite while still moving if close enough
        if (distanceToPlayer <= attackDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            BiteAttack();
        }
    }

    void ChasePlayer()
    {
        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Determine current speed from DDAManager if available, otherwise use fallback
        float currentSpeed = (DDAManager.Instance != null) ? DDAManager.Instance.currentEnemySpeed : fallbackMoveSpeed;

        // Move forward continuously
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Keep the leg walking animation active
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    void BiteAttack()
    {
        lastAttackTime = Time.time;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (animator != null)
        {
            animator.SetBool("Attack1", true);
        }

        yield return new WaitForSeconds(0.4f);

        // Deal damage if player is still in range
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                float currentDistance = Vector3.Distance(transform.position, player.position);
                if (currentDistance <= attackDistance + 0.5f)
                {
                    playerHealth.TakeDamage(biteDamage);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        if (animator != null)
        {
            animator.SetBool("Attack1", false);
        }
    }
}