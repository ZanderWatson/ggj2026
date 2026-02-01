// Ethan Le (1/30/2026): Projectile.cs 
// Handles projectile movement, damage, and projectile-vs-projectile cancellation.
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private Vector2 target;
    private float speed;
    private Rigidbody2D rb;
    private bool isPlayerProjectile = true; // Default: player shot it. Enemy masks pass false.

    void Awake() // Called when the script instance is being loaded.
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component. 
    }

    // Launch toward target (player aim)
    public void Launch(Vector2 targetPosition, float attackPower, float projectileSpeed, bool fromPlayer = true)
    {
        damage = attackPower; // Set projectile damage. 
        target = targetPosition; // Set target position. 
        speed = projectileSpeed; // Set projectile speed. 
        isPlayerProjectile = fromPlayer; // Set projectile type. 

        // Calculate direction and apply velocity: 
        Vector2 direction = (target - (Vector2)transform.position).normalized; // Direction vector from current position to target. 
        rb.linearVelocity = direction * speed; // Set Rigidbody2D velocity to move projectile. 
        Destroy(gameObject, 4f); // Destroy projectile after 4 seconds to prevent lingering. 
    }

    // Launch in a direction (e.g. Tiki mask 4-way shot)
    public void LaunchDirection(Vector2 direction, float attackPower, float projectileSpeed, bool fromPlayer = true)
    {
        damage = attackPower; // Set projectile damage. 
        isPlayerProjectile = fromPlayer; // Set projectile type. 
        rb.linearVelocity = direction.normalized * projectileSpeed; // Set Rigidbody2D velocity to move projectile with consistent speed. 
        Destroy(gameObject, 4f); // Destroy projectile after 4 seconds to prevent lingering. 
    }

    // Handle projectile vs projectile cancellation and damage. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerProjectile)
        {
            enemy e = collision.GetComponent<enemy>();
            if (e != null)
            {
                e.takeDamage(damage);
                Destroy(gameObject);
            }
        }
        
        /*
        // Projectile vs projectile: cancel both (not included currently)
        Projectile otherProj = collision.GetComponent<Projectile>();
        if (otherProj != null)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            return;
        }

        else
        {
            player p = collision.GetComponent<player>();
            if (p != null)
            {
                p.takeDamage(damage);
                Destroy(gameObject);
            }
        }
        */
    }
}