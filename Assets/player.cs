using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public GameObject projectile;
    [NonSerialized] public float speed;
    [NonSerialized] public bool isInvincible = false;
    [NonSerialized] public float damageTakenMultiplier = 1;
    [NonSerialized] public float attackPower = 1;
    public float health;
    float projSpeed = 10;
    public Vector2 velocity = Vector2.zero;
    float acceleration = 1;
    const float MAX_SPEED = 4;
    public static List<int> maskInventory = new List<int>();
    public Image mask1; public Image mask2; public Image mask3;
    public static List<Image> maskImages = new List<Image>();
    public Sprite rockMask; public Sprite skiMask; public Sprite bandanaMask; public Sprite hospitalMask; public Sprite spaMask;
    public Sprite gasMask; public Sprite tikiMask;
    List<GameObject> enemies;
    float attackSpeedTimer;

    bool up; bool left; bool right; bool down;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        speed = 8;
        health = 100;
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        maskImages.Add(mask1); maskImages.Add(mask2); maskImages.Add(mask3);
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
        // Press F key to challenge and enemy
        if (Input.GetKeyDown(KeyCode.F))
        {
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
            if (minDistanceToEnemy <= 1)
            {
                game_states.SwitchPhases();
                foreach (GameObject enemy in enemies)
                {
                    if (!enemy.Equals(closestEnemy))
                    {
                        enemy.SetActive(false);
                    }
                }
            }
        }
        // Manage inventory
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
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)velocity * Time.fixedDeltaTime, velocity.magnitude * Time.fixedDeltaTime);
    }

    public void takeDamage(float amount)
    {
        if (!isInvincible)
        {
            health -= amount * damageTakenMultiplier;
        }
        if (health <= 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
