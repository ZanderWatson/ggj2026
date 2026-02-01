using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour
{
    GameObject character;
    public bool hasMask = false;
    public int enemyMaskType = 0;
    public float health;
    Vector2 randomPosition;
    float randomSpeed;
    float moveTimer = 3;
    SpriteRenderer spriteRenderer;
    // attacking variables
    float attackSpeedTimer;
    [SerializeField] GameObject projectile;
    float projSpeed = 10;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        character = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // attacks
        if (game_states.duelPhase)
        {
            attackSpeedTimer -= Time.deltaTime;
            if (attackSpeedTimer <= 0)
            {
                attackSpeedTimer = Random.Range(0.4f, 0.6f);
                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                proj.SetActive(true);
                proj.GetComponent<Rigidbody2D>().AddForce((character.transform.position - transform.position + (Vector3) Random.insideUnitCircle).normalized * projSpeed, ForceMode2D.Impulse);
                proj.name = "enemy_basic_projectile";
                Destroy(proj, 3);
            }
        }

        // movement
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (game_states.duelPhase && collision.gameObject.name.Contains("player_basic_projectile"))
        {
            takeDamage(3);
            Destroy(collision.gameObject, 0.03f);
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(HurtColor());
        if (health <= 0)
        {
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
