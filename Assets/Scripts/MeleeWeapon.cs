using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [Tooltip("Massive damage to reward getting close!")]
    public int comboDamage = 5;

    [Tooltip("Prevents the game from dealing damage 60 times a second")]
    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    // OnTriggerStay checks every frame to see if an enemy is inside the Box Collider
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if the player is currently mashing the attack buttons
            bool rightClick = Mouse.current != null && Mouse.current.rightButton.isPressed;
            bool spaceBar = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

            // If attacking AND the cooldown timer is finished
            if ((rightClick || spaceBar) && Time.time >= nextAttackTime)
            {
                // Find the enemy's health script and obliterate them!
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(comboDamage);

                    // Reset the cooldown timer
                    nextAttackTime = Time.time + attackCooldown;

                    Debug.Log("CRITICAL HIT! Combo attack landed for " + comboDamage + " damage!");
                }
            }
        }
    }
}