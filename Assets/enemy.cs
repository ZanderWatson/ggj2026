using UnityEngine;

public class enemy : MonoBehaviour
{
    GameObject character;
    public float health;
    Vector2 randomPosition;
    float randomSpeed;
    float moveTimer = 3;

    void Start()
    {
        character = GameObject.Find("Player");
    }
    void Update()
    {
        if (game_states.prepPhase)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                randomPosition = transform.position + new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), 0) + (character.transform.position - transform.position).normalized * 2;
                randomSpeed = Random.Range(4, 8);
                moveTimer = 3;
            }
        }
        Vector2 beforeMove = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, randomPosition, Time.deltaTime * randomSpeed);
        Vector2 afterMove = transform.position;
        if (beforeMove.Equals(afterMove))
        {
            moveTimer = -1;
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            game_states.prepPhase = true;
            game_states.duelPhase = false;
        }
    }
}
