using System.Linq;
using UnityEngine;

public class mask : MonoBehaviour
{
    public GameObject character;
    SpriteRenderer spriteRenderer;
    GameObject enemyToFollow;
    bool followingEnemy = false;
    bool followingPlayer = false;
    int maskType;
    public Sprite rockMask; public Sprite skiMask; public Sprite bandanaMask; public Sprite hospitalMask; public Sprite spaMask;
    public Sprite gasMask; public Sprite tikiMask;
    void Start()
    {
        character = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        maskType = Random.Range(1, 8);
        if (maskType == 1) spriteRenderer.sprite = rockMask;
        else if (maskType == 2) spriteRenderer.sprite = skiMask;
        else if (maskType == 3) spriteRenderer.sprite = bandanaMask;
        else if (maskType == 4) spriteRenderer.sprite = hospitalMask;
        else if (maskType == 5) spriteRenderer.sprite = spaMask;
        else if (maskType == 6) spriteRenderer.sprite = gasMask;
        else if (maskType == 7) spriteRenderer.sprite = tikiMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (game_states.duelPhase)
        {
            Destroy(gameObject);
        }
        // track player if they are close and inventory isn't full
        if (Vector2.Distance(character.transform.position, transform.position) < 0.2f && player.maskInventory.Count < 3)
        {
            player.maskInventory.Add(maskType);
            if (player.maskLevels.Count < player.maskInventory.Count) player.maskLevels.Add(1);
            Destroy(gameObject);
        }
        // Enemy picks up mask
        if (enemyToFollow != null)
        {
            enemy enemyToFollowScript = enemyToFollow.GetComponent<enemy>();
            if (Vector2.Distance(enemyToFollow.transform.position, transform.position) < 0.2f && !enemyToFollowScript.hasMask)
            {
                enemyToFollowScript.enemyMaskType = maskType;
                enemyToFollowScript.hasMask = true;
                Destroy(gameObject);
            }
        }
        

    }
    private void FixedUpdate()
    {
        if (followingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.transform.position, 0.5f);
        }
        if (followingEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyToFollow.transform.position, 0.5f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && player.maskInventory.Count < 3)
        {
            followingPlayer = true;
        }
        if (collision.gameObject.layer == 7 && !collision.gameObject.GetComponent<enemy>().hasMask)
        {
            enemyToFollow = collision.gameObject;
            followingEnemy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && player.maskInventory.Count < 3)
        {
            followingEnemy = false;
        }
        if (collision.gameObject.layer == 7 && !collision.gameObject.GetComponent<enemy>().hasMask)
        {
            enemyToFollow = collision.gameObject;
            followingEnemy = false;
        }
    }
}
