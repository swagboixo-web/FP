using UnityEngine;

public class ToxicWormAI : MonoBehaviour
{
    [Header("Detection & Firing")]
    public float triggerRadius = 12f;
    public float fireRate = 2.5f;
    public GameObject acidBulletPrefab; // Projectile to fire upward
    public Transform mouthPoint;        // Point near the worm's head

    [Header("Projectile Settings")]
    public float spitForce = 12f;

    private Transform playerTarget;
    private float nextFireTime;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTarget = p.transform;
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);

        // If player flies above/near the worm, spit acid
        if (distance <= triggerRadius && Time.time >= nextFireTime)
        {
            SpitAcid();
            nextFireTime = Time.time + fireRate;
        }
    }

    void SpitAcid()
    {
        if (acidBulletPrefab == null || mouthPoint == null) return;

        // This creates an upward vector with a slight random tilt (like a bubble toy)
        Vector3 randomSpread = new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(-0.5f, 0.5f)).normalized;

        GameObject acid = Instantiate(acidBulletPrefab, mouthPoint.position, Quaternion.identity);

        Rigidbody rb = acid.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Pushes the bubble up and out
            rb.linearVelocity = randomSpread * spitForce;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}