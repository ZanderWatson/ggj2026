using System;
using System.Collections;
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
        if (Input.GetMouseButtonDown(0)) {
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
            rb.AddForceY(speed, ForceMode2D.Force);
        }
        if (left)
        {
            rb.AddForceX(-speed, ForceMode2D.Force);
        }
        if (down)
        {
            rb.AddForceY(-speed, ForceMode2D.Force);
        }
        if (right)
        {
            rb.AddForceX(speed, ForceMode2D.Force);
        }
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MAX_SPEED);
        
    }
}
