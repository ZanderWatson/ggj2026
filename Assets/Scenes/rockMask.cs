// Ethan Le (1/30/2026): rockMask.cs
// Allows player to use the Rock Mask's ability: temporary invincibility with reduced speed. 
using System.Collections;
using UnityEngine;

public class RockMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float abilityDuration = 4f;
    public float speedMultiplier = 0.5f;
    public float cooldownTime = 10f;

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = true; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    private player player;
    private float originalSpeed;

    public GameObject maskObject;

    void Start()
    {
        player = GetComponent<player>(); // Get the player.cs script component. 
        originalSpeed = player.maxSpeed; // Assign OG player speed to revert speed back later. 
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
        isOnCooldown = true;
        int level = player.GetMaskLevel(1); // Rock = type 1
        float duration = abilityDuration + (level - 1) * 0.5f; // +0.5s per level. 

        // Enable mask effects: 
        player.maxSpeed *= speedMultiplier; // Reduce player speed. 
        player.isInvincible = true; // Set player's invincibility flag to true. 

        // Particle System activate
        ParticleSystem playerParticles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        var main = playerParticles.main;
        main.startColor = Color.brown;
        main.duration = 4;
        playerParticles.Play();

        yield return new WaitForSeconds(duration); 

        // Disable mask effects after invincibility duration:
        player.maxSpeed = originalSpeed;
        player.isInvincible = false;

        // Start cooldown timer before ability can be used again:: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}