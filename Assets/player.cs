using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject projectile;
    [NonSerialized] public float speed;
    [NonSerialized] public bool isInvincible = false;
    [NonSerialized] public float attackPower = 1;
    float health;
    float projSpeed = 10;
    Vector2 velocity = Vector2.zero;
    float acceleration = 1;
    const float MAX_SPEED = 4;
    public static ArrayList masks = new ArrayList();
    float attackSpeedTimer;

    bool up; bool left; bool right; bool down;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 8;
        health = 100;
    }

    void Update()
    {
        // Movement
        up = Input.GetKey(KeyCode.W);
        left = Input.GetKey(KeyCode.A);
        down = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        // Shooting
        attackSpeedTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && attackSpeedTimer <= 0)
        {
            attackSpeedTimer = 0.25f;
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.SetActive(true);
            proj.GetComponent<Rigidbody2D>().AddForce((mouse - new Vector2(transform.position.x, transform.position.y)).normalized * projSpeed, ForceMode2D.Impulse);
            Destroy(proj, 3);
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
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        velocity = Vector2.MoveTowards(velocity, Vector2.zero, 0.5f);
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3) velocity * Time.fixedDeltaTime, velocity.magnitude * Time.fixedDeltaTime);
    }

    public void takeDamage(float amount)
    {
        if (!isInvincible)
        {
            health -= amount;
        }
        if (health <= 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
