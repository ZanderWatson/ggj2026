using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game_states : MonoBehaviour
{
    static GameObject character;
    public GameObject mask;
    static GameObject map;
    const int MASKS_PER_SPAWN = 3;
    float spawnMaskTimer = 5;
    static int round = 1;
    public static bool maskCollectingPhase;
    public static bool prepPhase;
    public static bool duelPhase;
    const float MASK_COLLECTING_TIME = 30;
    static float maskCollectingTimer;

    public static GameObject currentBattleEnemy;
    public static System.Collections.Generic.List<GameObject> hiddenEnemies = new System.Collections.Generic.List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GetComponent<UpgradeUI>() == null) gameObject.AddComponent<UpgradeUI>();
        if (GetComponent<EnemyHealthBar>() == null) gameObject.AddComponent<EnemyHealthBar>();
    }
    void Start()
    {
        character = GameObject.Find("Player");
        map = GameObject.Find("Map");
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
                        Vector2 randomSpawn = Random.insideUnitCircle * 24;
                        GameObject newMask = Instantiate(mask, randomSpawn, Quaternion.identity);
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
    public static void OnPlayerWon(enemy defeatedEnemy)
    {
        if (defeatedEnemy != null) Object.Destroy(defeatedEnemy.gameObject);
        currentBattleEnemy = null;
        UpgradeUI.Show();
    }

    public static void FinishPostBattle()
    {
        foreach (GameObject e in hiddenEnemies)
        {
            if (e != null) e.SetActive(true);
        }
        hiddenEnemies.Clear();
        SwitchPhases();
    }

    public static void SwitchPhases()
    {
        if (prepPhase)
        {
            character.transform.position = new Vector3(-5, 0, 0);
            prepPhase = false;
            duelPhase = true;
            map.GetComponent<SpriteRenderer>().color = Color.darkRed;
        } else if (duelPhase)
        {
            round += 1;
            character.transform.position = Vector3.zero;
            prepPhase = true;
            duelPhase = false;
            map.GetComponent<SpriteRenderer>().color = new Color(38 / 255, 91 / 255, 72 / 255);
        }
    }
}
