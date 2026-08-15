using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Base Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Ambush Boundary Settings")]
    [Tooltip("How close the player needs to be to trigger the full horde")]
    [SerializeField] private float ambushActivationDistance = 80f;
    [Tooltip("Max spiders allowed to hunt you BEFORE you reach the boundary")]
    [SerializeField] private int earlyGameMaxCap = 3;
    [Tooltip("How often they spawn before you reach the boundary")]
    [SerializeField] private float earlyGameSpawnRate = 6.0f;

    [Header("Dynamic Difficulty Adjustment (DDA)")]
    [Tooltip("Target player to monitor health for difficulty scaling")]
    [SerializeField] private Transform playerTransform;

    [Header("Easy Mode (Player Health < 30%)")]
    [SerializeField] private float easySpawnRate = 3.0f;
    [SerializeField] private int easyWaveSize = 3;
    [SerializeField] private int easyMaxCap = 30;

    [Header("Normal Mode (Player Health 30% - 70%)")]
    [SerializeField] private float normalSpawnRate = 2.0f;
    [SerializeField] private int normalWaveSize = 4;
    [SerializeField] private int normalMaxCap = 40;

    [Header("Hard Mode (Player Health > 70%)")]
    [SerializeField] private float hardSpawnRate = 1.0f;
    [SerializeField] private int hardWaveSize = 7;
    [SerializeField] private int hardMaxCap = 100;

    [Header("Spawn Positioning")]
    [SerializeField] private float spawnRadius = 40f;

    private float nextSpawnTime = 0f;
    private PlayerHealth playerHealthScript;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        if (playerTransform != null)
        {
            playerHealthScript = playerTransform.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            GetDifficultySettings(out float currentRate, out int currentWave, out int currentCap);
            SpawnEnemyWave(currentWave, currentCap);
            nextSpawnTime = Time.time + currentRate;
        }
    }

    private void GetDifficultySettings(out float spawnRate, out int waveSize, out int maxCap)
    {
        // 1. Check distance to player first to see if we are in Ambush mode!
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // If the player is far away, restrict the spawner to the "early game" trickle
            if (distanceToPlayer > ambushActivationDistance)
            {
                spawnRate = earlyGameSpawnRate;
                waveSize = 1; // Only trickle them in one at a time
                maxCap = earlyGameMaxCap;
                return; // Skip the DDA completely until they get closer
            }
        }

        // 2. If player crossed the boundary, unlock the full DDA limits!
        spawnRate = normalSpawnRate;
        waveSize = normalWaveSize;
        maxCap = normalMaxCap;

        if (playerHealthScript == null) return;

        float healthPercent = (float)playerHealthScript.currentHealth / playerHealthScript.maxHealth;

        if (healthPercent < 0.3f)
        {
            spawnRate = easySpawnRate;
            waveSize = easyWaveSize;
            maxCap = easyMaxCap;
        }
        else if (healthPercent > 0.7f)
        {
            spawnRate = hardSpawnRate;
            waveSize = hardWaveSize;
            maxCap = hardMaxCap;
        }
    }

    private void SpawnEnemyWave(int waveSize, int maxCap)
    {
        if (enemyPrefab == null) return;

        int currentCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;
        if (currentCount >= maxCap) return;

        int spawnAmount = Mathf.Min(waveSize, maxCap - currentCount);

        Vector3 spawnCenter = transform.position;

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                spawnCenter.x + randomPoint.x,
                transform.position.y,
                spawnCenter.z + randomPoint.y
            );

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Red circle is where they spawn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Blue circle is the "Ambush Boundary"
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ambushActivationDistance);
    }
}