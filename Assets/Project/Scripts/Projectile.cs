using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f; // Destroys itself after 3 seconds if it misses

    void Start()
    {
        // Automatically clean up memory
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move forward along the Z axis (up the screen)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    // This built-in Unity function automatically triggers when a collider hits another collider
    private void OnTriggerEnter(Collider other)
    {
        // FIX: Changed '==' to '.Contains()' to account for Unity adding "(Clone)"
        if (other.gameObject.name.Contains("TrackingEnemy"))
        {
        
          
            // Destroy the enemy cube
            Destroy(other.gameObject);

            ScoreManager.Instance.AddScore(1); // Adds 1 point!

            // Destroy the bullet itself
            Destroy(gameObject);
        }
    }
}