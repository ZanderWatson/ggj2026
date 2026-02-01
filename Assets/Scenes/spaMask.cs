// Ethan Le (1/31/2026): spaMask.cs
// Allows player to use the Spa Mask's ability: x2 speed but damage taken is doubled. 
using System.Collections;
using UnityEngine;

public class SpaMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float abilityDuration = 4f;
    public float speedMultiplier = 2f;
    public float spaDamageTakenMultiplier = 2f;
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
        originalSpeed = player.speed; // Assign OG player speed to revert speed back later. 
    }

    void Update()
    {
        if (!isOnCooldown) // Ability is ready to use. 
        {
            if (useMouseButton && Input.GetMouseButtonDown(1)) // Right mouse click to activate ability. 
            {
                StartCoroutine(ActivateSpaMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateSpaMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateSpaMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 
        int level = player.GetMaskLevel(5); // Spa: Type 5. 
        float coolTime = cooldownTime - (level - 1) * 0.5f; // -0.5s cooldown per level. 

        // Enable mask effects: 
        player.speed *= speedMultiplier; // Increase player speed. 
        player.damageTakenMultiplier *= spaDamageTakenMultiplier; // Increase damage taken. 

        // Particle System activate
        ParticleSystem playerParticles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        var main = playerParticles.main;
        main.startColor = Color.skyBlue;
        main.duration = 4;
        playerParticles.Play();

        // Wait for duration of speed increase before continuing code execution: 
        yield return new WaitForSeconds(abilityDuration); // Pauses code execution for set duration. 

        // Disable mask effects after speed increase duration:
        player.speed = originalSpeed;
        player.damageTakenMultiplier = 1f;

        // Start cooldown timer before ability can be used again:
        yield return new WaitForSeconds(coolTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}