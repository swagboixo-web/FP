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
        // 1. Ask the object we hit: "Did you sign the IDamageable contract?"
        IDamageable target = other.GetComponentInParent<IDamageable>();

        // 2. If it is NOT null, it signed the contract! Hurt it!
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 3. Keep your old fallback just in case those old cubes are still around
        if (other.gameObject.name.Contains("TrackingEnemy"))
        {
            Destroy(other.gameObject);
            // Assuming ScoreManager is a Singleton that still exists
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}