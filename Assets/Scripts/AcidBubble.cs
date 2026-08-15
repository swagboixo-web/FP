using UnityEngine;

public class AcidBubble : MonoBehaviour, IDamageable
{
    public int damageToPlayer = 10;
    public float upwardFloatSpeed = 15f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        if (rb != null)
        {
            
            rb.AddForce(Vector3.up * upwardFloatSpeed, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.name.Contains("Worm")) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }
}