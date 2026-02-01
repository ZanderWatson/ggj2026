using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game_states : MonoBehaviour
{
    static GameObject character;
    public GameObject mask;
    static GameObject map;

    static int round = 1;
    static int TOTAL_ROUNDS = 5;
    public static bool maskCollectingPhase;
    public static bool prepPhase;
    public static bool duelPhase;
    // Game end
    public static bool gameEnded;
    public static float gameTimer = 0;
    public static TextMeshProUGUI gameTimerText;
    public static TextMeshProUGUI gameEndText;
    // Mask Collection
    const float MASK_COLLECTING_TIME = 30;
    static float maskCollectingTimer;
    const int MASKS_PER_SPAWN = 3;
    float spawnMaskTimer = 5;
    public static Image maskBG1; public static Image maskBG2; public static Image maskBG3;
    public static GameObject currentBattleEnemy;
    public static List<GameObject> hiddenEnemies = new List<GameObject>();

    private void Awake()
    {
        if (GetComponent<UpgradeUI>() == null) gameObject.AddComponent<UpgradeUI>();
        if (GetComponent<EnemyHealthBar>() == null) gameObject.AddComponent<EnemyHealthBar>();
    }
    void Start()
    {
        character = GameObject.Find("Player");
        map = GameObject.Find("Map");
        gameEndText = GameObject.Find("Game End Text").GetComponent<TextMeshProUGUI>();
        gameTimerText = GameObject.Find("Game Timer").GetComponent<TextMeshProUGUI>();
        gameEndText.enabled = false;
        maskBG1 = GameObject.Find("Mask Inventory 1").GetComponent<Image>();
        maskBG2 = GameObject.Find("Mask Inventory 2").GetComponent<Image>();
        maskBG3 = GameObject.Find("Mask Inventory 3").GetComponent<Image>();
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
        if (!gameEnded)
        {
            gameTimer += Time.deltaTime;
        }
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        gameTimerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            
        
    }

    /** When the player wins a battle, enemy is destroyed and the upgrade UI is shown. */
    public static void OnPlayerWon(enemy defeatedEnemy)
    {
        if (defeatedEnemy != null) 
        {
            Object.Destroy(defeatedEnemy.gameObject);
        }
        
        currentBattleEnemy = null;
        //yield return new WaitForSeconds(1);
        UpgradeUI.Show();
        
    }

    public static void FinishPostBattle() 
    {
        foreach (GameObject e in hiddenEnemies)
        {
            if (e != null) 
            {
                e.SetActive(true);
            }
        }
        hiddenEnemies.Clear();
        if (character != null)
        {
            player p = character.GetComponent<player>();
            if (p != null) 
            {
                p.RestoreFullHealth(); // Restore player's HP after each fight (every fight starts anew).
            }
        }
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
            if (round > TOTAL_ROUNDS)
            {
                TriggerGameEnd();
            }
            player.activeMask = 0;
            maskBG1.color = Color.white; maskBG2.color = Color.white; maskBG3.color = Color.white;
            character.transform.position = Vector3.zero;
            prepPhase = true;
            maskCollectingPhase = true;
            duelPhase = false;
            map.GetComponent<SpriteRenderer>().color = Color.forestGreen;
        }
    }

    public static void TriggerGameEnd()
    {
        gameEndText.enabled = true;
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        gameEndText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}
