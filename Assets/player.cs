using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    [Header("Melee Settings")]
    public float punchRange = 1.5f;
    public float normalPunchDamage = 3f;
    public float chargedPunchDamage = 6f;
    public float chargeTimeRequired = 1f;
    public float normalPunchCooldown = 0.3f;
    public float chargedPunchCooldown = 0.5f;
    Vector2 punchDirection;
    [NonSerialized] public bool isInvincible = false;
    [NonSerialized] public float damageTakenMultiplier = 1;
    [NonSerialized] public float attackPower = 1;
    float health;
    Image healthBarFill;
    // Mask inventory variables
    public static int activeMask = 0;
    public static List<int> maskInventory = new List<int>();
    public static List<int> maskLevels = new List<int> { 1, 1, 1 }; // Default mask levels are 1. 
    public Image mask1; public Image mask2; public Image mask3;
    public static List<Image> maskImages = new List<Image>();
    public Sprite rockMask; public Sprite skiMask; public Sprite bandanaMask; public Sprite hospitalMask; public Sprite spaMask;
    public Sprite gasMask; public Sprite tikiMask;
    // Attacking variables
    List<GameObject> enemies;
    float punchCooldownTimer;
    float chargeHoldTimer;
    bool isChargingPunch;
    public GameObject punchSprite;
    public Animator punchAnimator;
    public bool isPunching = false;
    // Movement variables
    [NonSerialized] public float speed;
    public GameObject playerSprite;
    public Vector2 velocity = Vector2.zero;
    float acceleration = 1;
    public float maxSpeed = 6;
    bool up; bool left; bool right; bool down;
    public GameObject circle;
    void Start()
    {
        circle = GameObject.Find("punch debug");
        speed = 8;
        health = 100;
        healthBarFill = GameObject.Find("Health Bar Fill").GetComponent<Image>();
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        maskImages.Add(mask1); maskImages.Add(mask2); maskImages.Add(mask3);
        while (maskLevels.Count < 3) maskLevels.Add(1);
    }

    void Update()
    {
        // Movement
        up = Input.GetKey(KeyCode.W);
        left = Input.GetKey(KeyCode.A);
        down = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        // Melee (punch) - tap for normal, hold ~1 sec for charged punch
        punchCooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && punchCooldownTimer <= 0)
        {
            isChargingPunch = true;
            chargeHoldTimer = 0f;
        }

        if (isChargingPunch)
        {
            chargeHoldTimer += Time.deltaTime;
            if (Input.GetMouseButtonUp(0))
            {
                bool wasCharged = chargeHoldTimer >= chargeTimeRequired;
                PerformPunch(wasCharged);
                punchCooldownTimer = wasCharged ? chargedPunchCooldown : normalPunchCooldown;
                isChargingPunch = false;
            }
        }

        // Press F key to challenge an enemy
        if (Input.GetKeyDown(KeyCode.F) && game_states.prepPhase)
        {
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            float minDistanceToEnemy = float.MaxValue;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistanceToEnemy)
                {
                    minDistanceToEnemy = distance;
                    closestEnemy = enemy;
                }
            }
            if (minDistanceToEnemy <= 3)
            {
                game_states.currentBattleEnemy = closestEnemy;
                game_states.hiddenEnemies.Clear();
                choose_mask.chooseMaskForDuel();
                foreach (GameObject enemy in enemies)
                {
                    if (!enemy.Equals(closestEnemy))
                    {
                        enemy.SetActive(false);
                        game_states.hiddenEnemies.Add(enemy);
                    }
                }
            }
        }
        // Press 1, 2, or 3 to get rid of the mask in that slot
        if (Input.GetKeyDown(KeyCode.Alpha1) && game_states.prepPhase)
        {
            if (maskInventory.Count > 0)
            {
                maskInventory.RemoveAt(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && game_states.prepPhase)
        {
            if (maskInventory.Count > 1)
            {
                maskInventory.RemoveAt(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && game_states.prepPhase)
        {
            if (maskInventory.Count > 2)
            {
                maskInventory.RemoveAt(2);
            }
        }
        // Update inventory
        for (int i = 2; i >= maskInventory.Count; i--)
        {
            maskImages[i].color = Color.clear;
        }
        for (int i = 0; i < maskInventory.Count; i++)
        {
            maskImages[i].color = Color.white;
            if (maskInventory[i] == 0)
            {
                maskImages[i].color = Color.clear;
            }
            else if (maskInventory[i] == 1)
            {
                maskImages[i].sprite = rockMask;
            }
            else if (maskInventory[i] == 2)
            {
                maskImages[i].sprite = skiMask;
            }
            else if (maskInventory[i] == 3)
            {
                maskImages[i].sprite = bandanaMask;
            }
            else if (maskInventory[i] == 4)
            {
                maskImages[i].sprite = hospitalMask;
            }
            else if (maskInventory[i] == 5)
            {
                maskImages[i].sprite = spaMask;
            }
            else if (maskInventory[i] == 6)
            {
                maskImages[i].sprite = gasMask;
            }
            else if (maskInventory[i] == 7)
            {
                maskImages[i].sprite = tikiMask;
            }
        }
        // Fix sprite direction when attacking
        if (isPunching)
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            punchDirection = (mouseWorld - (Vector2)transform.position).normalized;
            if (punchDirection == Vector2.zero) punchDirection = velocity.normalized != Vector2.zero ? velocity.normalized : Vector2.up;
            float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;
            playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
        // return to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    private void FixedUpdate()
    {
        // Movement
        if (up)
        {
            velocity += Vector2.up * acceleration;
        }
        if (left)
        {
            velocity += Vector2.left * acceleration;
        }
        if (down)
        {
            velocity += Vector2.down * acceleration;
        }
        if (right)
        {
            velocity += Vector2.right * acceleration;
        }
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        velocity = Vector2.MoveTowards(velocity, Vector2.zero, 0.5f);
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)velocity * Time.fixedDeltaTime, velocity.magnitude * Time.fixedDeltaTime);
        // face towards velocity direction
        if (!velocity.Equals(Vector3.zero) && !isPunching)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("enemy_basic_projectile"))
        {
            takeDamage(5);
            StartCoroutine(HurtColor());
            Destroy(collision.gameObject, 0.03f);
        }
    }

    // Perform a punch attack (mouse left-click):
    void PerformPunch(bool isCharged)
    {
        if (game_states.duelPhase == false) return; // Only punch during battle

        Vector2 punchPosition = (Vector2)transform.position + punchDirection * (punchRange * 0.5f);
        StartCoroutine(ShowPunchAnim(punchDirection, isCharged));
    }
    IEnumerator ShowPunchAnim(Vector2 direction, bool isCharged)
    {
        punchAnimator.SetTrigger("Attack");
        isPunching = true;
        yield return new WaitForSeconds(0.33f);
        // Hitbox: circle in front of player
        Vector2 punchPosition = (Vector2)transform.position + punchDirection * (punchRange * 0.5f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(punchPosition, punchRange * 0.5f);
        float damage = (isCharged ? chargedPunchDamage : normalPunchDamage) * attackPower;

        Debug.Log("PUNCH DAMAGE: " + damage); 

        foreach (Collider2D hit in hits)
        {
            enemy e = hit.GetComponent<enemy>();
            if (e != null)
            {
                e.takeDamage(damage);
            }
        }
        yield return new WaitForSeconds(0.33f);
        isPunching = false;
    }

    public static int GetMaskLevel(int maskType)
    {
        for (int i = 0; i < maskInventory.Count; i++)
        {
            if (maskInventory[i] == maskType)
            {
                return i < maskLevels.Count ? maskLevels[i] : 1;
            }
        }
        return 1;
    }

    public void takeDamage(float amount)
    {
        if (!isInvincible && amount > 0)
        {
            Debug.Log("Damage taken by enemy: " + (amount * damageTakenMultiplier));
            health -= amount * damageTakenMultiplier;
        }
        if (amount < 0)
        {
            health -= amount;
        }
        health = Mathf.Clamp(health, 0, 100);
        if (healthBarFill != null) 
        {
            healthBarFill.fillAmount = Mathf.Clamp(health / 100, 0, 1);
        }
        if (health <= 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void RestoreFullHealth() // Method to restore player's HP after each fight (every fight starts anew). 
    {
        health = 100;
        if (healthBarFill != null) 
        {
            healthBarFill.fillAmount = 1f;
        }
    }
    IEnumerator HurtColor()
    {
        SpriteRenderer sprite = playerSprite.GetComponent<SpriteRenderer>();
        sprite.color = new Color(0.98f, 0.49f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1);
    }
}
