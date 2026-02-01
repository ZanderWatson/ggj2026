// Ethan Le (1/31/2026): EnemyHealthBar.cs

// Updates the enemy HP bar during duel phase.
// Setup: Your enemy health bar GameObject (e.g. "EnemyHealth" or "Enemy Health Bar") 
// must have a child named "Enemy Health Bar Fill" with Image Type: Filled, Fill Method: Horizontal.
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Image fillImage;
    GameObject enemyHealthBarRoot;

    void Start()
    {
        // Try "EnemyHealth" GameObject first, then "Enemy Health Bar" GameObject:
        enemyHealthBarRoot = GameObject.Find("EnemyHealth") ?? GameObject.Find("Enemy Health Bar");
        if (enemyHealthBarRoot != null)
        {
            Transform fill = enemyHealthBarRoot.transform.Find("Enemy Health Bar Fill");
            if (fill != null) 
            {
                fillImage = fill.GetComponent<Image>(); // Get the Image component of the fill. 
            }
            enemyHealthBarRoot.SetActive(false); // Start hidden; shown only during duel phase.
        }
    }

    void Update()
    {
        if (fillImage == null || enemyHealthBarRoot == null) return;

        if (game_states.duelPhase)
        {
            enemyHealthBarRoot.SetActive(true);
            enemy e = game_states.currentBattleEnemy != null ? game_states.currentBattleEnemy.GetComponent<enemy>() : null;
            if (e != null)
            {
                fillImage.fillAmount = e.GetHealthPercent(); // Update the fill amount of the enemy health bar based on its HP. 
            }
        }
        else
        {
            enemyHealthBarRoot.SetActive(false);
        }
    }
}
