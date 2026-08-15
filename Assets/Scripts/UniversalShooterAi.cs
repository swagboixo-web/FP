using UnityEngine;

public class UniversalShooterAI : MonoBehaviour
{
    [Header("Targeting Settings")]
    [Tooltip("The tag of the object this enemy wants to shoot (e.g., 'Player')")]
    [SerializeField] private string targetTag = "Player";
    [Tooltip("How close the target must be before it starts shooting")]
    [SerializeField] private float attackRange = 15f;

    [Header("Rotation Settings")]
    [Tooltip("Should the enemy rotate to look at the target?")]
    [SerializeField] private bool faceTarget = true;
    [Tooltip("Check this for ground units so they don't tilt up/down into the floor. Uncheck for flying units.")]
    [SerializeField] private bool lockVerticalRotation = false;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1.5f;
    [Tooltip("Drag your FirePoint objects in here. You can have 1, or 10!")]
    [SerializeField] private Transform[] firePoints;

    private Transform target;
    private float nextFireTime = 0f;

    void Update()
    {
        // Dynamically look for the target in case it was destroyed or hasn't spawned yet
        if (target == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObj != null) target = targetObj.transform;
            else return;
        }

        // Check how far away the target is
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            if (faceTarget)
            {
                AimAtTarget();
            }

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void AimAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // If it's a ground unit, lock the Y axis so it only spins left and right
        if (lockVerticalRotation)
        {
            direction.y = 0;
        }

        // Prevent Unity console errors if the enemy is exactly on top of the player
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoints.Length == 0) return;

        foreach (Transform fp in firePoints)
        {
            if (fp != null)
            {
                //Calculates the exact angle from the firepoint up to the flying player
                Vector3 aimDirection = (target.position - fp.position).normalized;

                // Aims the bullet at that exact upward angle before firing
                Quaternion bulletRotation = Quaternion.LookRotation(aimDirection);

                Instantiate(bulletPrefab, fp.position, bulletRotation);
            }
        }
    }

    // This draws a yellow sphere in the editor so you can perfectly measure the attack range!
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}