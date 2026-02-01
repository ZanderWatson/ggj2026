// Ethan Le (1/31/2026): gasMask.cs
// Allows player to use the Gas Mask's ability: place a poison cloud that damages enemies who enter it. 
using System.Collections;
using UnityEngine;

public class GasMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float cooldownTime = 10f;
    public GameObject poisonCloudPrefab; // Prefab of the poison cloud to instantiate. 

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
                StartCoroutine(ActivateGasMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateGasMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateGasMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 

        // Enable mask effects: 
        Instantiate(poisonCloudPrefab, player.transform.position, Quaternion.identity); // Spawn poison cloud at player's position. 

        // Start cooldown timer before ability can be used again:: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}