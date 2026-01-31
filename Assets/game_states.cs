using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_states : MonoBehaviour
{
    static GameObject character;
    public GameObject mask;
    const int MASKS_PER_SPAWN = 2;
    float spawnMaskTimer = 5;
    static int round = 1;
    public static bool maskCollectingPhase;
    public static bool prepPhase;
    public static bool duelPhase;
    const float MASK_COLLECTING_TIME = 30;
    static float maskCollectingTimer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        character = GameObject.Find("Player");
        prepPhase = true;
        maskCollectingPhase = true;
        maskCollectingTimer = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (prepPhase)
        {
            // Spawning MASKS_PER_SPAWN masks every 5 seconds
            if (maskCollectingPhase)
            {
                spawnMaskTimer -= Time.deltaTime;
                if (spawnMaskTimer < 0)
                {
                    spawnMaskTimer = 5;
                    foreach (int i in Enumerable.Range(1, MASKS_PER_SPAWN))
                    {
                        Vector2 randomSpawn = Random.insideUnitCircle * 25;
                        Instantiate(mask, randomSpawn, Quaternion.identity);
                    }
                }
                // Disable mask collecting phase to stop masks from spawning
                maskCollectingTimer -= Time.deltaTime;
                if (maskCollectingTimer < 0 )
                {
                    maskCollectingPhase = false;
                    maskCollectingTimer = MASK_COLLECTING_TIME;
                }
            }
        }
        
    }
    public static void SwitchPhases()
    {
        
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            character.transform.position = new Vector3(-5, 0, 0);
            prepPhase = false;
            duelPhase = true;
            SceneManager.LoadScene("Battle");
        } else if (SceneManager.GetActiveScene().name.Equals("Battle"))
        {
            round += 1;
            character.transform.position = Vector3.zero;
            prepPhase = true;
            duelPhase = false;
            SceneManager.LoadScene("Main");
        }
    }
}
