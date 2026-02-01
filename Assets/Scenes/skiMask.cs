// Ethan Le (1/30/2026): skiMask.cs
// Allows player to use the Ski Mask's ability: 1.5x speed but reduces attack power by 25%. 
using System.Collections;
using UnityEngine;

public class SkiMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float abilityDuration = 4f;
    public float speedMultiplier = 1.5f;
    public float cooldownTime = 7f;
    public float attackPower = 0.75f; // Reduce attack power by half. 

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = true; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    private player player;
    private float originalSpeed;
    private float originalAttackPower;

    public GameObject maskObject;

    void Start()
    {
        player = GetComponent<player>(); // Get the player.cs script component. 
        originalSpeed = player.maxSpeed; // Assign OG player speed to revert speed back later. 
        originalAttackPower = player.attackPower; // Assign OG attack power to revert back later. 
    }

    void Update()
    {
        if (!isOnCooldown) // Ability is ready to use. 
        {
            if (useMouseButton && Input.GetMouseButtonDown(1)) // Right mouse click to activate ability. 
            {
                StartCoroutine(ActivateSkiMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateSkiMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateSkiMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 
        int level = player.GetMaskLevel(2); // Ski = Type 2. 
        float spMultiplier = speedMultiplier + (level - 1) * 0.2f; // +0.2x speed per level. 

        // Enable mask effects: 
        player.maxSpeed *= spMultiplier; // Increase player speed. 
        player.attackPower *= attackPower; // Reduce player's attack power. 
        Debug.Log("Fast speed: " + player.maxSpeed); 

        // Particle System activate
        ParticleSystem playerParticles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        var main = playerParticles.main;
        main.startColor = Color.black;
        main.duration = 4;
        playerParticles.Play();

        // Wait for duration of invincibility before continuing code execution: 
        yield return new WaitForSeconds(abilityDuration); // Pauses code execution for set duration. 

        // Disable mask effects after invincibility duration:
        player.maxSpeed = originalSpeed;
        player.attackPower = originalAttackPower; // Revert attack power back to original. 

        // Start cooldown timer before ability can be used again:: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}