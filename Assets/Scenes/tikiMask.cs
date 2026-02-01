// Ethan Le (1/31/2026): tikiMask.cs
// Allows player to use the Tiki Mask's ability: Shoot 4 special projectiles with x1 Attack Power. 
using System.Collections;
using UnityEngine;

public class TikiMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public GameObject projectilePrefab; // Assign the projectile prefab in the Unity Inspector. 
    public float projectileSpeed = 10f; // Speed the projectile is launched at. 
    public float cooldownTime = 5f;

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = true; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    public GameObject maskObject;
    private Camera mainCamera; // Convert the mouse position to game world position. 
    private player player;

    void Start()
    {
        player = GetComponent<player>(); // Get the player.cs script component. 
        if (player == null) // Safety check. 
        {
            return;
        }

        mainCamera = Camera.main; // Cache the main camera reference. 
        if (mainCamera == null) // Safety check. 
        {
            return;
        }
    }

    void Update()
    {
        if (!isOnCooldown) // Ability is ready to use. 
        {
            if (useMouseButton && Input.GetMouseButtonDown(1)) // Right mouse click to activate ability. 
            {
                StartCoroutine(ActivateTikiMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateTikiMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateTikiMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 
        int level = player.GetMaskLevel(7); // Tiki: Type 7. 
        float mult = player.attackPower + (level - 1) * 0.2f; // +0.2x Projectile Attack Power per level. 

        // Get player facing vector:
        Vector2 facingVector = player.velocity.normalized;

        if (facingVector == Vector2.zero) // If the player is not moving, 
        {
            facingVector = Vector2.up; // default to facing "up". 
        }

        // Define the directions for the 4 projectiles to be fired towards:
        Vector2[] directions =
        {
            facingVector, // Forward. 
            Quaternion.Euler(0, 0, 90) * facingVector, // Right. 
            Quaternion.Euler(0, 0, 180) * facingVector, // Backward. 
            Quaternion.Euler(0, 0, -90) * facingVector, // Left. 
        };

        foreach (Vector2 direction in directions) // For each of the four directions, 
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); // spawn projectile starting at player's position. 

            Projectile projScript = projectile.GetComponent<Projectile>(); // Get the Projectile component of the projectile. 

            if (projScript != null) // Safety check. 
            {
                Debug.Log("Damage dealt: " + player.chargedPunchDamage * mult); 
                // Launch the projectile in the specified direction with dragonfly's attack power: 
                projScript.LaunchDirection(direction, player.chargedPunchDamage * mult, projectileSpeed); // Consistent speed in specified direction. 
            }
        }

        // Start cooldown timer before ability can be used again: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 
        isOnCooldown = false; // Reset cooldown flag to allow ability to be used again. 
    }

}