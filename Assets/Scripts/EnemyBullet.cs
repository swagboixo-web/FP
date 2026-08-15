using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 18f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 4f;

    void Start()
    {
        // Auto-cleanup so bullets don't clog up your game hierarchy
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Fly straight forward in the direction the launcher was facing
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore the enemy that shot it or other bullets
        if (other.CompareTag("Enemy") || other.CompareTag("Untagged")) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            // Destroy on contact with terrain or obstacles
            Destroy(gameObject);
        }
    }
}