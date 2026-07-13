using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Time in seconds between each shot. Lower means rapid fire!")]
    [SerializeField] private float fireRate = 0.15f;

    private float nextFireTime = 0f;

    void Update()
    {
        // Input.GetKey allows continuous firing while holding the button down
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();

            // Set the timer for the next allowed shot
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Please assign the PlayerBullet prefab to the PlayerShooting script component!");
            return;
        }

        // Spawn the bullet slightly ahead of the mosquito (positive Z axis) 
        // so it doesn't look like it's emerging from inside the body.
        Vector3 spawnPosition = transform.position + new Vector3(0f, 0f, 0.8f);

        // Spawn the bullet into the game world
        Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }
}