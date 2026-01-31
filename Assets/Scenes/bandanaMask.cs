// Ethan Le (1/30/2026): bandanaMask.cs
// Allows player to use the Bandana Mask's ability: Shoot a special projectile with x2 Attack Power. 
using System.Collections;
using UnityEngine;

public class BandanaMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public GameObject projectilePrefab; // Assign the projectile prefab in the Unity Inspector. 
    public float projectileSpeed = 15f; // Speed the projectile is launched at. 
    public float cooldownTime = 10f;
    public float attackPowerMultiplier = 2f; // Multiplier for attack power for the mask's projectile shot. 

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
                StartCoroutine(ActivateBandanaMask()); // Start ability duration routine. 
            }

            else if (!useMouseButton && Input.GetKeyDown(abilityKey)) // Keyboard key alternative to activate ability. 
            {
                StartCoroutine(ActivateBandanaMask()); // Start ability duration routine. 
            }
        }
    }

    private IEnumerator ActivateBandanaMask()
    {
        isOnCooldown = true; // Set cooldown flag for ability. 

        // Spawn the projectile and launch it towards the mouse position: 
        Vector3 mouseWorld3D = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world position. 
        Vector2 mouseWorldPosition = new Vector2(mouseWorld3D.x, mouseWorld3D.y); // Create a 2D vector for the mouse world position. 

        // Spawn the projectile starting from player's position: 
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); // Spawn projectile starting at player's position. 
        projectile.SetActive(true);
        // Launch projectile and set damage: 
        BandanaProjectile bp = projectile.GetComponent<BandanaProjectile>(); // Get the BandanaProjectile script component. 
        if (bp != null)
        {
            bp.Launch(mouseWorldPosition, player.attackPower * attackPowerMultiplier, projectileSpeed); // Launch the projectile towards the mouse position with player's attack power. 
        }

        else // Safety check. 
        {
            Destroy(projectile); // Destroy the projectile if it lacks the BandanaProjectile script. 
            yield break; // Exit the coroutine early. 
        }

        // Start cooldown timer before ability can be used again: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 
        isOnCooldown = false; // Reset cooldown flag to allow ability to be used again. 
    }

}