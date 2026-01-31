using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class mask : MonoBehaviour
{
    public GameObject character;
    SpriteRenderer spriteRenderer;
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
        if (Vector2.Distance(character.transform.position, transform.position) < 1.5f && player.maskInventory.Count < 3)
        {
            followingPlayer = true;
        } 
        if (Vector2.Distance(character.transform.position, transform.position) < 0.2f && player.maskInventory.Count < 3)
        {
            player.maskInventory.Add(maskType);
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
