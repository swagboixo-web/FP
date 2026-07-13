using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f; // Spawn an enemy every 2 seconds

    [Header("Spawn Boundary")]
    [SerializeField] private float spawnZ = 12f; // Position slightly past the top edge of the screen
    [SerializeField] private float minX = -6f;   // Leftmost spawn edge
    [SerializeField] private float maxX = 6f;    // Rightmost spawn edge

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

        // Pick a random horizontal position along the top edge
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, 0f, spawnZ);

        // Spawn the enemy prefab into the game world
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}