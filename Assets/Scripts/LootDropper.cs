using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public GameObject coinPrefab;
    [Range(0f, 100f)] public float dropChance = 100f;

    private bool isQuitting = false;
    void OnApplicationQuit() => isQuitting = true;

    void OnDestroy()
    {
        // We just spawn the coin. The new SmartCoin script handles everything else!
        if (!isQuitting && gameObject.scene.isLoaded && coinPrefab != null)
        {
            if (Random.Range(0f, 100f) <= dropChance)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}