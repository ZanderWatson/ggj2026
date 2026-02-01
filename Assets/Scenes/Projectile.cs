// Ethan Le (1/30/2026): Projectile.cs 
// Handles projectile movement and damage for the Bandana and Spa Mask abilities. 
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 15; // Damage this projectile will deal. 
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

    // Launch direction property:
    public void LaunchDirection(Vector2 direction, float attackPower, float projectileSpeed)
    {
        damage = attackPower; // Set projectile damage. 
        rb.linearVelocity = direction.normalized * projectileSpeed; // Set Rigidbody2D velocity to move projectile with consistent speed. 
        Destroy(gameObject, 4f); // Destroy projectile after 4 seconds to prevent lingering. 
    }
}