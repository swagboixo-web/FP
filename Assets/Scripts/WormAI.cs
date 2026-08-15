using UnityEngine;

public class WormAI : MonoBehaviour
{
    private enum State { Patrolling, Attacking }
    private State currentState = State.Patrolling;

    [Header("References")]
    [Tooltip("Leave empty to auto-find the player")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Where the bullet spawns (e.g., the worm's mouth)")]
    [SerializeField] private Transform firePoint;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float turnSpeed = 5f;

    private Vector3 startingPosition;
    private Vector3 currentPatrolTarget;

    [Header("Combat Settings")]
    [Tooltip("Distance at which the worm notices the player")]
    [SerializeField] private float aggroRange = 25f;
    [Tooltip("Distance at which the worm stops to shoot")]
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float fireRate = 2f;

    private float nextFireTime = 0f;

    void Start()
    {
        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) player = pObj.transform;
        }

        startingPosition = transform.position;
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // State Machine Logic
        if (distanceToPlayer <= aggroRange)
        {
            currentState = State.Attacking;
        }
        else
        {
            currentState = State.Patrolling;
        }

        // Execute current state
        if (currentState == State.Patrolling)
        {
            Patrol();
        }
        else if (currentState == State.Attacking)
        {
            Attack(distanceToPlayer);
        }
    }

    private void Patrol()
    {
        // Move towards patrol target
        Vector3 direction = (currentPatrolTarget - transform.position).normalized;
        direction.y = 0; // Keep movement flat

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime, Space.Self);
        }

        // Check if we reached the target
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(currentPatrolTarget.x, 0, currentPatrolTarget.z)) < 1f)
        {
            SetNewPatrolTarget();
        }
    }

    private void Attack(float distanceToPlayer)
    {
        // Always face the player when attacking
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // Move closer if outside attack range
        if (distanceToPlayer > attackRange)
        {
            transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Stop and shoot
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                // Instantiate bullet and aim it forward
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        currentPatrolTarget = startingPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    // Draws helpful visual rings in the Unity Editor to help you balance ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}