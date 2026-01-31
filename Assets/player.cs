using System;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Runtime.Intrinsics.X86;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject projectile;
    [NonSerialized] public float speed;
    float projSpeed = 10;
    const float MAX_SPEED = 10;
    public static ArrayList masks = new ArrayList();
    float attackSpeedTimer;

    bool up; bool left; bool right; bool down;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 8;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        up = Input.GetKey(KeyCode.W);
        left = Input.GetKey(KeyCode.A);
        down = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        // Ability
        attackSpeedTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && attackSpeedTimer < 0) {
            attackSpeedTimer = 0.5f;
        if (Input.GetMouseButtonDown(0))
        {
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
            rb.linearVelocity += Vector2.up;
        }
        if (left)
        {
            rb.linearVelocity += Vector2.left;
        }
        if (down)
        {
            rb.linearVelocity += Vector2.down;
        }
        if (right)
        {
            rb.linearVelocity += Vector2.right;
        }
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MAX_SPEED);
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, 0.5f);
    }
}
