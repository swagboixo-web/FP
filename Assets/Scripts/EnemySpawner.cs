using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f; // Spawn an enemy every 2 seconds

    [Header("Spawn Boundary")]
    [SerializeField] private float minX = -5f;   // Leftmost spawn edge relative to spawner
    [SerializeField] private float maxX = 5f;    // Rightmost spawn edge relative to spawner

    private float nextSpawnTime = 0f;

    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // This spawns the enemy at the spawner's exact Z location, 
        // and adds a random X offset based on the spawner's exact X location!
        float randomX = transform.position.x + Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        Instantiate(enemyPrefab, spawnPosition, transform.rotation);
    }
}