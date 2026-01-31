// Ethan Le (1/30/2026): bandanaProjectile.cs 
// Handles projectile movement and damage for the Bandana Mask ability. 
using UnityEngine;

public class BandanaProjectile : MonoBehaviour
{
    private float damage; // Damage this projectile will deal. 
    private Vector2 target; // Target position to move toward. 
    private float speed; // Projectile speed. 
    private Rigidbody2D rb; // Rigidbody2D component reference for physics movement. 

    void Awake() // Called when the script instance is being loaded. 
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component. 
        if (rb == null) // Safety check. 
        {
            return;
        }
    }

    // Launch projectile toward a target with given damage and speed. 
    public void Launch(Vector2 targetPosition, float attackPower, float projectileSpeed)
    {
        damage = attackPower; // Set projectile damage. 
        target = targetPosition; // Set target position. 
        speed = projectileSpeed; // Set projectile speed. 

        // Calculate direction and apply velocity: 
        Vector2 direction = (target - (Vector2)transform.position).normalized; // Direction vector from current position to target. 
        rb.linearVelocity = direction * speed; // Set Rigidbody2D velocity to move projectile. 

        // Destroy projectile after 4 seconds to prevent lingering: 
        Destroy(gameObject, 4f);
    }

    // Handle collision with enemies or environment: 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only interact with enemies (tagged as "Enemy"):
        if (collision.CompareTag("Enemy"))
        {
            enemy enemy = collision.GetComponent<enemy>(); // Get the Enemy script component. 
            if (enemy != null)
            {
                enemy.takeDamage(damage); // Apply damage to the enemy. 
            }
            Destroy(gameObject); // Destroy projectile on impact. 
        }

        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject); // Destroy projectile on impact with environment. 
        }
    }
}