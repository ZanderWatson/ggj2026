using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("Melee Settings")]
    public float punchRange = 1.5f;
    public float normalPunchDamage = 2f;
    public float chargedPunchDamage = 5f;
    public float punchCooldown = 1f;
    public float chargeChance = 0.3f; // 30% chance to charge before punching.
    [System.NonSerialized] public float attackPower = 1; 

    GameObject character;
    public bool hasMask = false;
    public int enemyMaskType = 0;
    public float health;
    public float maxHealth = 100f;
    Vector2 randomPosition;
    float randomSpeed;
    float moveTimer = 3;
    float punchCooldownTimer;
    float chargeHoldTimer;
    bool isChargingPunch;
    SpriteRenderer spriteRenderer;
    // attacking variables
    [SerializeField] GameObject projectile;

    void Start()
    {
        character = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (health <= 0) health = maxHealth;
    }

    public float GetHealthPercent() => Mathf.Clamp01(health / maxHealth);
    void Update()
    {
        // Attacks
        punchCooldownTimer -= Time.deltaTime;

        // Melee: in duel phase, punch when in range
        if (game_states.duelPhase && character != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, character.transform.position);

            if (isChargingPunch)
            {
                chargeHoldTimer += Time.deltaTime;
                if (chargeHoldTimer >= 1f)
                {
                    PerformPunch(true);
                    punchCooldownTimer = punchCooldown;
                    isChargingPunch = false;
                }
                return; // Don't move while charging
            }

            if (distToPlayer <= punchRange && punchCooldownTimer <= 0)
            {
                if (Random.value < chargeChance)
                {
                    isChargingPunch = true;
                    chargeHoldTimer = 0f;
                }
                else
                {
                    PerformPunch(false);
                    punchCooldownTimer = punchCooldown;
                }
                return;
            }
        }

        
        // Movement
        Vector3 playerDirection = Vector3.zero;
        Vector3 maskDirection = Vector3.zero;
        if (game_states.prepPhase)
        {
            playerDirection = 1 * (character.transform.position - transform.position).normalized;
            // if the enemy doesnt have a mask, it will find the closest mask and have a tendency to go closer to it to pick it up
            if (!hasMask)
            {
                GameObject closestMask = null;
                float distance = float.MaxValue;
                foreach (GameObject mask in GameObject.FindGameObjectsWithTag("Mask"))
                {
                    if (distance > Vector2.Distance(mask.transform.position, transform.position))
                    {
                        closestMask = mask;
                        distance = Vector2.Distance(mask.transform.position, transform.position);
                    }
                }
                if (closestMask != null)
                {
                    maskDirection = 2 * (closestMask.transform.position - transform.position).normalized;
                }
                
            }
        } 
        else if (game_states.duelPhase)
        {
            playerDirection = 5 * (character.transform.position - transform.position).normalized;
            maskDirection = Vector3.zero;
        }
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            randomPosition = transform.position + new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), 0) + playerDirection;
            randomSpeed = Random.Range(4, 8);
            moveTimer = 3;
        }
        Vector2 beforeMove = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, randomPosition, Time.deltaTime * randomSpeed);
        Vector2 afterMove = transform.position;
        if (beforeMove.Equals(afterMove))
        {
            moveTimer = 0;
        }
    }

    void PerformPunch(bool isCharged)
    {
        if (character == null) return;

        Vector2 punchDirection = ((Vector2)character.transform.position - (Vector2)transform.position).normalized;
        Vector2 punchPosition = (Vector2)transform.position + punchDirection * (punchRange * 0.5f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(punchPosition, punchRange * 0.5f);

        float damage = (isCharged ? chargedPunchDamage : normalPunchDamage) * attackPower;

        foreach (Collider2D hit in hits)
        {
            player p = hit.GetComponent<player>();
            if (p != null)
            {
                p.takeDamage(damage);
                break;
            }
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(HurtColor());
        if (health <= 0)
        {
            game_states.OnPlayerWon(this);
            Destroy(gameObject);
        }
    }

    IEnumerator HurtColor()
    {
        spriteRenderer.color = new Color(0.98f, 0.49f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1);
    }
}
