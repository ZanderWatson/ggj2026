// Ethan Le (1/31/2026): hospitalMask.cs
// Allows player to use the Hospital Mask's ability: restores HP. 
using System.Collections;
using UnityEngine;

public class HospitalMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float cooldownTime = 10f;
    public float hpRestore = 20f; // Amount of HP to restore.

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = true; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    private player player;

    public GameObject maskObject;

    void Start()
    {
        player = GetComponent<player>(); // Get the player.cs script component. 
    }

    void Update()
    {
        if (!isOnCooldown) // Ability is ready to use. 
        {
            if (useMouseButton && Input.GetMouseButtonDown(1)) // Right mouse click to activate ability. 
            {
                StartCoroutine(ActivateHospitalMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateHospitalMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateHospitalMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 
        int level = player.GetMaskLevel(4); // Hospital = type 4
        float heal = hpRestore + (level - 1) * 5f; // +5 HP per level
        // Particle System activate
        ParticleSystem playerParticles = GameObject.Find("Particles").GetComponent<ParticleSystem>();
        var main = playerParticles.main;
        main.startColor = Color.lightGreen;
        main.duration = 2;
        playerParticles.Play();
        // Enable mask effects: 
        player.takeDamage(-heal); // Restore HP (negative damage). 

        // Start cooldown timer before ability can be used again:
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}