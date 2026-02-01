// Ethan Le (1/31/2026): poisonCloud.cs 
// Handles the Poison Cloud's damage, size, duration, and collisions from the Gas Mask's ability.
using UnityEngine;

public class poisonCloud : MonoBehaviour
{
    public float damagePerSecond = 5f; // Damage dealt to enemies who enter the cloud per second. 
    public float cloudDuration = 3f; // Duration the cloud remains active. 

    private void Start()
    {
        Destroy(gameObject, cloudDuration); // Auto-destroy the cloud after its duration. 
    }

    private void Awake()
    {
        CircleCollider2D cloudCollider = GetComponent<CircleCollider2D>(); // Get the component that handles collisions in the cloud. 
        SpriteRenderer cloudSprite = GetComponent<SpriteRenderer>(); // Get the component that handles the cloud's visual representation. 

        if (cloudCollider != null && cloudSprite != null) // Safety check. 
        {
            cloudCollider.radius = cloudSprite.bounds.extents.x; // Set the collider radius to match the sprite size. 
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is an enemy (AKA, enemy enters the cloud):
        enemy enemyComponent = other.GetComponent<enemy>(); // Get the enemy component from the colliding object (the colliding object is supposed to be an enemy).
        if (enemyComponent != null) // If the colliding object has an enemy component (AKA, it is an enemy): 
        {
            enemyComponent.takeDamage(damagePerSecond * Time.deltaTime); // Deal damage to the enemy over time (per second) while it stays inside the cloud. 
        }
    }
}