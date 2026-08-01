using UnityEngine;
using UnityEngine.InputSystem; // Need this for the New Input System!

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("Where the bullet spawns (e.g., the mosquito's stinger)")]
    [SerializeField] private Transform firePoint;

    [Tooltip("How fast the mosquito shoots (seconds between bullets)")]
    [SerializeField] private float fireRate = 0.15f;

    private float nextFireTime = 0f;

    void Update()
    {
        // Sky Force style: Auto-fire continuously while touching the screen / clicking mouse
        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                // Set the cooldown timer for the next bullet
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Spawn the bullet
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Trigger your Week 11 Audio System to play the shoot sound!
            GameEvents.OnPlayerFired?.Invoke();
        }
        else
        {
            Debug.LogWarning("Missing Projectile Prefab or Fire Point in PlayerShooting script!");
        }
    }
}