using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour
{
    GameObject character;
    public float health;
    Vector2 randomPosition;
    float randomSpeed;
    float moveTimer = 3;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        character = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector3 playerDirection = Vector3.zero;
        if (game_states.prepPhase)
        {
            playerDirection = 2 * (character.transform.position - transform.position).normalized;
        } 
        else if (game_states.duelPhase)
        {
            playerDirection = 5 * (character.transform.position - transform.position).normalized;
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
        if (game_states.duelPhase && collision.gameObject.layer == 6)
        {
            takeDamage(15);
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
