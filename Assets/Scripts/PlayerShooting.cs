using UnityEngine;
using UnityEngine.InputSystem; // Need this for the New Input System!

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("Where the bullet spawns (e.g., the mosquito's stinger)")]
    [SerializeField] private Transform firePoint;

    [Tooltip("The starting fire rate before any upgrades are bought!")]
    [SerializeField] private float baseFireRate = 0.15f;

    // We will store the upgraded speed here
    private float currentFireRate;

    private float nextFireTime = 0f;

    void Start()
    {
        // 1. READ THE UPGRADE: Check memory for an upgraded fire rate. 
        // If the player hasn't bought any yet, it safely defaults to your baseFireRate (0.15).
        currentFireRate = PlayerPrefs.GetFloat("PlayerFireRate", baseFireRate);
    }

    void Update()
    {
        // Sky Force style: Auto-fire continuously while touching the screen / clicking mouse
        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();

                // 2. Set the cooldown timer using the UPGRADED fire rate, not the base one!
                nextFireTime = Time.time + currentFireRate;
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