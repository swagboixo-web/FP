using UnityEngine;

public class TrackingEnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [Tooltip("How close the player needs to be before the AI starts chasing.")]
    [SerializeField] private float detectionRange = 12f;

    private Transform playerTransform;
    private Rigidbody rb; // Rigidbody reference for kinematic movement

    void Start()
    {
        // Grab the Rigidbody component attached to this enemy
        rb = GetComponent<Rigidbody>();

        // Dynamically locate the player using the Tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    // Checking distance: Physics/Rigidbody alterations must happen in FixedUpdate
    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // Calculate current distance between this enemy and the player
        float distanceToPlayer = Vector3.Distance(rb.position, playerTransform.position);

        // Only track if within the AI's awareness zone
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // 1. Calculate the raw vector pointing from the enemy to the player
        Vector3 heading = playerTransform.position - rb.position;
        heading.y = 0f; // Lock Y axis

        // 2. Normalize the vector to get a pure direction
        Vector3 direction = heading.normalized;

        // 3. Movement logic: Move position using the physics engine instead of transform.Translate
        Vector3 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // Smoothly rotate the cube to face the direction it's chasing
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}