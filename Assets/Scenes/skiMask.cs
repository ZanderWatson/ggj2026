// Ethan Le (1/30/2026): skiMask.cs
// Allows player to use the Ski Mask's ability: 1.5x speed but reduces attack power by 50%. 
using System.Collections;
using UnityEngine;

public class RockMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float abilityDuration = 4f;
    public float speedMultiplier = 1.5f;
    public float cooldownTime = 10f;
    public float attackPower = 0.5f; // Reduce attack power by half. 

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = false; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    private player player;
    private float originalSpeed;

    public GameObject maskObject;

    void Start()
    {
        player = GetComponent<player>(); // Get the player.cs script component. 
        originalSpeed = player.speed; // Assign OG player speed to revert speed back later. 
        originalAttackPower = player.attackPower; // Assign OG attack power to revert back later. 
    }

    void Update()
    {
        if (!isOnCooldown) // Ability is ready to use. 
        {
            if (useMouseButton && Input.GetMouseButtonDown(1)) // Right mouse click to activate ability. 
            {
                StartCoroutine(ActivateRockMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateRockMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateRockMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 

        // Enable mask effects: 
        player.speed *= speedMultiplier; // Reduce player speed. 
        player.attackPower *= attackPower; // Reduce player's attack power. 

        // Wait for duration of invincibility before continuing code execution: 
        yield return new WaitForSeconds(abilityDuration); // Pauses code execution for set duration. 

        // Disable mask effects after invincibility duration:
        player.speed = originalSpeed;
        player.attackPower = originalAttackPower; // Revert attack power back to original. 

        // Start cooldown timer before ability can be used again:: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}