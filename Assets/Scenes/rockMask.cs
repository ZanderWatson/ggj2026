// Ethan Le (1/30/2026): rockMask.cs
// Allows player to use the Rock Mask's ability: temporary invincibility with reduced speed. 
public class RockMask : MonoBehaviour
{
    [Header("Ability Settings")]
    public float invincibilityDuration = 4f;
    public float speedMultipler = 0.5f;
    public float cooldownTime = 10f;

    [Header("Activation Key")]
    public KeyCode abilityKey = KeyCode.E; // In case players do not want to use the mouse. 
    public bool useMouseButton = false; // Toggle for mouse input. 

    private bool isOnCooldown = false;
    private Player player;
    private float originalSpeed;

    public GameObject maskObject;

    void Start()
    {
        player = GetComponent<Player>(); // Get the player.cs script component. 
        originalSpeed = player.speed; // Assign OG player speed to revert speed back later. 
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
        player.isInvincible = true; // Set player's invincibility flag to true. 

        // Wait for duration of invincibility before continuing code execution: 
        yield return new WaitForSeconds(invincibilityDuration); // Pauses code execution for set duration. 

        // Disable mask effects after invincibility duration:
        player.speed = originalSpeed;
        player.isInvincible = false;

        // Start cooldown timer before ability can be used again:: 
        yield return new WaitForSeconds(cooldownTime); // Pauses code execution for cooldown duration. 

        isOnCooldown = false; // Allow ability to be used again. 
    }
}