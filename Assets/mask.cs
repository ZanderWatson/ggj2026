using UnityEditor.Rendering;
using UnityEngine;

public class mask : MonoBehaviour
{
    public GameObject character;
    Rigidbody2D rb;
    bool followingPlayer = false;
    int maskType;
    void Start()
    {
        character = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        maskType = Random.Range(1, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(character.transform.position, transform.position) < 1.5f)
        {
            followingPlayer = true;
        } 
        if (Vector2.Distance(character.transform.position, transform.position) < 0.2f)
        {
            player.masks.Add(maskType);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (followingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.transform.position, 0.5f);
        }
    }
}
