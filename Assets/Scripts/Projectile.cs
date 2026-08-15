using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        IDamageable target = other.GetComponentInParent<IDamageable>();

       
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        
        if (other.gameObject.name.Contains("TrackingEnemy"))
        {
            Destroy(other.gameObject);
            // Assuming ScoreManager is a Singleton that still exists
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            // Pull the upgraded damage from memory (defaults to 1)
            int currentDamage = PlayerPrefs.GetInt("PlayerDamage", 1);

            // Apply that damage to the enemy
            other.GetComponent<EnemyHealth>().TakeDamage(currentDamage);

            Destroy(gameObject);
        }
    }
}